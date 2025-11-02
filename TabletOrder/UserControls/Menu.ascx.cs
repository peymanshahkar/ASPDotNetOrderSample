using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TabletOrder.Controls;
using TabletOrder;

namespace TabletOrder.UserControls
{
    public partial class Menu : Controls.UserControls
    {


        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsLoggedIn())
            {
                if (currentUserLabel.Text == "---")
                {

                    currentUserLabel.Text = ((LoginedData)Session["LoginData"]).UserName;

                }
                
            }

            DataBind();
        }
    }
}