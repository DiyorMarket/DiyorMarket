﻿namespace DiyorMarket.LoginModels
{
    public class RegisterRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
    }
}
