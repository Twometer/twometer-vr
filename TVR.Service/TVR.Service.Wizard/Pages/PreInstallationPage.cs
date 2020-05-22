using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;

namespace TVR.Service.Wizard.Pages
{
    public partial class PreInstallationPage : TVR.Service.Wizard.Pages.BasePage
    {
        public PreInstallationPage()
        {
            InitializeComponent();
            NavigatedAway += PreInstallationPage_NavigatedAway;
            NavigatedTo += PreInstallationPage_NavigatedTo;

            tbPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "TwometerVR");
        }

        private void PreInstallationPage_NavigatedTo(object sender, EventArgs e)
        {
            Context.NextButtonText = "Install";
        }

        private void PreInstallationPage_NavigatedAway(object sender, EventArgs e)
        {
            Context.InstallationPath = tbPath.Text;
            Context.CreateStartMenuEntry = cbStartMenu.Checked;
        }

        private void btnChooser_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog() { IsFolderPicker = true };
            dialog.InitialDirectory = Directory.Exists(tbPath.Text) ? tbPath.Text : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                tbPath.Text = dialog.FileName;
        }
    }
}
