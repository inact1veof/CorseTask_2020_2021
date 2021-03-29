using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Client
{
    public partial class LoadsAndDevices : System.Web.UI.Page
    {
        public static bool isAddBurden = false;
        public static bool isAddMetDevice = false;
        public static bool isDeleteBurden = false;
        public static bool isDeleteMet = false;
        public static int DeviceId = 0;
        public static int CountOfElements = 0;
        public static int countOfDays = 0;
        public static bool isLoad = false;
        public static int RedirectCounter = 0;

        public static DateTime[] famousDates;
        public static float[] famousValues;
        public static string[] famousValuesStr;
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie login = Request.Cookies["login"];
            HttpCookie sign = Request.Cookies["sing"];
            if (login != null && sign != null)
            {
                if (sign.Value == SighGenerator.GetSign(login.Value + "bytepp"))
                {
                    Label3.Font.Size = 24;
                    MakeDefaultLists();
                    if (UserPage.isAdmin != true)
                    {                        
                        Button6.Visible = false;
                        Button7.Visible = false;
                        Button8.Visible = false;
                        Button9.Visible = false;
                        Button10.Visible = false;
                        Button11.Visible = false;
                        Label1.Visible = false;
                        Label2.Visible = false;
                        TextBox1.Visible = false;
                        TextBox2.Visible = false;
                        Label3.Visible = false;
                    }
                    return;
                }
            }
            Response.Redirect("Login.aspx");
        }


        //главная редирект
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserPage.aspx");
        }

        //сотрудники редирект
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

        //текущая
        protected void Button5_Click(object sender, EventArgs e)
        {

        }


        //прогноз
        protected void Button4_Click(object sender, EventArgs e)
        {

        }


        //выход
        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("Logout.aspx");
        }

        protected void MakeDefaultLists()
        {
            DataSet set = new DataSet();
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand($"SELECT Id as Номер, Name as Наименование FROM Burden WHERE Company_Id = '{UserPage.UserCompanyId}'", sqlCon);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(set, "Burden");
            sqlCon.Close();
            GridView1.DataSource = set.Tables[0];
            GridView1.DataBind();
            set = new DataSet();
            cmd = new SqlCommand($"SELECT Metering_Devices.Id as Номер, Metering_Devices.Name as Наименование, Burden.Id as НомерНагрузки, Burden.Name as НаименованиеНагрузки  FROM (Company INNER JOIN Burden ON Company.Id = Burden.Company_Id) INNER JOIN Metering_Devices ON Burden.Id = Metering_Devices.Burden_Id WHERE Burden.Company_Id = '{UserPage.UserCompanyId}'", sqlCon);
            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(set, "Metering_Devices");
            sqlCon.Close();
            GridView2.DataSource = set.Tables[0];
            GridView2.DataBind();
            set = new DataSet();
            cmd = new SqlCommand($"SELECT Metering_Devices.Id, Metering_Devices.Name, Count(Real_Values.Value) AS [Количество показаний] FROM Metering_Devices INNER JOIN Real_Values ON Metering_Devices.Id = Real_Values.Device_Id GROUP BY Metering_Devices.Id, Metering_Devices.Name", sqlCon);
            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(set, "Metering_Devices");
            sqlCon.Close();
            GridView3.DataSource = set.Tables[0];
            GridView3.DataBind();
        }

        //добавление нагрузки
        protected void Button6_Click(object sender, EventArgs e)
        {
            TextBox1.Width = 125;
            isAddBurden = true;
            Label1.Visible = true;
            TextBox1.Visible = true;
            Button6.Enabled = false;
            Button7.Enabled = false;
            Button8.Enabled = false;
            Button9.Enabled = false;
            Button10.Visible = true;
            Button11.Visible = true;
        }


        //отмена
        protected void Button10_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
            Label1.Text = "Наименование: ";
            TextBox1.Width = 125;
            TextBox2.Width = 125;
            Label1.Visible = false;
            Label2.Visible = false;
            TextBox1.Visible = false;
            TextBox2.Visible = false;
            Button10.Visible = false;
            Button11.Visible = false;
            Button6.Enabled = true;
            Button7.Enabled = true;
            Button8.Enabled = true;
            Button9.Enabled = true;
            isAddBurden = false;
            isAddMetDevice = false;
            isDeleteBurden = false;
            isDeleteMet = false;
        }

        //добавление приббора учета
        protected void Button7_Click(object sender, EventArgs e)
        {
            isAddMetDevice = true;
            Label1.Visible = true;
            Label2.Visible = true;
            Label1.Text = "Наименование: ";
            TextBox2.Width = 120;
            TextBox1.Width = 125;
            TextBox1.Visible = true;
            TextBox2.Visible = true;
            Button6.Enabled = false;
            Button7.Enabled = false;
            Button8.Enabled = false;
            Button9.Enabled = false;
            Button10.Visible = true;
            Button11.Visible = true;

        }


        //удаление нагрузки
        protected void Button8_Click(object sender, EventArgs e)
        {
            isDeleteBurden = true;
            Label1.Text = "Номер нагрузки: ";
            TextBox1.Width = 50;
            Label1.Visible = true;
            TextBox1.Visible = true;
            Button6.Enabled = false;
            Button7.Enabled = false;
            Button8.Enabled = false;
            Button9.Enabled = false;
            Button10.Visible = true;
            Button11.Visible = true;
        }

        //удаление прибора учета
        protected void Button9_Click(object sender, EventArgs e)
        {
            isDeleteMet = true;
            Label1.Text = "Введите номер прибора: ";
            TextBox1.Width = 50;
            Label1.Visible = true;
            TextBox1.Visible = true;
            Button6.Enabled = false;
            Button7.Enabled = false;
            Button8.Enabled = false;
            Button9.Enabled = false;
            Button10.Visible = true;
            Button11.Visible = true;
        }

        //принять
        protected void Button11_Click(object sender, EventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd;
            if (isAddBurden == true)
            {
                cmd = new SqlCommand($"INSERT Burden VALUES ('{TextBox1.Text}', '{UserPage.CompanyUserId}')", sqlCon);
                cmd.ExecuteNonQuery();
                MakeDefaultLists();
                Button10_Click(sender, e);

            }
            if (isAddMetDevice == true)
            {
                cmd = new SqlCommand($"INSERT Metering_Devices VALUES ('{TextBox1.Text}', '{TextBox2.Text}')", sqlCon);
                cmd.ExecuteNonQuery();
                MakeDefaultLists();
                Button10_Click(sender, e);
            }
            if (isDeleteBurden == true)
            {
                cmd = new SqlCommand($"DELETE Burden WHERE Id = '{TextBox1.Text}'", sqlCon);
                cmd.ExecuteNonQuery();
                MakeDefaultLists();
                Button10_Click(sender, e);
            }
            if (isDeleteMet == true)
            {
                cmd = new SqlCommand($"DELETE Metering_Devices WHERE Id = '{TextBox1.Text}'", sqlCon);
                cmd.ExecuteNonQuery();
                MakeDefaultLists();
                Button10_Click(sender, e);
            }
            sqlCon.Close();
        }

        //продолжение до выбора файла
        //protected void Button12_Click(object sender, EventArgs e)
        //{
        //    Button15.Visible = true;
        //    DeviceId = Convert.ToInt32(TextBox3.Text);
        //}

        //отмена добавления данных
        protected void Button15_Click(object sender, EventArgs e)
        {
            Button15.Visible = false;
            TextBox3.Text = "";
            DeviceId = 0;
        }


        //выбор файла
        protected void Button13_Click(object sender, EventArgs e)
        {
            
        }

        //загрузка файла
        protected void Button12_Click(object sender, EventArgs e)
        {
            DeviceId = Convert.ToInt32(TextBox3.Text);
            string fdpath = @"C:\Data\data.xlsx";
            FileUpload1.SaveAs(fdpath);
            Excel.Application ObjWorkExcel = new Excel.Application();
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(fdpath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);
            string[,] list = new string[lastCell.Column, lastCell.Row];
            int iLastRow = ObjWorkSheet.Cells[ObjWorkSheet.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row;
            var arrData = (object[,])ObjWorkSheet.Range["A1:Z" + iLastRow].Value;
            CountOfElements = arrData.GetLength(0);
            countOfDays = CountOfElements / 24;
            famousDates = new DateTime[CountOfElements];
            famousValues = new float[CountOfElements];
            famousValuesStr = new string[CountOfElements];
            ObjWorkExcel.Quit();
            ObjWorkSheet = null;
            ObjWorkBook = null;
            ObjWorkExcel = null;
            GC.Collect();
            Process excelProc = System.Diagnostics.Process.GetProcessesByName("EXCEL").Last();
            excelProc.Kill();
            for (int i = 0; i < CountOfElements; i++)
            {
                famousValues[i] = (float)Convert.ToDouble(arrData[i + 1, 2]);
            }
            for (int i = 0; i < CountOfElements; i++)
            {
                famousDates[i] = Convert.ToDateTime(arrData[i + 1, 1]);
            }
            for (int i = 0; i < CountOfElements; i++)
            {
                famousValuesStr[i] = famousValues[i].ToString();
                famousValuesStr[i] = famousValuesStr[i].Replace(',', '.');
            }
            AddDataAsync();
            Label5.Visible = true;

        }

        protected void AddData()
        {
            int Device_Id = DeviceId;
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            sqlCon.Open();
            SqlCommand cmd;
            cmd = new SqlCommand($"DELETE FROM Real_Values WHERE Device_Id = {DeviceId}", sqlCon);
            cmd.ExecuteNonQuery();
            for (int i = 0; i < CountOfElements; i++)
            {
                cmd = new SqlCommand($"INSERT Real_Values VALUES ('{famousDates[i]}', '{famousValuesStr[i]}', '{DeviceId}')", sqlCon);
                cmd.ExecuteNonQuery();
            }
            sqlCon.Close();
            isLoad = false;
            RedirectCounter = 0;
        }
        protected async void AddDataAsync()
        {
            await Task.Run(() => AddData());
        }

        protected void Button4_Click1(object sender, EventArgs e)
        {
            Response.Redirect("ForecastPage.aspx");
        }
        
        protected void MakeCountList()
        {

        }
    }
}