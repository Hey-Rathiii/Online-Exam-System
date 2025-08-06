using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamLibrary.DAL;

namespace IIMTONLINEEXAM.Admin
{
    public partial class VerifyEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Guid ActivationId = Guid.Parse(Request.QueryString["id"]);
                AdminDAL.IsActivation(ActivationId);

                lblmsg.Text = "You have met all the Requiremnts. Email Verified!";
            }
            catch (Exception ex)
            {
                lblmsg.Text = "You are not Verifed " + ex.Message;
            }
        }
    }
}