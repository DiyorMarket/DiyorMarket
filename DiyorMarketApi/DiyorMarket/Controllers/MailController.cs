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

        public MailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost("login")]
        public async Task<ActionResult> SendLoginEmail(string receiver, string subject, string message)
        {
            //var receiver = "azamatgiasov04@gmail.com";
            //var subject = "Log in";
            //var message = "Hello World";

            await _emailSender.SendEmail(receiver, subject, message);

            return Ok();
        }
    }
}
