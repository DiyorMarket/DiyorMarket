namespace DiyorMarket.Domain.Interfaces.Services
{
    public interface IEmailSender
    {
        Task SendEmail(string email, string subject, string massege);
    }
}
