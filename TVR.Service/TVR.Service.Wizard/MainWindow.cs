﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TVR.Service.Wizard.Pages;

namespace TVR.Service.Wizard
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            pagedPanel1.Pages.AddRange(new BasePage[] {
                new WelcomePage(),
                new CameraBasicsPage(),
                new CalibrationPage(),
                new TrackingPage()
            });
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to cancel the setup?", "TwometerVR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pagedPanel1.PreviousPage();
            btnNextStep.Text = "Next";

            if (pagedPanel1.IsOnFirstPage)
                btnPrevious.Enabled = false;
        }

        private void btnNextStep_Click(object sender, EventArgs e)
        {
            btnPrevious.Enabled = true;
            if (pagedPanel1.IsOnLastPage)
            {
                OnWizardComplete();
                return;
            }
            else
            {
                pagedPanel1.NextPage();
                if (pagedPanel1.IsOnLastPage)
                    btnNextStep.Text = "Finish";
            }
        }

        private void OnWizardComplete()
        {
            MessageBox.Show("You have completed the setup for TwometerVR and can now start playing your favourite VR games!", "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
