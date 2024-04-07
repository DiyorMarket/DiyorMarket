namespace DiyorMarket.Domain.Interfaces.Services
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(string email, string subject, string massege);
    }
}
