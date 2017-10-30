using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace RavenfieldCheater
{
    public partial class RavenfieldCheater : Form
    {
        private string filePath;
        private string originalPath;
        ModuleDefinition assembly;
        public RavenfieldCheater()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog selectDLL = new FolderBrowserDialog();
            selectDLL.Description = "Please find ravenfield_Data in Ravenfield folder if on Windows/Linux or Data in Mac ravenfield.app/Contet/Resources.";
            if (selectDLL.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                originalPath = selectDLL.SelectedPath; // In case we need to access any other DLLs/Resources
                filePath = originalPath + "\\Managed\\Assembly-CSharp.dll"; // Where RF stores it's source code
            }
        }

        private void Backup_Click(object sender, EventArgs e)
        {
            if (filePath == null) { MessageBox.Show("Please select the folder Ravenfield is in."); return; }

            try
            {
                if (File.Exists(filePath + ".bak"))
                {
                    File.Delete(filePath + ".bak");

                }
                File.Copy(filePath, filePath + ".bak");
                MessageBox.Show("Backup Complete!");
            }
            catch (FileNotFoundException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            if (filePath == null) { MessageBox.Show("Please select the folder Ravenfield is in."); return; }

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                }
                File.Copy(filePath + ".bak", filePath);
                MessageBox.Show("Restore Complete!");
            }
            catch (FileNotFoundException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void NoFall_Click(object sender, EventArgs e)
        {
            if (filePath == null) { MessageBox.Show("Please select the folder Ravenfield is in."); return; }

            assembly = ModuleDefinition.ReadModule(filePath); // Load Assembly-CSharp.dll
            TypeDefinition ActorClass = assembly.Types.First(t => t.Name == "Actor"); // Finds Actor class (no namespace) in Assembly-CSharp.dll
            if (ActorClass == null) // If Actor does not exist, stop
            {
                MessageBox.Show("Actor Class Not Found!");
                return;
            }
            Application.DoEvents(); // Lets the program catch up

            MethodDefinition FallOverMethod = ActorClass.Methods.First(m => m.Name == "FallOver"); // Finds Actor.FallOver()
            if (FallOverMethod == null) // If Actor.FallOver() does not exist, stop
            {
                MessageBox.Show("Actor.FallOver() Method Not Found!");
                return;
            }
            Application.DoEvents(); // Catch up

            ILProcessor processor = FallOverMethod.Body.GetILProcessor();

            // Erases Actor.FallOver()
            foreach (var instuc in processor.Body.Instructions.ToArray())
            {
                processor.Body.Instructions.Remove(instuc);
            }
            // Added a "ret" instruction to the empty method so the function does run as
            // some methods are still depended on the method
            FallOverMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));

            try
            {
                assembly.Write(filePath); // Try to write it back to Assembly-CSharp.dll
            }
            catch (Mono.Cecil.AssemblyResolutionException error)
            {
                MessageBox.Show(error.Message);
                return;
            }
            MessageBox.Show("No Fall Added Successfully!");
            Application.DoEvents();
        }

        private void Fast_Click(object sender, EventArgs e)
        {
            if (filePath == null) { MessageBox.Show("Please select the folder Ravenfield is in."); return; }

            assembly = ModuleDefinition.ReadModule(filePath); // Load Assembly-CSharp.dll
            ModuleDefinition UnityDLL = ModuleDefinition.ReadModule(originalPath + "\\Managed\\UnityEngine.dll"); // Load UnityEngine for referencing

            TypeDefinition FpsActorControllerClass = assembly.Types.First(t => t.Name == "FpsActorController"); // Finds FpsActorController class (no namespace) in Assembly-CSharp.dll
            if (FpsActorControllerClass == null) // If FpsActorController does not exist, stop
            {
                MessageBox.Show("FpsActorController Class Not Found!");
                return;
            }
            Application.DoEvents(); // Lets the program catch up

            MethodDefinition UpdateMethod = FpsActorControllerClass.Methods.First(m => m.Name == "Update"); // Finds FpsActorControllerClass.Update()
            if (UpdateMethod == null) // If FpsActorController.Update() does not exist, stop
            {
                MessageBox.Show("FpsActorController.Update() Method Not Found!");
                return;
            }
            Application.DoEvents();

            ILProcessor processor = UpdateMethod.Body.GetILProcessor();

            Instruction original;

            original = processor.Body.Instructions[212];

            MethodReference timeScaleRef = assembly.Import(typeof(UnityEngine.Time).GetMethod("get_timeScale", new[] { typeof(UnityEngine.Object), typeof(UnityEngine.Object) }));
            foreach (var instuction in processor.Body.Instructions)
            {
                if (instuction.Operand == timeScaleRef)
                {
                    original = instuction.Next;
                    original = original.Next;
                }
            }

            // Changes if (Time.timeScale < 1f) to a > sign
            Instruction replace = original;
            replace.OpCode = OpCodes.Ble_Un;

            processor.Replace(original, replace);

            //original = processor.Body.Instructions[216];
            original = original.Next;
            original = original.Next;
            original = original.Next;
            original = original.Next;
            /*for (int i = i; i <= 4; i++)
            {
                original = original.Next;
            }*/

            // Changes Time.timeScale = 0.2f; to 2.
            replace = original;
            replace.Operand = 2f;

            processor.Replace(original, replace);
            try
            {
                assembly.Write(filePath); // Try to write it back to Assembly-CSharp.dll
            }
            catch (Mono.Cecil.AssemblyResolutionException error)
            {
                MessageBox.Show(error.Message);
                return;
            }
            MessageBox.Show("Fast Motion Added Successfully!");
            Application.DoEvents();
        }

        private void Ammo_Click(object sender, EventArgs e)
        {

            assembly = ModuleDefinition.ReadModule(filePath); // Load Assembly-CSharp.dll

            TypeDefinition WeaponClass = assembly.Types.First(t => t.Name == "Weapon"); // Finds Weapon class (no namespace) in Assembly-CSharp.dll
            if (WeaponClass == null) // If Actor does not exist, stop
            {
                MessageBox.Show("Weapon Class Class Not Found!");
                return;
            }
            Application.DoEvents(); // Lets the program catch up

            MethodDefinition LateUpdateMethod;

            // Does Weapon.LateUpdate() exist?
            try
            {
                WeaponClass.Methods.First<MethodReference>(m => m.Name == "LateUpdate");
            }
            catch (Exception exc)
            {
                // Creates Weapon.LateUpdate()
                LateUpdateMethod = new MethodDefinition("LateUpdate", MethodAttributes.Public, assembly.TypeSystem.Void);
                WeaponClass.Methods.Add(LateUpdateMethod);
                LateUpdateMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
                goto skip;
            }

            MessageBox.Show("Actor.LateUpdate() exists already. This may cause problems down the line.");
            LateUpdateMethod = WeaponClass.Methods.First<MethodDefinition>(m => m.Name == "LateUpdate");
            Application.DoEvents();

        skip:
            ILProcessor processor = LateUpdateMethod.Body.GetILProcessor();

            FieldReference WeaponAmmo = WeaponClass.Fields.First<FieldReference>(f => f.Name == "ammo");// Finds ammo for reference
            MethodReference WeaponUserIsPlayer;
            try
            {
                WeaponUserIsPlayer = WeaponClass.Methods.First<MethodReference>(m => m.Name == "UserIsPlayer"); // Finds UserIsPlayer() for reference
            }
            catch (Exception exc) // If Weapon.UserIsPlayer() does not exist, create it
            {
                MessageBox.Show("Weapon.UserIsPlayer() not found, creating");
                MethodDefinition method = new MethodDefinition("UserIsPlayer", MethodAttributes.Public, assembly.TypeSystem.Boolean);
                WeaponClass.Methods.Add(method);
                ILProcessor ilProc = method.Body.GetILProcessor();
                FieldReference user = WeaponClass.Fields.First<FieldReference>(f => f.Name == "user");
                FieldReference aiControlled = assembly.Types.First<TypeDefinition>(t => t.Name == "Actor")
                                                                                  .Fields.First(f => f.Name == "aiControlled");
                MethodReference uIneq = assembly.Import(typeof(UnityEngine.Object).GetMethod("op_Inequality", new[] { typeof(UnityEngine.Object), typeof(UnityEngine.Object) }));

                Instruction[] structs = new Instruction[13];
                structs[11] = ilProc.Create(OpCodes.Ldc_I4_0);
                structs[12] = ilProc.Create(OpCodes.Ret);
                structs = new Instruction[13]
                {
                    ilProc.Create(OpCodes.Ldarg_0),
                    ilProc.Create(OpCodes.Ldfld, user),
                    ilProc.Create(OpCodes.Ldnull),
                    ilProc.Create(OpCodes.Call, uIneq),
                    ilProc.Create(OpCodes.Brfalse, structs[11]),
                    ilProc.Create(OpCodes.Ldarg_0),
                    ilProc.Create(OpCodes.Ldfld, user),
                    ilProc.Create(OpCodes.Ldfld, aiControlled),
                    ilProc.Create(OpCodes.Ldc_I4_0),
                    ilProc.Create(OpCodes.Ceq),
                    ilProc.Create(OpCodes.Br_S, structs[12]),
                    ilProc.Create(OpCodes.Ldc_I4_0),
                    ilProc.Create(OpCodes.Ret)
                };
                foreach (Instruction i in structs) method.Body.Instructions.Add(i);
                method.Body.Instructions[4] = ilProc.Create(OpCodes.Brfalse, method.Body.Instructions[11]); // pointing Break If instructions to the right place
                method.Body.Instructions[10] = ilProc.Create(OpCodes.Br_S, method.Body.Instructions[12]);
                WeaponUserIsPlayer = WeaponClass.Methods.First<MethodReference>(m => m.Name == "UserIsPlayer");
            }

            // Inserts a if statment to check if Actor is Player, if so, make ammo 1000
            Instruction[] instructions = processor.Body.Instructions.ToArray();

            Instruction lastInstruc = instructions[instructions.Length - 1];
            Instruction insertInstruc = processor.Create(OpCodes.Stfld, WeaponAmmo);
            processor.InsertBefore(lastInstruc, insertInstruc);


            lastInstruc = insertInstruc;
            insertInstruc = processor.Create(OpCodes.Ldc_I4, 1000);
            processor.InsertBefore(lastInstruc, insertInstruc);

            lastInstruc = insertInstruc;
            insertInstruc = processor.Create(OpCodes.Ldarg_0);
            processor.InsertBefore(lastInstruc, insertInstruc);

            lastInstruc = insertInstruc;
            insertInstruc = processor.Create(OpCodes.Brfalse_S, instructions[instructions.Length - 1]);
            processor.InsertBefore(lastInstruc, insertInstruc);

            lastInstruc = insertInstruc;
            insertInstruc = processor.Create(OpCodes.Call, WeaponUserIsPlayer);
            processor.InsertBefore(lastInstruc, insertInstruc);

            lastInstruc = insertInstruc;
            insertInstruc = processor.Create(OpCodes.Ldarg_0);
            processor.InsertBefore(lastInstruc, insertInstruc);

            try
            {
                assembly.Write(filePath); // Try to write it back to Assembly-CSharp.dll
            }
            catch (Mono.Cecil.AssemblyResolutionException error)
            {
                MessageBox.Show(error.Message);
                return;
            }
            MessageBox.Show("Infinite Ammo Added Successfully!");
            Application.DoEvents();
        }

        private void Health_Click(object sender, EventArgs e)
        {
            if (filePath == null) { MessageBox.Show("Please select the folder Ravenfield is in."); return; }

            assembly = ModuleDefinition.ReadModule(filePath); // Load Assembly-CSharp.dll

            TypeDefinition ActorClass = assembly.Types.First(t => t.Name == "Actor"); // Finds Actor class (no namespace) in Assembly-CSharp.dll
            if (ActorClass == null) // If Actor does not exist, stop
            {
                MessageBox.Show("Actor Class Class Not Found!");
                return;
            }
            Application.DoEvents(); // Lets the program catch up

            MethodDefinition LateUpdateMethod;

            // Does Actor.LateUpdate() exist?
            try
            {
                ActorClass.Methods.First<MethodReference>(m => m.Name == "LateUpdate");
            }
            catch (Exception exc)
            {
                // Creates Actor.LateUpdate()
                LateUpdateMethod = new MethodDefinition("LateUpdate", MethodAttributes.Public, assembly.TypeSystem.Void);
                ActorClass.Methods.Add(LateUpdateMethod);
                LateUpdateMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
                goto skip;
            }

            MessageBox.Show("Actor.LateUpdate() exists already. This may cause problems down the line.");
            LateUpdateMethod = ActorClass.Methods.First<MethodDefinition>(m => m.Name == "LateUpdate");
            Application.DoEvents();

        skip:
            ILProcessor processor = LateUpdateMethod.Body.GetILProcessor();

            FieldReference ActorHealth = ActorClass.Fields.First<FieldReference>(f => f.Name == "health");// Finds health for reference
            FieldReference ActorAiControlled = ActorClass.Fields.First<FieldReference>(f => f.Name == "aiControlled");// Finds aiControlled for reference



            // Inserts a if statment to check if Actor is AI, if not, make health 1000f
            Instruction[] instructions = processor.Body.Instructions.ToArray();

            Instruction lastInstruc = instructions[instructions.Length - 1];
            Instruction insertInstruc = processor.Create(OpCodes.Stfld, ActorHealth);
            processor.InsertBefore(lastInstruc, insertInstruc);

            lastInstruc = insertInstruc;
            insertInstruc = processor.Create(OpCodes.Ldc_R4, 1000f);
            processor.InsertBefore(lastInstruc, insertInstruc);

            lastInstruc = insertInstruc;
            insertInstruc = processor.Create(OpCodes.Ldarg_0);
            processor.InsertBefore(lastInstruc, insertInstruc);

            lastInstruc = insertInstruc;
            insertInstruc = processor.Create(OpCodes.Brtrue_S, instructions[instructions.Length - 1]);
            processor.InsertBefore(lastInstruc, insertInstruc);

            lastInstruc = insertInstruc;
            insertInstruc = processor.Create(OpCodes.Ldfld, ActorAiControlled);
            processor.InsertBefore(lastInstruc, insertInstruc);

            lastInstruc = insertInstruc;
            insertInstruc = processor.Create(OpCodes.Ldarg_0);
            processor.InsertBefore(lastInstruc, insertInstruc);

            try
            {
                assembly.Write(filePath); // Try to write it back to Assembly-CSharp.dll
            }
            catch (Mono.Cecil.AssemblyResolutionException error)
            {
                MessageBox.Show(error.Message);
                return;
            }
            MessageBox.Show("Infinite Health Added Successfully!");
            Application.DoEvents();
        }

        private void SecretWeapons_Click(object sender, EventArgs e)
        {
            /*
            if (filePath == null) { MessageBox.Show("Please select the folder Ravenfield is in."); return; }

            assembly = ModuleDefinition.ReadModule(filePath); // Load Assembly-CSharp.dll

            TypeDefinition WeaponManagerClass = assembly.Types.First(t => t.Name == "WeaponManager"); // Finds WeaponManager class (no namespace) in Assembly-CSharp.dll
            if (WeaponManagerClass == null) // If WeaponManager does not exist, stop
            {
                MessageBox.Show("WeaponManager Class Class Not Found!");
                return;
            }
            Application.DoEvents(); // Lets the program catch up

            MethodDefinition AwakeMethod = WeaponManagerClass.Methods.First(m => m.Name == "Awake"); // Finds WeaponManager.Awake()
            if (AwakeMethod == null) // If WeaponManager.Awake() does not exist, stop
            {
                MessageBox.Show("WeaponManager.Awake() Method Not Found!");
                return;
            }
            Application.DoEvents();

            ILProcessor processor = AwakeMethod.Body.GetILProcessor();

            Instruction[] instructions = processor.Body.Instructions.ToArray();
            */
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}