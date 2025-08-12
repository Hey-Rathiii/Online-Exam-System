using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamLibrary.DAL;
using ExamLibrary.Model;
using OnlineExamSystem.Helper;

namespace IIMTONLINEEXAM.Admin
{
    public partial class AdminRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void BtnRegister_Click1(object sender, EventArgs e)
        {
            try
            {
                AdminDTO admin = new AdminDTO
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    PasswordHash = txtPassword.Text, // 🔐 Consider hashing
                    ContactNumber = txtContact.Text.Trim(),
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    ActivationId = Guid.NewGuid()
                };

                AdminDAL dal = new AdminDAL();
                bool isRegistered = dal.RegisterAdmin(admin); // returns false if email exists

                if (!isRegistered)
                {
                    lblMessage.Text = "You are already registered with this email.";
                    lblMessage.CssClass = "text-danger";
                    return;
                }

                // Send verification email
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                string verifyUrl = $"{baseUrl}/Admin/VerifyEmail.aspx?id={admin.ActivationId}";

                string subject = "Admin Email Verification - IIMT Online Exam";
                string body = $@"
                    <p>Dear {admin.FullName},</p>
                    <p>Thank you for registering as Admin. Please click the link below to verify your email address:</p>
                    <p><a href='{verifyUrl}'>Verify Email</a></p>
                    <br/>
                    <p>If you did not register, please ignore this message.</p>";

                bool isEmailSent = MailHelper.SendVerificationEmail(admin.Email, subject, body);

                if (isEmailSent)
                {
                    lblMessage.Text = "Admin registered successfully! A verification email has been sent.";
                    lblMessage.CssClass = "text-success";
                }
                else
                {
                    lblMessage.Text = "Admin registered, but failed to send verification email.";
                    lblMessage.CssClass = "text-warning";
                }

                ClearFields();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "text-danger";
            }
        }
        private void ClearFields()
        {
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtContact.Text = "";
        }

    }
}
