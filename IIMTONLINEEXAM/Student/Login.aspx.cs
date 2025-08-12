using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamLibrary.DAL;

namespace IIMTONLINEEXAM.Student
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                // Clear fields on initial page load
                txtEmail.Text = "";
                txtPassword.Text = "";
                lblMessage.Text = "";
                lblMessage.CssClass = "";
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Clear previous message
            lblMessage.CssClass = "";

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "⚠️ Please enter both Email and Password.";
                lblMessage.CssClass = "text-danger fw-semibold text-center d-block";
                return;
            }

            // Call DAL to validate student credentials
            int studentId = StudentDAL.IsStudentValid(email, password);

            if (studentId == 0)
            {
                lblMessage.Text = "❌ Invalid email or password.";
                lblMessage.CssClass = "text-danger fw-semibold text-center d-block";
            }
            else if(studentId== -1)
            {
                lblMessage.Text = "User is not Active";
                lblMessage.CssClass= "text-danger fw-semibold text-center d-block";
            }
            else
            {

                // Valid student - start session and redirect
                Session["StudentID"] = studentId;

                Response.Redirect("/Student/Home.aspx");
            }
            txtEmail.Text = "";
            txtPassword.Text = "";

        }
    }
}