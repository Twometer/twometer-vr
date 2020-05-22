using System;
using System.Windows.Forms;
using TVR.Service.Wizard.Model;

namespace TVR.Service.Wizard.Pages
{
    public class BasePage : UserControl
    {
        protected WizardContext Context => (WizardContext)Tag;

        public event EventHandler NavigatedTo;

        public event EventHandler NavigatedAway;

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

        public void OnNavigatedTo()
        {
            NavigatedTo?.Invoke(this, new EventArgs());
        }

        public void OnNavigatedAway()
        {
            NavigatedAway?.Invoke(this, new EventArgs());
        }

    }
}
