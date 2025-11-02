using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace TabletOrder.Controls
{
    public class BasePage: System.Web.UI.Page
    {
        protected LoginedData _loginData { get; set; }

        public BasePage()
        {

        }
        public void ShowPopUp(string errorMessage, string title)
        {

            ScriptManager.RegisterStartupScript
                (this, GetType(), "showError", $"showError('{errorMessage}','{title}');", true);
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (!User.Identity.IsAuthenticated)
            {
                _loginData = new LoginedData();

            }
            else
            {
                if (Session.Count > 0)
                {
                    _loginData = (LoginedData)Session["LoginData"];
                }
                else
                {
                    var _repository = new Data.DataRepository(new LoginedData());
                    var data= _repository.GetUserByName(User.Identity.Name);
                    _loginData = new LoginedData
                    {
                        IsAuthenticated = true ,
                        UserName= User.Identity.Name,
                        UserId=data.UserId
                    };

                    Session.Add("LoginData", _loginData);
                }
            }
        }

        public bool IsLoggedIn()
        {
            return User.Identity.IsAuthenticated;
        }
    }
}