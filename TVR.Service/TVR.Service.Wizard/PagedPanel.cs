using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TVR.Service.Wizard.Pages;

namespace TVR.Service.Wizard
{
    public class PagedPanel : Control
    {
        public List<BasePage> Pages { get; } = new List<BasePage>();

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

        private void SetContent(BasePage page)
        {
            if (Controls.Count > 0 && Controls[0] is BasePage oldPage)
                oldPage.OnNavigatedAway();

            Controls.Clear();
            page.Tag = Tag;
            page.Dock = DockStyle.Fill;
            Controls.Add(page);
            page.OnNavigatedTo();
        }
    }
}
