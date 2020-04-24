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

        private bool _allowNext;
        public bool AllowNext
        {
            set
            {
                _allowNext = value;
                AllowedStateChanged?.Invoke(this, new EventArgs());
            }
            get
            {
                return _allowNext;
            }
        }

        private bool _allowPrev;
        public bool AllowPrevious
        {
            set
            {
                _allowPrev = value;
                AllowedStateChanged?.Invoke(this, new EventArgs());
            }
            get
            {
                return _allowPrev;
            }
        }

        public event EventHandler AllowedStateChanged;


    }
}
