using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Service.Interfaces
{
    public interface IPaymentSnapshotExpirationService
    {
        Task ExpireAsync(CancellationToken cancellationToken);
    }
}
