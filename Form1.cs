﻿using System;
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
        ModuleDefinition assembly;
        public RavenfieldCheater()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog selectDLL = new FolderBrowserDialog();
            selectDLL.Description = "Please find the folder Ravenfield is in. e.g. Desktop if ravenfield_Data is in that folder.";
            if (selectDLL.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = selectDLL.SelectedPath + "\\ravenfield_Data\\Managed\\Assembly-CSharp.dll"; // Where RF stores it's source code
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

            Instruction original = processor.Body.Instructions[212];

            // Changes if (Time.timeScale < 1f) to a > sign
            Instruction replace = original; 
            replace.OpCode = OpCodes.Ble_Un;

            processor.Replace(original, replace);

            original = processor.Body.Instructions[216];

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

        }

        private void Health_Click(object sender, EventArgs e)
        {
            /*if (filePath == null) { MessageBox.Show("Please select the folder Ravenfield is in."); return; }

            assembly = ModuleDefinition.ReadModule(filePath); // Load Assembly-CSharp.dll

            TypeDefinition ActorClass = assembly.Types.First(t => t.Name == "Actor"); // Finds Actor class (no namespace) in Assembly-CSharp.dll
            if (ActorClass == null) // If Actor does not exist, stop
            {
                MessageBox.Show("Actor Class Class Not Found!");
                return;
            }
            Application.DoEvents(); // Lets the program catch up

            MethodDefinition UpdateMethod = ActorClass.Methods.First(m => m.Name == "Update"); // Finds FpsActorController.Update()
            if (UpdateMethod == null) // If Actor.Update() does not exist, stop
            {
                MessageBox.Show("Actor.Update() Method Not Found!");
                return;
            }
            Application.DoEvents();

            ILProcessor processor = UpdateMethod.Body.GetILProcessor();

            //FieldDefinition ActorHealth = ActorClass.Fields.First(f => f.Name == "health"); // Finds health for reference
            FieldReference ActorHealth = ActorClass.Fields.First<FieldReference>(f => f.Name == "health");

            // Inserts stfld, ldc.r4,  ildarg.0 into Actor.Update()
            Instruction[] instructions = processor.Body.Instructions.ToArray();
            Instruction lastInstruc = instructions[instructions.Length - 1];

            Instruction insertInstruc = processor.Create(OpCodes.Stfld, ActorHealth);
            //insertInstruc.Operand = ActorHealth; // float32 Actor::health
            processor.InsertBefore(lastInstruc, insertInstruc);

            lastInstruc = insertInstruc;
            insertInstruc = processor.Create(OpCodes.Ldc_I4, 1000f);
            //insertInstruc.Operand = 1000f;
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
            */
        }
    }
}