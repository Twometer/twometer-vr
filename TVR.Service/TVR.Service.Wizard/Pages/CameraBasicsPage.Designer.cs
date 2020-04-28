namespace TVR.Service.Wizard.Pages
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nFrameWidth = new System.Windows.Forms.NumericUpDown();
            this.nFrameHeight = new System.Windows.Forms.NumericUpDown();
            this.nCamIndex = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nFrameWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nFrameHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCamIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(27, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Configure your camera";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(25, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 29);
            this.label4.TabIndex = 6;
            this.label4.Text = "Camera";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(196, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 267);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "Camera index:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Frame width:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(27, 226);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "Frame height:";
            // 
            // nFrameWidth
            // 
            this.nFrameWidth.Location = new System.Drawing.Point(30, 187);
            this.nFrameWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nFrameWidth.Minimum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.nFrameWidth.Name = "nFrameWidth";
            this.nFrameWidth.Size = new System.Drawing.Size(123, 20);
            this.nFrameWidth.TabIndex = 12;
            this.nFrameWidth.Value = new decimal(new int[] {
            1280,
            0,
            0,
            0});
            // 
            // nFrameHeight
            // 
            this.nFrameHeight.Location = new System.Drawing.Point(30, 244);
            this.nFrameHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nFrameHeight.Minimum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.nFrameHeight.Name = "nFrameHeight";
            this.nFrameHeight.Size = new System.Drawing.Size(123, 20);
            this.nFrameHeight.TabIndex = 13;
            this.nFrameHeight.Value = new decimal(new int[] {
            720,
            0,
            0,
            0});
            // 
            // nCamIndex
            // 
            this.nCamIndex.Location = new System.Drawing.Point(30, 130);
            this.nCamIndex.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nCamIndex.Name = "nCamIndex";
            this.nCamIndex.Size = new System.Drawing.Size(123, 20);
            this.nCamIndex.TabIndex = 14;
            // 
            // CameraBasicsPage
            // 
            this.Controls.Add(this.nCamIndex);
            this.Controls.Add(this.nFrameHeight);
            this.Controls.Add(this.nFrameWidth);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Name = "CameraBasicsPage";
            this.Size = new System.Drawing.Size(510, 310);
            ((System.ComponentModel.ISupportInitialize)(this.nFrameWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nFrameHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCamIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nFrameWidth;
        private System.Windows.Forms.NumericUpDown nFrameHeight;
        private System.Windows.Forms.NumericUpDown nCamIndex;
    }
}
