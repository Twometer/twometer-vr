﻿namespace TVR.Service.Wizard.Pages
{
    partial class CameraBasicsPage
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(27, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Configure your camera";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(25, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 29);
            this.label4.TabIndex = 6;
            this.label4.Text = "Camera";
            // 
            // CameraBasicsPage
            // 
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Name = "CameraBasicsPage";
            this.Size = new System.Drawing.Size(579, 345);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
