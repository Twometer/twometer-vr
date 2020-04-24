using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TVR.Service.Wizard.Model;
using TVR.Service.Wizard.Pages;

namespace TVR.Service.Wizard
{
    public partial class MainWindow : Form
    {
        private WizardContext context = new WizardContext();

        public MainWindow()
        {
            InitializeComponent();
            pagedPanel.Pages.AddRange(new BasePage[] {
                new WelcomePage(),
                new PreInstallationPage(),
                new InstallationPage(),
                new CameraBasicsPage(),
                new CalibrationPage(),
                new TrackingPage()
            });
            pagedPanel.Tag = context;
            context.AllowedStateChanged += Context_AllowedStateChanged;
        }

        private void Context_AllowedStateChanged(object sender, EventArgs e)
        {
            btnNextStep.Enabled = context.AllowNext;
            btnPrevious.Enabled = context.AllowPrevious;
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
            pagedPanel.PreviousPage();
            btnNextStep.Text = "Next";

            if (pagedPanel.IsOnFirstPage)
                btnPrevious.Enabled = false;
        }

        private void btnNextStep_Click(object sender, EventArgs e)
        {
            btnPrevious.Enabled = true;
            if (pagedPanel.IsOnLastPage)
            {
                OnWizardComplete();
                return;
            }
            else
            {
                pagedPanel.NextPage();
                if (pagedPanel.IsOnLastPage)
                    btnNextStep.Text = "Finish";
            }
        }

        private void OnWizardComplete()
        {
            MessageBox.Show("You have completed the setup for TwometerVR and can now start playing your favourite VR games!", "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
