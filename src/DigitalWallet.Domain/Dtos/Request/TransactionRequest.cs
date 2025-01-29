namespace DigitalWallet.Domain.Dtos.Request
{
    public record TransactionRequest(Guid WalletId, decimal Amount);
}
