using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VaccinationManager.CommunicationAPI
{
    public class MailService
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public MailService(string name, string pass)
        {
            Username = name;
            Password = pass;
        }

        public void SendMail(string toAddress, string subject, string body)
        {
            //var smtp = new System.Net.Mail.SmtpClient();
            //{
            //    smtp.Host = "smtp.mweb.co.za";
            //    smtp.Port = 587;
            //    smtp.EnableSsl = false;
            //    smtp.UseDefaultCredentials = true;
            //    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //    smtp.Credentials = new NetworkCredential(Username, Password);
            //    smtp.Timeout = 20000;
            //}
            
            //    MailMessage message = new MailMessage( new MailAddress("do-not-reply@vaccinationmanager.co.za"), new MailAddress(toAddress));
            //message.Body = body;
            //message.BodyEncoding = Encoding.UTF8;
            //message.Subject = subject;
            //message.IsBodyHtml = false;

            //smtp.SendCompleted += (s, e) => {
            //    smtp.Dispose();
            //    message.Dispose();
            //};
            //smtp.SendAsync(message, null);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("vaccination@hileya.co.za");
                mail.To.Add(toAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                // Can set to false, if you are sending pure text.


                Task t = Task.Run(async () =>
                {
                    using (SmtpClient smtp = new SmtpClient("smtp.mweb.co.za", 25))
                    {
                        smtp.Credentials = new NetworkCredential(Username, Password);
                        smtp.EnableSsl = false;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    
                        await smtp.SendMailAsync(mail);
                    }
                }); t.Wait();
            }
        }

        public async void SendMailHtml(string toAddress, string body)
        {
            return;
        }
    }
}