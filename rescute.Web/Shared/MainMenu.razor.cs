using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.Web.Shared
{
    public partial class MainMenu
    {
        private const string ActiveMenuItemClass = "active";
        public string ReportsActive { get; set; } = ActiveMenuItemClass;
        public string AboutActive { get; set; }
        public string CreditActive { get; set; }
        public string SignOutActive { get; set; }




        private void ReportsClicked(MouseEventArgs e)
        {
            ClearActiveMenuItem();
            ReportsActive = ActiveMenuItemClass;
        }
        private void AboutClicked(MouseEventArgs e)
        {
            ClearActiveMenuItem();
            AboutActive = ActiveMenuItemClass;
        }
        private void SignOutClicked(MouseEventArgs e)
        {
            ClearActiveMenuItem();
            SignOutActive = ActiveMenuItemClass;
        }

        private void CreditClicked(MouseEventArgs e)
        {
            ClearActiveMenuItem();
            CreditActive = ActiveMenuItemClass;
        }


        private void ClearActiveMenuItem()
        {
            ReportsActive = string.Empty;
            AboutActive = string.Empty;
            CreditActive = string.Empty;
            SignOutActive = string.Empty;
        }


    }
}
