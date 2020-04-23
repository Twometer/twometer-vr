using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVR.Service.Wizard
{
    public class PagedPanel : Control
    {
        public List<Control> Pages { get; } = new List<Control>();

        public bool IsOnLastPage => currentPage >= Pages.Count - 1;

        public bool IsOnFirstPage => currentPage == 0;

        private int currentPage = 0;


        protected override void CreateHandle()
        {
            base.CreateHandle();
            if (Pages.Count > 0)
                SetContent(Pages[0]);
        }

        public void NextPage()
        {
            if (currentPage < Pages.Count - 1)
            {
                currentPage++;
                SetContent(Pages[currentPage]);
            }
        }

        public void PreviousPage()
        {
            if (currentPage > 0)
            {
                currentPage--;
                SetContent(Pages[currentPage]);
            }
        }

        private void SetContent(Control control)
        {
            Debug.WriteLine($"Showing page '{control.GetType().Name}'");
            Controls.Clear();
            control.Tag = Tag;
            control.Dock = DockStyle.Fill;
            Controls.Add(control);
        }
    }
}
