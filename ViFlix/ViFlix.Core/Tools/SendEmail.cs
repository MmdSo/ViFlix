using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FirstShop.Core.Tools
{
    public class SendEmail
    {
        public static void Send(string to, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("goldenscorpion3@gmail.com", "ViFlix"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(to.Trim());

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("goldenscorpion3@gmail.com", "bssr gkpr prmu ugpf");
                    smtp.EnableSsl = true;

                     smtp.SendMailAsync(mail);
                }
                Console.WriteLine("Email successifully sended! 🚀");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in sending email: {ex.Message}");
            }

            //try
            //{
            //    MailMessage mail = new MailMessage();
            //    mail.To.Add(to.Trim());
            //    mail.From = new MailAddress("goldenscorpion3@gmail.com", "حاج محمود");
            //    mail.Subject = subject;
            //    mail.Body = body;
            //    mail.IsBodyHtml = true;
            //    SmtpClient smtp = new SmtpClient();
            //    smtp.Credentials = new NetworkCredential("goldenscorpion3@gmail.com", "Mmd@1690230150");
            //    smtp.Host = "smtp.gmail.com";
            //    smtp.Port = 587;
            //    //Or your Smtp Email ID and Password
            //    //smtp.UseDefaultCredentials = true;
            //    smtp.EnableSsl = true;
            //    smtp.Send(mail);
            //}
            //catch {}
        }
    }
}
