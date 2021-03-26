<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminUsersList.aspx.cs" Inherits="Client.AdminUsersList" %>

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
        <asp:Button ID="Button1" runat="server" Text="Главная" OnClick ="Button1_Click"/> 
        <asp:Button ID="Button2" runat="server" Text="Список сотрудников"/> 
        <asp:Button ID="Button10" runat="server" Text="Нагрузки и приборы" OnClick ="Button10_Click"/> 
        <asp:Button ID="Button4" runat="server" Text="Прогнозы" OnClick ="Button4_Click"/>
        <asp:Button ID="Button3" runat="server" Text="Выход" OnClick ="Button3_Click"/> 
    </div>
    </nav>
        <div>
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell style ="vertical-align:top">
                        <h1 style ="margin-left: 20px">Список всех сотрудников:</h1>
                        <asp:GridView ID="GridView1" runat="server" style ="margin-left: 20px; text-align:center;" Font-Size ="16"> 
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                    </asp:TableCell>
                    <asp:TableCell style ="vertical-align:top">
                        <h1>Должности</h1>
                        <asp:GridView ID="GridView2" runat="server" Width ="80%" style ="text-align:center;" Font-Size ="16"> 
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                        <h1>Отделы</h1>
                        <asp:GridView ID="GridView3" runat="server" Width ="80%" style ="text-align:center;" Font-Size ="16"> 
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                    </asp:TableCell>
                    <asp:TableCell style ="vertical-align:top">
                        <h1 style ="text-align:left">Действия</h1><br /><br />
                        <asp:Button ID="Button5" runat="server" Text="Добавить нового сотрудника" Width ="250" OnClick ="Button5_Click"/> <br /><br />
                        <asp:Button ID="Button6" runat="server" Text="Изменить данные сотрудника" Width ="250" OnClick ="Button6_Click"/> <br /><br />
                        <asp:Button ID="Button7" runat="server" Text="Удалить сотрудника" Width ="250" OnClick ="Button7_Click"/> <br /><br />
                        <asp:Button ID="Button11" runat="server" Text="Добавить новую должность" Width ="250" OnClick ="Button11_Click"/> <br /><br />
                        <asp:Button ID="Button12" runat="server" Text="Удалить должность" Width ="250" OnClick ="Button12_Click"/> <br /><br />
                        <asp:Button ID="Button13" runat="server" Text="Добавить новый отдел" Width ="250" OnClick ="Button13_Click"/> <br /><br />
                        <asp:Button ID="Button14" runat="server" Text="Удалить отдел" Width ="250" OnClick ="Button14_Click"/> <br /><br />
                        <asp:Label ID="Label1" runat="server" Text="Введите код: " Visible ="false"></asp:Label><br /><br />
                        <asp:TextBox ID="TextBox1" runat="server" Width ="60" Visible ="false"></asp:TextBox>
                        <asp:Button ID="Button8" runat="server" Text="Отмена"  Width ="80" Visible ="false" OnClick ="Button8_Click"/>
                        <asp:Button ID="Button9" runat="server" Text="Принять" Width ="80" Visible ="false" OnClick ="Button9_Click1"/><br /><br />
                        <asp:Label ID="Label2" runat="server" Visible ="true" Width ="240"></asp:Label><br /><br />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </form>
</body>
</html>
