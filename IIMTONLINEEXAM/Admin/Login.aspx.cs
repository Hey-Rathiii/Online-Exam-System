using System;
using ExamLibrary.DAL;

namespace IIMTONLINEEXAM.Admin
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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

            lblMessage.Text = "";
            lblMessage.CssClass = "";

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "⚠️ Please enter both Email and Password.";
                lblMessage.CssClass = "text-danger fw-semibold text-center d-block";
                return;
            }

            // ✅ Call DAL to validate credentials
            var LoginResult = AdminDAL.IsAdminValid(email, password);
            int resultcode = LoginResult.ResultCode;

            if (resultcode == 0)
            {
                lblMessage.Text = "❌ Invalid email or password.";
                lblMessage.CssClass = "text-danger fw-semibold text-center d-block";
            }
            else if (resultcode == -1)
            {
                lblMessage.Text = "🚫 Your account is inactive.";
                lblMessage.CssClass = "text-warning fw-semibold text-center d-block";
            }
            else if (resultcode > 0)
            {
                // ✅ Store only the AdminID (integer) in Session
                Session["AdminID"] = LoginResult.AdminID;
                Session["AdminEmail"] = email;

                Response.Redirect("~/Admin/Dashboard.aspx");
            }

            else
            {
                lblMessage.Text = "⚠️ Unexpected error occurred.";
                lblMessage.CssClass = "text-danger fw-semibold text-center d-block";
            }

            txtEmail.Text = "";
            txtPassword.Text = "";
        }
    }
}
