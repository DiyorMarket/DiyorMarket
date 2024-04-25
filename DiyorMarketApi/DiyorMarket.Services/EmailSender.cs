using DiyorMarket.Domain.Interfaces.Services;
using System.Net;
using System.Net.Mail;

namespace DiyorMarket.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task<bool> SendEmail(string email, string subject, string massege)
        {
            MailAddress fromAddress = new MailAddress("gieosovazamat@gmail.com", "Inflow Market");
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
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                // Освободить ресурсы SmtpClient
                smtpClient.Dispose();
            }
        }
    }
}
