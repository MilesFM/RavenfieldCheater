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
using Mono.Cecil.Cil;
using Mono.Cecil;

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

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog selectDLL = new FolderBrowserDialog();
            selectDLL.Description = "Please find your Ravenfield folder";
            if (selectDLL.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = selectDLL.SelectedPath + "\\ravenfield_Data\\Managed\\Assembly-CSharp.dll";
            }
        }

        private void Backup_Click(object sender, EventArgs e)
        {
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
            assembly = ModuleDefinition.ReadModule(filePath);
            TypeDefinition ActorClass = assembly.Types.First(t => t.Name == "Actor");
            if (ActorClass == null)
            {
                MessageBox.Show("Actor Class Not Found!");
                return;
            }
            Application.DoEvents();

            MethodDefinition FallOverMethod = ActorClass.Methods.First(m => m.Name == "FallOver");
            if (FallOverMethod == null)
            {
                MessageBox.Show("Actor.FallOver() Method Not Found!");
                return;
            }
            Application.DoEvents();

            ILProcessor processor = FallOverMethod.Body.GetILProcessor();
            foreach (var instuc in processor.Body.Instructions.ToArray())
            {
                processor.Body.Instructions.Remove(instuc);
                //processor.Remove(instuc);
            }
            FallOverMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));

            try
            {
                assembly.Write(filePath);
            }
            catch (Mono.Cecil.AssemblyResolutionException error)
            {
                MessageBox.Show(error.Message);
            }
            MessageBox.Show("NoFall Added Successfully!");
            Application.DoEvents();
        }

        private void Fast_Click(object sender, EventArgs e)
        {

        }
    }
}