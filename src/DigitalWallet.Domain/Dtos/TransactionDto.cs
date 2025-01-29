using DigitalWallet.Domain.Enum;

namespace DigitalWallet.Domain.Dtos
{
    public record TransctionDto(Guid TransctionId, TransactionType type, decimal amount);
}
