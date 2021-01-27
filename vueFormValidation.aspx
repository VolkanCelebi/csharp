<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="vueFormValidation.aspx.cs" Inherits="WebApplication1.vueFormValidation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div id="app">
    <form id="form" runat="server" autocomplete="off">
        <div class="container">
            <h2>Form Doğrulama</h2>
            <div class="form-group">
                <label for="adsoyad">Ad Soyad</label>
                <asp:TextBox ID="adsoyad" runat="server" CssClass="form-control" placeholder="ad soyad"></asp:TextBox>
                
            </div>
            
            <div class="form-group">
                <label for="yas">Yaş</label>
                <asp:TextBox ID="yas" runat="server" CssClass="form-control" placeholder="yaş"></asp:TextBox>
            </div>
         
            
            <asp:Button ID="btnGiris" runat="server" Text="Giriş Yap" CssClass="btn btn-danger" />
        </div>
    </form>
    </div>
</body>
</html>
