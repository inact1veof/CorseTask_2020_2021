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
using Grafana.Models;
using Grafana.Exceptions;
using Grafana.Serialization;
using Grafana.Services.Impl;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Client
{
    public partial class ForecastPage : System.Web.UI.Page
    {
        public static int counterOpens = 0;
        public static Dictionary<string, int> namesAlg = new Dictionary<string, int>();
        public static bool isLoad = false;
        public static bool isMake = false;
        public static bool isDelete = false;
        public static bool isUpdate = false;
        public static List<int> prognosedDevices = new List<int>();
        public static List<int> prognosedDevicesFinal = new List<int>();
        public static bool isOpen = false;
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
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand($"SELECT Metering_Devices.Id FROM (Algoritm_Names INNER JOIN(Users INNER JOIN(Rmse_Values INNER JOIN(Metering_Devices INNER JOIN(Real_Values INNER JOIN Forecasts ON Real_Values.Id = Forecasts.RealValue_Id) ON Metering_Devices.Id = Real_Values.Device_Id) ON Rmse_Values.Id = Forecasts.Rmse_Id) ON Users.Id = Forecasts.User_Id) ON Algoritm_Names.Id = Rmse_Values.AlgId) INNER JOIN Algoritm_Results ON(Algoritm_Results.Id = Forecasts.Value_Id) AND(Algoritm_Names.Id = Algoritm_Results.Algoritm_Id) WHERE Users.Company_Id = '{UserPage.UserCompanyId}'", sqlCon);
            SqlDataReader rd = null;
            rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                prognosedDevices.Add(Convert.ToInt32(rd["Id"]));
            }
            rd.Close();
            prognosedDevicesFinal = prognosedDevices.Distinct().ToList();
            sqlCon.Close();
            bool flag = false;
            int DeviceId = Convert.ToInt32(TextBox1.Text);
            foreach (int r in prognosedDevicesFinal)
            {
                if (DeviceId == r)
                {
                    flag = true;
                    break;
                }
            }
            if (flag == false)
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
            else
            {
                string time = TextBox3.Text;
                string date = TextBox2.Text;
                string dateTime = date + " " + time;
                DateTime dt = DateTime.Parse(dateTime);
                List<int> algValues = new List<int>();
                int realValue = 0;
                int userId = UserPage.id;
                List<int> rmseValues = new List<int>();
                sqlCon = new SqlConnection(ConnectionString);
                sqlCon.Open();
                cmd = new SqlCommand($"SELECT Metering_Devices.Id FROM (Algoritm_Names INNER JOIN(Users INNER JOIN(Rmse_Values INNER JOIN(Metering_Devices INNER JOIN(Real_Values INNER JOIN Forecasts ON Real_Values.Id = Forecasts.RealValue_Id) ON Metering_Devices.Id = Real_Values.Device_Id) ON Rmse_Values.Id = Forecasts.Rmse_Id) ON Users.Id = Forecasts.User_Id) ON Algoritm_Names.Id = Rmse_Values.AlgId) INNER JOIN Algoritm_Results ON(Algoritm_Results.Id = Forecasts.Value_Id) AND(Algoritm_Names.Id = Algoritm_Results.Algoritm_Id) WHERE Users.Company_Id = '{UserPage.UserCompanyId}'", sqlCon);
                rd = null;
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    prognosedDevices.Add(Convert.ToInt32(rd["Id"]));
                }
                rd.Close();
                prognosedDevicesFinal = prognosedDevices.Distinct().ToList();
                List<DateTime> dates = new List<DateTime>();
                cmd = new SqlCommand($"SELECT DateTime FROM Real_Values WHERE Device_Id = {DeviceId}", sqlCon);
                rd = null;
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    dates.Add(Convert.ToDateTime(rd["DateTime"]));
                }
                rd.Close();
                int minute = 0;
                int seconde = 0;
                foreach(DateTime d in dates)
                {
                    if (d.Year == dt.Year)
                    {
                        if (d.Month == dt.Month)
                        {
                            if(d.Day == dt.Day)
                            {
                                if (d.Hour == dt.Hour)
                                {
                                    minute = d.Minute;
                                    seconde = d.Second;
                                }
                            }
                        }
                    }
                }
                DateTime newDt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, minute, seconde);
                cmd = new SqlCommand($"SELECT Id FROM Real_Values WHERE Device_Id = {DeviceId} AND DateTime = '{newDt}'", sqlCon);
                realValue = Convert.ToInt32(cmd.ExecuteScalar());
                cmd = new SqlCommand($"SELECT Id FROM Algoritm_Results WHERE Device_Id = {DeviceId} AND DateTime = '{newDt}' AND Algoritm_Id = '1'", sqlCon);
                algValues.Add(Convert.ToInt32(cmd.ExecuteScalar()));
                cmd = new SqlCommand($"SELECT Id FROM Algoritm_Results WHERE Device_Id = {DeviceId} AND DateTime = '{newDt}' AND Algoritm_Id = '2'", sqlCon);
                algValues.Add(Convert.ToInt32(cmd.ExecuteScalar()));
                cmd = new SqlCommand($"SELECT Id FROM Algoritm_Results WHERE Device_Id = {DeviceId} AND DateTime = '{newDt}' AND Algoritm_Id = '3'", sqlCon);
                algValues.Add(Convert.ToInt32(cmd.ExecuteScalar()));
                cmd = new SqlCommand($"SELECT Id FROM Algoritm_Results WHERE Device_Id = {DeviceId} AND DateTime = '{newDt}' AND Algoritm_Id = '4'", sqlCon);
                algValues.Add(Convert.ToInt32(cmd.ExecuteScalar()));
                cmd = new SqlCommand($"SELECT Id FROM Algoritm_Results WHERE Device_Id = {DeviceId} AND DateTime = '{newDt}' AND Algoritm_Id = '5'", sqlCon);
                algValues.Add(Convert.ToInt32(cmd.ExecuteScalar()));
                cmd = new SqlCommand($"SELECT Id FROM Rmse_Values WHERE Device_Id = {DeviceId} AND Date = '{newDt.ToShortDateString()}' AND AlgId = '1'", sqlCon);
                rmseValues.Add(Convert.ToInt32(cmd.ExecuteScalar()));
                cmd = new SqlCommand($"SELECT Id FROM Rmse_Values WHERE Device_Id = {DeviceId} AND Date = '{newDt.ToShortDateString()}' AND AlgId = '2'", sqlCon);
                rmseValues.Add(Convert.ToInt32(cmd.ExecuteScalar()));
                cmd = new SqlCommand($"SELECT Id FROM Rmse_Values WHERE Device_Id = {DeviceId} AND Date = '{newDt.ToShortDateString()}' AND AlgId = '3'", sqlCon);
                rmseValues.Add(Convert.ToInt32(cmd.ExecuteScalar()));
                cmd = new SqlCommand($"SELECT Id FROM Rmse_Values WHERE Device_Id = {DeviceId} AND Date = '{newDt.ToShortDateString()}' AND AlgId = '4'", sqlCon);
                rmseValues.Add(Convert.ToInt32(cmd.ExecuteScalar()));
                cmd = new SqlCommand($"SELECT Id FROM Rmse_Values WHERE Device_Id = {DeviceId} AND Date = '{newDt.ToShortDateString()}' AND AlgId = '5'", sqlCon);
                rmseValues.Add(Convert.ToInt32(cmd.ExecuteScalar()));
                for (int i = 0; i < algValues.Count; i++)
                {
                    cmd = new SqlCommand($"INSERT Forecasts VALUES('{i+1}', '{dt}', '{algValues[i]}', '{realValue}', '{userId}', '{rmseValues[i]}')", sqlCon);
                    cmd.ExecuteNonQuery();
                }
                sqlCon.Close();
                Label5.Text = "Прогноз совершен, обновите страницу";
                Label5.Visible = true;
            }
        }


        //просмотр графаны
        protected void Button8_Click(object sender, EventArgs e)
        {
            if (isOpen == false)
            {
                isOpen = true;
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("grafana-server.exe");
                p.StartInfo.WorkingDirectory = @"C:\Data\grafana\bin\";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
            }
            Process.Start("http://localhost:3000/d/keZhp9wMk/data?orgId=1");
        }


        //сделать план
        protected void Button7_Click(object sender, EventArgs e)
        {
            TextBox4.Text = "";
            TextBox5.Text = "";
            namesAlg.Clear();
            DropDownList1.Items.Clear();
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand($"SELECT Id, Name FROM Algoritm_Names", sqlCon);
            SqlDataReader rd = null;
            rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                DropDownList1.Items.Add(rd["Name"].ToString());
                namesAlg.Add(rd["Name"].ToString(), Convert.ToInt32(rd["Id"]));
            }
            rd.Close();
            sqlCon.Close();
            isMake = true;
            Button9.Enabled = false;
            Button7.Enabled = false;
            Button13.Enabled = false;
            Label10.Visible = true;
            TextBox4.Visible = true;
            Button10.Visible = true;
            Button11.Visible = true;
            Label6.Visible = true;
            DropDownList1.Visible = true;
            Label7.Visible = true;
            TextBox5.Visible = true;
        }

        protected void MakePlan()
        {

        }

        //загрузка плана
        protected void Button9_Click(object sender, EventArgs e)
        {
            TextBox4.Text = "";
            TextBox5.Text = "";
            namesAlg.Clear();
            DropDownList1.Items.Clear();
            isLoad = true;
            Button9.Enabled = false;
            Button7.Enabled = false;
            Button13.Enabled = false;
            Label10.Visible = true;
            TextBox4.Visible = true;
            Button10.Visible = true;
            Button11.Visible = true;
            Label7.Visible = true;
            TextBox5.Visible = true;
        }

        //отмена загрузки плана
        protected void Button10_Click(object sender, EventArgs e)
        {
            TextBox4.Text = "";
            TextBox5.Text = "";
            namesAlg.Clear();
            DropDownList1.Items.Clear();
            Button9.Enabled = true;
            Button7.Enabled = true;
            Button13.Enabled = true;
            Label10.Visible = false;
            TextBox4.Visible = false;
            Button10.Visible = false;
            Button11.Visible = false;
            Label6.Visible = false;
            DropDownList1.Visible = false;
            isLoad = false;
            isMake = false;
            Label7.Visible = false;
            TextBox5.Visible = false;
            DropDownList1.Items.Clear();
        }


        //принять загрузку плана
        protected void Button11_Click(object sender, EventArgs e)
        {
            if (isLoad == true)
            {
                DateTime dt = DateTime.Parse(TextBox5.Text);
                DateTime newtDt = dt.AddDays(1);
                DataSet set = new DataSet();
                Button9.Enabled = true;
                Button7.Enabled = true;
                Label10.Visible = false;
                TextBox4.Visible = false;
                Button10.Visible = false;
                Button11.Visible = false;
                string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(ConnectionString);
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand($"SELECT Id as Номер, DateTime as ДатаВремя, Value as Значение FROM [Plan] WHERE Device_id = '{TextBox4.Text}' AND (DateTime BETWEEN '{dt}' AND '{newtDt}')", sqlCon);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(set, "Plan");
                GridView2.DataSource = set.Tables[0];
                GridView2.DataBind();
                sqlCon.Close();
                isLoad = false;
                Button10_Click(sender, e);
            }
            if (isMake == true)
            {
                DateTime dt = DateTime.Parse(TextBox5.Text);
                DateTime newtDt = dt.AddDays(1);
                string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(ConnectionString);
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand($"SELECT DateTime, Value FROM Algoritm_Results WHERE Algoritm_Id = '{namesAlg[DropDownList1.SelectedValue]}' AND (DateTime BETWEEN '{dt}' AND '{newtDt}') AND Device_Id = '{TextBox4.Text}'",sqlCon);
                List<DateTime> arrData = new List<DateTime>();
                List<float> arrValue = new List<float>();
                List<string> arrValueStr = new List<string>();
                SqlDataReader rd = null;
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    arrData.Add(DateTime.Parse(rd["DateTime"].ToString()));
                    arrValue.Add((float)Convert.ToSingle(rd["Value"]));
                }
                rd.Close();
                for (int i = 0; i < arrData.Count; i++)
                {
                    arrValueStr.Add(arrValue[i].ToString());
                    arrValueStr[i] = arrValueStr[i].Replace(',', '.');
                }
                for (int i = 0; i < arrData.Count; i++) 
                {
                    cmd = new SqlCommand($"INSERT [Plan] VALUES('{TextBox4.Text}', '{arrData[i]}', '{arrValueStr[i]}')", sqlCon);
                    cmd.ExecuteNonQuery();
                }
                arrData.Clear();
                arrValue.Clear();
                arrValueStr.Clear();
                sqlCon.Close();
                isMake = false;
                Button10_Click(sender, e);
            }
            if (isDelete == true)
            {
                DateTime dt = DateTime.Parse(TextBox5.Text);
                DateTime newtDt = dt.AddDays(1);
                string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(ConnectionString);
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand($"DELETE FROM [Plan] WHERE (DateTime BETWEEN '{dt}' AND '{newtDt}') AND Device_Id = '{TextBox4.Text}'",sqlCon);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
                isDelete = false;
                Button10_Click(sender, e);
            }
            if (isUpdate == true)
            {
                string val = TextBox5.Text;
                string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(ConnectionString);
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand($"UPDATE [Plan] SET Value = '{val}' WHERE Id = {TextBox4.Text}", sqlCon);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
                isUpdate = false;
                Label10.Text = "Номер прибора учета: ";
                Label7.Text = "Дата";
                TextBox5.TextMode = TextBoxMode.Date;
                Button10_Click(sender, e);
            }
        }


        //свернуть развернуть
        protected void Button12_Click(object sender, EventArgs e)
        {
            if (counterOpens %2 == 0)
            {
                GridView2.Visible = false;
                counterOpens++;
            }
            else
            {
                GridView2.Visible = true;
                counterOpens++;
            }
        }

        //удаление плана
        protected void Button13_Click(object sender, EventArgs e)
        {
            TextBox4.Text = "";
            TextBox5.Text = "";
            namesAlg.Clear();
            DropDownList1.Items.Clear();
            Button9.Enabled = false;
            Button7.Enabled = false;
            Button13.Enabled = false;
            Label10.Visible = true;
            TextBox4.Visible = true;
            Button10.Visible = true;
            Button11.Visible = true;
            Label7.Visible = true;
            TextBox5.Visible = true;
            isDelete = true;
        }


        //изменение плана
        protected void Button14_Click(object sender, EventArgs e)
        {
            Label10.Text = "Номер: ";
            TextBox5.TextMode = TextBoxMode.SingleLine;
            Label7.Text = "Значение: ";
            Button9.Enabled = false;
            Button7.Enabled = false;
            Button13.Enabled = false;
            Label10.Visible = true;
            TextBox4.Visible = true;
            Button10.Visible = true;
            Button11.Visible = true;
            Label7.Visible = true;
            TextBox5.Visible = true;
            isUpdate = true;
        }
    }
}