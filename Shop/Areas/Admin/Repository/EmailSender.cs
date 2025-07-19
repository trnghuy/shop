
using System.Net.Mail;
using System.Net;

namespace Shop.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("dtm240603@gmail.com", "uunaleoetvcsinoq")
            };

            return client.SendMailAsync(
                new MailMessage(from: "dtm240603@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
