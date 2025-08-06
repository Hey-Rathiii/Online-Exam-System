using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIMTONLINEEXAM.Student
{
	public partial class Home : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["StudentID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Display welcome message (you can customize based on session values if stored)
            if (!IsPostBack)
            {
                lblStudentGreeting.Text = "Welcome, Student ID: " + Session["StudentID"].ToString();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}