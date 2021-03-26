<%@ Page Language="C#" Async ="true" AutoEventWireup="true" CodeBehind="LoadsAndDevices.aspx.cs" Inherits="Client.LoadsAndDevices" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Список нагрузок и приборов</title>
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
        <asp:Button ID="Button4" runat="server" Text="Прогнозы" OnClick ="Button4_Click1"/>
        <asp:Button ID="Button3" runat="server" Text="Выход" OnClick ="Button3_Click"/> 
    </div>
        </nav>
        <div>
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell style ="width:50%; vertical-align:top">
                        <h1 style ="margin-left: 20px">Список существующих нагрузок</h1>
                         <asp:GridView ID="GridView1" runat="server" style ="margin-left: 20px; text-align:center; width:50%" Font-Size ="16"> 
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                        <h1 style ="margin-left: 20px">Список существующих приборов учета</h1>
                        <asp:GridView ID="GridView2" runat="server" style ="margin-left: 20px; text-align:center; width:50%" Font-Size ="16"> 
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                    </asp:TableCell>
                    <asp:TableCell style ="vertical-align:top; text-align:center">
                        <h1 style ="margin-left: 20px">Добавить данные</h1>
                        <asp:Label ID="Label4" runat="server" Text="Номер прибора учета: " Visible ="true"></asp:Label>
                        <asp:TextBox ID="TextBox3" runat="server" Width ="60" Visible ="true"></asp:TextBox> <br /><br />
                        <asp:FileUpload ID="FileUpload1" runat="server" /> <br /><br />
                        <asp:Button ID="Button12" runat="server" Text="Добавить данные" Width ="250" Visible ="true" OnClick ="Button12_Click"/> <br /><br />
                        <asp:Label ID="Label5" runat="server" Text="Данные успешно добавлены" Visible ="false"></asp:Label>
                        <asp:Button ID="Button15" runat="server" Text="Отмена" Width ="250" Visible ="false" OnClick ="Button15_Click"/> <br /><br />
                    </asp:TableCell>
                    <asp:TableCell style ="vertical-align:top">
                        <asp:Label ID="Label3" runat="server" Text="Действия" Visible ="true"></asp:Label><br /><br />
                        <asp:Button ID="Button6" runat="server" Text="Добавить нагрузку" Width ="250" OnClick ="Button6_Click"/> <br /><br />
                        <asp:Button ID="Button7" runat="server" Text="Добавить прибор учета" Width ="250" OnClick ="Button7_Click"/><br /><br />
                        <asp:Button ID="Button8" runat="server" Text="Удалить нагрузку" Width ="250" OnClick ="Button8_Click"/><br /><br />
                        <asp:Button ID="Button9" runat="server" Text="Удалить прибор учета" Width ="250" OnClick ="Button9_Click"/><br /><br />
                        <asp:Label ID="Label1" runat="server" Text="Наименование: " Visible ="false"></asp:Label>
                        <asp:TextBox ID="TextBox1" runat="server" Width ="200" Visible ="false"></asp:TextBox> <br /><br />
                        <asp:Label ID="Label2" runat="server" Text="Номер нагрузки: " Visible ="false"></asp:Label>
                        <asp:TextBox ID="TextBox2" runat="server" Width ="193" Visible ="false"></asp:TextBox> <br /><br />
                        <asp:Button ID="Button10" runat="server" Text="Отмена" Width ="125" Visible ="false" OnClick ="Button10_Click"/>
                        <asp:Button ID="Button11" runat="server" Text="Принять" Width ="125" Visible ="false" OnClick ="Button11_Click"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </form>
</body>
</html>
