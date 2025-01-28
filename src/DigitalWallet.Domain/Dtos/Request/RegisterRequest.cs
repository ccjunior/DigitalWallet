namespace DigitalWallet.Domain.Dtos.Request
{
    public record RegisterRequest(string Name, string Email, string PasswordHash);
}
