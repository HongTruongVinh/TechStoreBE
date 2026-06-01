using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Cart;

namespace TechStore.Service.Interfaces
{
    public interface ICartService
    {
        Task<ServiceResult<CartItemResponseModel>> AddToCart(string userId, CartItemUpdateModel model);
        Task<ServiceResult<List<CartItemResponseModel>>> RemoveCartItems(string userId, List<string> listProductId);
        Task<ServiceResult<bool>> UpdateCart(string userId, CartItemUpdateModel model);
        Task<ServiceResult<List<CartItemResponseModel>>> GetCartItems(string userId, int pageNumber, int pageSize);
        Task<ServiceResult<bool>> ClearCart(string userId);
    }
}
