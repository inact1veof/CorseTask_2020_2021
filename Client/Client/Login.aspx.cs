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
    public partial class Login : System.Web.UI.Page
    {
        public Dictionary<string, string> db = null;
        private SqlConnection sqlCon = null;
        protected async void Page_Load(object sender, EventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            sqlCon = new SqlConnection(ConnectionString);

            await sqlCon.OpenAsync();
        }

        protected async void Button1_Click(object sender, EventArgs e)
        {
            db = new Dictionary<string, string>();

            SqlCommand getUsersCredCmd = new SqlCommand("Select [Login], [Password] from [Users]", sqlCon);

            SqlDataReader rd = null;
            try
            {
                rd = await getUsersCredCmd.ExecuteReaderAsync();
                while (await rd.ReadAsync())
                {
                    db.Add(rd["Login"].ToString(), rd["Password"].ToString());
                }
            }
            catch
            {

            }
            finally
            {
                if (rd != null)
                {
                    rd.Close();
                }
            }
            try
            {
                if (TextBox2.Text == db[TextBox1.Text])
                {
                    HttpCookie login = new HttpCookie("login", TextBox1.Text);

                    HttpCookie sign = new HttpCookie("sing", SighGenerator.GetSign(TextBox1.Text + "bytepp"));

                    Response.Cookies.Add(login);

                    Response.Cookies.Add(sign);

                    Response.Redirect("UserPage.aspx", false);
                }
            }
            catch
            {
                Response.Redirect("Logout.aspx");
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (sqlCon != null && sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
        }
    }
}