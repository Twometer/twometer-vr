using System.Windows;

namespace TVR.Service.UI
{
    /// <summary>
    /// Interaction logic for CommonDialog.xaml
    /// </summary>
    public partial class CommonDialog : Window
    {
        public string Caption
        {
            set
            {
                CaptionLabel.Content = value;
            }
        }

        public string ContentText
        {
            set
            {
                ContentLabel.Text = value;
            }
        }

        public CommonDialog()
        {
            InitializeComponent();
        }

        public static void ShowError(Window owner, string caption, string error)
        {
            Show(owner, "TwometerVR Error", caption, error);
        }

        public static void Show(Window owner, string title, string caption, string info)
        {
            new CommonDialog
            {
                Title = title,
                Caption = caption,
                ContentText = info,
                Owner = owner
            }.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
