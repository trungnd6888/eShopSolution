using MailKit.Net.Smtp;
using MimeKit;

namespace eShopSolution.Application.Common.Mail
{
    public class MailService : IMailService
    {
        public void SentMail(string content, string emailTo)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("eShop@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailTo));

            email.Subject = "Send mail from Asp.net Core";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = content
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("judson6@ethereal.email", "FZY724cGPw2M3EhCwk");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
