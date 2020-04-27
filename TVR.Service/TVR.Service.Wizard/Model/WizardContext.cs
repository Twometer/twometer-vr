using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Wizard.Model
{
    public class WizardContext
    {
        // Context for storing infomation about current wizard state
        public string InstallationPath { get; set; }

        public bool CreateStartMenuEntry { get; set; }

        private bool _allowNext = true;
        public bool AllowNext
        {
            set
            {
                _allowNext = value;
                UIStateChanged?.Invoke(this, new EventArgs());
            }
            get
            {
                return _allowNext;
            }
        }

        private bool _allowPrev = true;
        public bool AllowPrevious
        {
            set
            {
                _allowPrev = value;
                UIStateChanged?.Invoke(this, new EventArgs());
            }
            get
            {
                return _allowPrev;
            }
        }

        private string _nextButtonText = "Next";
        public string NextButtonText
        {
            set
            {
                _nextButtonText = value;
                UIStateChanged?.Invoke(this, new EventArgs());
            }
            get
            {
                return _nextButtonText;
            }
        }

        public event EventHandler UIStateChanged;


    }
}
