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

namespace TVR.Service.UI
{
    /// <summary>
    /// Interaction logic for NewCameraDialog.xaml
    /// </summary>
    public partial class NewCameraDialog : Window
    {
        public string NewCameraIdentifier { get; private set; }

        public NewCameraDialog()
        {
            InitializeComponent();
        }
    }
}
