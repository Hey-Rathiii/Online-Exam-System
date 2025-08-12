using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamLibrary.DAL;

namespace IIMTONLINEEXAM.Student
{
	public partial class VerifyEmail : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

           
            try
            {
                Guid ActivationId = Guid.Parse(Request.QueryString["id"]);
                StudentDAL.isActivation(ActivationId);
                lblmsg.Text = "✅ Your ID has been successfully activated. You can now login.";
            }
            catch (Exception ex)
            {
                lblmsg.Text = "❌ Activation failed. Please try again later.<br/><small>" + ex.Message + "</small>";
            }
        


    }
}
}