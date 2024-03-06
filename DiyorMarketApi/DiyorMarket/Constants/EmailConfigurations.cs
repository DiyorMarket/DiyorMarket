using DiyorMarket.Domain.Entities;

namespace DiyorMarket.Constants
{
    public static class EmailConfigurations
    {
        public const string Subject = "Welcome to InflowMarket!";
        public const string RegisterBody = $"Dear {{recipientName}},\r\n\r\nWelcome to InflowMarket! We're thrilled to have you on board.\r\n\r\nAs a registered member of InflowMarket, you now have access to exclusive features and benefits. We're committed to providing you with the best experience possible.\r\n\r\nShould you have any questions or need assistance, please don't hesitate to reach out to our customer support team at [customer support email or phone number]. We're here to help!\r\n\r\nOnce again, thank you for joining InflowMarket. We look forward to serving you and enhancing your experience with us.\r\n\r\nBest regards,\r\n\r\nMr.Azamat\r\nSoftware Engineer\r\nInflowMarket";
        public const string LoginBody = "Dear {{recipientName}},\r\n\r\nThank you for registering with InflowMarket! We're excited to have you as a member of our community.\r\n\r\nIf you encounter any issues or need assistance logging in, please don't hesitate to contact our customer support team at [customer support email or phone number]. We're here to help!\r\n\r\nThank you for choosing InflowMarket. We appreciate your trust in us and look forward to serving you.\r\n\r\nBest regards,\r\n\r\nMr.Azamat\r\nSoftware Engineer\r\nInflowMarket";
    }
}
