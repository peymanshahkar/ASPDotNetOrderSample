<%@ Page Title="صفحه اصلی" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="TabletOrder._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <!-- دکمه بالای صفحه -->
            <div class="d-flex justify-content-start align-items-center" style="text-align: right; margin: 20px;">
                <button type="button" class="btn btn-secondary align-items-center d-flex" onclick="showInvoicesModal()" style="height:42px;">
                    <span>میزهای باز</span>
                    <asp:Literal ID="LiteralOpenInvoices" runat="server" />
                    <img class="border-0" src="Content/Images/Documents-icon.png" width="24" height="24" />
                </button>
               <asp:ImageButton CssClass="btn btn-secondary ms-2" AlternateText="بروزرسانی" runat="server" ID="ImgRefresh" 
                   ImageUrl="~/Content/Images/refresh-icon.png" style="height: 42px;" OnClick="ImgRefresh_Click"/>
            </div>

            <!-- دکمه میانه صفحه -->
            <div style="text-align: center; margin-top: 100px;">
                <a href="Pages/InvoiceAdd.aspx">
                    <button type="button" id="ButtonCreateInvoice" class="btn btn-primary">
                        <span>سفارش جدید</span>
                        <img class="border-0" src="Content/Images/receipt-invoice-icon.png" />
                    </button>
                </a>
            </div>





            <!-- مودال فاکتورها -->
            <div class="modal fade" id="invoicesModal" tabindex="-1" role="dialog" aria-labelledby="invoicesModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="invoicesModalLabel">لیست میزهای باز</h5>
                            <button type="button" id="hClose" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <!-- محتوای لیست فاکتورها با Repeater -->
                            <div class="repeater-container">
                                <asp:Repeater ID="InvoicesRepeater" runat="server" >
                                    <ItemTemplate>
                                        <div class="invoice-item">
                                            <h5>شماره فاکتور: <%# Eval("FactNumber") %></h5>
                                            <p id="chair">
                                                <i class="fa fa-coffee"></i>
                                                <%# Eval("Miz", "{0:N0}") %> 
                                            </p>
                                            <p>تاریخ: <%# Eval("InvoiceDate", "{0:yyyy/MM/dd}") %></p>
                                            <p>مبلغ: <%# Eval("InvoiceAmount", "{0:N0}") %> </p>

                                            <a href="Pages/InvoiceEdit?InvoiceID=<%# Eval("SaleHID") %>">
                                                <button type="button" class="btn btn-info">
                                                    <span>ویرایش</span>
                                                    <i class="fa fa-edit"></i>
                                                </button>
                                            </a>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" id="fClose" class="btn btn-secondary" data-dismiss="modal">بستن</button>
                        </div>
                    </div>
                </div>
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
                    <img src="Content/Images/loading.gif" title="لطفا کمی صبر کنید..." />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <!-- اسکریپت برای نمایش مودال -->
    <script type="text/javascript">
        function showInvoicesModal() {
            $('#invoicesModal').modal('show');
        }
    </script>


    <!------- بستن مودال  -->
    <script>
        $(document).ready(function () {
            $('#hClose').on('click', function () {
                $('#invoicesModal').modal('hide');
            });

            $('#fClose').on('click', function () {
                $('#invoicesModal').modal('hide');
            });
        });

    </script>


</asp:Content>
