<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="TabletOrder.UserControls.Menu" %>

<nav class="navbar">
    <ul runat="server" visible="<%# !base.IsLoggedIn() %>">
        <li><a href="../Login">ورود به سیستم</a></li>
        <li><a href="../About">درباره ما</a></li>
        <li><a href="../LogOut">خروج</a></li>
    </ul>

    <ul runat="server" visible="<%#base.IsLoggedIn() %>">
        <li><a href="../Pages/InvoiceAdd">ثبت سفارش</a></li>
        <li><a href="../About">درباره ما</a></li>


        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/Logout" CssClass="nav-link menu-link">
                        <asp:Image runat="server" ImageUrl="~/Content/images/menu-logout.png" Height="16" />
                        خروج
            </asp:HyperLink>
        </li>
        <li>
            <a class="nav-link" href="#" id="currentUserMenu" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                <asp:Image runat="server" ImageUrl="~/Content/Images/user32.png" Height="16" />
                <asp:Label ID="currentUserLabel" runat="server" Text="---"></asp:Label>
            </a>
        </li>
    </ul>


   
</nav>
