using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Category;
using TechStore.Service.Implementations;

namespace TechStore.Service.Interfaces
{
    public class InitialDataService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceGeneratorService;
        private readonly ICategoryService _categoryService;

        public InitialDataService(IUnitOfWork uow, 
            SequenceGeneratorService sequenceGeneratorService,
            ICategoryService categoryService
            )
        {
            _uow = uow;
            _sequenceGeneratorService = sequenceGeneratorService;
            _categoryService = categoryService;
        }

        public async Task<string> InitData()
        {
            try
            {
                #region add category
                CategoryCreateModel category1 = new CategoryCreateModel
                {

                    Name = "Điện thoại",
                    Description = "Điện thoại thông minh",
                    Slug = "dien-thoai",
                    IconImageUrl = "https://img.icons8.com/?size=100&id=62862&format=png&color=000000"
                };

                CategoryCreateModel category2 = new CategoryCreateModel
                {
                    Name = "Laptop",
                    Description = "Máy tính xách tay",
                    Slug = "laptop",
                    IconImageUrl = CloudinaryFolders.DefaultImage
                };

                CategoryCreateModel category3 = new CategoryCreateModel
                {
                    Name = "Tablet",
                    Description = "Máy tính bảng",
                    Slug = "may-tinh-bang",
                    IconImageUrl = CloudinaryFolders.DefaultImage
                };

                CategoryCreateModel category4 = new CategoryCreateModel
                {
                    Name = "Smart Watch",
                    Description = "Đồng hồ thông minh",
                    Slug = "smart-watch",
                    IconImageUrl = CloudinaryFolders.DefaultImage
                };

                CategoryCreateModel category5 = new CategoryCreateModel
                {
                    Name = "Charge",
                    Description = "Sạc",
                    Slug = "cuc-sac",
                    IconImageUrl = CloudinaryFolders.DefaultImage
                };

                var resultCategory1 = await _categoryService.AddCategory(category1);
                var resultCategory2 = await _categoryService.AddCategory(category2);
                var resultCategory3 = await _categoryService.AddCategory(category3);
                var resultCategory4 = await _categoryService.AddCategory(category4);
                var resultCategory5 = await _categoryService.AddCategory(category5);

                #endregion


                return "Them du lieu thanh cong";
            }
            catch
            {
                return "Co loi xay ra khi them du lieu mau";
            }
        }

        public async Task<JsonResult> GetAllInitData()
        {
            //var users = await _userRepository.GetAllAsync();
            var categories = await _uow.Categories.GetAllAsync();
            //var brands = await _brandRepository.GetAllAsync();
            //var products = await _productRepository.GetAllAsync();
            //var orders = await _orderRepository.GetAllAsync();
            //var carts = await _cartRepository.GetAllAsync();
            //var qrCodes = await _qrCodeRepository.GetAllAsync();
            //var shippers = await _shipperRepository.GetAllAsync();
            //var shippingDetails = await _shippingDetailRepository.GetAllAsync();
            //var invalidTokens = await _invalidTokenRepository.GetAllAsync();

            //if (users.Count() == 0)
            //{
            //    return new JsonResult("Khong co du lieu");
            //}

            //var user1CartItems = await _cartRepository.GetProductsInCart(users.ElementAt(0).UserId);
            //var user2CartItems = await _cartRepository.GetProductsInCart(users.ElementAt(1).UserId);
            //var user3CartItems = await _cartRepository.GetProductsInCart(users.ElementAt(2).UserId);

            var data = new
            {
                //User = users,
                Category = categories,
                //Brand = brands,
                //Product = products,
                //Order = orders,
                //User1CartItems = user1CartItems,
                //User2CartItems = user2CartItems,
                //User3CartItems = user3CartItems,
                //QRCode = qrCodes,
                //Shippers = shippers,
                //ShippingDetails = shippingDetails,
                //InvalidTokens = invalidTokens
            };

            return new JsonResult(data);
        }

        public async Task<bool> DeleteAllInitData()
        {
            try
            {
                //await _userRepository.DeleteAllItemsAsync();
                await _uow.Categories.DeleteAllAsync();
                //await _productRepository.DeleteAllItemsAsync();
                //await _orderRepository.DeleteAllItemsAsync();
                //await _cartRepository.DeleteAllItemsAsync();
                //await _sequenceService.ResetSequenceAsync();
                //await _cartItemRepository.DeleteAllItemsAsync();
                //await _invoiceRepository.DeleteAllItemsAsync();
                //await _paymentRepository.DeleteAllItemsAsync();
                //await _brandRepository.DeleteAllItemsAsync();
                //await _orderItemRepository.DeleteAllItemsAsync();
                //await _qrCodeRepository.DeleteAllItemsAsync();
                //await _shipperRepository.DeleteAllItemsAsync();
                //await _shippingDetailRepository.DeleteAllItemsAsync();
                //await _invalidTokenRepository.DeleteAllItemsAsync();

                //await _uploadDataToCloudService.DeleteFolderAsync(CloudinaryFolders.Products);
                //await _uploadDataToCloudService.DeleteFolderAsync(CloudinaryFolders.Brands);
                //await _uploadDataToCloudService.DeleteFolderAsync(CloudinaryFolders.Categories);

                await _uow.CommitAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> ResetData()
        {
            string result = "";
            try
            {
                await DeleteAllInitData();
                result += "Xóa dữ liệu thành công";

                await InitData();
                result += " và khởi tạo lại dữ liệu mẫu thành công";

                return result;
            }
            catch
            {
                return result += "\ncó lỗi";
            }
        }
    }
}
