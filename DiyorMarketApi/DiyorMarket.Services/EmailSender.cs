using DiyorMarket.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Net.Mail;

namespace DiyorMarket.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmail(string email, string subject, string massege)
        {
            MailAddress fromAddress = new MailAddress("gieosovazamat@gmail.com", "Azamat");
            MailAddress toAddress = new MailAddress(email);
            MailMessage mailMessage = new MailMessage(fromAddress, toAddress);
            mailMessage.Body = massege;
            mailMessage.Subject = subject;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("gieosovazamat@gmail.com", "dhrk hrxo qrec cedz");

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                // Письмо успешно отправлено
            }
            catch (Exception ex)
            {
                // Обработать исключение
                throw new Exception($"Error: {ex}");
            }
            finally
            {
                // Освободить ресурсы SmtpClient
                smtpClient.Dispose();
            }

            //var mail = "gieosovazamat@gmail.com";
            //var password = "dhrk hrxo qrec cedz";
            //var smtpHost = "smtp.gmail.com"; // Адрес SMTP-сервера Outlook
            //var smtpPort = 587; // Порт SMTP-сервера Outlook

            //using (var client = new SmtpClient(smtpHost, smtpPort))
            //{
            //    client.EnableSsl = true;
            //    client.UseDefaultCredentials = false;
            //    client.Credentials = new NetworkCredential(mail, password);

            //    var mailMessage = new MailMessage(mail, email)
            //    {
            //        Subject = subject,
            //        Body = massege,
            //        IsBodyHtml = true // Для отправки HTML-сообщений
            //    };

            //    await client.SendMailAsync(mailMessage);
            //}
        }
    }
}
