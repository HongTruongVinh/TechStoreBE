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
        Task<ServiceResult<bool>> AddToCart(string userId, CartItemUpdateModel model);
        Task<ServiceResult<bool>> RemoveFromCart(string userId, CartItemUpdateModel model);
        Task<ServiceResult<bool>> UpdateCart(string userId, CartItemUpdateModel model);
        Task<ServiceResult<List<CartItemResponseModel>>> GetCartItems(string userId);
        Task<ServiceResult<bool>> ClearCart(string userId);
    }
}
