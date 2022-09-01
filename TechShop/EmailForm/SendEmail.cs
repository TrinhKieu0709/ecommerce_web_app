using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace TechShop.EmailForm
{
    public class SendEmail
    {
        public static bool GmailSend(string SenderGmail, string Subject, string Message, bool IsBodyHtml = false)
        {
            bool status = false;
            try
            {
                string Host = ConfigurationManager.AppSettings["SMTPHost"].ToString();
                string Port = ConfigurationManager.AppSettings["SMTPort"].ToString();
                string Gmail = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                string Password = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(Gmail);
                mailMessage.Subject = Subject;
                mailMessage.Body = Message;
                mailMessage.IsBodyHtml = IsBodyHtml;
                mailMessage.To.Add(new MailAddress(SenderGmail));
                SmtpClient smtp = new SmtpClient();
                smtp.Host = Host;
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential();
                networkCredential.UserName = mailMessage.From.Address;
                networkCredential.Password = Password;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = Convert.ToInt32(Port);
                smtp.Send(mailMessage);
                status = true;
                return status;
            }
            catch (Exception)
            {
                return status;
            }
        }
    }
}