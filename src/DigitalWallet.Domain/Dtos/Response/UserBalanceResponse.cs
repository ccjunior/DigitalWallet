namespace DigitalWallet.Domain.Dtos.Response
{
    public record UserBalanceResponse(Guid UserId, string Name, Guid WalletId ,decimal Balance);
}
