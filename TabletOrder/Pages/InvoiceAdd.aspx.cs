using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TabletOrder.Data;
using TabletOrder.Entity;
using Extention;
using static System.Net.Mime.MediaTypeNames;

namespace TabletOrder.Pages
{
    public partial class InvoiceAdd : Controls.BasePage
    {
        DataRepository _repository;
        decimal Amount = 0;
        decimal NetAmount = 0;
        decimal Tax = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            _repository = new DataRepository(_loginData);


            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx");
            }

          

            if (!Page.IsPostBack)
            {
                BindMizDataSource();
                BindProductGroups();
                LoadDate();
                BindFoodData();
            }
            
        }
       
        private void BindMizDataSource()
        {
           var tableS= _repository.GetUnusedTables(Server.MapPath("~/Data/Tables.xml"));
           drpMiz.DataSource = tableS;
            drpMiz.DataValueField = "TableId";
            drpMiz.DataTextField = "TableName";
            drpMiz.DataBind();

        }
        private void BindProductGroups()
        {
            var ProductGroups = _repository.GetProductGroups();
            ProductGroupDropDown.DataSource = ProductGroups;
            ProductGroupDropDown.DataBind();
        }
        protected void ProductGroupDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? productGroupId = null;
            if (ProductGroupDropDown.SelectedIndex > 0)
            {
                productGroupId = Convert.ToInt32(ProductGroupDropDown.SelectedValue);
            }
            BindFoodData(searchInput.Text, productGroupId);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
           SaleHeaderDto Saleheader= new SaleHeaderDto();
            try
            {
                Saleheader.Dt = DateTime.Now.ToPersianDate();
                Saleheader.Miz = drpMiz.Text.ToString();
                Saleheader.SaleDetaileS = new List<SaleDetailDto>();
                Saleheader.Tax = Tax;
              
                var selectedProducts = ((List<ProductDto>)ViewState["FoodList"]).
                                        Where(x => x.Qty > 0).ToList();
                if (selectedProducts.Count > 0)
                {
                    foreach (var product in selectedProducts)
                    {
                        Saleheader.SaleDetaileS.Add(new SaleDetailDto
                        {
                            PartID = product.ProductId,
                            Qty = product.Qty,
                            FeePrice = product.ProductPrice,
                            SumPrice = product.NetAmount,
                            IsDel = false,
                            Prnt = false,
                            NewValue = 0
                        });
                    }
                    Saleheader.Calc();
                    _repository.SaveSale(Saleheader);

                    ShowPopUp("سفارش با موفقیت ثبت شد", "اطلاعات");

                }
                else
                {
                    ShowPopUp("برای ثبت سفارش حداقل باید یک کالا انتخاب شود", "خطا");
                }
            }
            catch (Exception ex)
            {
                ShowPopUp(ex.Message, "خطا");
            }
            
        }
        private void LoadDate()
        {
            var foodList = _repository.GetAllProducts();

            ViewState["FoodList"] = foodList;
        }


        private void BindFoodData(string searchTerm = "", int? productGroupId=null) 
        { 
            var filteredList = (List<ProductDto>)ViewState["FoodList"];
            if (productGroupId!=null)
            {
                filteredList = filteredList.Where(f => f.ProductGroupId == (int)productGroupId).ToList();
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            { 
                filteredList = filteredList.Where(f => f.ProductName.Contains(searchTerm)).ToList();
            }
            
            FoodRepeater.DataSource = filteredList;
            FoodRepeater.DataBind();
            
        }

       
        protected void searchInput_TextChanged(object sender, EventArgs e)
        {
            int? productGroupId = null;
            if (ProductGroupDropDown.SelectedIndex > 0)
            {
                productGroupId = Convert.ToInt32(ProductGroupDropDown.SelectedValue);
            }
            BindFoodData(searchInput.Text,productGroupId);
        }
       
        protected void btnAdd_Command(object sender, CommandEventArgs e)
        {
            var productId = Convert.ToInt32(e.CommandArgument);
            var product = ((List<ProductDto>)ViewState["FoodList"]).FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                product.Qty++;
                //FoodRepeater.DataBind();
                ProductGroupDropDown_SelectedIndexChanged(null, null);
                Calc();
            }
        }
        private void Calc()
        {

            decimal _tax = 0;

            Tax = 0;
            Amount = 0;

            var selectedProducts = ((List<ProductDto>)ViewState["FoodList"]).Where(p => p.Qty > 0);
            foreach (var item in selectedProducts)
            {
                Amount += item.NetAmount;
                if (item.ProductGroup.IsHaveTax)
                {
                    _tax = (StaticValues.TaxPercent / 100) * item.NetAmount;
                    Tax += _tax;
                }
            }

            NetAmount = Amount + Tax;
           
            TxtTax.Text = Tax.ToString("N0");
            txtAmount.Text = Amount.ToString("N0");
            txtNetAmount.Text=NetAmount.ToString("N0");
        }
        protected void btnRemove_Command(object sender, CommandEventArgs e)
        {
            var productId = Convert.ToInt32(e.CommandArgument);
            var product = ((List<ProductDto>)ViewState["FoodList"]).FirstOrDefault(p => p.ProductId == productId);
            if (product != null && product.Qty > 0)
            {
                product.Qty--;
                ProductGroupDropDown_SelectedIndexChanged(null, null);
                Calc();
            }
        }
    }
}