<%@ Page Title="ویرایش سفارش" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InvoiceEdit.aspx.cs" Inherits="TabletOrder.Pages.InvoiceEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <link href="../Content/Menu.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Block">
        <ContentTemplate>
            <div class="container-sm">
                <div class="card mb-3">
                    <div class="card-body">
                        <div class="row input-panel">
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-md-4 col-form-label"><i class="fa fa-coffee"></i>شماره میز</label>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="drpMiz" runat="server" CssClass="form-control " AutoPostBack="True" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-md-4 col-form-label" for="searchInput">جستجو:</label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="searchInput" CssClass="form-control" runat="server" 
                                            OnTextChanged="searchInput_TextChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-md-4 col-form-label" for="foodCategory">گروه غذا:</label>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ProductGroupDropDown" runat="server" CssClass="form-control "
                                            OnSelectedIndexChanged="ProductGroupDropDown_SelectedIndexChanged" AutoPostBack="true"
                                            DataValueField="GroupID" DataTextField="GroupName">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


              <asp:Panel ID="pnlRepeater" class="food-list" runat="server">
                <asp:Repeater ID="FoodRepeater" runat="server">
                    <ItemTemplate>
                        <div class=" <%# Eval("Qty").ToString()!="0" ? "form-group row product-row food-item highlight":"form-group row product-row food-item" %>">
                            <div id="productname" class="col-md-6">
                                <%# Eval("ProductName") %>
                            </div>
                            <div class="col-md-2 quantity-controls">
                                <asp:Button ID="btnAdd" runat="server" Text="+" CommandArgument='<%# Eval("ProductId") %>'
                                    OnCommand="btnAdd_Command" CssClass="btn btn-success" OnClientClick="saveScrollPosition();" />
                                <label style="text-align: center;"><span class="qty align-content-center"><%#Eval("Qty") %> </span></label>
                                <asp:Button ID="btnRemove" runat="server" Text="_" CommandArgument='<%# Eval("ProductId") %>'
                                    OnCommand="btnRemove_Command" CssClass="btn btn-danger" OnClientClick="saveScrollPosition();" />
                            </div>
                            <div class="col-md-2">
                                <span>قیمت: <%# Eval("ProductPrice","{0:N0}") %></span>
                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
            <div class="form-group">
                <label>مبلغ سفارش</label>
                <asp:TextBox ID="txtAmount" CssClass="form-control" BackColor="YellowGreen"
                    ReadOnly="true" runat="server" />
                <label>مالیات</label>
                <asp:TextBox ID="TxtTax" CssClass="form-control" BackColor="YellowGreen"
                    ReadOnly="true" runat="server" />
                <label>مبلغ کل</label>
                <asp:TextBox ID="txtNetAmount" CssClass="form-control" BackColor="YellowGreen"
                    ReadOnly="true" runat="server" />
            </div>
            <div class="form-group text-center">
                <asp:Button ID="SubmitButton" runat="server" Text="ثبت سفارش"
                    OnClick="SubmitButton_Click" CssClass="btn btn-primary mt-3 btn-lg" /> 

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div class="overlay">
                <div class="spinner">
                    <div dir="rtl">
                        <span>لطفا کمی صبر کنید...</span>
                    </div>
                    <img src="../Content/Images/loading.gif" title="لطفا کمی صبر کنید..." />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <script>  
        var scrollPosition = 0;

        function saveScrollPosition() {
            var element = document.getElementById('<%= pnlRepeater.ClientID %>');
            scrollPosition = element.scrollTop; // ذخیره موقعیت اسکرول  
        }

        function restoreScrollPosition() {
            var element = document.getElementById('<%= pnlRepeater.ClientID %>');
            element.scrollTop = scrollPosition; // بازگرداندن موقعیت اسکرول  
        }


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(restoreScrollPosition);
    </script>



</asp:Content>
