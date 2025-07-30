using ExamLibrary.DAL;
using ExamLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIMTONLINEEXAM.Admin
{
    public partial class AdminRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                AdminDTO admin = new AdminDTO
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    PasswordHash = txtPassword.Text, // Ideally, hash before storing!
                    ContactNumber = txtContact.Text,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    ActivationId = Guid.NewGuid()
                };

                AdminDAL dal = new AdminDAL();
                bool isSuccess = dal.RegisterAdmin(admin);

                if (isSuccess)
                {
                    lblMessage.Text = "Admin registered successfully!";
                    lblMessage.CssClass = "text-success";
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
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