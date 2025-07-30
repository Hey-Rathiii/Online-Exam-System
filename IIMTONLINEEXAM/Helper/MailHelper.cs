using System;
using System.Net;
using System.Net.Mail;

namespace OnlineExamSystem.Helper
{
    public class MailHelper
    {
        public static bool SendVerificationEmail(string toEmail, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("Edu.paarth@gmail.com", " PAARTH INSTITUTE Online Exam System");
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("nitindagar8920@gmail.com", "wgpq wfur vqfp altb");
                smtp.EnableSsl = true;

                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}