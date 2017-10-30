namespace RavenfieldCheater
{
    partial class RavenfieldCheater
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RavenfieldCheater));
            this.Folder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Backup = new System.Windows.Forms.Button();
            this.Restore = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.NoFall = new System.Windows.Forms.Button();
            this.Fast = new System.Windows.Forms.Button();
            this.Ammo = new System.Windows.Forms.Button();
            this.Health = new System.Windows.Forms.Button();
            this.SecretWeapons = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Folder
            // 
            this.Folder.Location = new System.Drawing.Point(191, 39);
            this.Folder.Name = "Folder";
            this.Folder.Size = new System.Drawing.Size(129, 23);
            this.Folder.TabIndex = 0;
            this.Folder.Text = "Choose RF Folder";
            this.Folder.UseVisualStyleBackColor = true;
            this.Folder.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(79, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(362, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Please Choose The Folder Ravenfield Data folder.";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Backup
            // 
            this.Backup.Location = new System.Drawing.Point(117, 68);
            this.Backup.Name = "Backup";
            this.Backup.Size = new System.Drawing.Size(129, 23);
            this.Backup.TabIndex = 2;
            this.Backup.Text = "Backup RF";
            this.Backup.UseVisualStyleBackColor = true;
            this.Backup.Click += new System.EventHandler(this.Backup_Click);
            // 
            // Restore
            // 
            this.Restore.Location = new System.Drawing.Point(252, 68);
            this.Restore.Name = "Restore";
            this.Restore.Size = new System.Drawing.Size(129, 23);
            this.Restore.TabIndex = 3;
            this.Restore.Text = "Restore RF";
            this.Restore.UseVisualStyleBackColor = true;
            this.Restore.Click += new System.EventHandler(this.Restore_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(228, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tools";
            // 
            // NoFall
            // 
            this.NoFall.Location = new System.Drawing.Point(117, 133);
            this.NoFall.Name = "NoFall";
            this.NoFall.Size = new System.Drawing.Size(129, 23);
            this.NoFall.TabIndex = 5;
            this.NoFall.Text = "No Fall";
            this.NoFall.UseVisualStyleBackColor = true;
            this.NoFall.Click += new System.EventHandler(this.NoFall_Click);
            // 
            // Fast
            // 
            this.Fast.Location = new System.Drawing.Point(252, 133);
            this.Fast.Name = "Fast";
            this.Fast.Size = new System.Drawing.Size(129, 23);
            this.Fast.TabIndex = 6;
            this.Fast.Text = "Fast Motion";
            this.Fast.UseVisualStyleBackColor = true;
            this.Fast.Click += new System.EventHandler(this.Fast_Click);
            // 
            // Ammo
            // 
            this.Ammo.Location = new System.Drawing.Point(117, 162);
            this.Ammo.Name = "Ammo";
            this.Ammo.Size = new System.Drawing.Size(129, 23);
            this.Ammo.TabIndex = 7;
            this.Ammo.Text = "Infinite Ammo";
            this.Ammo.UseVisualStyleBackColor = true;
            this.Ammo.Click += new System.EventHandler(this.Ammo_Click);
            // 
            // Health
            // 
            this.Health.Location = new System.Drawing.Point(252, 162);
            this.Health.Name = "Health";
            this.Health.Size = new System.Drawing.Size(129, 23);
            this.Health.TabIndex = 8;
            this.Health.Text = "Infinite Health";
            this.Health.UseVisualStyleBackColor = true;
            this.Health.Click += new System.EventHandler(this.Health_Click);
            // 
            // SecretWeapons
            // 
            this.SecretWeapons.Location = new System.Drawing.Point(170, 191);
            this.SecretWeapons.Name = "SecretWeapons";
            this.SecretWeapons.Size = new System.Drawing.Size(180, 23);
            this.SecretWeapons.TabIndex = 10;
            this.SecretWeapons.Text = "Secret Weapons Unlocked (WIP)";
            this.SecretWeapons.UseVisualStyleBackColor = true;
            this.SecretWeapons.Click += new System.EventHandler(this.SecretWeapons_Click);
            // 
            // RavenfieldCheater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 240);
            this.Controls.Add(this.SecretWeapons);
            this.Controls.Add(this.Health);
            this.Controls.Add(this.Ammo);
            this.Controls.Add(this.Fast);
            this.Controls.Add(this.NoFall);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Restore);
            this.Controls.Add(this.Backup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Folder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RavenfieldCheater";
            this.Text = "Ravenfield Cheater";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Folder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Backup;
        private System.Windows.Forms.Button Restore;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button NoFall;
        private System.Windows.Forms.Button Fast;
        private System.Windows.Forms.Button Ammo;
        private System.Windows.Forms.Button Health;
        private System.Windows.Forms.Button SecretWeapons;
    }
}

