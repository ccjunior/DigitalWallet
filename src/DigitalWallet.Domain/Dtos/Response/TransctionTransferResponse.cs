namespace DigitalWallet.Domain.Dtos.Response
{
    public record TransctionTransferResponse(Guid WalletId, string Type, string DateTransaction, decimal Amount);
}
