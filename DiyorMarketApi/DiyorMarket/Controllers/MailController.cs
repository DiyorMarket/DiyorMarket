using DiyorMarket.Constants;
using DiyorMarket.Domain.Interfaces.Services;
using DocumentFormat.OpenXml.Wordprocessing;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace DiyorMarket.Controllers
{
    [Route("api/mail")]
    [ApiController]
    public class MailController : Controller
    {
        private readonly IEmailSender _emailSender;

        public MailController() { }
        public MailController(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        [HttpPost("register")]
        public async Task<ActionResult> SendRegisterEmail(string receiverEmail, string? name)
        {
            string subject = EmailConfigurations.Subject;
            string emailBody = EmailConfigurations.RegisterBody.Replace("{recipientName}", name);

            await _emailSender.SendEmail(receiverEmail, subject, emailBody);

            return Ok();
        }
    }
}
