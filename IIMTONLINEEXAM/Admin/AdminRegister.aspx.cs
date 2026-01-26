using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ExamLibrary.Model;
using ExamLibrary.DAL;
using OnlineExamSystem.Helper;

namespace IIMTONLINEEXAM.Admin
{
    public partial class AdminRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear fields on first load
                txtFullName.Text = "";
                txtEmail.Text = "";
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                txtContact.Text = "";
                lblMessage.Text = "";
                lblMessage.CssClass = "";

                // Mark as required (same style as StudentRegistration)
                txtFullName.Attributes.Add("required", "required");
                txtEmail.Attributes.Add("required", "required");
                txtPassword.Attributes.Add("required", "required");
                txtConfirmPassword.Attributes.Add("required", "required");
                txtContact.Attributes.Add("required", "required");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string contact = txtContact.Text.Trim();

            // Optional server-side check for password match
            if (password != confirmPassword)
            {
                lblMessage.Text = "⚠️ Password and Confirm Password do not match.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            // 1️⃣ Prepare DTO (password stored as entered – NOT hashed)
            AdminDTO admin = new AdminDTO
            {
                FullName = fullName,
                Email = email,
                PasswordHash = password,      // plain password, like Student
                ContactNumber = contact,
                CreatedDate = DateTime.Now,  // ✅ now a real date, not NULL
                IsActive = false,         // ✅ will become 1 only after email verify
                ActivationId = Guid.NewGuid() // you can set explicitly
                // CreatedDate, IsActive, ActivationId are set by default in AdminDTO
            };

            Guid activationId = admin.ActivationId;

            bool isRegister;

            try
            {
                // 2️⃣ Call DAL to register admin via stored procedure AdminRegister
                AdminDAL dal = new AdminDAL();
                isRegister = dal.RegisterAdmin(admin);
            }
            catch (Exception ex)
            {
                // Handles "This email is already registered." and other errors
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = Color.Red;
                return;
            }

            if (!isRegister)
            {
                lblMessage.Text = "⚠️ Error while registering admin.";
                lblMessage.ForeColor = Color.Red;
            }
            else
            {
                lblMessage.Text = "✅ Successfully Registered!";
                lblMessage.ForeColor = Color.DarkGreen;

                // 3️⃣ Construct verification email (same pattern as Student)
                string baseUrl = ConfigurationManager.AppSettings["SiteBaseUrl"];
                string verifyUrl = $"{baseUrl}/Admin/VerifyEmail.aspx?id={activationId}";

                string subject = "Admin Email Verification";
                string body = "Please click the link below to verify your email address:<br/>";
                body += $"<a href='{verifyUrl}'>Verify Email</a>";

                bool isEmailSent = MailHelper.SendVerificationEmail(email, subject, body);

                if (isEmailSent)
                {
                    lblMessage.Text += "<br/>📧 Verification email sent successfully.";
                }
                else
                {
                    lblMessage.Text += "<br/>❌ Failed to send verification email.";
                }

                // 4️⃣ Clear form fields after successful registration
                ClearForm();
            }
        }

        private void ClearForm()
        {
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtContact.Text = "";
        }
    }
}
