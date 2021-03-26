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
    public partial class AdminUsersList : System.Web.UI.Page
    {
        public static bool isDelete = false;
        public static int IdForUpdate = 0;
        public static bool isUpdate = false;
        public static bool isCreate = false;
        public static bool isCreatePos = false;
        public static bool isCreateDep = false;
        public static bool isDeletePos = false;
        public static bool isDeleteDep = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie login = Request.Cookies["login"];
            HttpCookie sign = Request.Cookies["sing"];
            if (login != null && sign != null)
            {
                if (sign.Value == SighGenerator.GetSign(login.Value + "bytepp"))
                {
                    MakeList();
                    MakePosList();
                    MakeDepList();
                    return;
                }
            }
            Response.Redirect("Login.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserPage.aspx");
        }
        //создание таблицы сотрудников
        protected void MakeList()
        {
            DataSet set = new DataSet();
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand($"SELECT Users.Id as КодСотрудника, Users.Name as Имя, Users.Surname as Фамилия, Company.Name as Компания, Departaments.Name as Отдел, Positions.Name as Должность, Users.Login as Логин, Users.Password as Пароль, Users.IsAdmin as 'Является администратором' FROM Departaments INNER JOIN ((Company INNER JOIN Users ON Company.Id = Users.Company_Id) INNER JOIN Positions ON (Positions.Id = Users.Positions_Id) AND (Company.Id = Positions.Company_Id)) ON (Departaments.Id = Users.Departaments_Id) WHERE Company.Id = {UserPage.UserCompanyId}", sqlCon);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(set, "Users");
            sqlCon.Close();
            GridView1.DataSource = set.Tables[0];
            GridView1.DataBind();

        }

        //выход
        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("LogOut.aspx");
        }

        //удаление сотрудника
        protected void Button7_Click(object sender, EventArgs e)
        {
            isDelete = true;
            Label1.Visible = true;
            TextBox1.Visible = true;
            Button8.Visible = true;
            Button9.Visible = true;
            Button5.Enabled = false;
            Button6.Enabled = false;
            Button7.Enabled = false;
            Button11.Enabled = false;
            Button12.Enabled = false;
            Button13.Enabled = false;
            Button14.Enabled = false;
        }

        //отмена
        protected void Button8_Click(object sender, EventArgs e)
        {
            Label1.Visible = false;
            TextBox1.Visible = false;
            Button8.Visible = false;
            Button9.Visible = false;
            Button5.Enabled = true;
            Button6.Enabled = true;
            Button7.Enabled = true;
            Button11.Enabled = true;
            Button12.Enabled = true;
            Button13.Enabled = true;
            Button14.Enabled = true;
            Label2.Text = "";

        }


        //изменениее данных о сотруднике
        protected void Button6_Click(object sender, EventArgs e)
        {
            isDelete = false;
            isUpdate = true;
            Label1.Visible = true;
            TextBox1.Visible = true;
            Button8.Visible = true;
            Button9.Visible = true;
            Button5.Enabled = false;
            Button6.Enabled = false;
            Button7.Enabled = false;
            Button11.Enabled = false;
            Button12.Enabled = false;
            Button13.Enabled = false;
            Button14.Enabled = false;
        }


        //изменение/удаление пользователя
        protected void Button9_Click(object sender, EventArgs e)
        {
            if (isDelete)
            {
                if (UserPage.id != Convert.ToInt32(TextBox1.Text)) 
                    {
                    string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                    SqlConnection sqlCon = new SqlConnection(ConnectionString);
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand($"DELETE from Users Where Id = {TextBox1.Text}", sqlCon);
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                    MakeList();
                }
            }

        }

        //нагрузки и приборы
        protected void Button10_Click(object sender, EventArgs e)
        {
            Response.Redirect("LoadsAndDevices.aspx");
        }

        //принять
        protected void Button9_Click1(object sender, EventArgs e)
        {
                Label2.Text = "";
                if (isDelete)
                {
                    if (UserPage.id != Convert.ToInt32(TextBox1.Text))
                    {
                        string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                        SqlConnection sqlCon = new SqlConnection(ConnectionString);
                        sqlCon.Open();
                        SqlCommand cmd = new SqlCommand($"DELETE from Users Where Id = {TextBox1.Text}", sqlCon);
                        cmd.ExecuteNonQuery();
                        sqlCon.Close();
                        MakeList();
                        isDelete = false;
                        Label1.Visible = false;
                        TextBox1.Visible = false;
                        TextBox1.Text = "";
                        Button8.Visible = false;
                        Button9.Visible = false;
                        Button5.Enabled = true;
                        Button6.Enabled = true;
                        Button7.Enabled = true;
                        Button11.Enabled = true;
                        Button12.Enabled = true;
                        Button13.Enabled = true;
                        Button14.Enabled = true;
                        isDelete = false;
                    }
                }
                if (isUpdate == true && isDelete == false && isCreatePos == false && isCreateDep == false && isDeletePos == false && isDeleteDep == false)
                {
                    IdForUpdate = Convert.ToInt32(TextBox1.Text);
                    isUpdate = true;
                    Response.Redirect("UserUpdate.aspx");
                }
                if (isCreatePos == true)
                {
                    string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                    SqlConnection sqlCon = new SqlConnection(ConnectionString);
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand($"INSERT Positions VALUES ('{TextBox1.Text}', {UserPage.UserCompanyId})", sqlCon);
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                    MakePosList();
                    isDelete = false;
                    Label1.Visible = false;
                    TextBox1.Visible = false;
                    TextBox1.Text = "";
                    Button8.Visible = false;
                    Button9.Visible = false;
                    Button5.Enabled = true;
                    Button6.Enabled = true;
                    Button7.Enabled = true;
                    Button11.Enabled = true;
                    Button12.Enabled = true;
                    Button13.Enabled = true;
                    Button14.Enabled = true;
                    Label1.Text = "Введите код сотрудника:";
                    isCreatePos = false;
                }
                if (isCreateDep == true)
                {
                    string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                    SqlConnection sqlCon = new SqlConnection(ConnectionString);
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand($"INSERT Departaments VALUES ('{TextBox1.Text}', {UserPage.UserCompanyId})", sqlCon);
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                    MakeDepList();
                    isDelete = false;
                    Label1.Visible = false;
                    TextBox1.Visible = false;
                    TextBox1.Text = "";
                    Button8.Visible = false;
                    Button9.Visible = false;
                    Button5.Enabled = true;
                    Button6.Enabled = true;
                    Button7.Enabled = true;
                    Button11.Enabled = true;
                    Button12.Enabled = true;
                    Button13.Enabled = true;
                    Button14.Enabled = true;
                    Label1.Text = "Введите код:";
                    isCreateDep = false;
                }
                try {
                if (isDeletePos == true)
                {
                    string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                    SqlConnection sqlCon = new SqlConnection(ConnectionString);
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand($"DELETE from Positions Where Id = {TextBox1.Text}", sqlCon);
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                    MakePosList();
                    isDelete = false;
                    Label1.Visible = false;
                    TextBox1.Visible = false;
                    TextBox1.Text = "";
                    Button8.Visible = false;
                    Button9.Visible = false;
                    Button5.Enabled = true;
                    Button6.Enabled = true;
                    Button7.Enabled = true;
                    Button11.Enabled = true;
                    Button12.Enabled = true;
                    Button13.Enabled = true;
                    Button14.Enabled = true;
                    isDeletePos = false;
                }
                if (isDeleteDep == true)
                {
                    string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                    SqlConnection sqlCon = new SqlConnection(ConnectionString);
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand($"DELETE from Departaments Where Id = {TextBox1.Text}", sqlCon);
                    cmd.ExecuteNonQuery();
                    sqlCon.Close();
                    MakeDepList();
                    isDelete = false;
                    Label1.Visible = false;
                    TextBox1.Visible = false;
                    TextBox1.Text = "";
                    Button8.Visible = false;
                    Button9.Visible = false;
                    Button5.Enabled = true;
                    Button6.Enabled = true;
                    Button7.Enabled = true;
                    Button11.Enabled = true;
                    Button12.Enabled = true;
                    Button13.Enabled = true;
                    Button14.Enabled = true;
                    isDeletePos = false;
                }
            }
            catch
            {
                Label2.Text = "В выбранном отделе/должности есть сотрудники! Сначала измените данные о сотрудниках, далее продолжите процедуру удаления.";
            }
            
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            IdForUpdate = Convert.ToInt32(UserPage.id);
            isCreate = true;
            Response.Redirect("UserUpdate.aspx");
        }

        protected void MakePosList()
        {
            DataSet set = new DataSet();
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand($"SELECT Id as Номер, Name as Название FROM Positions WHERE Company_Id = {UserPage.UserCompanyId}", sqlCon);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(set, "Positions");
            sqlCon.Close();
            GridView2.DataSource = set.Tables[0];
            GridView2.DataBind();
        }
        protected void MakeDepList()
        {
            DataSet set = new DataSet();
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand($"SELECT Id as Номер, Name as Название FROM Departaments WHERE Company_Id = {UserPage.UserCompanyId}", sqlCon);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(set, "Departaments");
            sqlCon.Close();
            GridView3.DataSource = set.Tables[0];
            GridView3.DataBind();
        }

        //удаление департамента
        protected void Button11_Click(object sender, EventArgs e)
        {
            isCreatePos = true;
            Label1.Text = "Введите название:";
            Label1.Visible = true;
            TextBox1.Visible = true;
            Button8.Visible = true;
            Button9.Visible = true;
            Button5.Enabled = false;
            Button6.Enabled = false;
            Button7.Enabled = false;
            Button11.Enabled = false;
            Button12.Enabled = false;
            Button13.Enabled = false;
            Button14.Enabled = false;
        }

        //создание департамента
        protected void Button13_Click(object sender, EventArgs e)
        {
            isCreateDep = true;
            Label1.Text = "Введите название:";
            Label1.Visible = true;
            TextBox1.Visible = true;
            Button8.Visible = true;
            Button9.Visible = true;
            Button5.Enabled = false;
            Button6.Enabled = false;
            Button7.Enabled = false;
            Button11.Enabled = false;
            Button12.Enabled = false;
            Button13.Enabled = false;
            Button14.Enabled = false;
        }

        //удаление должности
        protected void Button12_Click(object sender, EventArgs e)
        {
            isDeletePos = true;
            Label1.Visible = true;
            TextBox1.Visible = true;
            Button8.Visible = true;
            Button9.Visible = true;
            Button5.Enabled = false;
            Button6.Enabled = false;
            Button7.Enabled = false;
            Button11.Enabled = false;
            Button12.Enabled = false;
            Button13.Enabled = false;
            Button14.Enabled = false;
        }

        //удаление департамента
        protected void Button14_Click(object sender, EventArgs e)
        {
            isDeleteDep = true;
            Label1.Visible = true;
            TextBox1.Visible = true;
            Button8.Visible = true;
            Button9.Visible = true;
            Button5.Enabled = false;
            Button6.Enabled = false;
            Button7.Enabled = false;
            Button11.Enabled = false;
            Button12.Enabled = false;
            Button13.Enabled = false;
            Button14.Enabled = false;
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForecastPage.aspx");
        }
    }
}