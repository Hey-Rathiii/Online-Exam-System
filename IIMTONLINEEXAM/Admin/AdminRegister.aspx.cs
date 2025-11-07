using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace IIMTONLINEEXAM.Admin
{
    public partial class AdminRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string contact = txtContact.Text.Trim();

            // ✅ Server-side validation (important even if client-side is done)
            if (string.IsNullOrEmpty(fullName) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword) ||
                string.IsNullOrEmpty(contact))
            {
                lblMessage.Text = "All fields are required.";
                return;
            }

            if (password != confirmPassword)
            {
                lblMessage.Text = "Passwords do not match.";
                return;
            }

            // Hash password before saving
            string hashedPassword = HashPassword(password);

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["DBConcetion"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // Check if email already exists
                    string checkQuery = "SELECT COUNT(*) FROM Admins WHERE Email=@Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int exists = (int)checkCmd.ExecuteScalar();
                        if (exists > 0)
                        {
                            lblMessage.Text = "Email already registered.";
                            return;
                        }
                    }

                    // Insert new admin
                    string insertQuery = "INSERT INTO Admins (FullName, Email, PasswordHash, ContactNumber) VALUES (@FullName, @Email, @PasswordHash, @ContactNumber)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        cmd.Parameters.AddWithValue("@ContactNumber", contact);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            lblMessage.Text = "Registration successful!";
                            ClearForm();
                        }
                        else
                        {
                            lblMessage.Text = "Error during registration. Try again.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
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
