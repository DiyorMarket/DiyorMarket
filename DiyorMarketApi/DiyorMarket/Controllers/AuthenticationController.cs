﻿using DiyorMarket.Constants;
using DiyorMarket.Domain.Entities;
using DiyorMarket.Domain.Interfaces.Services;
using DiyorMarket.Extensions;
using DiyorMarket.Infrastructure.Persistence;
using DiyorMarket.LoginModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DiyorMarket.Controllers
{
    [Route("api/auth")]
    [ApiController]

    public class AuthenticationController : ControllerBase
    {
        private readonly DiyorMarketDbContext _context;
        private readonly IEmailSender _emailSender;
        public AuthenticationController(DiyorMarketDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [HttpPost("login")]
        public ActionResult<string> Login(LoginRequest request)
        {
            var user = Authenticate(request.Login, request.Password);

            if (user is null)
            {
                return Unauthorized();
            }

            if (!FindUser(request.Login, request.Password))
            {
                return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("anvarSekretKalitSozMalades"));
            var signingCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Phone));
            claimsForToken.Add(new Claim("name", user.Name));

            var jwtSecurityToken = new JwtSecurityToken(
                "anvar-api",
                "anvar-mobile",
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(5),
                signingCredentials);

            var token = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            var response = new
            {
                Token = token,
                UserId = user.Id,
                UserName = user.Name,
                UserEmail = user.Login,
                UserPhone = user.Phone,
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            var existingUser = FindUser(request.Login);
            if (existingUser != null)
            {
                return Conflict("User with this login already exists.");
            }

            var user = new User
            {
                Login = request.Login,
                Password = request.Password,
                Name = request.FullName,
                Phone = request.Phone,
                Role = request.Role.ToLower().FirstLetterToUpper()
            };

            bool isEmailSented = await _emailSender.SendEmail(request.Login, EmailConfigurations.Subject, EmailConfigurations.RegisterBody.Replace("{recipientName}", request.FullName));

            if (!isEmailSented)
            {
                return StatusCode(500, "Failed to send email. Please try again later.");
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            if(request.Role.Trim().ToLower() == "customer")
            {
                AddCustomerWithUser(user);
            }

            _emailSender.SendEmail(request.Login, EmailConfigurations.Subject, EmailConfigurations.RegisterBody.Replace("{recipientName}", request.FullName));

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("anvarSekretKalitSozMalades"));
            var signingCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Phone));
            claimsForToken.Add(new Claim("name", user.Name));

            var jwtSecurityToken = new JwtSecurityToken(
                "anvar-api",
                "anvar-mobile",
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(30),
                signingCredentials);

            var token = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            var response = new
            {
                Token = token,
                UserId = user.Id,
                UserName = user.Name,
                UserEmail = user.Login,
                UserPhone = user.Phone,
            };

            return Ok(response);
        }

        [HttpPost("forgotPassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = FindUser(request.Login);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var resetCode = GenerateResetCode();

            bool isCodeSent = await _emailSender.SendEmail(request.Login, "Password Reset Code", $"Your reset code is: {resetCode}");

            if (!isCodeSent)
            {
                return StatusCode(500, "Failed to send reset code. Please try again later.");
            }

            user.ResetCode = resetCode;
            user.ResetCodeCreatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok("Reset code sent successfully.");
        }

        [HttpPost("resetPassword")]
        public ActionResult ResetPassword(ResetPasswordRequest request)
        {
            var user = FindUser(request.Login);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.ResetCode != request.ResetCode)
            {
                return BadRequest("Invalid or expired reset code.");
            }

            if (user.ResetCodeCreatedAt.HasValue && (DateTime.UtcNow - user.ResetCodeCreatedAt.Value).TotalMinutes > 30)
            {
                return BadRequest("Reset code has expired. Please request a new one.");
            }

            user.Password = request.newPassword;
            user.ResetCode = null;
            user.ResetCodeCreatedAt = null;
            _context.SaveChanges();

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("anvarSekretKalitSozMalades"));
            var signingCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Phone));
            claimsForToken.Add(new Claim("name", user.Name));

            var jwtSecurityToken = new JwtSecurityToken(
                "anvar-api",
                "anvar-mobile",
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(30),
                signingCredentials);

            var token = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            var response = new
            {
                Token = token,
                UserId = user.Id,
                UserName = user.Name,
                UserEmail = user.Login,
                UserPhone = user.Phone,
            };

            return Ok(response);
        }

        private string GenerateResetCode()
        {
            var random = new Random();
            int resetCode = random.Next(1000, 9999);
            return resetCode.ToString();
        }

        private bool FindUser(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);

            if (user is null || user.Password != password)
            {
                return false;
            }

            _emailSender.SendEmail(user.Login, EmailConfigurations.Subject, EmailConfigurations.LoginBody.Replace("{recipientName}", user.Name));

            return true;
        }

        private User FindUser(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        private void AddCustomerWithUser(User user)
        {
            var customer = new Customer
            {
                FullName = user.Name,
                PhoneNumber = user.Phone,
                UserId = user.Id
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        private User Authenticate(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);

            if (user != null && user.Password == password)
            {
                return new User()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Password = user.Password,
                    Name = user.Name,
                    Phone = user.Phone
                };
            }

            return null;
        }   
    }
}