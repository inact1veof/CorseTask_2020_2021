using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Client
{
    public partial class UserPage : System.Web.UI.Page
    {
        public static Dictionary<string, int> users = new Dictionary<string, int>();
        public static int id = 0;
        public static int UserCompanyId = 0;
        public static bool isAdmin = false;


        public static string UserName;
        public static string UserLogin;
        public static string UserSurname;
        public static string UserPassword;
        public static int DepartamentUserId;
        public static int PositionUserId;
        public static int CompanyUserId;
        public static DateTime BithDateUser;
        public static bool IsAdminUser;


        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie login = Request.Cookies["login"];
            HttpCookie sign = Request.Cookies["sing"];
            if (login != null && sign != null)
            {
                if (sign.Value == SighGenerator.GetSign(login.Value + "bytepp"))
                {
                    users.Clear();
                    Label1.Text = login.Value;
                    SearchData();
                    UserCompanyId = GetCompanyId();
                    return;
                }
            }
            Response.Redirect("Login.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Logout.aspx");
        }
        protected int GetCompanyId()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand($"Select Company_Id from Users Where Id = {id}", sqlCon);
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }
        protected void SearchData()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand getUsersCredCmd = new SqlCommand("Select [Id], [Login] from [Users]", sqlCon);
            SqlDataReader rd = getUsersCredCmd.ExecuteReader();
            while (rd.Read())
            {
                users.Add(rd["Login"].ToString(), Convert.ToInt32(rd["Id"]));
            }
            rd.Close();
            id = users[Label1.Text];
            SqlCommand cmd = new SqlCommand($"SELECT [Name] from [Users] where [id] = {id}", sqlCon);
            Label9.Text = cmd.ExecuteScalar().ToString();
            UserName = Label9.Text;
            cmd = new SqlCommand($"SELECT [Surname] from [Users] where [id] = {id}", sqlCon);
            Label11.Text = cmd.ExecuteScalar().ToString();
            UserSurname = Label11.Text;
            cmd = new SqlCommand($"SELECT [BithDate] from [Users] where [id] = {id}", sqlCon);
            string BithDate = cmd.ExecuteScalar().ToString();
            DateTime Date = Convert.ToDateTime(BithDate);
            BithDate = Date.ToShortDateString();
            Label15.Text = BithDate;
            BithDateUser = Date;
            cmd = new SqlCommand($"SELECT [Positions].[Name] from [Positions] JOIN [Users] ON [Positions].[Id] = [Users].[Positions_Id] where [Users].[id] = {id}", sqlCon);
            Label14.Text = cmd.ExecuteScalar().ToString();
            cmd = new SqlCommand($"SELECT [Positions].[Id] from [Positions] JOIN [Users] ON [Positions].[Id] = [Users].[Positions_Id] where [Users].[id] = {id}", sqlCon);
            PositionUserId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd = new SqlCommand($"SELECT [Departaments].[Name] from [Departaments] JOIN [Users] ON [Departaments].[Id] = [Users].[Departaments_Id] where [Users].[id] = {id}", sqlCon);
            Label13.Text = cmd.ExecuteScalar().ToString();
            cmd = new SqlCommand($"SELECT [Departaments].[Id] from [Departaments] JOIN [Users] ON [Departaments].[Id] = [Users].[Departaments_Id] where [Users].[id] = {id}", sqlCon);
            DepartamentUserId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd = new SqlCommand($"SELECT [isAdmin] from [Users] where [id] = {id}", sqlCon);
            isAdmin = Convert.ToBoolean(cmd.ExecuteScalar());
            IsAdminUser = isAdmin;
            cmd = new SqlCommand($"SELECT [Company].[Name] from [Company] JOIN [Users] ON [Users].[Company_Id] = [Company].[Id] where [Users].[id] = {id}", sqlCon);
            Label12.Text = cmd.ExecuteScalar().ToString();
            cmd = new SqlCommand($"SELECT [Company].[Id] from [Company] JOIN [Users] ON [Users].[Company_Id] = [Company].[Id] where [Users].[id] = {id}", sqlCon);
            CompanyUserId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd = new SqlCommand($"SELECT [Login] from [Users] where [id] = {id}", sqlCon);
            UserLogin = cmd.ExecuteScalar().ToString();
            cmd = new SqlCommand($"SELECT [Password] from [Users] where [id] = {id}", sqlCon);
            UserPassword = cmd.ExecuteScalar().ToString();
            sqlCon.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (!isAdmin)
            {
                Response.Redirect("UsersList.aspx");
            }
            else
            {
                Response.Redirect("AdminUsersList.aspx");
            }

        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            Table1.Width = 1200;
            TextBox1.Text = Label9.Text;
            TextBox2.Text = Label11.Text;
            TextBox1.Visible = true;
            TextBox2.Visible = true;
            Button6.Visible = true;
            Button7.Visible = true;
            Button5.Visible = false;
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            Table1.Width = 600;
            TextBox1.Visible = false;
            TextBox2.Visible = false;
            Button6.Visible = false;
            Button7.Visible = false;
            Button5.Visible = true;
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            Table1.Width = 600;
            TextBox1.Visible = false;
            TextBox2.Visible = false;
            Button6.Visible = false;
            Button7.Visible = false;
            Button5.Visible = true;
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand($"UPDATE [Users] set [Name] = '{TextBox1.Text}' where [id] = {id}", sqlCon);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand($"UPDATE [Users] set [Surname] = '{TextBox2.Text}' where [id] = {id}", sqlCon);
            cmd.ExecuteNonQuery();
            sqlCon.Close();
            RefreshData();
        }
        protected void RefreshData()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand($"SELECT [Name] from [Users] where [id] = {id}", sqlCon);
            Label9.Text = cmd.ExecuteScalar().ToString();
            cmd = new SqlCommand($"SELECT [Surname] from [Users] where [id] = {id}", sqlCon);
            Label11.Text = cmd.ExecuteScalar().ToString();
            cmd = new SqlCommand($"SELECT [BithDate] from [Users] where [id] = {id}", sqlCon);
            string BithDate = cmd.ExecuteScalar().ToString();
            DateTime Date = Convert.ToDateTime(BithDate);
            BithDate = Date.ToShortDateString();
            Label15.Text = BithDate;
            cmd = new SqlCommand($"SELECT [Positions].[Name] from [Positions] JOIN [Users] ON [Positions].[Id] = [Users].[Positions_Id] where [Users].[id] = {id}", sqlCon);
            Label14.Text = cmd.ExecuteScalar().ToString();
            cmd = new SqlCommand($"SELECT [Departaments].[Name] from [Departaments] JOIN [Users] ON [Departaments].[Id] = [Users].[Departaments_Id] where [Users].[id] = {id}", sqlCon);
            Label13.Text = cmd.ExecuteScalar().ToString();
            sqlCon.Close();
        }


        //нагрузки и приборы
        protected void Button8_Click(object sender, EventArgs e)
        {
            Response.Redirect("LoadsAndDevices.aspx");
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForecastPage.aspx");
        }
    }
}