namespace DiyorMarket.Domain.DTOs.Registration;

public record RegisterRequestDto(string Login, string Password, string FullName, string Phone);