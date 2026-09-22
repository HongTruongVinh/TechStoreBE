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

                #region add products

                #region add many products
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
                    IsFeatured = false,
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
                    IsFeatured = false,
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
                    IsFeatured = false,
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
                    IsFeatured = false,
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
                    IsFeatured = false,
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
                    IsFeatured = false, StartSellingDate = TimeZoneHelper.GetUtcNow(), 
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

                var resultProduct3 = await _productService.AddProduct(productModel3);
                var resultProduct4 = await _productService.AddProduct(productModel4);
                var resultProduct5 = await _productService.AddProduct(productModel5);
                var resultProduct6 = await _productService.AddProduct(productModel6);
                var resultProduct7 = await _productService.AddProduct(productModel7);
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

                #endregion


                #region add sample product
                //add sample samsung product
                var samsungyear = 26;
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
                        IsFeatured = false,
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
                        IsFeatured = false,
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
                        IsFeatured = false,
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
                        IsFeatured = false,
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
                        IsFeatured = false,
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
                        IsFeatured = false,
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
                #endregion


                #region Add Iphone 15 -> 18 Series
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
                    IsFeatured = false,
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

                await _productService.AddProduct(ip17);
                await _productService.AddProduct(ip17pro);
                await _productService.AddProduct(ip17promax);

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

                await _productService.AddProduct(ip18pro);
                await _productService.AddProduct(ip18promax);
                #endregion

                int abcd = 1;
                if (abcd == 1)
                {
                    #region Xiaomi Phones (2M - 10M VND)

                    // 1. Xiaomi Redmi A3 (~2.29 triệu)
                    var xiaomiRedmiA3 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi Redmi A3",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-redmi-a3",
                        Tags = new List<string> { "xiaomi", "redmi", "redmi-a3", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "3GB/64GB",
                            Description = "",
                            ImportPrice = 1990000m,
                            Price = 2290000m,
                            Storage = "64GB",
                            AvailableStorage = "52GB",
                            Ram = "3GB",
                            OperatingSystem = "Android 14 (Go Edition)",
                            Cpu = "MediaTek Helio G36 8 nhân",
                            Gpu = "PowerVR GE8320",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Băng Giá",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Ánh Sao",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Rừng Sâu",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 2. Xiaomi Redmi 13C (~2.89 triệu)
                    var xiaomiRedmi13C = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi Redmi 13C",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-redmi-13c",
                        Tags = new List<string> { "xiaomi", "redmi", "redmi-13c", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "4GB/128GB",
                            Description = "",
                            ImportPrice = 2590000m,
                            Price = 2890000m,
                            Storage = "128GB",
                            AvailableStorage = "110GB",
                            Ram = "4GB",
                            OperatingSystem = "MIUI 14, Android 13",
                            Cpu = "MediaTek Helio G85 8 nhân",
                            Gpu = "Mali-G52 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Bóng Đêm",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Navy",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Cỏ Ba Lá",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 3. Xiaomi Redmi 14C (~3.29 triệu)
                    var xiaomiRedmi14C = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi Redmi 14C",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-redmi-14c",
                        Tags = new List<string> { "xiaomi", "redmi", "redmi-14c", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "4GB/128GB",
                            Description = "",
                            ImportPrice = 2890000m,
                            Price = 3290000m,
                            Storage = "128GB",
                            AvailableStorage = "110GB",
                            Ram = "4GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "MediaTek Helio G81-Ultra 8 nhân",
                            Gpu = "Mali-G52 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Màn Đêm",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Xô Thơm",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Mộng Mơ",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 4. Xiaomi Redmi 12 (~3.79 triệu)
                    var xiaomiRedmi12 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi Redmi 12",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-redmi-12",
                        Tags = new List<string> { "xiaomi", "redmi", "redmi-12", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 3390000m,
                            Price = 3790000m,
                            Storage = "128GB",
                            AvailableStorage = "112GB",
                            Ram = "8GB",
                            OperatingSystem = "MIUI 14, Android 13",
                            Cpu = "MediaTek Helio G88 8 nhân",
                            Gpu = "Mali-G52 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Bạc Cực Quang",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Bầu Trời",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc Ánh Cực Quang",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 5. Xiaomi Redmi 13 (~4.29 triệu)
                    var xiaomiRedmi13 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi Redmi 13",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-redmi-13",
                        Tags = new List<string> { "xiaomi", "redmi", "redmi-13", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "6GB/128GB",
                            Description = "",
                            ImportPrice = 3890000m,
                            Price = 4290000m,
                            Storage = "128GB",
                            AvailableStorage = "112GB",
                            Ram = "6GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "MediaTek Helio G91-Ultra 8 nhân",
                            Gpu = "Mali-G52 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Huyền Bí",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Sóng Nước",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng Ngọc Trai",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 6. Xiaomi Redmi Note 13 (~4.79 triệu)
                    var xiaomiRedmiNote13 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi Redmi Note 13",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-redmi-note-13",
                        Tags = new List<string> { "xiaomi", "redmi", "redmi-note-13", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "6GB/128GB",
                            Description = "",
                            ImportPrice = 4390000m,
                            Price = 4790000m,
                            Storage = "128GB",
                            AvailableStorage = "110GB",
                            Ram = "6GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "Qualcomm Snapdragon 685 8 nhân",
                            Gpu = "Adreno 610",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Huyền Bí",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Băng Tuyết",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng Hoàng Hôn",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 7. Xiaomi POCO M6 Pro (~5.69 triệu)
                    var xiaomiPocoM6Pro = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi POCO M6 Pro",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-poco-m6-pro",
                        Tags = new List<string> { "xiaomi", "poco", "poco-m6-pro", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 5190000m,
                            Price = 5690000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "8GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "MediaTek Helio G99-Ultra 8 nhân",
                            Gpu = "Mali-G57 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Tối Thượng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lam",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Huyền Ảo",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 8. Xiaomi Redmi Note 13 5G (~6.39 triệu)
                    var xiaomiRedmiNote135G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi Redmi Note 13 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-redmi-note-13-5g",
                        Tags = new List<string> { "xiaomi", "redmi", "redmi-note-13-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 5890000m,
                            Price = 6390000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "8GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "MediaTek Dimensity 6080 8 nhân",
                            Gpu = "Mali-G57 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Graphite",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Băng Giá",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lục Bảo",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 9. Xiaomi Redmi Note 13 Pro (~7.19 triệu)
                    var xiaomiRedmiNote13Pro = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi Redmi Note 13 Pro",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-redmi-note-13-pro",
                        Tags = new List<string> { "xiaomi", "redmi", "redmi-note-13-pro", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 6690000m,
                            Price = 7190000m,
                            Storage = "128GB",
                            AvailableStorage = "110GB",
                            Ram = "8GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "MediaTek Helio G99-Ultra 8 nhân",
                            Gpu = "Mali-G57 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Bóng Đêm",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Lavender",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Rừng Xanh",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 10. Xiaomi Redmi Note 13 Pro 5G (~8.99 triệu)
                    var xiaomiRedmiNote13Pro5G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi Redmi Note 13 Pro 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-redmi-note-13-pro-5g",
                        Tags = new List<string> { "xiaomi", "redmi", "redmi-note-13-pro-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 8490000m,
                            Price = 8990000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "8GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "Snapdragon 7s Gen 2 8 nhân",
                            Gpu = "Adreno 710",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Bán Dạ",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Đại Dương",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Cực Quang",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    await _productService.AddProduct(xiaomiRedmiA3);
                    await _productService.AddProduct(xiaomiRedmi13C);
                    await _productService.AddProduct(xiaomiRedmi14C);
                    await _productService.AddProduct(xiaomiRedmi12);
                    await _productService.AddProduct(xiaomiRedmi13);
                    await _productService.AddProduct(xiaomiRedmiNote13);
                    await _productService.AddProduct(xiaomiPocoM6Pro);
                    await _productService.AddProduct(xiaomiRedmiNote135G);
                    await _productService.AddProduct(xiaomiRedmiNote13Pro);
                    await _productService.AddProduct(xiaomiRedmiNote13Pro5G);

                    #endregion

                    #region Xiaomi Phones (10M - 20M VND)

                    // 1. Xiaomi Redmi Note 13 Pro+ 5G (~10.49 triệu)
                    var xiaomiRedmiNote13ProPlus5G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi Redmi Note 13 Pro+ 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-redmi-note-13-pro-plus-5g",
                        Tags = new List<string> { "xiaomi", "redmi", "redmi-note-13-pro-plus", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 9690000m,
                            Price = 10490000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "8GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "MediaTek Dimensity 7200-Ultra 8 nhân",
                            Gpu = "Mali-G610 MC4",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Bán Dạ",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Ánh Trăng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Cực Quang",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 2. Xiaomi POCO X6 Pro 5G (~10.99 triệu)
                    var xiaomiPocoX6Pro5G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi POCO X6 Pro 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-poco-x6-pro-5g",
                        Tags = new List<string> { "xiaomi", "poco", "poco-x6-pro", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/512GB",
                            Description = "",
                            ImportPrice = 10190000m,
                            Price = 10990000m,
                            Storage = "512GB",
                            AvailableStorage = "485GB",
                            Ram = "12GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "MediaTek Dimensity 8300-Ultra 8 nhân",
                            Gpu = "Mali-G615 MC6",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng POCO",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 3. Xiaomi 13 Lite (~11.49 triệu)
                    var xiaomi13Lite = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 13 Lite",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-13-lite",
                        Tags = new List<string> { "xiaomi", "xiaomi-13-lite", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 10690000m,
                            Price = 11490000m,
                            Storage = "256GB",
                            AvailableStorage = "238GB",
                            Ram = "8GB",
                            OperatingSystem = "MIUI 14, Android 13",
                            Cpu = "Qualcomm Snapdragon 7 Gen 1 8 nhân",
                            Gpu = "Adreno 644",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Dương",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 4. Xiaomi POCO F6 (~12.49 triệu)
                    var xiaomiPocoF6 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi POCO F6",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-poco-f6",
                        Tags = new List<string> { "xiaomi", "poco", "poco-f6", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/512GB",
                            Description = "",
                            ImportPrice = 11490000m,
                            Price = 12490000m,
                            Storage = "512GB",
                            AvailableStorage = "485GB",
                            Ram = "12GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "Qualcomm Snapdragon 8s Gen 3 8 nhân",
                            Gpu = "Adreno 735",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Titan",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lục",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 5. Xiaomi 13T (~12.99 triệu)
                    var xiaomi13T = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 13T",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-13t",
                        Tags = new List<string> { "xiaomi", "xiaomi-13t", "leica", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/256GB",
                            Description = "",
                            ImportPrice = 11990000m,
                            Price = 12990000m,
                            Storage = "256GB",
                            AvailableStorage = "236GB",
                            Ram = "12GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "MediaTek Dimensity 8200-Ultra 8 nhân",
                            Gpu = "Mali-G610 MC6",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Tuyết Tùng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Đồng Cỏ",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 6. Xiaomi 14T (~13.99 triệu)
                    var xiaomi14T = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 14T",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-14t",
                        Tags = new List<string> { "xiaomi", "xiaomi-14t", "leica", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/512GB",
                            Description = "",
                            ImportPrice = 12990000m,
                            Price = 13990000m,
                            Storage = "512GB",
                            AvailableStorage = "485GB",
                            Ram = "12GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "MediaTek Dimensity 8300-Ultra 8 nhân",
                            Gpu = "Mali-G615 MC6",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Titan",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Titan",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Titan",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 7. Xiaomi POCO F6 Pro (~15.49 triệu)
                    var xiaomiPocoF6Pro = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi POCO F6 Pro",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-poco-f6-pro",
                        Tags = new List<string> { "xiaomi", "poco", "poco-f6-pro", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/512GB",
                            Description = "",
                            ImportPrice = 14290000m,
                            Price = 15490000m,
                            Storage = "512GB",
                            AvailableStorage = "485GB",
                            Ram = "12GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "Qualcomm Snapdragon 8 Gen 2 8 nhân",
                            Gpu = "Adreno 740",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Huyền Ảo",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Tinh Khôi",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 8. Xiaomi 13T Pro (~16.49 triệu)
                    var xiaomi13TPro = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 13T Pro",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-13t-pro",
                        Tags = new List<string> { "xiaomi", "xiaomi-13t-pro", "leica", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/512GB",
                            Description = "",
                            ImportPrice = 15190000m,
                            Price = 16490000m,
                            Storage = "512GB",
                            AvailableStorage = "485GB",
                            Ram = "12GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "MediaTek Dimensity 9200+ 8 nhân",
                            Gpu = "Immortalis-G715 MC11",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Tuyết Tùng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Đồng Cỏ",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 9. Xiaomi 14T Pro (~17.99 triệu)
                    var xiaomi14TPro = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 14T Pro",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-14t-pro",
                        Tags = new List<string> { "xiaomi", "xiaomi-14t-pro", "leica", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/512GB",
                            Description = "",
                            ImportPrice = 16490000m,
                            Price = 17990000m,
                            Storage = "512GB",
                            AvailableStorage = "485GB",
                            Ram = "12GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "MediaTek Dimensity 9300+ 8 nhân",
                            Gpu = "Immortalis-G720 MC12",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Titan",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Titan",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Titan",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 10. Xiaomi 14 (~19.99 triệu)
                    var xiaomi14 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 14",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-14",
                        Tags = new List<string> { "xiaomi", "xiaomi-14", "leica", "flagship", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/256GB",
                            Description = "",
                            ImportPrice = 18490000m,
                            Price = 19990000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "12GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "Qualcomm Snapdragon 8 Gen 3 8 nhân",
                            Gpu = "Adreno 750",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Cổ Điển",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Tuyết",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Ngọc Bích",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    await _productService.AddProduct(xiaomiRedmiNote13ProPlus5G);
                    await _productService.AddProduct(xiaomiPocoX6Pro5G);
                    await _productService.AddProduct(xiaomi13Lite);
                    await _productService.AddProduct(xiaomiPocoF6);
                    await _productService.AddProduct(xiaomi13T);
                    await _productService.AddProduct(xiaomi14T);
                    await _productService.AddProduct(xiaomiPocoF6Pro);
                    await _productService.AddProduct(xiaomi13TPro);
                    await _productService.AddProduct(xiaomi14TPro);
                    await _productService.AddProduct(xiaomi14);

                    #endregion

                    #region Xiaomi Phones (Flagship > 20M VND)

                    // 1. Xiaomi 14 Pro (~21.99 triệu)
                    var xiaomi14Pro = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 14 Pro",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-14-pro",
                        Tags = new List<string> { "xiaomi", "xiaomi-14-pro", "leica", "flagship", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/512GB",
                            Description = "",
                            ImportPrice = 19990000m,
                            Price = 21990000m,
                            Storage = "512GB",
                            AvailableStorage = "485GB",
                            Ram = "12GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "Qualcomm Snapdragon 8 Gen 3 8 nhân",
                            Gpu = "Adreno 750",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Ngọc Bích",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 2. Xiaomi 14 Ultra (~29.99 triệu)
                    var xiaomi14Ultra = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 14 Ultra",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-14-ultra",
                        Tags = new List<string> { "xiaomi", "xiaomi-14-ultra", "leica", "flagship", "camera" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "16GB/512GB",
                            Description = "",
                            ImportPrice = 27490000m,
                            Price = 29990000m,
                            Storage = "512GB",
                            AvailableStorage = "480GB",
                            Ram = "16GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "Qualcomm Snapdragon 8 Gen 3 8 nhân",
                            Gpu = "Adreno 750",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Da Nano",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Da Nano",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Titan",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 3. Xiaomi 15 (~22.49 triệu)
                    var xiaomi15 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 15",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-15",
                        Tags = new List<string> { "xiaomi", "xiaomi-15", "leica", "flagship", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/256GB",
                            Description = "",
                            ImportPrice = 20490000m,
                            Price = 22490000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "12GB",
                            OperatingSystem = "Xiaomi HyperOS 2, Android 15",
                            Cpu = "Snapdragon 8 Elite 8 nhân",
                            Gpu = "Adreno 830",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lục Nhạt",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc Ánh Trăng",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 4. Xiaomi 15 Pro (~27.99 triệu)
                    var xiaomi15Pro = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 15 Pro",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-15-pro",
                        Tags = new List<string> { "xiaomi", "xiaomi-15-pro", "leica", "flagship", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "16GB/512GB",
                            Description = "",
                            ImportPrice = 25490000m,
                            Price = 27990000m,
                            Storage = "512GB",
                            AvailableStorage = "480GB",
                            Ram = "16GB",
                            OperatingSystem = "Xiaomi HyperOS 2, Android 15",
                            Cpu = "Snapdragon 8 Elite 8 nhân",
                            Gpu = "Adreno 830",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Titan",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Vân Đá",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 5. Xiaomi 15 Ultra (~34.99 triệu)
                    var xiaomi15Ultra = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 15 Ultra",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-15-ultra",
                        Tags = new List<string> { "xiaomi", "xiaomi-15-ultra", "leica", "flagship", "camera" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "16GB/1TB",
                            Description = "",
                            ImportPrice = 31990000m,
                            Price = 34990000m,
                            Storage = "1TB",
                            AvailableStorage = "960GB",
                            Ram = "16GB",
                            OperatingSystem = "Xiaomi HyperOS 2, Android 15",
                            Cpu = "Snapdragon 8 Elite 8 nhân",
                            Gpu = "Adreno 830",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Da Cao Cấp",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc Titan",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Đậm",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 6. Xiaomi MIX Flip (~24.99 triệu)
                    var xiaomiMixFlip = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi MIX Flip",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-mix-flip",
                        Tags = new List<string> { "xiaomi", "mix-flip", "foldable", "leica", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/512GB",
                            Description = "",
                            ImportPrice = 22990000m,
                            Price = 24990000m,
                            Storage = "512GB",
                            AvailableStorage = "485GB",
                            Ram = "12GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "Qualcomm Snapdragon 8 Gen 3 8 nhân",
                            Gpu = "Adreno 750",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Huyền Ảo",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 7. Xiaomi MIX Fold 3 (~28.99 triệu)
                    var xiaomiMixFold3 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi MIX Fold 3",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-mix-fold-3",
                        Tags = new List<string> { "xiaomi", "mix-fold-3", "foldable", "leica", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "16GB/512GB",
                            Description = "",
                            ImportPrice = 26490000m,
                            Price = 28990000m,
                            Storage = "512GB",
                            AvailableStorage = "480GB",
                            Ram = "16GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 13",
                            Cpu = "Qualcomm Snapdragon 8 Gen 2 Leading Version 8 nhân",
                            Gpu = "Adreno 740",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Sợi Carbon",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng Ánh Kim",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 8. Xiaomi MIX Fold 4 (~36.99 triệu)
                    var xiaomiMixFold4 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi MIX Fold 4",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-mix-fold-4",
                        Tags = new List<string> { "xiaomi", "mix-fold-4", "foldable", "leica", "flagship" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "16GB/1TB",
                            Description = "",
                            ImportPrice = 33990000m,
                            Price = 36990000m,
                            Storage = "1TB",
                            AvailableStorage = "960GB",
                            Ram = "16GB",
                            OperatingSystem = "Xiaomi HyperOS, Android 14",
                            Cpu = "Qualcomm Snapdragon 8 Gen 3 8 nhân",
                            Gpu = "Adreno 750",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Composite",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Tinh Khiết",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lam",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 9. Xiaomi 13 Pro (~20.99 triệu)
                    var xiaomi13Pro = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 13 Pro",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-13-pro",
                        Tags = new List<string> { "xiaomi", "xiaomi-13-pro", "leica", "flagship", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "12GB/256GB",
                            Description = "",
                            ImportPrice = 18990000m,
                            Price = 20990000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "12GB",
                            OperatingSystem = "MIUI 14, Android 13",
                            Cpu = "Qualcomm Snapdragon 8 Gen 2 8 nhân",
                            Gpu = "Adreno 740",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Gốm",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Gốm",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Cỏ",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 10. Xiaomi 13 Ultra (~25.99 triệu)
                    var xiaomi13Ultra = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandXiaomi.Data,
                        Name = "Xiaomi 13 Ultra",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "xiaomi-13-ultra",
                        Tags = new List<string> { "xiaomi", "xiaomi-13-ultra", "leica", "flagship", "camera" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "16GB/512GB",
                            Description = "",
                            ImportPrice = 23490000m,
                            Price = 25990000m,
                            Storage = "512GB",
                            AvailableStorage = "480GB",
                            Ram = "16GB",
                            OperatingSystem = "MIUI 14, Android 13",
                            Cpu = "Qualcomm Snapdragon 8 Gen 2 8 nhân",
                            Gpu = "Adreno 740",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Da Nano",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Ô Liu",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Da Nano",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    await _productService.AddProduct(xiaomi14Pro);
                    await _productService.AddProduct(xiaomi14Ultra);
                    await _productService.AddProduct(xiaomi15);
                    await _productService.AddProduct(xiaomi15Pro);
                    await _productService.AddProduct(xiaomi15Ultra);
                    await _productService.AddProduct(xiaomiMixFlip);
                    await _productService.AddProduct(xiaomiMixFold3);
                    await _productService.AddProduct(xiaomiMixFold4);
                    await _productService.AddProduct(xiaomi13Pro);
                    await _productService.AddProduct(xiaomi13Ultra);

                    #endregion

                    #region Samsung Phones (3M - 10M VND)

                    // 1. Samsung Galaxy A05 (~3.09 triệu)
                    var samsungGalaxyA05 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A05",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a05",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a05", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "4GB/128GB",
                            Description = "",
                            ImportPrice = 2690000m,
                            Price = 3090000m,
                            Storage = "128GB",
                            AvailableStorage = "105GB",
                            Ram = "4GB",
                            OperatingSystem = "One UI Core 5.1, Android 13",
                            Cpu = "MediaTek Helio G85 8 nhân",
                            Gpu = "Mali-G52 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Tuyền",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Matcha",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc Xỉu",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 2. Samsung Galaxy A05s (~3.59 triệu)
                    var samsungGalaxyA05s = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A05s",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a05s",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a05s", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "4GB/128GB",
                            Description = "",
                            ImportPrice = 3190000m,
                            Price = 3590000m,
                            Storage = "128GB",
                            AvailableStorage = "105GB",
                            Ram = "4GB",
                            OperatingSystem = "One UI Core 5.1, Android 13",
                            Cpu = "Qualcomm Snapdragon 680 8 nhân",
                            Gpu = "Adreno 610",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Đá",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Matcha",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc Xỉu",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 3. Samsung Galaxy A06 (~3.49 triệu)
                    var samsungGalaxyA06 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A06",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a06",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a06", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "4GB/128GB",
                            Description = "",
                            ImportPrice = 3090000m,
                            Price = 3490000m,
                            Storage = "128GB",
                            AvailableStorage = "108GB",
                            Ram = "4GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "MediaTek Helio G85 8 nhân",
                            Gpu = "Mali-G52 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lam",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 4. Samsung Galaxy M14 5G (~3.99 triệu)
                    var samsungGalaxyM145G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy M14 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-m14-5g",
                        Tags = new List<string> { "samsung", "galaxy-m", "galaxy-m14-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "4GB/128GB",
                            Description = "",
                            ImportPrice = 3490000m,
                            Price = 3990000m,
                            Storage = "128GB",
                            AvailableStorage = "105GB",
                            Ram = "4GB",
                            OperatingSystem = "One UI Core 5.1, Android 13",
                            Cpu = "Exynos 1330 8 nhân",
                            Gpu = "Mali-G68 MP2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Hải Quân",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Ánh Băng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 5. Samsung Galaxy A15 (~4.59 triệu)
                    var samsungGalaxyA15 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A15",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a15",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a15", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 4090000m,
                            Price = 4590000m,
                            Storage = "128GB",
                            AvailableStorage = "108GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.0, Android 14",
                            Cpu = "MediaTek Helio G99 8 nhân",
                            Gpu = "Mali-G57 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Bản Lĩnh",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lạc Quan",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng Cá Tính",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 6. Samsung Galaxy A15 5G (~5.29 triệu)
                    var samsungGalaxyA155G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A15 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a15-5g",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a15-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 4690000m,
                            Price = 5290000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.0, Android 14",
                            Cpu = "MediaTek Dimensity 6100+ 8 nhân",
                            Gpu = "Mali-G57 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Dương Thẫm",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lơ",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng Tươi",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 7. Samsung Galaxy M15 5G (~4.99 triệu)
                    var samsungGalaxyM155G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy M15 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-m15-5g",
                        Tags = new List<string> { "samsung", "galaxy-m", "galaxy-m15-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "6GB/128GB",
                            Description = "",
                            ImportPrice = 4390000m,
                            Price = 4990000m,
                            Storage = "128GB",
                            AvailableStorage = "108GB",
                            Ram = "6GB",
                            OperatingSystem = "One UI 6.0, Android 14",
                            Cpu = "MediaTek Dimensity 6100+ 8 nhân",
                            Gpu = "Mali-G57 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Thạch Anh",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Ngọc",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Đậm Tối Giản",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 8. Samsung Galaxy A16 5G (~5.79 triệu)
                    var samsungGalaxyA165G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A16 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a16-5g",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a16-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 5190000m,
                            Price = 5790000m,
                            Storage = "128GB",
                            AvailableStorage = "108GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "MediaTek Dimensity 6300 8 nhân",
                            Gpu = "Mali-G57 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Tinh Quái",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Bạc Hà",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng Rực Rỡ",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 9. Samsung Galaxy A25 5G (~6.19 triệu)
                    var samsungGalaxyA255G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A25 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a25-5g",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a25-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 5490000m,
                            Price = 6190000m,
                            Storage = "128GB",
                            AvailableStorage = "108GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.0, Android 14",
                            Cpu = "Exynos 1280 8 nhân",
                            Gpu = "Mali-G68",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Bản Lĩnh",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lạc Quan",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng Cá Tính",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 10. Samsung Galaxy M34 5G (~6.69 triệu)
                    var samsungGalaxyM345G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy M34 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-m34-5g",
                        Tags = new List<string> { "samsung", "galaxy-m", "galaxy-m34-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 5990000m,
                            Price = 6690000m,
                            Storage = "128GB",
                            AvailableStorage = "108GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 5.1, Android 13",
                            Cpu = "Exynos 1280 8 nhân",
                            Gpu = "Mali-G68",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Thác Nước",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Ánh Băng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc Ánh Sao",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 11. Samsung Galaxy A34 5G (~7.19 triệu)
                    var samsungGalaxyA345G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A34 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a34-5g",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a34-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 6390000m,
                            Price = 7190000m,
                            Storage = "128GB",
                            AvailableStorage = "108GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.0, Android 14",
                            Cpu = "MediaTek Dimensity 1080 8 nhân",
                            Gpu = "Mali-G68 MC4",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc Bất Phàm",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Dũng Mãnh",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Chiến Binh",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 12. Samsung Galaxy A35 5G (~7.79 triệu)
                    var samsungGalaxyA355G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A35 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a35-5g",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a35-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 6990000m,
                            Price = 7790000m,
                            Storage = "128GB",
                            AvailableStorage = "108GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Exynos 1380 8 nhân",
                            Gpu = "Mali-G68 MP5",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Iceblue",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Navy",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng Lemon",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 13. Samsung Galaxy M54 5G (~8.49 triệu)
                    var samsungGalaxyM545G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy M54 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-m54-5g",
                        Tags = new List<string> { "samsung", "galaxy-m", "galaxy-m54-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 7590000m,
                            Price = 8490000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 5.1, Android 13",
                            Cpu = "Exynos 1380 8 nhân",
                            Gpu = "Mali-G68 MP5",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc Ánh Sao",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Đậm",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 14. Samsung Galaxy A54 5G (~8.99 triệu)
                    var samsungGalaxyA545G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A54 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a54-5g",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a54-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 7990000m,
                            Price = 8990000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.0, Android 14",
                            Cpu = "Exynos 1380 8 nhân",
                            Gpu = "Mali-G68 MP5",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Dũng Mãnh",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Chiến Binh",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Oải Hương",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 15. Samsung Galaxy A55 5G (~9.69 triệu)
                    var samsungGalaxyA555G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A55 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a55-5g",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a55-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 8690000m,
                            Price = 9690000m,
                            Storage = "128GB",
                            AvailableStorage = "108GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Exynos 1480 8 nhân",
                            Gpu = "Xclipse 530",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Iceblue",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Navy",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Lilac",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    await _productService.AddProduct(samsungGalaxyA05);
                    await _productService.AddProduct(samsungGalaxyA05s);
                    await _productService.AddProduct(samsungGalaxyA06);
                    await _productService.AddProduct(samsungGalaxyM145G);
                    await _productService.AddProduct(samsungGalaxyA15);
                    await _productService.AddProduct(samsungGalaxyA155G);
                    await _productService.AddProduct(samsungGalaxyM155G);
                    await _productService.AddProduct(samsungGalaxyA165G);
                    await _productService.AddProduct(samsungGalaxyA255G);
                    await _productService.AddProduct(samsungGalaxyM345G);
                    await _productService.AddProduct(samsungGalaxyA345G);
                    await _productService.AddProduct(samsungGalaxyA355G);
                    await _productService.AddProduct(samsungGalaxyM545G);
                    await _productService.AddProduct(samsungGalaxyA545G);
                    await _productService.AddProduct(samsungGalaxyA555G);

                    #endregion

                    #region Samsung Phones (10M - 20M VND)

                    // 1. Samsung Galaxy A55 5G (8GB/256GB) (~10.49 triệu)
                    var samsungGalaxyA55256GB = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A55 5G (8GB/256GB)",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a55-5g-256gb",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a55", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 9690000m,
                            Price = 10490000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Exynos 1480 8 nhân",
                            Gpu = "Xclipse 530",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Iceblue",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Navy",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Lilac",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 2. Samsung Galaxy M55 5G (~10.99 triệu)
                    var samsungGalaxyM555G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy M55 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-m55-5g",
                        Tags = new List<string> { "samsung", "galaxy-m", "galaxy-m55-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 10190000m,
                            Price = 10990000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Qualcomm Snapdragon 7 Gen 1 8 nhân",
                            Gpu = "Adreno 644",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Bóng Đêm",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lam Nhẹ",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 3. Samsung Galaxy A73 5G (~11.49 triệu)
                    var samsungGalaxyA735G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy A73 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-a73-5g",
                        Tags = new List<string> { "samsung", "galaxy-a", "galaxy-a73-5g", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 10590000m,
                            Price = 11490000m,
                            Storage = "128GB",
                            AvailableStorage = "108GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.0, Android 14",
                            Cpu = "Qualcomm Snapdragon 778G 5G 8 nhân",
                            Gpu = "Adreno 642L",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Tinh Tế",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Thời Thượng",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Tinh Khôi",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 4. Samsung Galaxy S21 FE 5G (~11.99 triệu)
                    var samsungGalaxyS21FE5G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S21 FE 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s21-fe-5g",
                        Tags = new List<string> { "samsung", "galaxy-s", "galaxy-s21-fe", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 11090000m,
                            Price = 11990000m,
                            Storage = "128GB",
                            AvailableStorage = "105GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Exynos 2100 8 nhân",
                            Gpu = "Mali-G78 MP14",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Olive",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Lavender",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Tinh Khiết",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Graphite",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 5. Samsung Galaxy XCover 7 (~12.49 triệu)
                    var samsungGalaxyXCover7 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy XCover 7",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-xcover-7",
                        Tags = new List<string> { "samsung", "xcover", "rugged", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "6GB/128GB",
                            Description = "",
                            ImportPrice = 11490000m,
                            Price = 12490000m,
                            Storage = "128GB",
                            AvailableStorage = "106GB",
                            Ram = "6GB",
                            OperatingSystem = "One UI 6.0, Android 14",
                            Cpu = "MediaTek Dimensity 6100+ 8 nhân",
                            Gpu = "Mali-G57 MC2",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Siêu Bền",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 6. Samsung Galaxy S23 FE (8GB/128GB) (~12.99 triệu)
                    var samsungGalaxyS23FE = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S23 FE",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s23-fe",
                        Tags = new List<string> { "samsung", "galaxy-s", "galaxy-s23-fe", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 11990000m,
                            Price = 12990000m,
                            Storage = "128GB",
                            AvailableStorage = "105GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Exynos 2200 8 nhân",
                            Gpu = "Xclipse 920",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Mint",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Purple",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Graphite",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Cream",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 7. Samsung Galaxy S23 FE (8GB/256GB) (~13.99 triệu)
                    var samsungGalaxyS23FE256GB = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S23 FE (8GB/256GB)",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s23-fe-256gb",
                        Tags = new List<string> { "samsung", "galaxy-s", "galaxy-s23-fe", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 12890000m,
                            Price = 13990000m,
                            Storage = "256GB",
                            AvailableStorage = "233GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Exynos 2200 8 nhân",
                            Gpu = "Xclipse 920",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Mint",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Graphite",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Purple",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 8. Samsung Galaxy S22 5G (~13.49 triệu)
                    var samsungGalaxyS225G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S22 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s22-5g",
                        Tags = new List<string> { "samsung", "galaxy-s", "galaxy-s22", "flagship", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 12490000m,
                            Price = 13490000m,
                            Storage = "128GB",
                            AvailableStorage = "105GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Qualcomm Snapdragon 8 Gen 1 8 nhân",
                            Gpu = "Adreno 730",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Phantom",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Phantom",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Zeta",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng Blossom",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 9. Samsung Galaxy Z Flip4 5G (~14.49 triệu)
                    var samsungGalaxyZFlip45G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy Z Flip4 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-z-flip-4-5g",
                        Tags = new List<string> { "samsung", "galaxy-z", "z-flip-4", "foldable", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 13290000m,
                            Price = 14490000m,
                            Storage = "128GB",
                            AvailableStorage = "105GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Qualcomm Snapdragon 8+ Gen 1 8 nhân",
                            Gpu = "Adreno 730",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Bora",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Graphite",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Hồng Champagne",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Lovebird",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 10. Samsung Galaxy S24 FE (8GB/128GB) (~15.49 triệu)
                    var samsungGalaxyS24FE = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S24 FE",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s24-fe",
                        Tags = new List<string> { "samsung", "galaxy-s", "galaxy-s24-fe", "galaxy-ai", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 14290000m,
                            Price = 15490000m,
                            Storage = "128GB",
                            AvailableStorage = "105GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1.1, Android 14",
                            Cpu = "Exynos 2400e 10 nhân",
                            Gpu = "Xclipse 940",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Topaz",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Graphite",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng Chanh",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Ngọc Hera",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 11. Samsung Galaxy S22+ 5G (~15.99 triệu)
                    var samsungGalaxyS22Plus5G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S22+ 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s22-plus-5g",
                        Tags = new List<string> { "samsung", "galaxy-s", "galaxy-s22-plus", "flagship", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 14790000m,
                            Price = 15990000m,
                            Storage = "128GB",
                            AvailableStorage = "105GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Qualcomm Snapdragon 8 Gen 1 8 nhân",
                            Gpu = "Adreno 730",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Phantom",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Trắng Phantom",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Zeta",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 12. Samsung Galaxy S24 FE (8GB/256GB) (~16.49 triệu)
                    var samsungGalaxyS24FE256GB = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S24 FE (8GB/256GB)",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s24-fe-256gb",
                        Tags = new List<string> { "samsung", "galaxy-s", "galaxy-s24-fe", "galaxy-ai", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 15190000m,
                            Price = 16490000m,
                            Storage = "256GB",
                            AvailableStorage = "233GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1.1, Android 14",
                            Cpu = "Exynos 2400e 10 nhân",
                            Gpu = "Xclipse 940",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Topaz",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Graphite",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Ngọc Hera",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 13. Samsung Galaxy S23 5G (8GB/128GB) (~16.99 triệu)
                    var samsungGalaxyS235G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S23 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s23-5g",
                        Tags = new List<string> { "samsung", "galaxy-s", "galaxy-s23", "flagship", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/128GB",
                            Description = "",
                            ImportPrice = 15690000m,
                            Price = 16990000m,
                            Storage = "128GB",
                            AvailableStorage = "105GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Snapdragon 8 Gen 2 for Galaxy 8 nhân",
                            Gpu = "Adreno 740",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Phantom",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Kem Cotton",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Botanic",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Lavender",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 14. Samsung Galaxy Z Flip5 5G (~17.99 triệu)
                    var samsungGalaxyZFlip55G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy Z Flip5 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-z-flip-5-5g",
                        Tags = new List<string> { "samsung", "galaxy-z", "z-flip-5", "foldable", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 16590000m,
                            Price = 17990000m,
                            Storage = "256GB",
                            AvailableStorage = "235GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Snapdragon 8 Gen 2 for Galaxy 8 nhân",
                            Gpu = "Adreno 740",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Mint",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Indie",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Kem Latte",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Tím Fancy",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    // 15. Samsung Galaxy S23+ 5G (~19.49 triệu)
                    var samsungGalaxyS23Plus5G = new ProductCreateModel
                    {
                        CategoryId = resultCategoryMobilephone.Data,
                        BrandId = resultBrandSamsung.Data,
                        Name = "Samsung Galaxy S23+ 5G",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "samsung-galaxy-s23-plus-5g",
                        Tags = new List<string> { "samsung", "galaxy-s", "galaxy-s23-plus", "flagship", "smartphone" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 17990000m,
                            Price = 19490000m,
                            Storage = "256GB",
                            AvailableStorage = "233GB",
                            Ram = "8GB",
                            OperatingSystem = "One UI 6.1, Android 14",
                            Cpu = "Snapdragon 8 Gen 2 for Galaxy 8 nhân",
                            Gpu = "Adreno 740",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Phantom",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Kem Cotton",
                                    Stock = 50,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Botanic",
                                    Stock = 50,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    await _productService.AddProduct(samsungGalaxyA55256GB);
                    await _productService.AddProduct(samsungGalaxyM555G);
                    await _productService.AddProduct(samsungGalaxyA735G);
                    await _productService.AddProduct(samsungGalaxyS21FE5G);
                    await _productService.AddProduct(samsungGalaxyXCover7);
                    await _productService.AddProduct(samsungGalaxyS23FE);
                    await _productService.AddProduct(samsungGalaxyS23FE256GB);
                    await _productService.AddProduct(samsungGalaxyS225G);
                    await _productService.AddProduct(samsungGalaxyZFlip45G);
                    await _productService.AddProduct(samsungGalaxyS24FE);
                    await _productService.AddProduct(samsungGalaxyS22Plus5G);
                    await _productService.AddProduct(samsungGalaxyS24FE256GB);
                    await _productService.AddProduct(samsungGalaxyS235G);
                    await _productService.AddProduct(samsungGalaxyZFlip55G);
                    await _productService.AddProduct(samsungGalaxyS23Plus5G);

                    #endregion

                    #region Apple MacBook Laptops

                    var macbookAirM1 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryLaptop.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "MacBook Air 13\" M1 8GB 256GB",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "macbook-air-13-m1-8gb-256gb",
                        Tags = new List<string> { "apple", "macbook", "macbook-air", "m1", "laptop" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 20000000m,
                            Price = 22990000m,
                            Storage = "256GB",
                            AvailableStorage = "240GB",
                            Ram = "8GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M1 8 nhân",
                            Gpu = "Apple M1 7 nhân GPU",
                            ScreenSize = "13.3 inch",
                            RefreshRate = "60Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Vàng Ánh Kim",
                                    Stock = 30,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 30,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Xịt",
                                    Stock = 30,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    var macbookAirM2 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryLaptop.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "MacBook Air 13\" M2 8GB 256GB",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "macbook-air-13-m2-8gb-256gb",
                        Tags = new List<string> { "apple", "macbook", "macbook-air", "m2", "laptop" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 25000000m,
                            Price = 27990000m,
                            Storage = "256GB",
                            AvailableStorage = "240GB",
                            Ram = "8GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M2 8 nhân",
                            Gpu = "Apple M2 8 nhân GPU",
                            ScreenSize = "13.6 inch",
                            RefreshRate = "60Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Bầu Trời",
                                    Stock = 30,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Ánh Sao",
                                    Stock = 30,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xám Xịt",
                                    Stock = 30,
                                    ImageUrl = ""
                                }
                            }
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/512GB",
                            Description = "",
                            ImportPrice = 29000000m,
                            Price = 32990000m,
                            Storage = "512GB",
                            AvailableStorage = "490GB",
                            Ram = "8GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M2 8 nhân",
                            Gpu = "Apple M2 10 nhân GPU",
                            ScreenSize = "13.6 inch",
                            RefreshRate = "60Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Bầu Trời",
                                    Stock = 20,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Ánh Sao",
                                    Stock = 20,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    var macbookAirM2_15 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryLaptop.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "MacBook Air 15\" M2 8GB 256GB",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "macbook-air-15-m2-8gb-256gb",
                        Tags = new List<string> { "apple", "macbook", "macbook-air", "m2", "laptop", "15-inch" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 29000000m,
                            Price = 31990000m,
                            Storage = "256GB",
                            AvailableStorage = "240GB",
                            Ram = "8GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M2 8 nhân",
                            Gpu = "Apple M2 10 nhân GPU",
                            ScreenSize = "15.3 inch",
                            RefreshRate = "60Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Ánh Sao",
                                    Stock = 30,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đêm Tối",
                                    Stock = 30,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    var macbookAirM3 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryLaptop.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "MacBook Air 13\" M3 8GB 256GB",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "macbook-air-13-m3-8gb-256gb",
                        Tags = new List<string> { "apple", "macbook", "macbook-air", "m3", "laptop" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 27500000m,
                            Price = 29990000m,
                            Storage = "256GB",
                            AvailableStorage = "240GB",
                            Ram = "8GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M3 8 nhân",
                            Gpu = "Apple M3 10 nhân GPU",
                            ScreenSize = "13.6 inch",
                            RefreshRate = "60Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Ánh Sao",
                                    Stock = 40,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đêm Tối",
                                    Stock = 40,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Xanh Bầu Trời",
                                    Stock = 40,
                                    ImageUrl = ""
                                }
                            }
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "16GB/512GB",
                            Description = "",
                            ImportPrice = 33000000m,
                            Price = 36990000m,
                            Storage = "512GB",
                            AvailableStorage = "490GB",
                            Ram = "16GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M3 8 nhân",
                            Gpu = "Apple M3 10 nhân GPU",
                            ScreenSize = "13.6 inch",
                            RefreshRate = "60Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Ánh Sao",
                                    Stock = 20,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đêm Tối",
                                    Stock = 20,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    var macbookAirM3_15 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryLaptop.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "MacBook Air 15\" M3 8GB 256GB",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "macbook-air-15-m3-8gb-256gb",
                        Tags = new List<string> { "apple", "macbook", "macbook-air", "m3", "laptop", "15-inch" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/256GB",
                            Description = "",
                            ImportPrice = 31000000m,
                            Price = 34990000m,
                            Storage = "256GB",
                            AvailableStorage = "240GB",
                            Ram = "8GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M3 8 nhân",
                            Gpu = "Apple M3 10 nhân GPU",
                            ScreenSize = "15.3 inch",
                            RefreshRate = "60Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Ánh Sao",
                                    Stock = 30,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đêm Tối",
                                    Stock = 30,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    var macbookPro14M3 = new ProductCreateModel
                    {
                        CategoryId = resultCategoryLaptop.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "MacBook Pro 14\" M3 8GB 512GB",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "macbook-pro-14-m3-8gb-512gb",
                        Tags = new List<string> { "apple", "macbook", "macbook-pro", "m3", "laptop", "14-inch" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "8GB/512GB",
                            Description = "",
                            ImportPrice = 37000000m,
                            Price = 41990000m,
                            Storage = "512GB",
                            AvailableStorage = "490GB",
                            Ram = "8GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M3 8 nhân",
                            Gpu = "Apple M3 10 nhân GPU",
                            ScreenSize = "14.2 inch",
                            RefreshRate = "120Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 25,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Vũ Trụ",
                                    Stock = 25,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    var macbookPro14M3Pro = new ProductCreateModel
                    {
                        CategoryId = resultCategoryLaptop.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "MacBook Pro 14\" M3 Pro 18GB 512GB",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "macbook-pro-14-m3-pro-18gb-512gb",
                        Tags = new List<string> { "apple", "macbook", "macbook-pro", "m3-pro", "laptop", "14-inch" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "18GB/512GB",
                            Description = "",
                            ImportPrice = 51000000m,
                            Price = 57990000m,
                            Storage = "512GB",
                            AvailableStorage = "490GB",
                            Ram = "18GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M3 Pro 11 nhân",
                            Gpu = "Apple M3 Pro 14 nhân GPU",
                            ScreenSize = "14.2 inch",
                            RefreshRate = "120Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 20,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Vũ Trụ",
                                    Stock = 20,
                                    ImageUrl = ""
                                }
                            }
                        },
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "18GB/1TB",
                            Description = "",
                            ImportPrice = 59000000m,
                            Price = 65990000m,
                            Storage = "1TB",
                            AvailableStorage = "950GB",
                            Ram = "18GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M3 Pro 11 nhân",
                            Gpu = "Apple M3 Pro 14 nhân GPU",
                            ScreenSize = "14.2 inch",
                            RefreshRate = "120Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 15,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Vũ Trụ",
                                    Stock = 15,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    var macbookPro14M3Max = new ProductCreateModel
                    {
                        CategoryId = resultCategoryLaptop.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "MacBook Pro 14\" M3 Max 36GB 1TB",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "macbook-pro-14-m3-max-36gb-1tb",
                        Tags = new List<string> { "apple", "macbook", "macbook-pro", "m3-max", "laptop", "14-inch" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "36GB/1TB",
                            Description = "",
                            ImportPrice = 75000000m,
                            Price = 84990000m,
                            Storage = "1TB",
                            AvailableStorage = "950GB",
                            Ram = "36GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M3 Max 14 nhân",
                            Gpu = "Apple M3 Max 30 nhân GPU",
                            ScreenSize = "14.2 inch",
                            RefreshRate = "120Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 10,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Vũ Trụ",
                                    Stock = 10,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    var macbookPro16M3Pro = new ProductCreateModel
                    {
                        CategoryId = resultCategoryLaptop.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "MacBook Pro 16\" M3 Pro 18GB 512GB",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "macbook-pro-16-m3-pro-18gb-512gb",
                        Tags = new List<string> { "apple", "macbook", "macbook-pro", "m3-pro", "laptop", "16-inch" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "18GB/512GB",
                            Description = "",
                            ImportPrice = 60000000m,
                            Price = 67990000m,
                            Storage = "512GB",
                            AvailableStorage = "490GB",
                            Ram = "18GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M3 Pro 12 nhân",
                            Gpu = "Apple M3 Pro 18 nhân GPU",
                            ScreenSize = "16.2 inch",
                            RefreshRate = "120Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 15,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Vũ Trụ",
                                    Stock = 15,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    var macbookPro16M3Max = new ProductCreateModel
                    {
                        CategoryId = resultCategoryLaptop.Data,
                        BrandId = resultBrandApple.Data,
                        Name = "MacBook Pro 16\" M3 Max 48GB 1TB",
                        ShortDescription = "",
                        Description = "",
                        Warranty = 12,
                        Slug = "macbook-pro-16-m3-max-48gb-1tb",
                        Tags = new List<string> { "apple", "macbook", "macbook-pro", "m3-max", "laptop", "16-inch" },
                        IsFeatured = false,
                        StartSellingDate = TimeZoneHelper.GetUtcNow(),
                        MainImageUrl = "",
                        GalleryImageUrls = new List<string>(),
                        SalePrice = 0,
                        PublishDate = TimeZoneHelper.GetUtcNow(),
                        Variants = new List<Model.DTOs.ProductVariant.ProductVariantCreateModel>
                    {
                        new Model.DTOs.ProductVariant.ProductVariantCreateModel
                        {
                            Name = "48GB/1TB",
                            Description = "",
                            ImportPrice = 84000000m,
                            Price = 94990000m,
                            Storage = "1TB",
                            AvailableStorage = "950GB",
                            Ram = "48GB",
                            OperatingSystem = "macOS",
                            Cpu = "Apple M3 Max 16 nhân",
                            Gpu = "Apple M3 Max 40 nhân GPU",
                            ScreenSize = "16.2 inch",
                            RefreshRate = "120Hz",
                            Options = new List<Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel>
                            {
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Bạc",
                                    Stock = 8,
                                    ImageUrl = ""
                                },
                                new Model.DTOs.ProductVariantOption.ProductVariantOptionCreateModel
                                {
                                    Name = "Đen Vũ Trụ",
                                    Stock = 8,
                                    ImageUrl = ""
                                }
                            }
                        }
                    }
                    };

                    await _productService.AddProduct(macbookAirM1);
                    await _productService.AddProduct(macbookAirM2);
                    await _productService.AddProduct(macbookAirM2_15);
                    await _productService.AddProduct(macbookAirM3);
                    await _productService.AddProduct(macbookAirM3_15);
                    await _productService.AddProduct(macbookPro14M3);
                    await _productService.AddProduct(macbookPro14M3Pro);
                    await _productService.AddProduct(macbookPro14M3Max);
                    await _productService.AddProduct(macbookPro16M3Pro);
                    await _productService.AddProduct(macbookPro16M3Max);

                    #endregion

                }
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
