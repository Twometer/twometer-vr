using Microsoft.WindowsAPICodePack.Dialogs;
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
            context.UIStateChanged += Context_UIStateChanged;
        }

        private void Context_UIStateChanged(object sender, EventArgs e)
        {
            btnNextStep.Text = context.NextButtonText;
            btnNextStep.Enabled = context.AllowNext;
            btnPrevious.Enabled = context.AllowPrevious;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var dialog = new TaskDialog
            {
                Icon = TaskDialogStandardIcon.Error,
                Caption = "TwometerVR Setup",
                InstructionText = "Cancel setup?",
                Text = "Progress made during the setup will be lost.",
                StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No
            };
            if (dialog.Show() == TaskDialogResult.Yes)
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
            var dialog = new TaskDialog
            {
                Icon = TaskDialogStandardIcon.Information,
                Caption = "TwometerVR Setup",
                InstructionText = "Congratulations!",
                Text = "You have completed the setup for TwometerVR and can now start playing your favourite VR games!",
                StandardButtons = TaskDialogStandardButtons.Ok
            };
            dialog.Show();
            Close();
        }
    }
}
