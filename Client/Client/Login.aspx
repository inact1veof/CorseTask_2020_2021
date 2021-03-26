<%@ Page Language="C#" AutoEventWireup="true" Async ="true" CodeBehind="Login.aspx.cs" Inherits="Client.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Вход в личный кабинет</title>
    <link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="site.css" />
    
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
    <div class="container">
        <a>Вход в ЛК</a>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
    </div>
</nav>
</head>
<body>
<div class="text-center" runat ="server">
    <h1 class="display-4">Личный кабинет</h1>
    <p>Необходимо ввести свой логин и пароль, далее нажмите кнопку "Вход"</p>
    <form runat ="server">       
        <label id="loginLabel">Логин: </label>
        <asp:TextBox ID ="TextBox1" runat ="server" Width ="150"></asp:TextBox><br />
        <label id="PasswordLabel">Пароль:</label>
        <asp:TextBox ID ="TextBox2" runat ="server" Width ="140" TextMode="Password"></asp:TextBox><br />
        <asp:Button ID ="Button1" runat ="server" Text ="Вход" OnClick ="Button1_Click" />
    </form>
</div>
</body>
</html>
