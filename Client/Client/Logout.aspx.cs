using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie login = new HttpCookie("login", String.Empty);
            HttpCookie sign = new HttpCookie("sign", String.Empty);

            login.Expires = DateTime.Now.AddDays(-1);
            sign.Expires = DateTime.Now.AddDays(-1);

            Response.Cookies.Add(login);
            Response.Cookies.Add(sign);
            Response.Redirect("Login.aspx");
        }
    }
}