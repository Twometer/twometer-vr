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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BasePage
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Name = "BasePage";
            this.Size = new System.Drawing.Size(510, 310);
            this.ResumeLayout(false);

        }
    }
}
