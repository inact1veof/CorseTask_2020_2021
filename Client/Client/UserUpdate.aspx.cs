using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

namespace Client
{
    public partial class UserUpdate : System.Web.UI.Page
    {
        public static int id = 0;
        //public Dictionary<string, int> users = new Dictionary<string, int>();
        public static string UserName;
        public static string UserLogin;
        public static string UserSurname;
        public static string UserPassword;
        public static int DepartamentUserId;
        public static int PositionUserId;
        public static int CompanyUserId;
        public static DateTime BithDateUser;
        public static bool IsAdminUser;

        public static Dictionary<string, int> DepartamentList = new Dictionary<string, int>();
        public static Dictionary<string, int> PositionList = new Dictionary<string, int>();

        public static Dictionary<int, string> DepartamentListReverse = new Dictionary<int, string>();
        public static Dictionary<int, string> PositionListReverse = new Dictionary<int, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie login = Request.Cookies["login"];
                HttpCookie sign = Request.Cookies["sing"];
                if (login != null && sign != null)
                {
                    if (sign.Value == SighGenerator.GetSign(login.Value + "bytepp"))
                    {
                        id = AdminUsersList.IdForUpdate;
                        LoadData();
                        return;
                    }
                }
                Response.Redirect("Login.aspx");
            }
        }
        protected void LoadData()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd;
            if (AdminUsersList.isUpdate == true)
            {
                cmd = new SqlCommand($"SELECT [Name] from [Users] where [id] = {id}", sqlCon);
                UserName = cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand($"SELECT [Surname] from [Users] where [id] = {id}", sqlCon);
                UserSurname = cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand($"SELECT [BithDate] from [Users] where [id] = {id}", sqlCon);
                string BithDate = cmd.ExecuteScalar().ToString();
                DateTime Date = Convert.ToDateTime(BithDate);
                BithDate = Date.ToShortDateString();
                BithDateUser = Date;

                cmd = new SqlCommand($"SELECT [Positions].[Id] from [Positions] JOIN [Users] ON [Positions].[Id] = [Users].[Positions_Id] where [Users].[id] = {id}", sqlCon);
                PositionUserId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand($"SELECT [Departaments].[Id] from [Departaments] JOIN [Users] ON [Departaments].[Id] = [Users].[Departaments_Id] where [Users].[id] = {id}", sqlCon);
                DepartamentUserId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand($"SELECT [isAdmin] from [Users] where [id] = {id}", sqlCon);
                IsAdminUser = Convert.ToBoolean(cmd.ExecuteScalar());

                cmd = new SqlCommand($"SELECT [Company].[Id] from [Company] JOIN [Users] ON [Users].[Company_Id] = [Company].[Id] where [Users].[id] = {id}", sqlCon);
                CompanyUserId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand($"SELECT [Login] from [Users] where [id] = {id}", sqlCon);
                UserLogin = cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand($"SELECT [Password] from [Users] where [id] = {id}", sqlCon);
                UserPassword = cmd.ExecuteScalar().ToString();
            }
            if (AdminUsersList.isUpdate == false)
            {
                CompanyUserId = UserPage.CompanyUserId;
            }
            cmd = new SqlCommand($"SELECT Positions.Id, Positions.Name from Positions where Company_Id = '{CompanyUserId}'", sqlCon);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                PositionList.Add(rd["Name"].ToString(), Convert.ToInt32(rd["Id"]));
                PositionListReverse.Add(Convert.ToInt32(rd["Id"]), rd["Name"].ToString());
            }
            rd.Close();
            cmd = new SqlCommand($"SELECT Departaments.Id, Departaments.Name from Departaments where Company_Id = '{CompanyUserId}'", sqlCon);
            rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                DepartamentList.Add(rd["Name"].ToString(), Convert.ToInt32(rd["Id"]));
                DepartamentListReverse.Add(Convert.ToInt32(rd["Id"]), rd["Name"].ToString());
            }
            rd.Close();
            sqlCon.Close();
            foreach (string s in PositionList.Keys)
            {
                DropDownList2.Items.Add(s);
            }
            foreach (string s in DepartamentList.Keys)
            {
                DropDownList1.Items.Add(s);
            }
            if (AdminUsersList.isUpdate == true)
            {
                TextBox1.Text = UserName;
                TextBox2.Text = UserSurname;
                TextBox3.Text = BithDateUser.ToString("yyyy-MM-dd");
                TextBox6.Text = UserLogin;
                TextBox7.Text = UserPassword;
                CheckBox1.Checked = IsAdminUser;
                DropDownList1.SelectedValue = DepartamentListReverse[DepartamentUserId];
                DropDownList2.SelectedValue = PositionListReverse[PositionUserId];
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminUsersList.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (AdminUsersList.isUpdate == true)
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(ConnectionString);
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand($"UPDATE Users SET Name = '{TextBox1.Text}', Surname = '{TextBox2.Text}', BithDate = '{TextBox3.Text}', Positions_Id = '{PositionList[DropDownList2.SelectedValue]}', Departaments_Id = '{DepartamentList[DropDownList1.SelectedValue]}', Login = '{TextBox6.Text}', Password = '{TextBox7.Text}', IsAdmin = '{CheckBox1.Checked}', Company_Id = '{CompanyUserId}' WHERE Id = {id}", sqlCon);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
                DepartamentList.Clear();
                PositionList.Clear();
                DepartamentListReverse.Clear();
                PositionListReverse.Clear();
                DropDownList1.Dispose();
                DropDownList2.Dispose();
                AdminUsersList.isUpdate = false;
                Response.Redirect("AdminUsersList.aspx");
            }
            if (AdminUsersList.isCreate == true)
            {
                bool check = false;
                string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(ConnectionString);
                sqlCon.Open();
                foreach (string s in UserPage.users.Keys)
                {
                    if (TextBox6.Text == s)
                    {
                        Label1.Visible = true;
                        check = true;
                    }
                }
                if (!check)
                {
                    SqlCommand cmd = new SqlCommand($"INSERT Users VALUES ('{TextBox1.Text}', '{TextBox2.Text}', '{TextBox3.Text}', '{PositionList[DropDownList2.SelectedValue]}', '{DepartamentList[DropDownList1.SelectedValue]}', '{TextBox6.Text}', '{TextBox7.Text}', '{CheckBox1.Checked}', '{CompanyUserId}')", sqlCon);
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                    DepartamentList.Clear();
                    PositionList.Clear();
                    DepartamentListReverse.Clear();
                    PositionListReverse.Clear();
                    DropDownList1.Dispose();
                    DropDownList2.Dispose();
                    AdminUsersList.isUpdate = false;
                    Response.Redirect("AdminUsersList.aspx");
                }
                else 
                {

                }
            }
        }
    }
}