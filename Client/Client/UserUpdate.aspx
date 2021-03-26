<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserUpdate.aspx.cs" Inherits="Client.UserUpdate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Изменение данных о сотруднике</title>
    <link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="site.css" />
    
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
    <div class="container">
        <a>Изменить данные сотрудника</a>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
    </div>
</nav>
</head>
<body>
    <div class="text-center" runat ="server">
    <form runat ="server"> 
        <h1 style ="margin: 200px 200px 20px 200px">Изменение данных</h1>
        <asp:Table runat ="server" style ="margin-left: 20px; text-align:left; margin:auto;">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:label runat="server" id="loginLabel">Имя: </asp:label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID ="TextBox1" runat ="server" Width ="150"></asp:TextBox><br />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:label runat="server" id="PasswordLabel">Фамилия:</asp:label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID ="TextBox2" runat ="server" Width ="150"></asp:TextBox><br />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:label runat="server" id="loginLabel2">Дата рождения: </asp:label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID ="TextBox3" runat ="server" Width ="150" TextMode ="Date"></asp:TextBox><br />
                </asp:TableCell>
             </asp:TableRow>
             <asp:TableRow>
                 <asp:TableCell>
                    <label id="loginLabel3">Отдел: </label>
                 </asp:TableCell>
                 <asp:TableCell>
                     <asp:DropDownList ID="DropDownList1" runat="server" Width ="150"></asp:DropDownList>
                 </asp:TableCell>
             </asp:TableRow>
             <asp:TableRow>
                 <asp:TableCell>
                    <label id="loginLabel4">Должность: </label>
                 </asp:TableCell>
                 <asp:TableCell>
                     <asp:DropDownList ID="DropDownList2" runat="server" Width ="150"></asp:DropDownList> 
                 </asp:TableCell>
             </asp:TableRow>
             <asp:TableRow>
                 <asp:TableCell>
                    <label id="loginLabel5">Логин: </label>
                 </asp:TableCell>
                 <asp:TableCell>
                    <asp:TextBox ID ="TextBox6" runat ="server" Width ="150"></asp:TextBox><br />
                 </asp:TableCell>
             </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <label id="loginLabel6">Пароль: </label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID ="TextBox7" runat ="server" Width ="150"></asp:TextBox><br />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <label id="loginLabel8">Администратор? </label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:CheckBox ID="CheckBox1" runat="server"/><br />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button ID ="Button1" runat ="server" Text ="Отмена" Width ="150" OnClick ="Button1_Click"/>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button ID ="Button2" runat ="server" Text ="Принять" Width ="150" OnClick ="Button2_Click"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="Label1" runat="server" Text="Label" Visible ="false">Такой логин уже существует, попробуйте другой</asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </form>
</div>
</body>
</html>
