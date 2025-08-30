using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Authentication;
using TechStore.Model.DTOs.Brand;
using TechStore.Model.DTOs.Category;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Product;
using TechStore.Model.DTOs.Shipper;
using TechStore.Model.DTOs.User;
using TechStore.Service.Interfaces;

namespace TechStore.Service.Implementations
{
    public class InitialDataService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceGeneratorService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IShipperService _shipperService;
        private readonly IAuthenticationService _authenticationService;

        public InitialDataService(IUnitOfWork uow, 
            SequenceGeneratorService sequenceGeneratorService,
            ICategoryService categoryService,
            IBrandService brandService,
            IProductService productService,
            IUserService userService,
            IAuthenticationService authenticationService,
            IShipperService shipperService,
            IOrderService orderService)
        {
            _uow = uow;
            _sequenceGeneratorService = sequenceGeneratorService;
            _categoryService = categoryService;
            _brandService = brandService;
            _productService = productService;
            _userService = userService;
            _authenticationService = authenticationService;
            _shipperService = shipperService;
            _orderService = orderService;
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

                #region add brands
                var brand1 = new BrandCreateModel
                {
                    Name = "Apple",
                    Description = "Apple from USA",
                    Slug = "apple",
                    IconImageUrl = "https://img.icons8.com/?size=100&id=uoRwwh0lz3Jp&format=png&color=000000"
                };

                var brand2 = new BrandCreateModel
                {
                    Name = "Samsung",
                    Description = "Samsung from South Korea",
                    Slug = "samsung",
                    IconImageUrl = CloudinaryFolders.DefaultImage
                };

                var brand3 = new BrandCreateModel
                {
                    Name = "Xiaomi",
                    Description = "Xiaomi from China",
                    Slug = "xiaomi",
                    IconImageUrl = CloudinaryFolders.DefaultImage
                };

                var brand4 = new BrandCreateModel
                {
                    Name = "Dell",
                    Description = "Dell from USA",
                    Slug = "dell",
                    IconImageUrl = CloudinaryFolders.DefaultImage
                };

                var brand5 = new BrandCreateModel
                {
                    Name = "Sony",
                    Description = "Sony",
                    Slug = "sony",
                    IconImageUrl = CloudinaryFolders.DefaultImage
                };

                var resultBrand1 = await _brandService.AddBrand(brand1);
                var resultBrand2 = await _brandService.AddBrand(brand2);
                var resultBrand3 = await _brandService.AddBrand(brand3);
                var resultBrand4 = await _brandService.AddBrand(brand4);
                var resultBrand5 = await _brandService.AddBrand(brand5);

                #endregion

                #region add shippers
                var shipper1 = new ShipperCreateModel
                {
                    Name = "Viettel Post",
                    SupportPhone = "1900 8098",
                    Website = "https://viettelpost.com.vn",
                    LogoUrl = CloudinaryFolders.DefaultImage,
                    IsActive = true,
                    Description = "Viettel Post is a leading logistics company in Vietnam, providing fast and reliable delivery services."
                };

                var shipper2 = new ShipperCreateModel
                {
                    Name = "J&T Express",
                    SupportPhone = "1900 8888",
                    Website = "https://jtexpress.vn/",
                    LogoUrl = CloudinaryFolders.DefaultImage,
                    IsActive = true,
                    Description = "Công ty TNHH một thành viên chuyển phát nhanh Thuận Phong"
                };

                var shipper3 = new ShipperCreateModel
                {
                    Name = "Giao Hàng Nhanh",
                    SupportPhone = "1900 1234",
                    Website = "https://ghn.vn/",
                    LogoUrl = CloudinaryFolders.DefaultImage,
                    IsActive = true,
                    Description = "Công ty giao nhận đầu tiên tại Việt Nam được thành lập với sứ mệnh phục vụ nhu cầu vận chuyển chuyên nghiệp của các đối tác Thương mại điện tử trên toàn quốc."
                };

                var resultShipper1 = await _shipperService.AddShipper(shipper1);
                var resultShipper2 = await _shipperService.AddShipper(shipper2);
                var resultShipper3 = await _shipperService.AddShipper(shipper3);

                #endregion

                #region register user
                var admin1 = new UserCreateModel
                {
                    LastName = "Admin",
                    FirstName = "1",
                    PasswordHash = "1",
                    Email = "admin@gmail.com",
                    Address = "HCM",
                    PhoneNumber = "0122334455",
                    Gender = EGender.Male,
                    Birthday = new DateTime(2002, 2, 28)
                };

                RegisterModel registerAdmin1 = new RegisterModel
                {
                    RegisterIdentifier = admin1.Email,
                    Password = admin1.PasswordHash,
                    RegisterType = ERegisterType.Email,
                    UserInformation = admin1
                };

                var resultRegisterAdmin1 = await _authenticationService.AdminRegisterWithEmail(registerAdmin1);

                if (resultRegisterAdmin1.Data == null)
                {
                    throw new Exception("Đã có lỗi xảy ra trong quá trình tạo tài khoản");
                }

                var user1 = new UserCreateModel
                {
                    LastName = "User1",
                    FirstName = "11",
                    PasswordHash = "string",
                    Email = "user1@gmail.com",
                    Address = "Hà Nội",
                    PhoneNumber = "0123456001",
                    Gender = EGender.Male,
                    Birthday = new DateTime(2002, 2, 28)
                };

                RegisterModel register1 = new RegisterModel
                {
                    RegisterIdentifier = user1.Email,
                    Password = user1.PasswordHash,
                    RegisterType = ERegisterType.Email,
                    UserInformation = user1
                };

                var resultRegister1 = await _authenticationService.Register(register1);

                if (resultRegister1.Data == null)
                {
                    throw new Exception("Đã có lỗi xảy ra trong quá trình tạo tài khoản");
                }

                var user2 = new UserCreateModel
                {
                    LastName = "User2",
                    FirstName = "22",
                    PasswordHash = "1",
                    Email = "user2@gmail.com",
                    Address = "Hà Nội",
                    PhoneNumber = "0123456002",
                    Gender = EGender.Female,
                    Birthday = new DateTime(2002, 2, 28),
                };

                RegisterModel register2 = new RegisterModel
                {
                    RegisterIdentifier = user2.Email,
                    Password = user2.PasswordHash,
                    RegisterType = ERegisterType.Email,
                    UserInformation = user2
                };

                var resultRegister2 = await _authenticationService.Register(register2);

                if (resultRegister2.Data == null)
                {
                    throw new Exception("Đã có lỗi xảy ra trong quá trình tạo tài khoản");
                }

                var user3 = new UserCreateModel
                {
                    LastName = "User3",
                    FirstName = "33",
                    PasswordHash = "1",
                    Email = "user3@gmail.com",
                    Address = "Hà Nội",
                    PhoneNumber = "0123456003",
                    Gender = EGender.Male,
                    Birthday = new DateTime(2002, 2, 28)
                };

                RegisterModel register3 = new RegisterModel
                {
                    RegisterIdentifier = user3.Email,
                    Password = user3.PasswordHash,
                    RegisterType = ERegisterType.Email,
                    UserInformation = user3
                };

                var resultRegister3 = await _authenticationService.Register(register3);

                if (resultRegister3.Data == null)
                {
                    throw new Exception("Đã có lỗi xảy ra trong quá trình tạo tài khoản");
                }

                for (int i = 4; i <= 14; i++)
                {
                    var user = new UserCreateModel
                    {
                        LastName = $"User{i}",
                        FirstName = $"{i}",
                        PasswordHash = "1",
                        Email = $"user{i}@gmail.com",
                        Address = "Hà Nội",
                        PhoneNumber = $"0912345{i:D3}",
                        Gender = EGender.Male,
                        Birthday = new DateTime(2002, 2, 28)
                    };

                    var register = new RegisterModel
                    {
                        RegisterIdentifier = user.Email,
                        Password = user.PasswordHash,
                        RegisterType = ERegisterType.Email,
                        UserInformation = user
                    };

                    var resultRegister = await _authenticationService.Register(register);
                }

                for (int i = 1; i <= 3; i++)
                {
                    var user = new UserCreateModel
                    {
                        LastName = $"Staff{i}",
                        FirstName = $"{i}",
                        PasswordHash = "1",
                        Email = $"satff{i}@gmail.com",
                        Address = "Hà Nội",
                        PhoneNumber = $"01234567{i:D3}",
                        Gender = EGender.Male,
                        Birthday = new DateTime(2002, 2, 28)
                    };

                    var register = new RegisterModel
                    {
                        RegisterIdentifier = user.Email,
                        Password = user.PasswordHash,
                        RegisterType = ERegisterType.Email,
                        UserInformation = user
                    };

                    var resultRegister = await _authenticationService.UserRegisterWithEmail(register);
                }

                #endregion

                #region add product

                var productModel1 = new ProductCreateModel
                {
                    CategoryId = resultCategory1.Data,
                    BrandId = resultBrand1.Data,
                    Name = "Iphone 14 Pro Max",
                    ShortDescription = "Điện thoại thông minh",
                    Description = "Điện thoại thông minh",
                    Slug = "iphone-14-pro-max",
                    Tag = new List<string> { "iphone", "iphone14" },
                    IsFeatured = true,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 35000000,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    Price = 45000000,
                    Stock = 100,
                    SalePrice = 0
                };

                var productModel2 = new ProductCreateModel
                {
                    CategoryId = resultCategory1.Data,
                    BrandId = resultBrand1.Data,
                    Name = "Iphone 14 Pro",
                    ShortDescription = "Điện thoại thông minh",
                    Description = "Điện thoại thông minh",
                    Slug = "iphone-14-pro",
                    Tag = new List<string> { "iphone", "iphone14" },
                    IsFeatured = true,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 25000000,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    Price = 35000000,
                    Stock = 100,
                    SalePrice = 0
                };

                var productModel3 = new ProductCreateModel
                {
                    CategoryId = resultCategory2.Data,
                    Name = "Dell XPS 13",
                    BrandId = resultBrand4.Data,
                    ShortDescription = "Máy tính xách tay",
                    Description = "Máy tính xách tay",
                    Slug = "dell-xps-13",
                    Tag = new List<string> { "dell", "dell-xps" },
                    IsFeatured = true,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 15000000,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    Price = 25000000,
                    Stock = 100,
                    SalePrice = 0
                };

                var productModel4 = new ProductCreateModel
                {
                    CategoryId = resultCategory2.Data,
                    Name = "Macbook Pro 16",
                    BrandId = resultBrand1.Data,
                    ShortDescription = "Máy tính xách tay",
                    Description = "Máy tính xách tay",
                    Slug = "mac-pro-16",
                    Tag = new List<string> { "mac", "macbook" },
                    IsFeatured = true,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 35000000,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    Price = 50000000,
                    Stock = 100,
                    SalePrice = 0
                };

                var productModel5 = new ProductCreateModel
                {
                    CategoryId = resultCategory3.Data,
                    Name = "Ipad Pro 12.9",
                    BrandId = resultBrand1.Data,
                    ShortDescription = "Máy tính bảng",
                    Description = "Máy tính bảng",
                    Slug = "ipad-12-pro",
                    Tag = new List<string> { "ipad", "ipad-pro" },
                    IsFeatured = true,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 15000000,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    Price = 25000000,
                    Stock = 100,
                    SalePrice = 0
                };

                var productModel6 = new ProductCreateModel
                {
                    CategoryId = resultCategory3.Data,
                    Name = "Samsung Galaxy Tab S8",
                    BrandId = resultBrand2.Data,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    ShortDescription = "Máy tính bảng",
                    Description = "Máy tính bảng",
                    Slug = "samsung-galaxy-tab-s8",
                    Tag = new List<string> { "samsung", "samsung-tab" },
                    IsFeatured = true,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 15000000,
                    Price = 20000000,
                    Stock = 100,
                    SalePrice = 0
                };

                var productModel7 = new ProductCreateModel
                {
                    CategoryId = resultCategory3.Data,
                    Name = "Xiaomi mi 8",
                    BrandId = resultBrand3.Data,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    ShortDescription = "Xiao mi",
                    Description = "Xiao mi",
                    Slug = "xiaomi-mi-8",
                    Tag = new List<string> { "xiaomi", "xiaomi-mi8" },
                    IsFeatured = true,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 500000,
                    Price = 8000000,
                    Stock = 70,
                    SalePrice = 0
                };

                var productModel8 = new ProductCreateModel
                {
                    CategoryId = resultCategory1.Data,
                    BrandId = resultBrand1.Data,
                    Name = "iPhone 13",
                    ShortDescription = "Điện thoại thông minh",
                    Description = "iPhone 13 chính hãng Apple",
                    Slug = "iphone-13",
                    Tag = new List<string> { "iphone", "iphone13" },
                    IsFeatured = false,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 18000000,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    Price = 23000000,
                    Stock = 80,
                    SalePrice = 0
                };
                var productModel9 = new ProductCreateModel
                {
                    CategoryId = resultCategory1.Data,
                    BrandId = resultBrand2.Data,
                    Name = "Samsung Galaxy S23 Ultra",
                    ShortDescription = "Flagship Samsung",
                    Description = "Siêu phẩm điện thoại Samsung",
                    Slug = "samsung-galaxy-s23-ultra",
                    Tag = new List<string> { "samsung", "galaxy" },
                    IsFeatured = true,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 27000000,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    Price = 32000000,
                    Stock = 100,
                    SalePrice = 0
                };
                var productModel10 = new ProductCreateModel
                {
                    CategoryId = resultCategory2.Data,
                    BrandId = resultBrand4.Data,
                    Name = "HP Spectre x360",
                    ShortDescription = "Laptop 2-trong-1",
                    Description = "Laptop cao cấp từ HP",
                    Slug = "hp-spectre-x360",
                    Tag = new List<string> { "hp", "laptop" },
                    IsFeatured = false,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 22000000,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    Price = 28000000,
                    Stock = 90,
                    SalePrice = 0
                };
                var productModel11 = new ProductCreateModel
                {
                    CategoryId = resultCategory2.Data,
                    BrandId = resultBrand1.Data,
                    Name = "MacBook Air M2",
                    ShortDescription = "Laptop Apple nhẹ",
                    Description = "MacBook Air với chip M2",
                    Slug = "macbook-air-m2",
                    Tag = new List<string> { "macbook", "apple" },
                    IsFeatured = true,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 24000000,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    Price = 30000000,
                    Stock = 100,
                    SalePrice = 0
                };
                var productModel12 = new ProductCreateModel
                {
                    CategoryId = resultCategory3.Data,
                    BrandId = resultBrand3.Data,
                    Name = "Xiaomi Pad 5",
                    ShortDescription = "Máy tính bảng Xiaomi",
                    Description = "Tablet giá rẻ cấu hình mạnh",
                    Slug = "xiaomi-pad-5",
                    Tag = new List<string> { "xiaomi", "tablet" },
                    IsFeatured = false,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 9000000,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    Price = 12000000,
                    Stock = 60,
                    SalePrice = 0
                };
                var productModel13 = new ProductCreateModel
                {
                    CategoryId = resultCategory1.Data,
                    BrandId = resultBrand3.Data,
                    Name = "Poco X5 Pro",
                    ShortDescription = "Điện thoại chơi game giá rẻ",
                    Description = "Cấu hình mạnh trong phân khúc",
                    Slug = "poco-x5-pro",
                    Tag = new List<string> { "poco", "xiaomi" },
                    IsFeatured = false,
                    StartSellingDate = DateTime.UtcNow,
                    ImportPrice = 6500000,
                    MainImageUrl = CloudinaryFolders.DefaultImage,
                    Price = 8500000,
                    Stock = 100,
                    SalePrice = 0
                };
                var productModel14 = new ProductCreateModel { Name = "Asus ROG Phone 7", CategoryId = resultCategory1.Data, BrandId = resultBrand4.Data, ShortDescription = "Điện thoại gaming", Description = "Asus ROG Phone mạnh mẽ", Slug = "asus-rog-phone-7", Tag = new List<string> { "asus", "rog" }, IsFeatured = true, StartSellingDate = DateTime.UtcNow, ImportPrice = 20000000, MainImageUrl = CloudinaryFolders.DefaultImage, Price = 25000000, Stock = 70, SalePrice = 0 };
                var productModel15 = new ProductCreateModel { Name = "Lenovo Legion 5 Pro", CategoryId = resultCategory2.Data, BrandId = resultBrand4.Data, ShortDescription = "Laptop gaming", Description = "Laptop mạnh mẽ cho game thủ", Slug = "lenovo-legion-5-pro", Tag = new List<string> { "lenovo", "gaming" }, IsFeatured = true, StartSellingDate = DateTime.UtcNow, ImportPrice = 26000000, MainImageUrl = CloudinaryFolders.DefaultImage, Price = 32000000, Stock = 50, SalePrice = 0 };
                var productModel16 = new ProductCreateModel { Name = "iPad Air 2022", CategoryId = resultCategory3.Data, BrandId = resultBrand1.Data, ShortDescription = "Tablet Apple", Description = "iPad Air nhẹ, mạnh", Slug = "ipad-air-2022", Tag = new List<string> { "ipad", "apple" }, IsFeatured = false, StartSellingDate = DateTime.UtcNow, ImportPrice = 15000000, MainImageUrl = CloudinaryFolders.DefaultImage, Price = 18000000, Stock = 100, SalePrice = 0 };
                var productModel17 = new ProductCreateModel { Name = "Surface Pro 9", CategoryId = resultCategory2.Data, BrandId = resultBrand4.Data, ShortDescription = "Tablet lai laptop", Description = "Microsoft Surface Pro mới nhất", Slug = "surface-pro-9", Tag = new List<string> { "microsoft", "surface" }, IsFeatured = true, StartSellingDate = DateTime.UtcNow, ImportPrice = 25000000, MainImageUrl = CloudinaryFolders.DefaultImage, Price = 32000000, Stock = 80, SalePrice = 0 };
                var productModel18 = new ProductCreateModel { Name = "Oppo Find X5", CategoryId = resultCategory1.Data, BrandId = resultBrand2.Data, ShortDescription = "Điện thoại Oppo", Description = "Camera siêu nét", Slug = "oppo-find-x5", Tag = new List<string> { "oppo", "smartphone" }, IsFeatured = false, StartSellingDate = DateTime.UtcNow, ImportPrice = 16000000, MainImageUrl = CloudinaryFolders.DefaultImage, Price = 20000000, Stock = 90, SalePrice = 0 };
                var productModel19 = new ProductCreateModel { Name = "Realme GT Neo 5", CategoryId = resultCategory1.Data, BrandId = resultBrand2.Data, ShortDescription = "Điện thoại Realme", Description = "Pin trâu, sạc siêu nhanh", Slug = "realme-gt-neo-5", Tag = new List<string> { "realme", "smartphone" }, IsFeatured = false, StartSellingDate = DateTime.UtcNow, ImportPrice = 8000000, MainImageUrl = CloudinaryFolders.DefaultImage, Price = 11000000, Stock = 100, SalePrice = 0 };
                var productModel20 = new ProductCreateModel { Name = "Samsung Galaxy Watch 6", CategoryId = resultCategory4.Data, BrandId = resultBrand2.Data, ShortDescription = "Đồng hồ thông minh", Description = "Smartwatch Samsung", Slug = "galaxy-watch-6", Tag = new List<string> { "samsung", "watch" }, IsFeatured = true, StartSellingDate = DateTime.UtcNow, ImportPrice = 5000000, MainImageUrl = CloudinaryFolders.DefaultImage, Price = 7000000, Stock = 120, SalePrice = 0 };
                var productModel21 = new ProductCreateModel { Name = "Apple Watch Series 8", CategoryId = resultCategory4.Data, BrandId = resultBrand1.Data, ShortDescription = "Đồng hồ Apple", Description = "Apple Watch mới nhất", Slug = "apple-watch-series-8", Tag = new List<string> { "apple", "watch" }, IsFeatured = true, StartSellingDate = DateTime.UtcNow, ImportPrice = 7000000, MainImageUrl = CloudinaryFolders.DefaultImage, Price = 9000000, Stock = 100, SalePrice = 0 };
                var productModel22 = new ProductCreateModel { Name = "Sony WH-1000XM5", CategoryId = resultCategory5.Data, BrandId = resultBrand5.Data, ShortDescription = "Tai nghe chống ồn", Description = "Tai nghe Sony cao cấp", Slug = "sony-wh-1000xm5", Tag = new List<string> { "sony", "headphone" }, IsFeatured = false, StartSellingDate = DateTime.UtcNow, ImportPrice = 6000000, MainImageUrl = CloudinaryFolders.DefaultImage, Price = 8000000, Stock = 60, SalePrice = 0 };
                var productModel23 = new ProductCreateModel { Name = "JBL Charge 5", CategoryId = resultCategory5.Data, BrandId = resultBrand5.Data, ShortDescription = "Loa bluetooth", Description = "Loa di động chống nước", Slug = "jbl-charge-5", Tag = new List<string> { "jbl", "bluetooth" }, IsFeatured = false, StartSellingDate = DateTime.UtcNow, ImportPrice = 3000000, MainImageUrl = CloudinaryFolders.DefaultImage, Price = 4000000, Stock = 80, SalePrice = 0 };
                var productModel24 = new ProductCreateModel { Name = "Kindle Paperwhite", CategoryId = resultCategory5.Data, BrandId = resultBrand5.Data, ShortDescription = "Máy đọc sách", Description = "Máy đọc sách Amazon", Slug = "kindle-paperwhite", Tag = new List<string> { "kindle", "ebook" }, IsFeatured = false, StartSellingDate = DateTime.UtcNow, ImportPrice = 2800000, MainImageUrl = CloudinaryFolders.DefaultImage, Price = 3500000, Stock = 90, SalePrice = 0 };

                var resultProduct1 = await _productService.SeedDataProduct(productModel1);
                var resultProduct2 = await _productService.SeedDataProduct(productModel2);
                var resultProduct3 = await _productService.SeedDataProduct(productModel3);
                var resultProduct4 = await _productService.SeedDataProduct(productModel4);
                var resultProduct5 = await _productService.SeedDataProduct(productModel5);
                var resultProduct6 = await _productService.SeedDataProduct(productModel6);
                var resultProduct7 = await _productService.SeedDataProduct(productModel7);
                var resultProduct8 = await _productService.SeedDataProduct(productModel8);
                var resultProduct9 = await _productService.SeedDataProduct(productModel9);
                var resultProduct10 = await _productService.SeedDataProduct(productModel10);
                var resultProduct11 = await _productService.SeedDataProduct(productModel11);
                var resultProduct12 = await _productService.SeedDataProduct(productModel12);
                var resultProduct13 = await _productService.SeedDataProduct(productModel13);
                var resultProduct14 = await _productService.SeedDataProduct(productModel14);
                var resultProduct15 = await _productService.SeedDataProduct(productModel15);
                var resultProduct16 = await _productService.SeedDataProduct(productModel16);
                var resultProduct17 = await _productService.SeedDataProduct(productModel17);
                var resultProduct18 = await _productService.SeedDataProduct(productModel18);
                var resultProduct19 = await _productService.SeedDataProduct(productModel19);
                var resultProduct20 = await _productService.SeedDataProduct(productModel20);
                var resultProduct21 = await _productService.SeedDataProduct(productModel21);
                var resultProduct22 = await _productService.SeedDataProduct(productModel22);
                var resultProduct23 = await _productService.SeedDataProduct(productModel23);
                var resultProduct24 = await _productService.SeedDataProduct(productModel24);



                #endregion


                #region add order

                OrderCreateModel createOrderRequest1 = new OrderCreateModel
                {
                    CustomerId = resultRegister1.Data,
                    CustomerName = user1.LastName,
                    CustomerEmail = user1.Email,
                    ShippingAddress = user1.Address,
                    PaymentMethod = EPaymentMethod.COD,
                    CustomerPhonenumber = user1.PhoneNumber,
                    Items = new List<OrderItemCreateModel>
                {
                    new OrderItemCreateModel
                    {
                        ProductId = resultProduct1,
                        Quantity = 1,
                        PriceAtOrderTime = productModel1.Price,
                        Discount = 0,
                        TotalPrice = productModel1.Price
                    },
                    new OrderItemCreateModel
                    {
                        ProductId = resultProduct2,
                        Quantity = 1,
                        PriceAtOrderTime = productModel2.Price,
                        Discount = 0,
                        TotalPrice = productModel2.Price
                    }
                }
                };

                OrderCreateModel createOrderRequest2 = new OrderCreateModel
                {
                    CustomerId = resultRegister2.Data,
                    CustomerName = user2.LastName,
                    ShippingAddress = user2.Address,
                    CustomerEmail = user2.Email,
                    CustomerPhonenumber = user2.PhoneNumber,
                    PaymentMethod = EPaymentMethod.COD,
                    Items = new List<OrderItemCreateModel>
                {
                    new OrderItemCreateModel
                    {
                        ProductId = resultProduct3,
                        Quantity = 1,
                        PriceAtOrderTime = productModel3.Price,
                        Discount = 0,
                        TotalPrice = productModel3.Price
                    }
                }
                };

                OrderCreateModel createOrderRequest3 = new OrderCreateModel
                {
                    CustomerId = resultRegister3.Data,
                    CustomerName = user3.LastName,
                    ShippingAddress = user3.Address,
                    CustomerEmail = user3.Email,
                    CustomerPhonenumber = user3.PhoneNumber,
                    PaymentMethod = EPaymentMethod.COD,
                    Items = new List<OrderItemCreateModel>
                {
                    new OrderItemCreateModel
                    {
                        ProductId = resultProduct4,
                        Quantity = 1,
                        PriceAtOrderTime = productModel4.Price,
                        Discount = 0,
                        TotalPrice = productModel4.Price
                    }
                }
                };

                var resultOrder1 = await _orderService.CreateCODOnlineOrderAsync(resultRegister1.Data, createOrderRequest1);
                var resultOrder2 = await _orderService.CreateCODOnlineOrderAsync(resultRegister2.Data, createOrderRequest2);
                var resultOrder3 = await _orderService.CreateCODOnlineOrderAsync(resultRegister3.Data, createOrderRequest3);


                var users = await _uow.Users.GetAllAsync();
                var products = await _uow.Products.GetAllAsync();
                var usersList = users.ToList();
                var productsList = products.ToList();

                for (int i = 0; i < 20; i++)
                {
                    var user = usersList[i % usersList.Count];
                    var product = productsList[i % productsList.Count];

                    int quantity = 1 + (i % 3); // số lượng từ 1 đến 3

                    var order = new OrderCreateModel
                    {
                        CustomerId = user.PublicId,
                        CustomerName = user.LastName,
                        ShippingAddress = user.Address,
                        CustomerEmail = user.Email,
                        CustomerPhonenumber = user.PhoneNumber,
                        PaymentMethod = EPaymentMethod.COD,
                        Items = new List<OrderItemCreateModel>
                        {
                            new OrderItemCreateModel
                            {
                                ProductId = product.PublicId,
                                Quantity = quantity,
                                PriceAtOrderTime = product.Price,
                                Discount = 0,
                                TotalPrice = product.Price * quantity
                            }
                        }
                    };

                    var resultOrder = await _orderService.SeedDataOrderAsync(user.PublicId, order);
                }



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
            var users = await _uow.Users.GetAllAsync();
            var categories = await _uow.Categories.GetAllAsync();
            var brands = await _uow.Brands.GetAllAsync();
            var shippers = await _uow.Shippers.GetAllAsync();
            var products = await _uow.Products.GetAllAsync();
            var orders = await _uow.Orders.GetAllAsync();
            //var carts = await _cartRepository.GetAllAsync();
            //var qrCodes = await _qrCodeRepository.GetAllAsync();
            //var shippingDetails = await _shippingDetailRepository.GetAllAsync();
            //var invalidTokens = await _invalidTokenRepository.GetAllAsync();

            //var user1CartItems = await _cartRepository.GetProductsInCart(users.ElementAt(0).UserId);
            //var user2CartItems = await _cartRepository.GetProductsInCart(users.ElementAt(1).UserId);
            //var user3CartItems = await _cartRepository.GetProductsInCart(users.ElementAt(2).UserId);


            var data = new
            {
                Categories = categories,
                Brands = brands,
                Products = products,
                Orders = orders,
                //User1CartItems = user1CartItems,
                //User2CartItems = user2CartItems,
                //User3CartItems = user3CartItems,
                //QRCodes = qrCodes,
                Shippers = shippers,
                //ShippingDetails = shippingDetails,
                //InvalidTokens = invalidTokens
                Users = users,
            };

            return new JsonResult(data);
        }

        public async Task<bool> DeleteAllInitData()
        {
            try
            {
                await _uow.Users.DeleteAllAsync();
                await _uow.Categories.DeleteAllAsync();
                await _uow.Brands.DeleteAllAsync();
                await _uow.Shippers.DeleteAllAsync();
                await _uow.Sequences.DeleteAllAsync();
                await _uow.Products.DeleteAllAsync();
                await _uow.OrderItems.DeleteAllAsync();
                await _uow.Orders.DeleteAllAsync();

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
