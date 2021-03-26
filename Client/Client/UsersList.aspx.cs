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
    public partial class UsersList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie login = Request.Cookies["login"];
            HttpCookie sign = Request.Cookies["sing"];
            if (login != null && sign != null)
            {
                if (sign.Value == SighGenerator.GetSign(login.Value + "bytepp"))
                {
                    MakeList();
                    return;
                }
            }
            Response.Redirect("Login.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserPage.aspx");
        }
        protected void MakeList()
        {
            DataSet set = new DataSet();
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand($"SELECT Users.Name as Имя, Users.Surname as Фамилия, Departaments.Name as Отдел, Positions.Name as Должность, Company.Name as Компания FROM Departaments INNER JOIN ((Company INNER JOIN Users ON Company.Id = Users.Company_Id) INNER JOIN Positions ON (Positions.Id = Users.Positions_Id) AND (Company.Id = Positions.Company_Id)) ON (Departaments.Id = Users.Departaments_Id) WHERE Company.Id = {UserPage.UserCompanyId}", sqlCon);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(set, "Users");
            sqlCon.Close();
            GridView1.DataSource = set.Tables[0];
            GridView1.DataBind();
            
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("Logout.aspx");
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