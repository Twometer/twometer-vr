using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TVR.Service.Wizard.Pages
{
    public partial class CalibrationPage : TVR.Service.Wizard.Pages.BasePage
    {
        public CalibrationPage()
        {
            InitializeComponent();
        }

        private void btnAutoDetect_Click(object sender, EventArgs e)
        {
            using (var dialog = new TaskDialog())
            {
                var pb = new TaskDialogProgressBar();
                dialog.InstructionText = "Auto-detect in progress";
                dialog.Text = "Please wait while we are detecting the optimal settings for your webcam setup.";
                dialog.Caption = "TwometerVR Setup";
                dialog.DetailsExpandedText = "Initializing...";
                dialog.StandardButtons = TaskDialogStandardButtons.Cancel;
                dialog.OwnerWindowHandle = Handle;
                dialog.ProgressBar = pb;
                dialog.Show();

                // TODO auto-detect here
            }
        }
    }
}
