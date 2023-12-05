using System.Net;
using System.Net.Mail;

namespace OnlineShop
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("daniyalahmed383@gmail.com", "ljrjuylhuvhxykpf")
            };

            return client.SendMailAsync(
                new MailMessage(from: "daniyalahmed383@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}

