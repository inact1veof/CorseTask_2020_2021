<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="Client.UserPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Личный кабинет</title>
    <link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="site.css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
    <div class="container">
        <a>Ваш личный кабинет</a>
        <asp:Button ID="Button1" runat="server" Text="Главная"/> 
        <asp:Button ID="Button2" runat="server" Text="Список сотрудников" OnClick ="Button2_Click" /> 
        <asp:Button ID="Button8" runat="server" Text="Нагрузки и приборы" OnClick ="Button8_Click"/> 
        <asp:Button ID="Button4" runat="server" Text="Прогнозы" OnClick ="Button4_Click"/>
        <asp:Button ID="Button3" runat="server" Text="Выход" OnClick="Button1_Click" /> 
    </div>
    </nav>
        <div>
            <asp:Label ID="Label2" runat="server" Text="Label" Font-Size ="16" style ="margin-left: 20px" > Ваш логин: </asp:Label>
            <asp:Label ID="Label1" runat="server" Text="Label" Font-Size ="16" ForeColor ="Green"></asp:Label><br />

            <asp:Table ID="Table1" runat="server" Width ="600" Height ="400" style ="margin-left: 20px">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>
                        <asp:Label ID="Label10" runat="server" Text="Label" Font-Size ="16" >Ваши данные</asp:Label>
                    </asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label3" runat="server" Text="Label" Font-Size ="14" > Имя: </asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label9" runat="server" Text="Label" Font-Size ="14" ></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="TextBox1" runat="server" Visible ="false"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label4" runat="server" Text="Label" Font-Size ="14" > Фамилия: </asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label11" runat="server" Text="Label" Font-Size ="14" ></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="TextBox2" runat="server" Visible ="false"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>               
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label8" runat="server" Text="Label" Font-Size ="14" > Отдел: </asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label13" runat="server" Text="Label" Font-Size ="14" ></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label5" runat="server" Text="Label" Font-Size ="14" > Должность: </asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label14" runat="server" Text="Label" Font-Size ="14" ></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label6" runat="server" Text="Label" Font-Size ="14" > Дата рождения: </asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label15" runat="server" Text="Label" Font-Size ="14" ></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="Label7" runat="server" Text="Label" Font-Size ="14" > Компания: </asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="Label12" runat="server" Text="Label" Font-Size ="14" ></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="Button5" runat="server" Text="Редактировать данные" OnClick ="Button5_Click"/>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="Button6" runat="server" Text="Принять изменения" Visible ="false" OnClick="Button6_Click"/>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="Button7" runat="server" Text="Отмена" Visible ="false" OnClick="Button7_Click"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>


        </div>
    </form>
</body>
</html>
