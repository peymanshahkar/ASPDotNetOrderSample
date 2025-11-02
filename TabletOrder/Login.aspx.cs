using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using TabletOrder;

namespace TabletOrder
{
    public partial class Login : Controls.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            _Repository = new Data.DataRepository( _loginData);


        }
      //  protected LoginedData _loginData;
        private static Data.DataRepository _Repository;




     

        protected void submit_Click(object sender, EventArgs e)
        {
            try
            {
                    
                if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Text))
                {
                    lblErrorMessage.Text = "نام کاربری و رمز عبور را وارد کنید";
                    ShowPopUp(lblErrorMessage.Text,"خطا");
                    return;
                }
                List<Data.Parameter> ParamS = new List<Data.Parameter>
                {
                    new Data.Parameter
                    {
                        ParameterName="UserName",
                        ParameterValue=username.Text
                    },
                    new Data.Parameter
                    {
                        ParameterName="PassWord",
                        ParameterValue=password.Text
                    }
                };


                var result = _Repository.Login(ParamS);
                if (result != null)
                {
                    _loginData = new LoginedData
                    {
                        IsAuthenticated = true,
                        UserId = result.UserId,
                        UserName = result.UserName
                    };
                    Session.Add("LoginData", _loginData);
                    FormsAuthentication.RedirectFromLoginPage(result.UserName, false);

                    //Response.Redirect("");
                }
                else
                {
                    lblErrorMessage.Text = "نام کاربری یا رمز عبور اشتباه است";
                    ShowPopUp(lblErrorMessage.Text, "خطا");
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                ShowPopUp(ex.Message, "خطا");
            }

        }
    }
}