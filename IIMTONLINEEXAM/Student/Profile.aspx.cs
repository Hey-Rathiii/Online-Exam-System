using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace IIMTONLINEEXAM.Student
{
    public partial class Profile : System.Web.UI.Page
    {
        string cs = System.Configuration.ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // If not logged in, go back to login
                if (Session["StudentID"] == null)
                {
                    Response.Redirect("Login.aspx");
                }

                LoadProfile();
            }
        }

        // ---------------------------------------
        //  LOAD PROFILE
        // ---------------------------------------
        void LoadProfile()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetStudentProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentID", Session["StudentID"]);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtFullName.Text = dr["FullName"].ToString();
                    txtEmail.Text = dr["Email"].ToString();
                    txtContact.Text = dr["ContactNumber"].ToString();

                    string photo = dr["PhotoPath"].ToString();

                    if (!string.IsNullOrEmpty(photo))
                        imgProfile.ImageUrl = photo;
                    else
                        imgProfile.ImageUrl = "~/Student/profile-default.png";
                }
                con.Close();
            }
        }

        // ---------------------------------------
        //  UPDATE PROFILE DETAILS
        // ---------------------------------------
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateStudentProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentID", Session["StudentID"]);
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@ContactNumber", txtContact.Text.Trim());

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            lblMessage.Text = "Profile updated successfully!";
            lblMessage.ForeColor = System.Drawing.Color.LightGreen;

            LoadProfile();
        }

        // ---------------------------------------
        //  SAVE PROFILE PHOTO
        // ---------------------------------------
        protected void btnSavePhoto_Click(object sender, EventArgs e)
        {
            if (!filePhoto.HasFile)
            {
                lblMessage.Text = "Please choose a photo.";
                lblMessage.ForeColor = System.Drawing.Color.LightCoral;
                return;
            }

            string ext = Path.GetExtension(filePhoto.FileName).ToLower();

            // Allowed formats
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
            {
                lblMessage.Text = "Only JPG or PNG images allowed!";
                lblMessage.ForeColor = System.Drawing.Color.LightCoral;
                return;
            }

            // Upload folder
            string folder = "~/Student/Uploads/";

            // Create folder if missing
            if (!Directory.Exists(Server.MapPath(folder)))
                Directory.CreateDirectory(Server.MapPath(folder));

            // Unique filename
            string fileName = "STU_" + Session["StudentID"] + "_" + DateTime.Now.Ticks + ext;
            string fullPath = folder + fileName;

            // Save file
            filePhoto.SaveAs(Server.MapPath(fullPath));

            // Update DB
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateStudentPhoto", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentID", Session["StudentID"]);
                cmd.Parameters.AddWithValue("@PhotoPath", fullPath);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            lblMessage.Text = "Profile photo updated!";
            lblMessage.ForeColor = System.Drawing.Color.LightGreen;

            // Update UI immediately
            imgProfile.ImageUrl = fullPath;
        }
    }
}
