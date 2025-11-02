<%@ Page Title="ورود به سیستم" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TabletOrder.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .Logincontainer {
            max-width: 400px;
            margin: auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }

        input[type="text"], input[type="password"] {
            width: 75%;
            padding: 10px;
            margin: 10px 0;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

        input[type="submit"] {
            background-color: #4CAF50;
            color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 5px;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <br />
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Logincontainer body">
        <asp:UpdatePanel ID="mainUpdatePanel" runat="server">

            <ContentTemplate>

                <h2>ورود به نرم افزار</h2>

                <div>
                    <div class="form-group">
                        <label for="username">نام کاربری:</label>
                        <asp:TextBox ID="username" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="password">رمز عبور:</label>
                        <asp:TextBox ID="password" runat="server" TextMode="Password"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Button ID="submit" runat="server" Text="ورود" OnClick="submit_Click" />
                    </div>
                    <asp:Label ID="lblErrorMessage" ForeColor="Red" runat="server" />
                </div>


            </ContentTemplate>
        </asp:UpdatePanel>


    </div>
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
</asp:Content>
