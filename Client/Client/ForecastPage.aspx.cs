using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Drawing;

namespace Client
{
    public partial class ForecastPage : System.Web.UI.Page
    {
        static int port = 8686;
        static string address = "127.0.0.1";
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Font.Size = 24;
            Label1.Font.Bold = true;
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

        //главная
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserPage.aspx");
        }

        //сотрудники
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (UserPage.isAdmin == true)
            {
                Response.Redirect("AdminUsersList.aspx");
            }
            else
            {
                Response.Redirect("UsersList.aspx");
            }
        }
        //нагрузки и приборы
        protected void Button5_Click(object sender, EventArgs e)
        {
            Response.Redirect("LoadsAndDevices.aspx");
        }

        //текущая
        protected void Button4_Click(object sender, EventArgs e)
        {

        }

        //выход
        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("Logout.aspx");
        }

        protected void MakeList()
        {
            DataSet set = new DataSet();
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand($"SELECT Forecasts.Id as Id, Metering_Devices.Id as DeviceId, Users.Name as Имя, Users.Surname as Фамилия, Algoritm_Names.Name as Алгоритм, Forecasts.DateForForecast as ДатаПрогноза, Algoritm_Results.Value as ЗначениеПрогноза, Real_Values.Value as РеальноеЗначение, Rmse_Values.Value as RMSE FROM (Algoritm_Names INNER JOIN(Users INNER JOIN(Rmse_Values INNER JOIN(Metering_Devices INNER JOIN(Real_Values INNER JOIN Forecasts ON Real_Values.Id = Forecasts.RealValue_Id) ON Metering_Devices.Id = Real_Values.Device_Id) ON Rmse_Values.Id = Forecasts.Rmse_Id) ON Users.Id = Forecasts.User_Id) ON Algoritm_Names.Id = Rmse_Values.AlgId) INNER JOIN Algoritm_Results ON(Algoritm_Results.Id = Forecasts.Value_Id) AND(Algoritm_Names.Id = Algoritm_Results.Algoritm_Id) WHERE Users.Company_Id = '{UserPage.UserCompanyId}'",sqlCon);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(set, "Forecasts");
            GridView1.DataSource = set.Tables[0];
            GridView1.DataBind();
            sqlCon.Close();
        }

        //делать прогноз
        protected void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                string time = TextBox3.Text + ":00";
                string message = $"{TextBox2.Text},{time},{UserPage.id},{TextBox1.Text}";
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);
                // получаем ответ
                data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт
                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                Label5.ForeColor = Color.Green;
                Label5.Visible = true;
                Label5.Text = builder.ToString();

                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch
            {
                Label5.ForeColor = Color.Red;
                Label5.Visible = true;
                Label5.Text = "Сервер прогнозов не доступен";
            }
        }
    }
}