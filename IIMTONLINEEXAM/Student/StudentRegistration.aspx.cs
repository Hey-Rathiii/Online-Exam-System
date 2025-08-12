using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamLibrary.Model;
using ExamLibrary.DAL;
using OnlineExamSystem.Helper;


namespace IIMTONLINEEXAM.Student
{
    public partial class StudentRegistration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFullName.Text = "";
                txtEmail.Text = "";
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                txtContact.Text = "";
                lblMessage.Text = "";
                lblMessage.CssClass = "";
                // Set attributes for required fields

                txtFullName.Attributes.Add("required", "required");
                txtEmail.Attributes.Add("required", "required");
                txtPassword.Attributes.Add("required", "required");
                txtConfirmPassword.Attributes.Add("required", "required");
                txtContact.Attributes.Add("required", "required");
            }

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {

            AdminDTO Student = new AdminDTO
            {
                FullName = txtFullName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                PasswordHash = txtPassword.Text,
                ContactNumber = txtContact.Text
            };

            Guid ActivationId = Student.ActivationId;

            bool isRegister = StudentDAL.RegisterStudent(Student);

            if (!isRegister)
            {
                lblMessage.Text = "⚠️ You are already registered.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblMessage.Text = "✅ Successfully Registered!";
                lblMessage.ForeColor = System.Drawing.Color.DarkGreen;

                // Construct verification email
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                string verifyUrl = baseUrl + $"/Student/VerifyEmail.aspx?id={ActivationId}";

                string strSubject = "Email Verification";
                string strBody = "Please click the link below to verify your email address:<br/>";
                strBody += $"<a href='{verifyUrl}'>Verify Email</a>";

                bool isEmailSent = MailHelper.SendVerificationEmail(txtEmail.Text, strSubject, strBody);

                if (isEmailSent)
                {
                    lblMessage.Text += "<br/>📧 Verification email sent successfully.";
                }
                else
                {
                    lblMessage.Text += "<br/>❌ Failed to send verification email.";
                }

                // 🔄 Clear form fields after successful registration
                txtFullName.Text = "";
                txtEmail.Text = "";
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                txtContact.Text = "";
            }
        }
        

       
    }
}