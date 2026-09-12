using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Extensions;
using TechStore.Common.Helpers;
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
    public class MockingDataService
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

        public MockingDataService(IUnitOfWork uow, 
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
                CategoryCreateModel mobilephone = new CategoryCreateModel
                {

                    Name = "Điện thoại",
                    Description = "Điện thoại thông minh",
                    Slug = "dien-thoai",
                    IconImageUrl = "https://img.icons8.com/?size=100&id=62862&format=png&color=000000"
                };

                CategoryCreateModel laptop = new CategoryCreateModel
                {
                    Name = "Laptop",
                    Description = "Máy tính xách tay",
                    Slug = "laptop",
                    IconImageUrl = DefaultImageLinks.DefaultLaptopCategoryImage
                };

                CategoryCreateModel tablet = new CategoryCreateModel
                {
                    Name = "Máy tính bảng",
                    Description = "Tablet",
                    Slug = "may-tinh-bang",
                    IconImageUrl = DefaultImageLinks.DefaultTabletCategoryImage
                };

                CategoryCreateModel smartwatch = new CategoryCreateModel
                {
                    Name = "Đồng hồ thông minh",
                    Description = "Smart Watch",
                    Slug = "smart-watch",
                    IconImageUrl = DefaultImageLinks.DefaultSmartwatchCategoryImage
                };

                CategoryCreateModel charger = new CategoryCreateModel
                {
                    Name = "Sạc",
                    Description = "Charger",
                    Slug = "cuc-sac",
                    IconImageUrl = DefaultImageLinks.DefaultChargerCategoryImage
                };

                CategoryCreateModel usb = new CategoryCreateModel
                {
                    Name = "USB",
                    Description = "USB",
                    Slug = "usb",
                    IconImageUrl = DefaultImageLinks.DefaultUsbCategoryImage
                };

                CategoryCreateModel disk = new CategoryCreateModel
                {
                    Name = "Ổ cứng",
                    Description = "Ổ cứng",
                    Slug = "o-cung",
                    IconImageUrl = DefaultImageLinks.DefaultMemoryCategoryImage
                };

                CategoryCreateModel ram = new CategoryCreateModel
                {
                    Name = "RAM",
                    Description = "RAM",
                    Slug = "ram",
                    IconImageUrl = DefaultImageLinks.DefaultRamCategoryImage
                };

                CategoryCreateModel headphone = new CategoryCreateModel
                {
                    Name = "Tai nghe",
                    Description = "Tai nghe",
                    Slug = "tai-nghe",
                    IconImageUrl = DefaultImageLinks.DefaultHeadphoneCategoryImage
                };

                var resultCategoryMobilephone = await _categoryService.AddCategory(mobilephone);
                var resultCategoryLaptop = await _categoryService.AddCategory(laptop);
                var resultCategoryTablet = await _categoryService.AddCategory(tablet);
                var resultCategorySmartWatch = await _categoryService.AddCategory(smartwatch);
                var resultCategoryCharger = await _categoryService.AddCategory(charger);
                var resultCategoryUsb = await _categoryService.AddCategory(usb);
                var resultCategoryDisk = await _categoryService.AddCategory(disk);
                var resultCategoryRam = await _categoryService.AddCategory(ram);
                var resultCategoryHeadphone = await _categoryService.AddCategory(headphone);

                #endregion

                #region add brands
                var apple = new BrandCreateModel
                {
                    Name = "Apple",
                    Description = "Apple from USA",
                    Slug = "apple",
                    IconImageUrl = "https://img.icons8.com/?size=100&id=uoRwwh0lz3Jp&format=png&color=000000"
                };

                var samsung = new BrandCreateModel
                {
                    Name = "Samsung",
                    Description = "Samsung from South Korea",
                    Slug = "samsung",
                    IconImageUrl = DefaultImageLinks.DefaultSamsungLogo
                };

                var xiaomi = new BrandCreateModel
                {
                    Name = "Xiaomi",
                    Description = "Xiaomi from China",
                    Slug = "xiaomi",
                    IconImageUrl = DefaultImageLinks.DefaultXiaomiLogo
                };

                var oppo = new BrandCreateModel
                {
                    Name = "Oppo",
                    Description = "oppo from China",
                    Slug = "oppo",
                    IconImageUrl = DefaultImageLinks.DefaultOppoLogo
                };

                var realme = new BrandCreateModel
                {
                    Name = "Realme",
                    Description = "realme from China",
                    Slug = "realme",
                    IconImageUrl = DefaultImageLinks.DefaultRealmeLogo
                };

                var nokia = new BrandCreateModel
                {
                    Name = "Nokia",
                    Description = "nokia from China",
                    Slug = "nokia",
                    IconImageUrl = DefaultImageLinks.DefaultNokiaLogo
                };

                var dell = new BrandCreateModel
                {
                    Name = "Dell",
                    Description = "Dell from USA",
                    Slug = "dell",
                    IconImageUrl = DefaultImageLinks.DefaultDellLogo
                };

                var huawei = new BrandCreateModel
                {
                    Name = "Huawei",
                    Description = "Huawei from China",
                    Slug = "huawei",
                    IconImageUrl = DefaultImageLinks.DefaultHuaweiLogo
                };

                var resultBrandApple = await _brandService.AddBrand(apple);
                var resultBrandSamsung = await _brandService.AddBrand(samsung);
                var resultBrandXiaomi = await _brandService.AddBrand(xiaomi);
                var resultBrandOppo = await _brandService.AddBrand(oppo);
                var resultBrandRealme = await _brandService.AddBrand(realme);
                var resultBrandNokia = await _brandService.AddBrand(nokia);
                var resultBrandDell = await _brandService.AddBrand(dell);
                var resultBrandHuawei = await _brandService.AddBrand(huawei);

                #endregion

                #region add shippers
                var shipper1 = new ShipperCreateModel
                {
                    Name = "Viettel Post",
                    SupportPhone = "1900 8098",
                    Website = "https://viettelpost.com.vn",
                    LogoUrl = DefaultImageLinks.DefaultSmartphoneImage,
                    IsActive = true,
                    Description = "Viettel Post is a leading logistics company in Vietnam, providing fast and reliable delivery services."
                };

                var shipper2 = new ShipperCreateModel
                {
                    Name = "J&T Express",
                    SupportPhone = "1900 8888",
                    Website = "https://jtexpress.vn/",
                    LogoUrl = DefaultImageLinks.DefaultSmartphoneImage,
                    IsActive = true,
                    Description = "Công ty TNHH một thành viên chuyển phát nhanh Thuận Phong"
                };

                var shipper3 = new ShipperCreateModel
                {
                    Name = "Giao Hàng Nhanh",
                    SupportPhone = "1900 1234",
                    Website = "https://ghn.vn/",
                    LogoUrl = DefaultImageLinks.DefaultSmartphoneImage,
                    IsActive = true,
                    Description = "Công ty giao nhận đầu tiên tại Việt Nam được thành lập với sứ mệnh phục vụ nhu cầu vận chuyển chuyên nghiệp của các đối tác Thương mại điện tử trên toàn quốc."
                };

                var resultShipper1 = await _shipperService.AddShipper(shipper1);
                var resultShipper2 = await _shipperService.AddShipper(shipper2);
                var resultShipper3 = await _shipperService.AddShipper(shipper3);

                #endregion

                #region add vouchers

                var voucher1 = new Voucher
                {
                    Id = Guid.NewGuid(),
                    PublicId = ShareFunctions.GenerateRandomStringId(),
                    Code = "D99",
                    Description = "Giảm giá 99.99% cho đơn hàng đầu tiên. (Mục đích cho việc thử nghiệm chuyển khoản với số tiền thấp)",
                    DiscountType = EDiscountType.Percentage,
                    DiscountValue = 0.9999m,
                    MaxDiscountAmount = 1000000000,
                    MinOrderPrice = 1,
                    UsageLimit = 100,
                    ReservedCount = 0,
                    UsedCount = 0,
                    Status = EVoucherStatus.Active,
                    StartDate = TimeZoneHelper.GetUtcNow(),
                    EndDate = TimeZoneHelper.GetUtcNow().AddMonths(1),
                    CreatedAt = TimeZoneHelper.GetUtcNow(),
                };

                var voucher2 = new Voucher
                {
                    Id = Guid.NewGuid(),
                    PublicId = ShareFunctions.GenerateRandomStringId(),
                    Code = "D10",
                    Description = "Giảm giá 10% cho đơn hàng đầu tiên",
                    DiscountType = EDiscountType.Percentage,
                    DiscountValue = 0.1m,
                    MaxDiscountAmount = 1000000,
                    MinOrderPrice = 5000000,
                    UsageLimit = 2,
                    ReservedCount = 0,
                    UsedCount = 0,
                    Status = EVoucherStatus.Active,
                    StartDate = TimeZoneHelper.GetUtcNow(),
                    EndDate = TimeZoneHelper.GetUtcNow().AddMonths(1),
                    CreatedAt = TimeZoneHelper.GetUtcNow(),
                };

                var voucher3 = new Voucher
                {
                    Id = Guid.NewGuid(),
                    PublicId = ShareFunctions.GenerateRandomStringId(),
                    Code = "D1tr",
                    Description = "Giảm giá 1.000.000 đồng trong mùa hè não nhiệt",
                    DiscountType = EDiscountType.FixedAmount,
                    DiscountValue = 1000000,
                    MaxDiscountAmount = 1000000,
                    MinOrderPrice = 5000000,
                    UsageLimit = 1,
                    ReservedCount = 0,
                    UsedCount = 0,
                    Status = EVoucherStatus.Active,
                    StartDate = TimeZoneHelper.GetUtcNow(),
                    EndDate = TimeZoneHelper.GetUtcNow().AddMonths(1),
                    CreatedAt = TimeZoneHelper.GetUtcNow(),
                };

                await _uow.Vouchers.AddAsync(voucher1);
                await _uow.Vouchers.AddAsync(voucher2);
                await _uow.Vouchers.AddAsync(voucher3);

                #endregion

                #region register user
                string password = "Abcd1234";

                var admin1 = new UserCreateModel
                {
                    LastName = "Hoàng Kim",
                    FirstName = "Ngân",
                    PasswordHash = "Hoangkimngan@1",
                    Email = "hoangkimngan@gmail.com",
                    City = "HCM",
                    District = "",
                    Address = "",
                    PhoneNumber = "0122334455",
                    Gender = EGender.Male,
                    Birthday = new DateTime(1989, 2, 28)
                };

                RegisterModel registerAdmin1 = new RegisterModel
                {
                    Phonenumber = admin1.Email,
                    Password = admin1.PasswordHash,
                    UserInformation = admin1
                };

                var resultRegisterAdmin1 = await _authenticationService.RegisterAdminByEmail(registerAdmin1);

                if (resultRegisterAdmin1.Data == null)
                {
                    throw new Exception("Đã có lỗi xảy ra trong quá trình tạo tài khoản");
                }

                var user1 = new CustomerRegisterModel
                {
                    LastName = "Nguyễn Huy",
                    FirstName = "Hoàng",
                    Password = password,
                    Email = "nguyenhuyhoang@gmail.com",
                    City = "HCM",
                    District = "Q1",
                    Address = "123, HCM",
                    PhoneNumber = "0345600000",
                };

                var resultRegister1 = await _authenticationService.RegisterCustomer(user1);

                if (resultRegister1.IsSuccess == false)
                {
                    throw new Exception("Đã có lỗi xảy ra trong quá trình tạo tài khoản");
                }

                var user2 = new CustomerRegisterModel
                {
                    LastName = "Nguyễn Thị",
                    FirstName = "Lan",
                    Password = password,
                    Email = "nguyenthilan@gmail.com",
                    City = "Hà Nội",
                    District = "",
                    Address = "123",
                    PhoneNumber = "0345600001",
                };

                var resultRegister2 = await _authenticationService.RegisterCustomer(user2);

                if (resultRegister2.IsSuccess == false)
                {
                    throw new Exception("Đã có lỗi xảy ra trong quá trình tạo tài khoản");
                }

                var user3 = new CustomerRegisterModel
                {
                    LastName = "Nguyễn Văn",
                    FirstName = "Hải",
                    Password = password,
                    Email = "nguyenvanhai@gmail.com",
                    City = "Hà Nội",
                    District = "",
                    Address = "123",
                    PhoneNumber = "0345600002",
                };

                var resultRegister3 = await _authenticationService.RegisterCustomer(user3);

                if (resultRegister3.IsSuccess == false)
                {
                    throw new Exception("Đã có lỗi xảy ra trong quá trình tạo tài khoản");
                }

                var user4 = new CustomerRegisterModel
                {
                    LastName = "Phạm Văn",
                    FirstName = "Hiếu",
                    Password = password,
                    Email = "phamvanhieu@gmail.com",
                    City = "HCM",
                    District = "Q12",
                    Address = "123 Q12",
                    PhoneNumber = "0345678900",
                };

                var resultRegister4 = await _authenticationService.RegisterCustomer(user4);


                if (resultRegister4.IsSuccess == false)
                {
                    throw new Exception("Đã có lỗi xảy ra trong quá trình tạo tài khoản");
                }

                var user5 = new CustomerRegisterModel
                {
                    LastName = "Hồng Trường",
                    FirstName = "Vinh",
                    Password = password,
                    Email = "hongtruongvinh@gmail.com",
                    City = "HCM",
                    District = "Thu Duc",
                    Address = "123 Thu Duc",
                    PhoneNumber = "0345678999",
                };

                var resultRegister5 = await _authenticationService.RegisterCustomer(user5);


                if (resultRegister4.IsSuccess == false)
                {
                    throw new Exception("Đã có lỗi xảy ra trong quá trình tạo tài khoản");
                }

                for (int i = 0; i <= 14; i++)
                {
                    var user = new CustomerRegisterModel
                    {
                        LastName = $"User{i}",
                        FirstName = $"{i}",
                        Password = password,
                        Email = $"user{i}@gmail.com",
                        City = "Hà Nội",
                        District = "",
                        Address = "123",
                        PhoneNumber = $"0345000{i:D3}",
                    };

                    var resultRegister = await _authenticationService.RegisterCustomer(user);
                }

                for (int i = 1; i <= 3; i++)
                {
                    var user = new UserCreateModel
                    {
                        LastName = $"Staff{i}",
                        FirstName = $"{i}",
                        PasswordHash = password,
                        Email = $"staff{i}@gmail.com",
                        City = "Hà Nội",
                        District = "",
                        Address = "789",
                        PhoneNumber = $"0340000{i:D3}",
                        Gender = EGender.Male,
                        Birthday = new DateTime(2002, 2, 28)
                    };

                    var register = new RegisterModel
                    {
                        Phonenumber = user.Email,
                        Password = user.PasswordHash,
                        UserInformation = user
                    };

                    var resultRegister = await _authenticationService.RegisterAdminByEmail(register);
                }

                #endregion

                #region create products
                string productDescripotions = "Thông tin sản phẩm đang được cập nhật";
                //const string iphone16ImageUrl = "https://www.apple.com/newsroom/images/2024/09/get-ready-to-upgrade-to-the-new-iphone-16-apple-watch-and-airpods-lineups/article/Apple-iPhone-16_inline.jpg.large.jpg";
                //const string iphone16ProImageUrl = "https://www.apple.com/newsroom/images/2024/09/get-ready-to-upgrade-to-the-new-iphone-16-apple-watch-and-airpods-lineups/article/Apple-iPhone-16-Pro_inline.jpg.large.jpg";
                //const string galaxyS25ImageUrl = "https://image-us.samsung.com/us/smartphones/galaxy-s25/images/galaxy-s25-features-kv.jpg?imbypass=true";
                //const string galaxyS25UltraImageUrl = "https://image-us.samsung.com/us/smartphones/galaxy-s25-ultra/images/galaxy-s25-ultra-features-kv.jpg?imbypass=true";

                //var productModel1 = new ProductCreateModel
                //{
                //    CategoryId = resultCategoryMobilephone.Data,
                //    Name = "iPhone 16",
                //    BrandId = resultBrandApple.Data,
                //    ShortDescription = "iPhone 16 với chip A18, Camera Control và camera Fusion 48MP.",
                //    Description = "iPhone 16 sở hữu màn hình Super Retina XDR OLED 6,1 inch, chip A18 mạnh mẽ và tiết kiệm năng lượng. Hệ thống camera Fusion 48MP hỗ trợ chụp ảnh độ phân giải cao, zoom quang học 2x cùng camera Ultra Wide có khả năng chụp macro. Nút Action và Camera Control giúp truy cập nhanh các tính năng thường dùng, trong khi thiết kế nhôm bền bỉ đạt chuẩn kháng nước, bụi IP68.",
                //    Warranty = 12,
                //    Slug = "iphone-16",
                //    Tags = new List<string> { "apple", "iphone", "iphone-16", "smartphone" },
                //    IsFeatured = true,
                //    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                //    PublishDate = TimeZoneHelper.GetUtcNow(),
                //    MainImageUrl = iphone16ImageUrl,
                //    GalleryImageUrls = new List<string> { iphone16ImageUrl },
                //    SalePrice = 1000000,
                //    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                //    {
                //        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                //        {
                //            Name = "128GB",
                //            Description = "Phiên bản bộ nhớ 128GB, phù hợp nhu cầu sử dụng hằng ngày.",
                //            ImportPrice = 18000000,
                //            Price = 22990000,
                //            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                //            {
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Ultramarine", Stock = 30, ImageUrl = iphone16ImageUrl },
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Black", Stock = 25, ImageUrl = iphone16ImageUrl },
                //            },
                //        },
                //        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                //        {
                //            Name = "256GB",
                //            Description = "Phiên bản bộ nhớ 256GB cho nhu cầu lưu trữ ảnh, video và ứng dụng lớn.",
                //            ImportPrice = 21000000,
                //            Price = 25990000,
                //            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                //            {
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Ultramarine", Stock = 20, ImageUrl = iphone16ImageUrl },
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "White", Stock = 20, ImageUrl = iphone16ImageUrl },
                //            },
                //        },
                //    },
                //};

                //var productModel2 = new ProductCreateModel
                //{
                //    CategoryId = resultCategoryMobilephone.Data,
                //    Name = "iPhone 16 Pro",
                //    BrandId = resultBrandApple.Data,
                //    ShortDescription = "iPhone cao cấp với chip A18 Pro, khung titan và camera chuyên nghiệp.",
                //    Description = "iPhone 16 Pro trang bị màn hình Super Retina XDR 6,3 inch với ProMotion, khung titan nhẹ và bền cùng chip A18 Pro. Cụm camera gồm camera Fusion 48MP, Ultra Wide 48MP và Telephoto 5x, hỗ trợ quay video 4K Dolby Vision ở tốc độ 120 fps. Camera Control giúp thao tác chụp nhanh, còn thời lượng pin được cải thiện để đáp ứng công việc, sáng tạo nội dung và chơi game cường độ cao.",
                //    Warranty = 12,
                //    Slug = "iphone-16-pro",
                //    Tags = new List<string> { "apple", "iphone", "iphone-16-pro", "smartphone", "flagship" },
                //    IsFeatured = true,
                //    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                //    PublishDate = TimeZoneHelper.GetUtcNow(),
                //    MainImageUrl = iphone16ProImageUrl,
                //    GalleryImageUrls = new List<string> { iphone16ProImageUrl },
                //    SalePrice = 1500000,
                //    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                //    {
                //        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                //        {
                //            Name = "256GB",
                //            Description = "Bộ nhớ 256GB cân bằng giữa hiệu năng và khả năng lưu trữ.",
                //            ImportPrice = 27000000,
                //            Price = 31990000,
                //            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                //            {
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Desert Titanium", Stock = 25, ImageUrl = iphone16ProImageUrl },
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Black Titanium", Stock = 20, ImageUrl = iphone16ProImageUrl },
                //            },
                //        },
                //        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                //        {
                //            Name = "512GB",
                //            Description = "Bộ nhớ 512GB dành cho quay video chất lượng cao và lưu trữ chuyên nghiệp.",
                //            ImportPrice = 33000000,
                //            Price = 37990000,
                //            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                //            {
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Desert Titanium", Stock = 15, ImageUrl = iphone16ProImageUrl },
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Natural Titanium", Stock = 15, ImageUrl = iphone16ProImageUrl },
                //            },
                //        },
                //    },
                //};

                //var productModel8 = new ProductCreateModel
                //{
                //    CategoryId = resultCategoryMobilephone.Data,
                //    Name = "Samsung Galaxy S25",
                //    BrandId = resultBrandSamsung.Data,
                //    ShortDescription = "Galaxy S25 nhỏ gọn với Galaxy AI và Snapdragon 8 Elite for Galaxy.",
                //    Description = "Samsung Galaxy S25 kết hợp thiết kế nhỏ gọn, khung Armor Aluminum và màn hình Dynamic AMOLED 2X mượt mà. Vi xử lý Snapdragon 8 Elite for Galaxy cùng 12GB RAM mang lại hiệu năng nhanh cho công việc và giải trí. Camera chính 50MP được hỗ trợ bởi AI ProVisual Engine, pin 4.000mAh đáp ứng thời gian sử dụng dài và chuẩn IP68 tăng khả năng bảo vệ trong điều kiện hằng ngày.",
                //    Warranty = 12,
                //    Slug = "samsung-galaxy-s25",
                //    Tags = new List<string> { "samsung", "galaxy", "galaxy-s25", "smartphone", "galaxy-ai" },
                //    IsFeatured = true,
                //    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                //    PublishDate = TimeZoneHelper.GetUtcNow(),
                //    MainImageUrl = galaxyS25ImageUrl,
                //    GalleryImageUrls = new List<string> { galaxyS25ImageUrl },
                //    SalePrice = 1500000,
                //    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                //    {
                //        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                //        {
                //            Name = "12GB 256GB",
                //            Description = "RAM 12GB và bộ nhớ 256GB cho đa nhiệm mượt mà.",
                //            ImportPrice = 17000000,
                //            Price = 22990000,
                //            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                //            {
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Icyblue", Stock = 30, ImageUrl = galaxyS25ImageUrl },
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Navy", Stock = 25, ImageUrl = galaxyS25ImageUrl },
                //            },
                //        },
                //        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                //        {
                //            Name = "12GB 512GB",
                //            Description = "RAM 12GB và bộ nhớ lớn 512GB cho ảnh, video và ứng dụng.",
                //            ImportPrice = 21000000,
                //            Price = 26990000,
                //            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                //            {
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Silver Shadow", Stock = 20, ImageUrl = galaxyS25ImageUrl },
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Mint", Stock = 20, ImageUrl = galaxyS25ImageUrl },
                //            },
                //        },
                //    },
                //};

                //var productModel9 = new ProductCreateModel
                //{
                //    CategoryId = resultCategoryMobilephone.Data,
                //    Name = "Samsung Galaxy S25 Ultra",
                //    BrandId = resultBrandSamsung.Data,
                //    ShortDescription = "Flagship Galaxy với camera 200MP, S Pen và khung titan.",
                //    Description = "Samsung Galaxy S25 Ultra sở hữu khung titan, kính Gorilla Armor 2 và khả năng kháng nước, bụi IP68. Màn hình lớn sắc nét đi cùng Snapdragon 8 Elite for Galaxy, RAM 12GB và pin 5.000mAh. Camera chính 200MP kết hợp AI ProVisual Engine hỗ trợ chụp ảnh, quay video chi tiết trong nhiều điều kiện; S Pen tích hợp giúp ghi chú, phác thảo và xử lý công việc chính xác hơn.",
                //    Warranty = 12,
                //    Slug = "samsung-galaxy-s25-ultra",
                //    Tags = new List<string> { "samsung", "galaxy", "galaxy-s25-ultra", "smartphone", "flagship", "s-pen" },
                //    IsFeatured = true,
                //    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                //    PublishDate = TimeZoneHelper.GetUtcNow(),
                //    MainImageUrl = galaxyS25UltraImageUrl,
                //    GalleryImageUrls = new List<string> { galaxyS25UltraImageUrl },
                //    SalePrice = 2000000,
                //    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                //    {
                //        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                //        {
                //            Name = "12GB 256GB",
                //            Description = "Phiên bản 256GB dành cho nhu cầu cao cấp hằng ngày.",
                //            ImportPrice = 27000000,
                //            Price = 33990000,
                //            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                //            {
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Titanium Silverblue", Stock = 25, ImageUrl = galaxyS25UltraImageUrl },
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Titanium Black", Stock = 25, ImageUrl = galaxyS25UltraImageUrl },
                //            },
                //        },
                //        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                //        {
                //            Name = "12GB 512GB",
                //            Description = "Phiên bản 512GB phù hợp sáng tạo nội dung và lưu trữ dung lượng lớn.",
                //            ImportPrice = 32000000,
                //            Price = 38990000,
                //            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                //            {
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Titanium Gray", Stock = 15, ImageUrl = galaxyS25UltraImageUrl },
                //                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel { Name = "Titanium Whitesilver", Stock = 15, ImageUrl = galaxyS25UltraImageUrl },
                //            },
                //        },
                //    },
                //};

                var productModel3 = new ProductCreateModel
                {
                    CategoryId = resultCategoryLaptop.Data,
                    Name = "Dell XPS 13",
                    BrandId = resultBrandDell.Data,
                    ShortDescription = "Máy tính xách tay",
                    Description = "Máy tính xách tay",
                    Warranty = 12,
                    Slug = "dell-xps-13",
                    Tags = new List<string> { "dell", "dell-xps" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultLaptopImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 512GB",
                            Description= "Desciption",
                            ImportPrice = 25000000,
                            Price = 35000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultLaptopImage,
                                },
                            },
                        }
                    }
                };
                var productModel4 = new ProductCreateModel
                {
                    CategoryId = resultCategoryLaptop.Data,
                    Name = "MacBook Pro M5 Pro 16 inch",
                    BrandId = resultBrandApple.Data,
                    ShortDescription = "Máy tính xách tay",
                    Description = "Máy tính xách tay",
                    Warranty = 12,
                    Slug = "macbook-pro-m5-pro-16-inch",
                    Tags = new List<string> { "mac", "macbook" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultMacbookImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "18CPU - 20GPU 48GB - 1TB",
                            Description= "Desciption",
                            ImportPrice = 84000000,
                            Price = 85000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black Space",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultMacbookImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultMacbookImage,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "18CPU - 20GPU 48GB - 2TB",
                            Description= "Desciption",
                            ImportPrice = 94000000,
                            Price = 95000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black Space",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultMacbookImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultMacbookImage,
                                }
                            },
                        }
                    }
                };
                var productModel5 = new ProductCreateModel
                {
                    CategoryId = resultCategoryTablet.Data,
                    Name = "Ipad A6",
                    BrandId = resultBrandApple.Data,
                    ShortDescription = "Máy tính bảng",
                    Description = "Máy tính bảng",
                    Warranty = 12,
                    Slug = "ipad-12-pro",
                    Tags = new List<string> { "ipad", "ipad-pro" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultTabletImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 25000000,
                            Price = 28000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultTabletImage,
                                },
                            },
                        }
                    }
                };
                var productModel6 = new ProductCreateModel
                {
                    CategoryId = resultCategoryTablet.Data,
                    Name = "Samsung Galaxy Tab S8",
                    BrandId = resultBrandSamsung.Data,
                    MainImageUrl = DefaultImageLinks.DefaultSamsungImage,
                    GalleryImageUrls = new List<string>(),
                    ShortDescription = "Máy tính bảng",
                    Description = "Máy tính bảng",
                    Slug = "samsung-galaxy-tab-s8",
                    Tags = new List<string> { "samsung", "samsung-tab" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    Warranty = 12,  
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB 512GB",
                            Description= "Desciption",
                            ImportPrice = 23000000,
                            Price = 25000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungImage,
                                },
                            },
                        }
                    }
                };
                var productModel7 = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    Name = "Xiaomi mi 8",
                    BrandId = resultBrandXiaomi.Data,
                    MainImageUrl = DefaultImageLinks.DefaultXiaomiImage,
                    GalleryImageUrls = new List<string>(),
                    ShortDescription = "Xiao mi",
                    Description = "Xiao mi",
                    Warranty = 12,
                    Slug = "xiaomi-mi-8",
                    Tags = new List<string> { "xiaomi", "xiaomi-mi8" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 800000,
                            Price = 11000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultXiaomiImage,
                                },
                            },
                        }
                    }
                };
                var productModel10 = new ProductCreateModel
                {
                    CategoryId = resultCategoryLaptop.Data,
                    BrandId = resultBrandDell.Data,
                    Name = "HP Spectre x360",
                    ShortDescription = "Laptop 2-trong-1",
                    Description = "Laptop cao cấp từ HP",
                    Warranty = 12,
                    Slug = "hp-spectre-x360",
                    Tags = new List<string> { "hp", "laptop" },
                    IsFeatured = false,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultLaptopImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 13000000,
                            Price = 15000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultLaptopImage,
                                },
                            },
                        }
                    }
                };
                var productModel11 = new ProductCreateModel
                {
                    CategoryId = resultCategoryLaptop.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "MacBook Air M2",
                    ShortDescription = "Laptop Apple nhẹ",
                    Description = "MacBook Air với chip M2",
                    Warranty = 12,
                    Slug = "macbook-air-m2",
                    Tags = new List<string> { "macbook", "apple" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultMacbookImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "32GB 1T",
                            Description= "Desciption",
                            ImportPrice = 30000000,
                            Price = 34000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultMacbookImage,
                                },
                            },
                        }
                    }
                };
                var productModel12 = new ProductCreateModel
                {
                    CategoryId = resultCategoryTablet.Data,
                    BrandId = resultBrandXiaomi.Data,
                    Name = "Xiaomi Pad 5",
                    ShortDescription = "Máy tính bảng Xiaomi",
                    Description = "Tablet giá rẻ cấu hình mạnh",
                    Warranty = 12,
                    Slug = "xiaomi-pad-5",
                    Tags = new List<string> { "xiaomi", "tablet" },
                    IsFeatured = false,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultXiaomiImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 9000000,
                            Price = 11000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultXiaomiImage,
                                },
                            },
                        }
                    }
                };
                var productModel13 = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandOppo.Data,
                    Name = "Poco X5 Pro",
                    ShortDescription = "Điện thoại chơi game giá rẻ",
                    Description = "Cấu hình mạnh trong phân khúc",
                    Warranty = 12,
                    Slug = "poco-x5-pro",
                    Tags = new List<string> { "poco", "xiaomi" },
                    IsFeatured = false,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 128GB",
                            Description= "Desciption",
                            ImportPrice = 7000000,
                            Price = 10000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                                },
                            },
                        }
                    }
                };
                var productModel14 = new ProductCreateModel 
                { 
                    Name = "Asus ROG Phone 7", 
                    CategoryId = resultCategoryMobilephone.Data, 
                    BrandId = resultBrandDell.Data, 
                    ShortDescription = "Điện thoại gaming", 
                    Description = "Asus ROG Phone mạnh mẽ",
                    Warranty = 12,
                    Slug = "asus-rog-phone-7", 
                    Tags = new List<string> { "asus", "rog" }, 
                    IsFeatured = true, 
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 128GB",
                            Description= "Desciption",
                            ImportPrice = 6000000,
                            Price = 8000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                                },
                            },
                        }
                    }
                };
                var productModel15 = new ProductCreateModel 
                { 
                    Name = "Lenovo Legion 5 Pro", 
                    CategoryId = resultCategoryLaptop.Data, 
                    BrandId = resultBrandDell.Data, 
                    ShortDescription = "Laptop gaming", 
                    Description = "Laptop mạnh mẽ cho game thủ",
                    Warranty = 12,
                    Slug = "lenovo-legion-5-pro",
                    Tags = new List<string> { "lenovo", "gaming" }, 
                    IsFeatured = true, StartSellingDate = TimeZoneHelper.GetUtcNow(), 
                    MainImageUrl = DefaultImageLinks.DefaultLaptopImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 512GB",
                            Description= "Desciption",
                            ImportPrice = 27000000,
                            Price = 35000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Deep Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultLaptopImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultLaptopImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "16GB 1T",
                            Description= "Desciption",
                            ImportPrice = 34000000,
                            Price = 39000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Deep Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultLaptopImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultLaptopImage,
                                },
                            },
                        }
                    }
                };
                var productModel16 = new ProductCreateModel 
                { 
                    Name = "iPad Air 2022", 
                    CategoryId = resultCategoryTablet.Data, 
                    BrandId = resultBrandApple.Data, 
                    ShortDescription = "Tablet Apple", 
                    Description = "iPad Air nhẹ, mạnh",
                    Warranty = 12,
                    Slug = "ipad-air-2022", 
                    Tags = new List<string> { "ipad", "apple" }, 
                    IsFeatured = false, 
                    StartSellingDate = TimeZoneHelper.GetUtcNow(), 
                    MainImageUrl = DefaultImageLinks.DefaultTabletImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 17000000,
                            Price = 20000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultTabletImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultTabletImage,
                                },
                            },
                        }
                    }
                };
                var productModel17 = new ProductCreateModel
                {
                    Name = "OPPO A56",
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandOppo.Data,
                    ShortDescription = "Tablet lai laptop",
                    Description = "OPPO A56 mới nhất",
                    Warranty = 12,
                    Slug = "oppo-a56",
                    Tags = new List<string> { "oppo", "a56" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 6500000,
                            Price = 8500000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                                },
                            },
                        }
                    }
                };
                var productModel18 = new ProductCreateModel
                {
                    Name = "Oppo Find X5",
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandOppo.Data,
                    ShortDescription = "Điện thoại Oppo",
                    Description = "Camera siêu nét",
                    Warranty = 12,
                    Slug = "oppo-find-x5",
                    Tags = new List<string> { "oppo", "smartphone" },
                    IsFeatured = false,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 400000,
                            Price = 8000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                                },
                            },
                        }
                    }
                };
                var productModel19 = new ProductCreateModel
                {
                    Name = "Realme GT Neo 5",
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandOppo.Data,
                    ShortDescription = "Điện thoại Realme",
                    Description = "Pin trâu, sạc siêu nhanh",
                    Warranty = 12,
                    Slug = "realme-gt-neo-5",
                    Tags = new List<string> { "realme", "smartphone" },
                    IsFeatured = false,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "6GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 4000000,
                            Price = 70000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartphoneImage,
                                },
                            },
                        }
                    }
                };
                var productModel20 = new ProductCreateModel
                {
                    Name = "Samsung Galaxy Watch 6",
                    CategoryId = resultCategorySmartWatch.Data,
                    BrandId = resultBrandSamsung.Data,
                    ShortDescription = "Đồng hồ thông minh",
                    Description = "Smartwatch Samsung",
                    Warranty = 12,
                    Slug = "galaxy-watch-6",
                    Tags = new List<string> { "samsung", "watch" },
                    IsFeatured = false,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultSmartwatchImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "Classic 47mm Bluetooth",
                            Description= "Desciption",
                            ImportPrice = 4000000,
                            Price = 6000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartwatchImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartwatchImage,
                                },
                            },
                        }
                    }
                };
                var productModel21 = new ProductCreateModel
                {
                    Name = "Apple Watch Series 8",
                    CategoryId = resultCategorySmartWatch.Data,
                    BrandId = resultBrandApple.Data,
                    ShortDescription = "Đồng hồ Apple",
                    Description = "Apple Watch mới nhất",
                    Warranty = 12,
                    Slug = "apple-watch-series-8",
                    Tags = new List<string> { "apple", "watch" },
                    IsFeatured = false,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultSmartwatchImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8 45mm GPS",
                            Description= "Desciption",
                            ImportPrice = 400000,
                            Price = 7000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartwatchImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSmartwatchImage,
                                },
                            },
                        }
                    }
                };
                var productModel22 = new ProductCreateModel
                {
                    Name = "Huawei WH-1000XM5",
                    CategoryId = resultCategoryHeadphone.Data,
                    BrandId = resultBrandHuawei.Data,
                    ShortDescription = "Tai nghe chống ồn",
                    Description = "Tai nghe Huawei cao cấp",
                    Warranty = 12,
                    Slug = "huawei-wh-1000xm5",
                    Tags = new List<string> { "Huawei", "headphone" },
                    IsFeatured = false,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultHeadphoneImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "WH-1000XM4",
                            Description= "Desciption",
                            ImportPrice = 8000000,
                            Price = 15000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultHeadphoneImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "WH-1000XM5",
                            Description= "Desciption",
                            ImportPrice = 8000000,
                            Price = 15000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultHeadphoneImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "WH-1000XM6",
                            Description= "Desciption",
                            ImportPrice = 8000000,
                            Price = 15000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultHeadphoneImage,
                                },
                            },
                        }
                    }
                };
                var productModel23 = new ProductCreateModel
                {
                    Name = "JBL Charge 5",
                    CategoryId = resultCategoryCharger.Data,
                    BrandId = resultBrandHuawei.Data,
                    ShortDescription = "Loa bluetooth",
                    Description = "Loa di động chống nước",
                    Warranty = 12,
                    Slug = "jbl-charge-5",
                    Tags = new List<string> { "jbl", "bluetooth" },
                    IsFeatured = false,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultChargerImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "Charge 4",
                            Description= "Desciption",
                            ImportPrice = 200000,
                            Price = 3000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultChargerImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultChargerImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "Charge 5",
                            Description= "Desciption",
                            ImportPrice = 100000,
                            Price = 2000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultChargerImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultChargerImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "Charge 6",
                            Description= "Desciption",
                            ImportPrice = 100000,
                            Price = 2000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultChargerImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultChargerImage,
                                },
                            },
                        }
                    }
                };
                var productModel24 = new ProductCreateModel
                {
                    Name = "Redmi note 14",
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandXiaomi.Data,
                    ShortDescription = "Xiaomi redmi note 14",
                    Description = "Xiaomi redmi note 14",
                    Warranty = 12,
                    Slug = "",
                    Tags = new List<string> { "xiaomi", "smartphone" },
                    IsFeatured = false,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = DefaultImageLinks.DefaultXiaomiImage,
                    GalleryImageUrls = new List<string>(),
                    SalePrice = 0,
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "6GB 128GB",
                            Description= "Desciption",
                            ImportPrice = 7000000,
                            Price = 10000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultXiaomiImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultXiaomiImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 7000000,
                            Price = 10000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultXiaomiImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultXiaomiImage,
                                },
                            },
                        }
                    }
                };

                //var resultProduct1 = await _productService.AddProduct(productModel1);
                //var resultProduct2 = await _productService.AddProduct(productModel2);
                var resultProduct3 = await _productService.AddProduct(productModel3);
                var resultProduct4 = await _productService.AddProduct(productModel4);
                var resultProduct5 = await _productService.AddProduct(productModel5);
                var resultProduct6 = await _productService.AddProduct(productModel6);
                var resultProduct7 = await _productService.AddProduct(productModel7);
                //var resultProduct8 = await _productService.AddProduct(productModel8);
                //var resultProduct9 = await _productService.AddProduct(productModel9);
                var resultProduct10 = await _productService.AddProduct(productModel10);
                var resultProduct11 = await _productService.AddProduct(productModel11);
                var resultProduct12 = await _productService.AddProduct(productModel12);
                var resultProduct13 = await _productService.AddProduct(productModel13);
                var resultProduct14 = await _productService.AddProduct(productModel14);
                var resultProduct15 = await _productService.AddProduct(productModel15);
                var resultProduct16 = await _productService.AddProduct(productModel16);
                var resultProduct17 = await _productService.AddProduct(productModel17);
                var resultProduct18 = await _productService.AddProduct(productModel18);
                var resultProduct19 = await _productService.AddProduct(productModel19);
                var resultProduct20 = await _productService.AddProduct(productModel20);
                var resultProduct21 = await _productService.AddProduct(productModel21);
                var resultProduct22 = await _productService.AddProduct(productModel22);
                var resultProduct23 = await _productService.AddProduct(productModel23);
                var resultProduct24 = await _productService.AddProduct(productModel24);

                var samsungyear = 26;
                //add sample samsung product
                for (int i = 20; i <= samsungyear; i++)
                {
                    //normal
                    await _productService.AddProduct(new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S" + i,
                        ShortDescription = "Flagship Samsung",
                        Description = "Siêu phẩm điện thoại Samsung",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s" + i,
                        Tags = new List<string> { "samsung", "S" + i },
                        IsFeatured = true,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = DefaultImageLinks.DefaultSamsungS26PurpleImage,
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                        {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "6GB 128GB",
                            Description= "Desciption",
                            ImportPrice = 21000000 + (i-samsungyear)*2000000,
                            Price = 22000000 + (i-samsungyear)*2000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26WhiteImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlueImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "violet",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26PurpleImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 23000000 + (i-samsungyear)*2000000,
                            Price = 24000000 + (i-samsungyear)*2000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26WhiteImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlueImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "violet",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26PurpleImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12Gb 512GB",
                            Description= "Desciption",
                            ImportPrice = 25000000 + (i-samsungyear)*2000000,
                            Price = 26000000 + (i-samsungyear)*2000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26WhiteImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlueImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "violet",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26PurpleImage,
                                },
                            },
                        }
                    }
                    });

                    //plus
                    await _productService.AddProduct(new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S" + i + " Plus",
                        ShortDescription = "Flagship Samsung",
                        Description = "Siêu phẩm điện thoại Samsung",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s" + i + "-plus",
                        Tags = new List<string> { "samsung", "S" + i + "Plus" },
                        IsFeatured = true,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = DefaultImageLinks.DefaultSamsungS26BlueImage,
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                        {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "6GB 128GB",
                            Description= "Desciption",
                            ImportPrice = 25000000 + (i-samsungyear)*2000000,
                            Price = 26000000 + (i-samsungyear)*2000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26WhiteImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlueImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "violet",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26PurpleImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 28000000 + (i-samsungyear)*2000000,
                            Price = 29000000 + (i-samsungyear)*2000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26WhiteImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlueImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "violet",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26PurpleImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB 512GB",
                            Description= "Desciption",
                            ImportPrice = 29000000 + (i-samsungyear)*2000000,
                            Price = 30000000 + (i-samsungyear)*2000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26WhiteImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlueImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "violet",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26PurpleImage,
                                },
                            },
                        }
                    }
                    });

                    //ultra
                    await _productService.AddProduct(new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S" + i + " Ultra",
                        ShortDescription = "Flagship Samsung",
                        Description = "Siêu phẩm điện thoại Samsung",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s" + i + "-ultra",
                        Tags = new List<string> { "samsung", "S" + i + "Ultra" },
                        IsFeatured = true,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = DefaultImageLinks.DefaultSamsungS26WhiteImage,
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                        {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB 256GB",
                            Description= "Desciption",
                            ImportPrice = 29000000 + (i-samsungyear)*2000000,
                            Price = 30000000 + (i-samsungyear)*2000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26WhiteImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlueImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "violet",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26PurpleImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB 512GB",
                            Description= "Desciption",
                            ImportPrice = 32000000 + (i-samsungyear)*2000000,
                            Price = 33000000 + (i-samsungyear)*2000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26WhiteImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlueImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "violet",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26PurpleImage,
                                },
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12Gb 1T",
                            Description= "Desciption",
                            ImportPrice = 34000000 + (i-samsungyear)*2000000,
                            Price = 35000000 + (i-samsungyear)*2000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "White",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26WhiteImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Black",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Blue",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26BlueImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "violet",
                                    Stock = 35,
                                    ImageUrl = DefaultImageLinks.DefaultSamsungS26PurpleImage,
                                },
                            },
                        }
                    }
                    });
                }

                //add sample apple product
                var ipyear = 15;
                for (int i = 11; i < ipyear; i++)
                {
                    //normal
                    await _productService.AddProduct(new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "Iphone " + i,
                        ShortDescription = "Điện thoại thông minh",
                        Description = "Điện thoại thông minh",
                        Warranty = 12,
                        Slug = "iphone-" + i,
                        Tags = new List<string> { "iphone", "iphone" + i },
                        IsFeatured = true,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = DefaultImageLinks.DefaultIphone14PurpleImage,
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "128GB",
                            Description= "Description",
                            ImportPrice = 18000000 + (i-ipyear)*1000000,
                            Price = 20000000 + (i-ipyear)*1000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Space Black",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Deep Purple",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14PurpleImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14SilverImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Gold",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14GoldImage,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Desciption",
                            ImportPrice = 21000000 + (i-ipyear)*1000000,
                            Price = 22000000 + (i-ipyear)*1000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Space Black",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Deep Purple",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14PurpleImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14SilverImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Gold",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14GoldImage,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 25000000 + (i-ipyear)*1000000,
                            Price = 26000000 + (i-ipyear)*1000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Space Black",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Deep Purple",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14PurpleImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14SilverImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Gold",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14GoldImage,
                                }
                            },
                        }
                    }
                    });

                    //pro
                    await _productService.AddProduct(new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "Iphone " + i + " Pro",
                        ShortDescription = "Điện thoại thông minh",
                        Description = "Điện thoại thông minh",
                        Warranty = 12,
                        Slug = "iphone-" + i + "-pro",
                        Tags = new List<string> { "iphone", "iphone" + i + "pro" },
                        IsFeatured = true,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = DefaultImageLinks.DefaultIphone14GoldImage,
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "128GB",
                            Description= "Description",
                            ImportPrice = 27000000 + (i-ipyear)*1000000,
                            Price = 28000000 + (i-ipyear)*1000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Space Black",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Deep Purple",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14PurpleImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14SilverImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Gold",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14GoldImage,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Desciption",
                            ImportPrice = 30000000 + (i-ipyear)*1000000,
                            Price = 31000000 + (i-ipyear)*1000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Space Black",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Deep Purple",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14PurpleImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14SilverImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Gold",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14GoldImage,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 36000000 + (i-ipyear)*1000000,
                            Price = 37000000 + (i-ipyear)*1000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Space Black",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Deep Purple",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14PurpleImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14SilverImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Gold",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14GoldImage,
                                }
                            },
                        }
                    }
                    });

                    //pro-max
                    await _productService.AddProduct(new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "Iphone " + i + " Pro Max",
                        ShortDescription = "Điện thoại thông minh",
                        Description = "Điện thoại thông minh",
                        Warranty = 12,
                        Slug = "iphone-" + i + "-pro-max",
                        Tags = new List<string> { "iphone", "iphone" + i + "pro-max" },
                        IsFeatured = true,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = DefaultImageLinks.DefaultIphone14SilverImage,
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "128GB",
                            Description= "Description",
                            ImportPrice = 28000000 + (i-ipyear)*1000000,
                            Price = 29000000 + (i-ipyear)*1000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Space Black",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Deep Purple",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14PurpleImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14SilverImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Gold",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14GoldImage,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Desciption",
                            ImportPrice = 30000000 + (i-ipyear)*1000000,
                            Price = 31000000 + (i-ipyear)*1000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Space Black",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Deep Purple",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14PurpleImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14SilverImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Gold",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14GoldImage,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 37000000 + (i-ipyear)*1000000,
                            Price = 38000000 + (i-ipyear)*1000000,
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Space Black",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14BlackImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Deep Purple",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14PurpleImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Silver",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14SilverImage,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Gold",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.DefaultIphone14GoldImage,
                                }
                            },
                        }
                    }
                    });
                }

                var ip15promax = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 15 Pro Max",
                    ShortDescription = "Iphone 15 Pro Max",
                    Description = "Iphone 15 Pro Max",
                    Warranty = 12,
                    Slug = "iphone-15-pro-max",
                    Tags = new List<string> { "iphone", "iphone-15-pro-max" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/g1.webp",
                    GalleryImageUrls = new List<string>()
                    {
                        "TechShop/images/products/smartphone/iphone-15-pro-max/g1.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 24590000m,
                            Price = 26590000m,
                            Ram = "8GB",
                            Storage = "256GB",
                            AvailableStorage = "241GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A17 Pro 6 nhân",
                            Gpu = "GPU 6 lõi mới + Neural Engine 16 lõi mới",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 28490000m,
                            Price = 30490000m,
                            Ram = "8GB",    
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A17 Pro 6 nhân",
                            Gpu = "GPU 6 lõi mới + Neural Engine 16 lõi mới",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "1TB",
                            Description= "Desciption",
                            ImportPrice = 32990000m,
                            Price = 34990000m,
                            Storage = "1TB",
                            Ram = "8GB",
                            AvailableStorage = "976GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A17 Pro 6 nhân",
                            Gpu = "GPU 6 lõi mới + Neural Engine 16 lõi mới",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        }
                    }
                };

                var ip15pro = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 15 Pro",
                    ShortDescription = "Iphone 15 Pro",
                    Description = "Iphone 15 Pro",
                    Warranty = 12,
                    Slug = "iphone-15-pro",
                    Tags = new List<string> { "iphone", "iphone-15-pro" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/g1.webp",
                    GalleryImageUrls = new List<string>()
                    {
                        "TechShop/images/products/smartphone/iphone-15-pro-max/g1.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "128GB",
                            Description= "Description",
                            ImportPrice = 21590000m,
                            Price = 23590000m,
                            Storage = "128GB",
                            Ram = "8GB",
                            AvailableStorage = "113GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A17 Pro 6 nhân",
                            Gpu = "GPU 6 lõi mới + Neural Engine 16 lõi mới",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 24299000m,
                            Price = 26299000m,
                            Storage = "256GB",
                            Ram = "8GB",
                            AvailableStorage = "241GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A17 Pro 6 nhân",
                            Gpu = "GPU 6 lõi mới + Neural Engine 16 lõi mới",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 24799000m,
                            Price = 26799000m,
                            Storage = "512GB",
                            Ram = "8GB",
                            AvailableStorage = "497GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A17 Pro 6 nhân",
                            Gpu = "GPU 6 lõi mới + Neural Engine 16 lõi mới",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "1TB",
                            Description= "Desciption",
                            ImportPrice = 26990000m,
                            Price = 28990000m,
                            Storage = "1TB",
                            Ram = "8GB",
                            AvailableStorage = "976GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A17 Pro 6 nhân",
                            Gpu = "GPU 6 lõi mới + Neural Engine 16 lõi mới",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        }
                    }
                };

                var ip15plus = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 15 Plus",
                    ShortDescription = "Iphone 15 Plus",
                    Description = "Iphone 15 Plus",
                    Warranty = 12,
                    Slug = "iphone-15-plus",
                    Tags = new List<string> { "iphone", "iphone-15-plus" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-15-plus/g1.webp",
                    GalleryImageUrls = new List<string>()
                    {
                        "TechShop/images/products/smartphone/iphone-15-plus/g1.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "128GB",
                            Description= "Description",
                            ImportPrice = 16490000m,
                            Price = 18490000m,
                            Storage = "128GB",
                            AvailableStorage = "113GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A16 Bionic 6 nhân",
                            Gpu = "GPU 5 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/yellow.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh lá", 
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 18990000m,
                            Price = 20999000m,
                            Storage = "256GB",
                            AvailableStorage = "241GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A16 Bionic 6 nhân",
                            Gpu = "GPU 5 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/yellow.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh lá",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 21399000m,
                            Price = 23399000m,
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A16 Bionic 6 nhân",
                            Gpu = "GPU 5 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/yellow.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh lá",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        }
                    }
                };

                var ip15 = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 15",
                    ShortDescription = "Iphone 15",
                    Description = "Iphone 15",
                    Warranty = 12,
                    Slug = "iphone-15",
                    Tags = new List<string> { "iphone", "iphone-15" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-15-plus/g1.webp",
                    GalleryImageUrls = new List<string>()
                    {
                        "TechShop/images/products/smartphone/iphone-15-plus/g1.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "128GB",
                            Description= "Description",
                            ImportPrice = 14490000m,
                            Price = 16490000m,
                            Storage = "128GB",
                            AvailableStorage = "113GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A16 Bionic 6 nhân",
                            Gpu = "GPU 5 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/yellow.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh lá",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 16990000m,
                            Price = 18999000m,
                            Storage = "256GB",
                            AvailableStorage = "241GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A16 Bionic 6 nhân",
                            Gpu = "GPU 5 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/yellow.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh lá",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 20399000m,
                            Price = 22399000m,
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 17",
                            Cpu = "Apple A16 Bionic 6 nhân",
                            Gpu = "GPU 5 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/yellow.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh lá",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-15-pro-max/black.webp",
                                }
                            },
                        }
                    }
                };


                await _productService.AddProduct(ip15);
                await _productService.AddProduct(ip15plus);
                await _productService.AddProduct(ip15pro);
                await _productService.AddProduct(ip15promax);

                var ip16promax = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 16 Pro Max",
                    ShortDescription = "Iphone 16 Pro Max",
                    Description = "Iphone 16 Pro Max",
                    Warranty = 12,
                    Slug = "iphone-16-pro-max",
                    Tags = new List<string> { "iphone", "iphone-16-pro-max" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/g1.webp",
                    GalleryImageUrls = new List<string>()
                    {
                        "TechShop/images/products/smartphone/iphone-16-pro-max/g1.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 27990000m,
                            Price = 29990000m,
                            Storage = "256GB",
                            Ram = "8GB",
                            AvailableStorage = "241GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 Pro (3 nm)",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xa Mạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 30490000m,
                            Price = 32490000m,
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            Ram = "8GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 Pro (3 nm)",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xa Mạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "1TB",
                            Description= "Desciption",
                            ImportPrice = 39990000m,
                            Price = 41990000m,
                            Storage = "1TB",
                            AvailableStorage = "976GB",
                            Ram = "8GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 Pro (3 nm)",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xa Mạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        }
                    }
                };

                var ip16pro = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 16 Pro",
                    ShortDescription = "Iphone 16 Pro",
                    Description = "Iphone 16 Pro",
                    Warranty = 12,
                    Slug = "iphone-16-pro",
                    Tags = new List<string> { "iphone", "iphone-16-pro" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/g1.webp",
                    GalleryImageUrls = new List<string>()
                    {
                        "TechShop/images/products/smartphone/iphone-16-pro-max/g1.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "128GB",
                            Description= "Description",
                            ImportPrice = 27990000m,
                            Price = 29990000m,
                            Storage = "128GB",
                            AvailableStorage = "113GB",
                            Ram = "8GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 Pro (3 nm)",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xa Mạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 27990000m,
                            Price = 29990000m,
                            Storage = "256GB",
                            AvailableStorage = "241GB",
                            Ram = "8GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 Pro (3 nm)",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xa Mạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 30490000m,
                            Price = 32490000m,
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            Ram = "8GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 Pro (3 nm)",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xa Mạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "1TB",
                            Description= "Desciption",
                            ImportPrice = 39990000m,
                            Price = 41990000m,
                            Storage = "1TB",
                            AvailableStorage = "976GB",
                            Ram = "8GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 Pro (3 nm)",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Tự Nhiên",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/titan.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Xa Mạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/desert.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Titan Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        }
                    }
                };

                var ip16plus = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 16 Plus",
                    ShortDescription = "Iphone 16 Plus",
                    Description = "Iphone 16 Plus",
                    Warranty = 12,
                    Slug = "iphone-16-plus",
                    Tags = new List<string> { "iphone", "iphone-16-plus" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-16/g1.webp",
                    GalleryImageUrls = new List<string>()
                    {
                        "TechShop/images/products/smartphone/iphone-16/g1.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "128GB",
                            Description= "Description",
                            ImportPrice = 20990000m,
                            Price = 22990000m,
                            Storage = "128GB",
                            AvailableStorage = "113GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 (3 nm)",
                            Gpu = "Apple GPU 5 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Mòng Két",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lưu Ly",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 23990000m,
                            Price = 25990000m,
                            Storage = "256GB",
                            AvailableStorage = "241GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 (3 nm)",
                            Gpu = "Apple GPU 5 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Mòng Két",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lưu Ly",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 27290000m,
                            Price = 29290000m,
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 (3 nm)",
                            Gpu = "Apple GPU 5 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Mòng Két",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lưu Ly",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        }
                    }
                };

                var ip16 = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 16",
                    ShortDescription = "Iphone 16",
                    Description = "Iphone 16",
                    Warranty = 12,
                    Slug = "iphone-16",
                    Tags = new List<string> { "iphone", "iphone-16" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-16/g1.webp",
                    GalleryImageUrls = new List<string>()
                    {
                        "TechShop/images/products/smartphone/iphone-16/g1.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "128GB",
                            Description= "Description",
                            ImportPrice = 16990000m,
                            Price = 18990000m,
                            Storage = "128GB",
                            AvailableStorage = "113GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 (3 nm)",
                            Gpu = "Apple GPU 5 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Mòng Két",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lưu Ly",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 20990000m,
                            Price = 22990000m,
                            Storage = "256GB",
                            AvailableStorage = "241GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 (3 nm)",
                            Gpu = "Apple GPU 5 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Mòng Két",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lưu Ly",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 25290000m,
                            Price = 27290000m,
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            Ram = "6GB",
                            OperatingSystem = "iOS 18",
                            Cpu = "Apple A18 (3 nm)",
                            Gpu = "Apple GPU 5 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Mòng Két",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/green.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/white.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lưu Ly",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16/blue.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/pink.webp",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-16-pro-max/black.webp",
                                }
                            },
                        }
                    }
                };

                await _productService.AddProduct(ip16);
                await _productService.AddProduct(ip16plus);
                await _productService.AddProduct(ip16pro);
                await _productService.AddProduct(ip16promax);

                var ip17promax = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 17 Pro Max",
                    ShortDescription = "Iphone 17 Pro Max",
                    Description = "Iphone 17 Pro Max",
                    Warranty = 12,
                    Slug = "iphone-17-pro-max",
                    Tags = new List<string> { "iphone", "iphone-17-pro-max" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-17-pro-max/g1.webp",
                    GalleryImageUrls = new List<string>()
                    {
                        "TechShop/images/products/smartphone/iphone-17-pro-max/g1.webp",
                        "TechShop/images/products/smartphone/iphone-17-pro-max/g2.webp",
                        "TechShop/images/products/smartphone/iphone-17-pro-max/g3.webp",
                        "TechShop/images/products/smartphone/iphone-17-pro-max/g4.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 32590000m,
                            Price = 34590000m,
                            Storage = "256GB",
                            AvailableStorage = "241GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A19 Pro 6 nhân",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Cam",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxOrange,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh đậm",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxDeepblue,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxSilver,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 39490000m,
                            Price = 41490000m,
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A19 Pro 6 nhân",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Cam",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxOrange,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh đậm",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxDeepblue,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxSilver,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "1TB",
                            Description= "Desciption",
                            ImportPrice = 45990000m,
                            Price = 47990000m,
                            Storage = "1TB",
                            AvailableStorage = "976GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A19 Pro 6 nhân",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Cam",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxOrange,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh đậm",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxDeepblue,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxSilver,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "2TB",
                            Description= "Desciption",
                            ImportPrice = 58990000m,
                            Price = 60990000m,
                            Storage = "2TB",
                            AvailableStorage = "1952GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A19 Pro 6 nhân",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Cam",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxOrange,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh đậm",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxDeepblue,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxSilver,
                                }
                            },
                        }
                    }
                };

                var ip17pro = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 17 Pro",
                    ShortDescription = "Iphone 17 Pro",
                    Description = "Iphone 17 Pro",
                    Warranty = 12,
                    Slug = "iphone-17-pro",
                    Tags = new List<string> { "iphone", "iphone-17-pro" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-17-pro-max/g1.webp",
                    GalleryImageUrls = new List<string>()
                    {
                        "TechShop/images/products/smartphone/iphone-17-pro-max/g1.webp",
                        "TechShop/images/products/smartphone/iphone-17-pro-max/g2.webp",
                        "TechShop/images/products/smartphone/iphone-17-pro-max/g3.webp",
                        "TechShop/images/products/smartphone/iphone-17-pro-max/g4.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 30990000m,
                            Price = 31900000m,
                            Storage = "256GB",
                            AvailableStorage = "241GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A19 Pro 6 nhân",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Cam",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxOrange,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh đậm",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxDeepblue,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxSilver,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 36490000m,
                            Price = 38490000m,
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A19 Pro 6 nhân",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Cam",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxOrange,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh đậm",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxDeepblue,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxSilver,
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "1TB",
                            Description= "Desciption",
                            ImportPrice = 42990000m,
                            Price = 44990000m,
                            Storage = "1TB",
                            AvailableStorage = "976GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A19 Pro 6 nhân",
                            Gpu = "Apple GPU 6 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Cam",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxOrange,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh đậm",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxDeepblue,
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = DefaultImageLinks.Ip17promaxSilver,
                                }
                            },
                        },
                    }
                };

                var ip17 = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 17",
                    ShortDescription = "Iphone 17",
                    Description = "Iphone 17",
                    Warranty = 12,
                    Slug = "iphone-17",
                    Tags = new List<string> { "iphone", "iphone-17" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-17/g1.jpg",
                    GalleryImageUrls = new List<string>()
                    {
                        "TechShop/images/products/smartphone/iphone-17/g1.jpg",
                        "TechShop/images/products/smartphone/iphone-17/g2.jpg",
                        "TechShop/images/products/smartphone/iphone-17/g3.jpg",
                        "TechShop/images/products/smartphone/iphone-17/g4.jpg",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 22590000m,
                            Price = 24590000m,
                            Storage = "256GB",
                            AvailableStorage = "241GB",
                            Ram = "8GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A19 6 nhân",
                            Gpu = "Apple GPU 5 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lam Khói",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-17/blue.jpg",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lá Xô Thơm",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-17/green.jpg",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-17/white.jpg",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Oải Hương",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-17/purple.jpg",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-17/black.jpg",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 28590000m,
                            Price = 30590000m,
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            Ram = "8GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A19 6 nhân",
                            Gpu = "Apple GPU 5 nhân",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lam Khói",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-17/blue.jpg",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lá Xô Thơm",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-17/green.jpg",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-17/white.jpg",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Oải Hương",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-17/purple.jpg",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-17/black.jpg",
                                }
                            },
                        }
                    }
                };

                await _productService.AddProduct(ip17promax);
                await _productService.AddProduct(ip17pro);
                await _productService.AddProduct(ip17);

                var ip18promax = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 18 Pro Max",
                    ShortDescription = "Iphone 18 Pro Max",
                    Description = "Iphone 18 Pro Max",
                    Warranty = 12,
                    Slug = "iphone-18-pro-max",
                    Tags = new List<string> { "iphone", "iphone-18-pro-max" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/red-burgundy.png",
                    GalleryImageUrls = new List<string>()
                    {
                        //"TechShop/images/products/smartphone/iphone-18-pro-max/g1.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 39990000m,
                            Price = 41990000m,
                            Storage = "256GB",
                            AvailableStorage = "241GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A20 Pro 6 lõi",
                            Gpu = "GPU 7 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đỏ Burgundy",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/red-burgundy.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Băng Thanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/blue.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/silver.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/black.png",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 46490000m,
                            Price = 48490000m,
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A20 Pro 6 lõi",
                            Gpu = "GPU 7 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đỏ Burgundy",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/red-burgundy.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Băng Thanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/blue.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/silver.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/black.png",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "1TB",
                            Description= "Desciption",
                            ImportPrice = 59490000m,
                            Price = 61490000m,
                            Storage = "1TB",
                            AvailableStorage = "976GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A20 Pro 6 lõi",
                            Gpu = "GPU 7 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đỏ Burgundy",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/red-burgundy.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Băng Thanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/blue.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/silver.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/black.png",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "2TB",
                            Description= "Desciption",
                            ImportPrice = 78990000m,
                            Price = 80990000m,
                            Storage = "2TB",
                            AvailableStorage = "1952GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A20 Pro 6 lõi",
                            Gpu = "GPU 7 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đỏ Burgundy",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/red-burgundy.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Băng Thanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/blue.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/silver.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/black.png",
                                }
                            },
                        }
                    }
                };

                var ip18pro = new ProductCreateModel
                {
                    CategoryId = resultCategoryMobilephone.Data,
                    BrandId = resultBrandApple.Data,
                    Name = "Iphone 18 Pro",
                    ShortDescription = "Iphone 18 Pro",
                    Description = "Iphone 18 Pro",
                    Warranty = 12,
                    Slug = "iphone-18-pro",
                    Tags = new List<string> { "iphone", "iphone-18-pro" },
                    IsFeatured = true,
                    StartSellingDate = TimeZoneHelper.GetUtcNow(),
                    MainImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/red-burgundy.png",
                    GalleryImageUrls = new List<string>()
                    {
                        //"TechShop/images/products/smartphone/iphone-18-pro-max/g1.webp",
                        //"TechShop/images/products/smartphone/iphone-18-pro-max/g2.webp",
                        //"TechShop/images/products/smartphone/iphone-18-pro-max/g3.webp",
                        //"TechShop/images/products/smartphone/iphone-18-pro-max/g4.webp",
                    },
                    SalePrice = 0,
                    PublishDate = TimeZoneHelper.GetUtcNow(),
                    Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "256GB",
                            Description= "Description",
                            ImportPrice = 36990000m,
                            Price = 38990000m,
                            Storage = "256GB",
                            AvailableStorage = "241GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A20 Pro 6 lõi",
                            Gpu = "GPU 7 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đỏ Burgundy",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/red-burgundy.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Băng Thanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/blue.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/silver.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/black.png",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "512GB",
                            Description= "Desciption",
                            ImportPrice = 43490000m,
                            Price = 45490000m,
                            Storage = "512GB",
                            AvailableStorage = "497GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A20 Pro 6 lõi",
                            Gpu = "GPU 7 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đỏ Burgundy",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/red-burgundy.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Băng Thanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/blue.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/silver.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/black.png",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "1TB",
                            Description= "Desciption",
                            ImportPrice = 56490000m,
                            Price = 58490000m,
                            Storage = "1TB",
                            AvailableStorage = "976GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A20 Pro 6 lõi",
                            Gpu = "GPU 7 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đỏ Burgundy",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/red-burgundy.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Băng Thanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/blue.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/silver.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/black.png",
                                }
                            },
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "2TB",
                            Description= "Desciption",
                            ImportPrice = 75990000m,
                            Price = 77990000m,
                            Storage = "2TB",
                            AvailableStorage = "1952GB",
                            Ram = "12GB",
                            OperatingSystem = "iOS 26",
                            Cpu = "Apple A20 Pro 6 lõi",
                            Gpu = "GPU 7 lõi",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đỏ Burgundy",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/red-burgundy.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Băng Thanh",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/blue.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/silver.png",
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = "TechShop/images/products/smartphone/iphone-18-pro-max/black.png",
                                }
                            },
                        }
                    }
                };


                await _productService.AddProduct(ip18promax);
                await _productService.AddProduct(ip18pro);

                #endregion

                #region add orders
                var productVariantOptions = await _uow.ProductVariantOptions.GetAllAsync();
                    var users = await _uow.Users.FindManyAsync(u => u.LastName.Contains("User"));

                if (productVariantOptions != null && users != null)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        var user = users[i % users.Count];
                        var product = productVariantOptions[Random.Shared.Next(1, productVariantOptions.Count)];

                        int quantity = 1 + (i % 3); // số lượng từ 1 đến 3

                        var order = new OrderCreateModel
                        {
                            CustomerName = user.LastName,
                            ShippingAddress = user.Address,
                            CustomerEmail = user.Email,
                            CustomerPhoneNumber = user.PhoneNumber,
                            Items = new List<OrderItemCreateModel>
                            {
                                new OrderItemCreateModel
                                {
                                    ProductVariantOptionId = product.PublicId,
                                    Quantity = quantity,
                                }
                            }
                        };

                        var resultOrder = await _orderService.CreateCODOnlineOrderAsync(user.PublicId, order, Guid.NewGuid().ToString());
                    }
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
            var voucher = await _uow.Vouchers.GetAllAsync();
            var categories = await _uow.Categories.GetAllAsync();
            var brands = await _uow.Brands.GetAllAsync();
            var shippers = await _uow.Shippers.GetAllAsync();
            var users = await _uow.Users.GetAllAsync();
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
                Voucher = voucher,
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
                await _uow.VoucherUsages.DeleteAllAsync();
                await _uow.IdempotencyKeys.DeleteAllAsync();
                await _uow.StockReservations.DeleteAllAsync();
                await _uow.PaymentSnapshotItems.DeleteAllAsync();
                await _uow.PaymentSnapshots.DeleteAllAsync();
                await _uow.PaymentTransactions.DeleteAllAsync();
                await _uow.CartItems.DeleteAllAsync();
                await _uow.OrderItems.DeleteAllAsync();
                await _uow.Orders.DeleteAllAsync();
                await _uow.Categories.DeleteAllAsync();
                await _uow.Brands.DeleteAllAsync();
                await _uow.ProductVariantOptions.DeleteAllAsync();
                await _uow.ProductVariants.DeleteAllAsync();
                await _uow.Products.DeleteAllAsync();
                //await _uow.Comments.DeleteAllAsync();
                //await _uow.Reports.DeleteAllAsync();
                await _uow.Users.DeleteAllAsync();
                await _uow.Comments.DeleteAllAsync();
                await _uow.Vouchers.DeleteAllAsync();
                await _uow.ShippingDetails.DeleteAllAsync();
                await _uow.InvalidTokens.DeleteAllAsync();
                await _uow.Shippers.DeleteAllAsync();
                await _uow.Sequences.DeleteAllAsync();
                await _uow.Invoices.DeleteAllAsync();
                await _uow.Payments.DeleteAllAsync();
                await _uow.SearchKeywords.DeleteAllAsync();

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
