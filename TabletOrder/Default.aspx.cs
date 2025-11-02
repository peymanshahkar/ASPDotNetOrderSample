using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TabletOrder.Data;

namespace TabletOrder
{
    public partial class _default : Controls.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
            }

            _dataRepository = new DataRepository(_loginData);
            if (!Page.IsPostBack)
            {
                BindInvoiceData();
            }

            LiteralOpenInvoices.Text = CountOfOpenInvoices();

            // InvoicesRepeater.PreRender += InvoicesRepeater_PreRender;

           
        }

        DataRepository _dataRepository;


        private int _OpenInvoiceCount = 0;

        protected string CountOfOpenInvoices()
        {
            return "("+ _OpenInvoiceCount.ToString()+")";
        }
        private void BindInvoiceData()
        {
            try
            {
                var invoices = _dataRepository.GetAllOpenInvoices();
                _OpenInvoiceCount = invoices.Count();
                InvoicesRepeater.DataSource = invoices;
                InvoicesRepeater.DataBind();
            }
            catch (Exception ex)
            {
                ShowPopUp(ex.Message, "خطا");
            }
        }
        protected void ButtonCreateFactor_Click(object sender, EventArgs e)
        {

        }

        protected void ImgRefresh_Click(object sender, ImageClickEventArgs e)
        {
            BindInvoiceData();
        }
    }
}