using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Domain.Dtos.Request
{
    public record TransactionRequest(Guid ReceiverUserId, decimal Amount);
}
