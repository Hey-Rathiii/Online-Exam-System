using System;
using ExamLibrary.DAL;

namespace IIMTONLINEEXAM.Admin
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "⚠️ Email and Password are required.";
                lblMessage.CssClass = "text-danger";
                return;
            }

            try
            {
                // Call DAL (or stored procedure) to validate login
                AdminLoginResult result = AdminDAL.IsAdminValid(email, password);

                if (result.ResultCode == 0)
                {
                    lblMessage.Text = "❌ Invalid email or password.";
                    lblMessage.CssClass = "text-danger";
                }
                else if (result.ResultCode == 1)
                {
                    lblMessage.Text = "🚫 Your account is inactive.";
                    lblMessage.CssClass = "text-warning";
                }
                else if (result.ResultCode == 2 && result.AdminId.HasValue)
                {
                    Session["AdminID"] = result.AdminId.Value;
                    Session["AdminEmail"] = email;
                    Response.Redirect("Dashboard.aspx"); // ✅ change this to your dashboard page
                }
                else
                {
                    lblMessage.Text = "⚠️ Unexpected error occurred.";
                    lblMessage.CssClass = "text-danger";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Error: " + ex.Message;
                lblMessage.CssClass = "text-danger";
            }
        }
    }
}
