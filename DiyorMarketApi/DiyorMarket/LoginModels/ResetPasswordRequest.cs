namespace DiyorMarket.LoginModels
{
    public class ResetPasswordRequest
    {
        public string Login { get; set; }
        public string ResetCode { get; set; }
        public string newPassword { get; set; }
    }
}
