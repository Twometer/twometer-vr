namespace TVR.Service.Wizard.Pages
{
    partial class PreInstallationPage
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
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btnChooser = new System.Windows.Forms.Button();
            this.cbStartMenu = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(25, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 29);
            this.label4.TabIndex = 11;
            this.label4.Text = "Installation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 13;
            this.label1.Text = "Install location:";
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(130, 120);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(319, 20);
            this.tbPath.TabIndex = 14;
            // 
            // btnChooser
            // 
            this.btnChooser.Location = new System.Drawing.Point(455, 119);
            this.btnChooser.Name = "btnChooser";
            this.btnChooser.Size = new System.Drawing.Size(31, 22);
            this.btnChooser.TabIndex = 15;
            this.btnChooser.Text = "...";
            this.btnChooser.UseVisualStyleBackColor = true;
            this.btnChooser.Click += new System.EventHandler(this.btnChooser_Click);
            // 
            // cbStartMenu
            // 
            this.cbStartMenu.AutoSize = true;
            this.cbStartMenu.Checked = true;
            this.cbStartMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStartMenu.Location = new System.Drawing.Point(30, 183);
            this.cbStartMenu.Name = "cbStartMenu";
            this.cbStartMenu.Size = new System.Drawing.Size(128, 17);
            this.cbStartMenu.TabIndex = 16;
            this.cbStartMenu.Text = "Create start menu link";
            this.cbStartMenu.UseVisualStyleBackColor = true;
            // 
            // PreInstallationPage
            // 
            this.Controls.Add(this.cbStartMenu);
            this.Controls.Add(this.btnChooser);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Name = "PreInstallationPage";
            this.Size = new System.Drawing.Size(510, 310);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btnChooser;
        private System.Windows.Forms.CheckBox cbStartMenu;
    }
}
