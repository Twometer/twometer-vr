namespace TVRSvc
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.lbTracker1Pos = new System.Windows.Forms.Label();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbTracker2Pos = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.glControl1 = new OpenTK.GLControl();
            this.label2 = new System.Windows.Forms.Label();
            this.lbfps = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.lbConnectedDrivers = new System.Windows.Forms.Label();
            this.lbConnectedControllers = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Begin";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbTracker1Pos
            // 
            this.lbTracker1Pos.AutoSize = true;
            this.lbTracker1Pos.Location = new System.Drawing.Point(28, 74);
            this.lbTracker1Pos.Name = "lbTracker1Pos";
            this.lbTracker1Pos.Size = new System.Drawing.Size(79, 13);
            this.lbTracker1Pos.TabIndex = 1;
            this.lbTracker1Pos.Text = "<position here>";
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(319, 12);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(610, 484);
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(265, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Preview:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Tracker 1:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Tracker 2:";
            // 
            // lbTracker2Pos
            // 
            this.lbTracker2Pos.AutoSize = true;
            this.lbTracker2Pos.Location = new System.Drawing.Point(28, 119);
            this.lbTracker2Pos.Name = "lbTracker2Pos";
            this.lbTracker2Pos.Size = new System.Drawing.Size(79, 13);
            this.lbTracker2Pos.TabIndex = 7;
            this.lbTracker2Pos.Text = "<position here>";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(16, 380);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 17);
            this.radioButton1.TabIndex = 8;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Raw";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 355);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Preview stream:";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(16, 403);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(71, 17);
            this.radioButton2.TabIndex = 10;
            this.radioButton2.Text = "Tracker 1";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(16, 426);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(71, 17);
            this.radioButton3.TabIndex = 11;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Tracker 2";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 478);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(100, 17);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Visualize circles";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(1007, 12);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(570, 483);
            this.glControl1.TabIndex = 13;
            this.glControl1.VSync = true;
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(952, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "3D view:";
            // 
            // lbfps
            // 
            this.lbfps.AutoSize = true;
            this.lbfps.Location = new System.Drawing.Point(935, 483);
            this.lbfps.Name = "lbfps";
            this.lbfps.Size = new System.Drawing.Size(30, 13);
            this.lbfps.TabIndex = 15;
            this.lbfps.Text = "0 fps";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 16;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(16, 449);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(47, 17);
            this.radioButton4.TabIndex = 16;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Both";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // lbConnectedDrivers
            // 
            this.lbConnectedDrivers.AutoSize = true;
            this.lbConnectedDrivers.Location = new System.Drawing.Point(13, 205);
            this.lbConnectedDrivers.Name = "lbConnectedDrivers";
            this.lbConnectedDrivers.Size = new System.Drawing.Size(105, 13);
            this.lbConnectedDrivers.TabIndex = 17;
            this.lbConnectedDrivers.Text = "Connected drivers: 0";
            // 
            // lbConnectedControllers
            // 
            this.lbConnectedControllers.AutoSize = true;
            this.lbConnectedControllers.Location = new System.Drawing.Point(13, 227);
            this.lbConnectedControllers.Name = "lbConnectedControllers";
            this.lbConnectedControllers.Size = new System.Drawing.Size(119, 13);
            this.lbConnectedControllers.TabIndex = 18;
            this.lbConnectedControllers.Text = "Connected controllers:0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1599, 509);
            this.Controls.Add(this.lbConnectedControllers);
            this.Controls.Add(this.lbConnectedDrivers);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.lbfps);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.lbTracker2Pos);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.lbTracker1Pos);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "TwometerVR Tracking Service";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbTracker1Pos;
        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbTracker2Pos;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.CheckBox checkBox1;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbfps;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Label lbConnectedDrivers;
        private System.Windows.Forms.Label lbConnectedControllers;
        private System.Windows.Forms.Timer timer2;
    }
}

