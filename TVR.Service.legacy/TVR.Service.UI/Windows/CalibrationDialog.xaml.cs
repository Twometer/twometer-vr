using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TVR.Service.UI.Windows
{
    /// <summary>
    /// Interaction logic for CalibrationDialog.xaml
    /// </summary>
    public partial class CalibrationDialog : Window
    {
        public CalibrationDialog()
        {
            InitializeComponent();
        }

        public void UpdateState(string title, string message)
        {
            CaptionLabel.Content = title;
            ContentLabel.Text = message;
        }
    }
}
