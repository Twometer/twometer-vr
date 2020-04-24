using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TVR.Service.Wizard.Model;

namespace TVR.Service.Wizard.Pages
{
    public class BasePage : UserControl
    {
        protected WizardContext Context => (WizardContext)Tag;


    }
}
