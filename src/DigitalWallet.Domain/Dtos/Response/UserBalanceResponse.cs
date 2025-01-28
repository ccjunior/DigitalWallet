using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Domain.Dtos.Response
{
    public record UserBalanceResponse(Guid UserId, string Name, decimal Balance);
}
