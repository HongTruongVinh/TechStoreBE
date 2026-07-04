using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
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
                    City = "Hà Nội",
                    District = "Hoàn Kiếm",
                    Address = "123, Hoàn Kiếm, Hà Nội",
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

                for (int i = 4; i <= 14; i++)
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
                        PhoneNumber = $"0912345{i:D3}",
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
                        PhoneNumber = $"01234567{i:D3}",
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
                var ipyear = 17;
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
                        Tags = new List<string> { "iphone", "iphone" + i},
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

                        var resultOrder = await _orderService.CreateCODOnlineOrderAsync(user.PublicId, order);
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
                await _uow.CartItems.DeleteAllAsync();
                await _uow.OrderItems.DeleteAllAsync();
                await _uow.Orders.DeleteAllAsync();
                await _uow.Categories.DeleteAllAsync();
                await _uow.Brands.DeleteAllAsync();
                await _uow.ProductVariantOptions.DeleteAllAsync();
                await _uow.ProductVariants.DeleteAllAsync();
                await _uow.Products.DeleteAllAsync();
                await _uow.Users.DeleteAllAsync();
                await _uow.ShippingDetails.DeleteAllAsync();
                await _uow.InvalidTokens.DeleteAllAsync();
                //await _uow.QRCodes.DeleteAllAsync();
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
