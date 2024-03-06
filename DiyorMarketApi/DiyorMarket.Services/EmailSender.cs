using DiyorMarket.Domain.Interfaces.Services;
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
        }
    }
}
