using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Service.Interfaces
{
    public interface IVietQrService
    {
        Task<string?> GenerateQrAsync(decimal amount, string content);
    }
}
