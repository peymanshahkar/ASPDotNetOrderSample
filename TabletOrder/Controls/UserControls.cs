using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace TabletOrder.Controls
{
    public class UserControls: System.Web.UI.UserControl
    {
        public new TabletOrder.Controls.BasePage Page
        {
            get
            {
                return (BasePage)base.Page;
            }
            set
            {
                base.Page = value;
            }
        }

        public bool IsLoggedIn()
        {
            return Page.IsLoggedIn();
        }
    }
}