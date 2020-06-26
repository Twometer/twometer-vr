using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using TVR.Service.Core.IO;
using TVR.Service.Core.Model.Config;
using TVR.Service.UI.Config;

namespace TVR.Service.UI.Windows
{
    /// <summary>
    /// Interaction logic for ConfigDialog.xaml
    /// </summary>
    public partial class ConfigDialog : Window
    {
        public bool RequiresRestart { get; private set; }

        private readonly Dictionary<string, Page> pages = new Dictionary<string, Page>() {
            { "general", new GeneralPage() },
            { "hardware", new HardwarePage() },
            { "input", new InputPage() }
        };
        private readonly UserConfig userConfig;

        public ConfigDialog(UserConfig userConfig)
        {
            InitializeComponent();
            this.userConfig = userConfig;

            foreach (var file in FileManager.Instance.ProfilesFolder.EnumerateFiles())
            {
                var profile = CameraProfileIO.LoadCameraProfile(file);
                CamerasItem.Items.Add(new TreeViewItem { Header = profile.Model, Tag = profile });
            }

            // Show start page
            ShowPage("general");
            ((TreeViewItem)ConfigTree.Items[0]).IsSelected = true;
        }

        private void ConfigTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem item)
                ShowPage(item.Tag);
        }

        private void ShowPage(object tag)
        {
            if (tag is string str && pages.ContainsKey(str))
                ConfigContent.Content = pages[str];
        }

        private void OpenConfigFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(FileManager.Instance.ConfigFile.DirectoryName);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
