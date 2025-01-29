namespace DigitalWallet.Domain.Dtos.Response
{
    public record UserFullResponseDto(Guid UserId, string Name, string Email, string PasswordHash, DateTime DateCreated, decimal Balance);
}
