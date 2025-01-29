namespace DigitalWallet.Domain.Dtos.Response
{
    public record UserResponseDto(Guid UserId, string Name, string Email, decimal Balance);
}
