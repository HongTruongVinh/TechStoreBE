using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Home;
using TechStore.Model.DTOs.Product;

namespace TechStore.Service.Interfaces
{
    public interface IHomeService
    {
        Task<ServiceResult<HomeResponseModel>> GetHomeProduct();
        Task<ServiceResult<List<string>>> GetHotProdctsImage();


        Task<ServiceResult<List<ListItemProductModel>>> GetFeaturedProducts();
        Task<ServiceResult<List<ListItemProductModel>>> GetProductsByBrandName(string brandName, int page, int pageSize);
    }
}
