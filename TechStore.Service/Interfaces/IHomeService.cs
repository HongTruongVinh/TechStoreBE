using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Home;

namespace TechStore.Service.Interfaces
{
    public interface IHomeService
    {
        Task<ServiceResult<HomeResponseModel>> GetHomeProduct();
        Task<ServiceResult<List<string>>> GetHotProdctsImage();
    }
}
