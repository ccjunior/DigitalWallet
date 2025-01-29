namespace DigitalWallet.Domain.Dtos.Request
{
    public record TransferRequest(Guid FromWalletId, Guid ToWalletId, decimal Amount);
}
