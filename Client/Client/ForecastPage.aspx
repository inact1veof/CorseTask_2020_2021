<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForecastPage.aspx.cs" Inherits="Client.ForecastPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Список прогнозов</title>
    <link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="site.css" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
    <div class="container">
        <a>Ваш личный кабинет</a>
        <asp:Button ID="Button1" runat="server" Text="Главная" OnClick ="Button1_Click"/> 
        <asp:Button ID="Button2" runat="server" Text="Список сотрудников" OnClick ="Button2_Click"/> 
        <asp:Button ID="Button5" runat="server" Text="Нагрузки и приборы" OnClick ="Button5_Click"/> 
        <asp:Button ID="Button4" runat="server" Text="Прогнозы" OnClick ="Button4_Click"/>
        <asp:Button ID="Button3" runat="server" Text="Выход" OnClick ="Button3_Click"/> 
    </div>
            </nav>
    <div>
        <asp:Table ID="Table1" runat="server" Width="100%">
            <asp:TableRow>
                <asp:TableCell style ="vertical-align:top">
                    <h1 style ="margin-left: 20px">Список выполненных прогнозов</h1>
                         <asp:GridView ID="GridView1" runat="server" style ="margin-left: 20px; text-align:center; width:50%" Font-Size ="16"> 
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                </asp:TableCell>
                <asp:TableCell style ="vertical-align:top;text-align:center">
                    <h1 style ="margin-left: 20px">Сооздать прогноз</h1>
                    <asp:Label ID="Label2" runat="server" Text="Номер прибора учета: " Visible ="true"></asp:Label>
                    <asp:TextBox ID="TextBox1" runat="server" Width ="60" Visible ="true"></asp:TextBox> <br /><br />
                    <asp:Label ID="Label3" runat="server" Text="Дата для прогноза: " Visible ="true"></asp:Label>
                    <asp:TextBox ID ="TextBox2" runat ="server" Width ="150" TextMode ="Date"></asp:TextBox><br /><br />
                    <asp:Label ID="Label4" runat="server" Text="Время для прогноза: " Visible ="true"></asp:Label>
                    <asp:TextBox ID ="TextBox3" runat ="server" Width ="150" TextMode ="Time"></asp:TextBox><br /><br />
                    <asp:Button ID="Button6" runat="server" Text="Создать прогноз" Width ="250" OnClick ="Button6_Click"/><br /><br />
                    <asp:Label ID="Label5" runat="server" Text="Ответ сокета" Visible ="false"></asp:Label><br /><br />
                    <asp:Button ID="Button8" runat="server" Text="Просмотр графика" Width ="250" OnClick ="Button8_Click"/><br /><br />
                </asp:TableCell>
                <asp:TableCell style ="vertical-align:top">
                    <asp:Label ID="Label1" runat="server" Text="Управление планом" Visible ="true"></asp:Label><br /><br /> 
                    <asp:Button ID="Button12" runat="server" Text="Свернуть/Развернуть план" Width ="250" OnClick ="Button12_Click"/><br /><br />
                    <asp:GridView ID="GridView2" runat="server" style ="margin-left: 20px; text-align:center; width:20%" Font-Size ="16"> 
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                    <asp:Button ID="Button9" runat="server" Text="Загрузить план" Width ="250" OnClick ="Button9_Click"/><br /><br />
                    <asp:Button ID="Button7" runat="server" Text="Добавить план" Width ="250" OnClick ="Button7_Click"/><br /><br />
                    <asp:Button ID="Button13" runat="server" Text="Удалить план" Width ="250" OnClick ="Button13_Click"/><br /><br />
                    <asp:Button ID="Button14" runat="server" Text="Изменить план" Width ="250" OnClick ="Button14_Click"/><br /><br />
                    <asp:Label ID="Label10" runat="server" Text="Номер прибора учета: " Visible ="false"></asp:Label>
                    <asp:TextBox ID="TextBox4" runat="server" Width ="60" Visible ="false"></asp:TextBox> <br /><br />
                    <asp:Label ID="Label6" runat="server" Text="Алгоритм прогноза: " Visible ="false"></asp:Label>
                    <asp:DropDownList ID="DropDownList1" runat="server" Visible ="false"></asp:DropDownList><br /><br />
                    <asp:Label ID="Label7" runat="server" Text="Дата: " Visible ="false"></asp:Label>
                    <asp:TextBox ID="TextBox5" runat="server" Width ="150" Visible ="false" TextMode ="Date"></asp:TextBox> <br /><br />
                    <asp:Button ID="Button10" runat="server" Text="Отмена" Width ="100" Visible ="false" OnClick ="Button10_Click"/>
                    <asp:Button ID="Button11" runat="server" Text="Принять" Width ="100" Visible ="false" OnClick ="Button11_Click"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
