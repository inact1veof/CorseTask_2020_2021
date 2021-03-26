<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsersList.aspx.cs" Inherits="Client.UsersList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Список сотрудников</title>
    <link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="site.css" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
    <div class="container">
        <a>Ваш личный кабинет</a>
        <asp:Button ID="Button1" runat="server" Text="Главная" OnClick ="Button1_Click" /> 
        <asp:Button ID="Button2" runat="server" Text="Список сотрудников"/> 
        <asp:Button ID="Button8" runat="server" Text="Нагрузки и приборы" OnClick ="Button8_Click"/> 
        <asp:Button ID="Button4" runat="server" Text="Прогнозы" OnClick ="Button4_Click"/>
        <asp:Button ID="Button3" runat="server" Text="Выход" OnClick ="Button3_Click"/> 
    </div>
    </nav>
        <div>
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width ="80%">
                        <h1 style ="margin-left: 20px">Список всех сотрудников:</h1>
                        <asp:GridView ID="GridView1" runat="server" Width ="80%" style ="margin-left: 20px" Font-Size ="16"> </asp:GridView>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </form>
</body>
</html>
