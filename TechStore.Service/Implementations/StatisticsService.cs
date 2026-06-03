using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Helpers;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Product;
using TechStore.Model.DTOs.Statistic;
using TechStore.Model.DTOs.User;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;

        public StatisticsService(IUnitOfWork uow,
            SequenceGeneratorService sequenceService
            )
        {
            _uow = uow;
            _sequenceService = sequenceService;
        }

        public async Task<ServiceResult<DashboardOverviewModel>> GetDashboardOverviewData()
        {
            var serviceResult = new ServiceResult<DashboardOverviewModel>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.GetDataSuccessful
            };

            var overviewData = new DashboardOverviewModel
            {
                ProcessingOfPendingOrders = new RadialBarChartData(),
                DeliveringOfProcessingOrders = new RadialBarChartData(),
                ComplatedOfDeliveringOrders = new RadialBarChartData(),
                CategoryChartData = new List<ChartData>(),
                TotalRevenueChartData = new List<ChartDecimalData>(),
                TotalProfitChartData = new List<ChartDecimalData>(),
                TotalOrdersChartData = new List<ChartData>(),
                HotProducts = new List<AdminProductListItemModel>(),
                TopSoldProducts = new List<AdminProductListItemModel>(),
                TopRatedProducts = new List<AdminProductListItemModel>(),
                LoyalCustomer = new List<UserListItemResponseModel>(),
                RecentlyActions = new List<ActionModel>()
            };

            DateTime currentDatetime = DateTime.Now.Date;

            overviewData.ProcessingOfPendingOrders = await GetProcessingOfPendingOrders();
            overviewData.DeliveringOfProcessingOrders = await GetDeliveringOfProcessingOrders();
            overviewData.ComplatedOfDeliveringOrders = await GetComplatedOfDeliveringOrders();
            overviewData.CategoryChartData = await GetCategoryChartData(currentDatetime.Year);
            overviewData.TotalRevenueChartData = await GetMonthlyRevenueSeries(currentDatetime);//doanh thu
            overviewData.TotalProfitChartData = await GetMonthlyProfitSeriesAsync();//loi nhuan
            overviewData.TotalOrdersChartData = await GetMonthlyTotalOrdersSeries();
            overviewData.HotProducts = await GetHotProducts();
            overviewData.TopSoldProducts = await GetTopSoldProducts();
            overviewData.TopRatedProducts = await GetTopRatedProductsAsync();
            overviewData.LoyalCustomer = await GetLoyalCustomers();
            overviewData.RecentlyActions = await GetActionsRecently();


            serviceResult.Data = overviewData;

            return serviceResult;
        }

        private async Task<List<ChartData>> GetCategoryChartData(int year)
        {
            var data = new List<ChartData>();

            var orders = await _uow.Orders.GetOrdersIncludeItemsDetailAsync(o => o.CreatedAt.Year == year, 1, int.MaxValue);

            var categoryStatistics = orders.SelectMany(order => order.OrderItems).
                GroupBy(item => item.ProductVariantOption.ProductVariant.Product.Category.Name)
                .Select(group => new ChartData
                {
                    Name = group.Key,
                    Value = group.Sum(item => item.Quantity)
                })
                .OrderByDescending(x => x.Value)
                .ToList();

            data = categoryStatistics;

            return data;
        }

        public async Task<List<ChartDecimalData>> GetMonthlyRevenueSeries(DateTime fromDate)
        {
            //var orderModels = allOrderModels.Where(x => x.OrderStatus == EOrderStatus.Completed).ToList();
            var orders = await _uow.Orders.FindManyAsync(o => o.CreatedAt.Year == fromDate.Year);

            var data = new List<ChartDecimalData>();

            var now = DateTime.UtcNow;
            var currentYear = now.Year;

            for (int i = 1; i <= now.Month; i++)
            {
                data.Add(new ChartDecimalData
                {
                    //Name = new DateTime(currentYear, i, 1).ToString("MMMM"), // "January", "February", ...
                    Name = i.ToString(),
                    Value = 0
                });
            }

            foreach (var order in orders)
            {
                if (order.CreatedAt.Year == currentYear)
                {
                    int monthIndex = order.CreatedAt.Month - 1; // từ 0 đến 11
                    data[monthIndex].Value += order.TotalPrice;
                }
            }

            return data;
        }

        public async Task<List<ChartDecimalData>> GetMonthlyProfitSeriesAsync()
        {
            //var orderModels = allOrderModels.Where(x => x.OrderStatus == EOrderStatus.Completed).ToList();
            //var orders = await _uow.Orders.FindManyAsync(o => o.OrderStatus == EOrderStatus.Completed);
            var orders = await _uow.Orders.TableNoTracking.Where(o => o.OrderStatus == EOrderStatus.Completed).Include(o => o.OrderItems).ToListAsync();


            var data = new List<ChartDecimalData>();
            //var orders = await _orderService.GetAllOrdersAsync();

            var now = DateTime.UtcNow;
            var currentYear = now.Year;

            // Khởi tạo mảng 12 tháng từ January đến December với giá trị mặc định = 0
            for (int i = 1; i <= now.Month; i++)
            {
                data.Add(new ChartDecimalData
                {
                    //Name = new DateTime(currentYear, i, 1).ToString("MMMM"), // "January", "February", ...
                    Name = i.ToString(),
                    Value = 0
                });
            }

            foreach (var order in orders)
            {
                if (order.CreatedAt.Year == currentYear)
                {
                    int monthIndex = order.CreatedAt.Month - 1; // từ 0 đến 11

                    decimal incomeOfOrderItem = 0m;

                    foreach (var item in order.OrderItems)
                    {
                        var pVO = await _uow.ProductVariantOptions.GetProductVariantOptionDetailByInternalIdAsync(item.ProductVariantOptionId);

                        if (pVO != null)
                        {
                            incomeOfOrderItem += item.TotalPrice - pVO.ProductVariant.ImportPrice * item.Quantity;
                        }
                    }


                    data[monthIndex].Value += incomeOfOrderItem;
                }
            }

            return data;
        }

        public async Task<List<ChartData>> GetMonthlyTotalOrdersSeries()
        {
            var data = new List<ChartData>();
            var orders = await _uow.Orders.FindManyAsync(o => o.CreatedAt.Year == DateTime.Now.Date.Year);

            var now = DateTime.UtcNow;
            var currentYear = now.Year;

            // Khởi tạo mảng 12 tháng từ January đến December với giá trị mặc định = 0
            for (int i = 1; i <= now.Month; i++)
            {
                data.Add(new ChartData
                {
                    //Name = new DateTime(currentYear, i, 1).ToString("MMMM"), // "January", "February", ...
                    Name = i.ToString(),
                    Value = 0
                });
            }

            foreach (var order in orders)
            {
                if (order.CreatedAt.Year == currentYear)
                {
                    int monthIndex = order.CreatedAt.Month - 1; // từ 0 đến 11

                    data[monthIndex].Value += 1;
                }
            }

            return data;
        }

        public async Task<RadialBarChartData> GetProcessingOfPendingOrders()
        {
            var data = new RadialBarChartData
            {
                Goal = 0,
                Progress = 0,
                ProgressPercent = 0
            };

            var pendingOrderCount = await _uow.Orders.CountAsync(o => o.OrderStatus == EOrderStatus.Pending);
            var processingOrderCount = await _uow.Orders.CountAsync(o => o.OrderStatus == EOrderStatus.Processing);

            if (pendingOrderCount == 0)
            {
                data.Goal = 0;
                data.Progress = processingOrderCount;
                data.ProgressPercent = 100;
            }
            else
            {
                data.Goal = pendingOrderCount;
                data.Progress = processingOrderCount;
                data.ProgressPercent = (processingOrderCount / pendingOrderCount) * 100;
            }

            return data;
        }

        public async Task<RadialBarChartData> GetDeliveringOfProcessingOrders()
        {
            var data = new RadialBarChartData
            {
                Goal = 0,
                Progress = 0,
                ProgressPercent = 0
            };
            var deliveringOrderCount = await _uow.Orders.CountAsync(o => o.OrderStatus == EOrderStatus.Delivering);
            var processingOrderCount = await _uow.Orders.CountAsync(o => o.OrderStatus == EOrderStatus.Processing);

            if (processingOrderCount == 0)
            {
                data.Goal = 0;
                data.Progress = deliveringOrderCount;
                data.ProgressPercent = 100;
            }
            else
            {
                data.Goal = processingOrderCount;
                data.Progress = deliveringOrderCount;
                data.ProgressPercent = (deliveringOrderCount / processingOrderCount) * 100;
            }

            return data;
        }

        public async Task<RadialBarChartData> GetComplatedOfDeliveringOrders()
        {
            var data = new RadialBarChartData
            {
                Goal = 0,
                Progress = 0,
                ProgressPercent = 0
            };

            var deliveringOrderCount = await _uow.Orders.CountAsync(o => o.OrderStatus == EOrderStatus.Delivering);
            var complatedOrderCount = await _uow.Orders.CountAsync(o => o.OrderStatus == EOrderStatus.Completed);

            if (deliveringOrderCount == 0)
            {
                data.Goal = 0;
                data.Progress = deliveringOrderCount;
                data.ProgressPercent = 100;
            }
            else
            {
                data.Goal = deliveringOrderCount;
                data.Progress = complatedOrderCount;
                data.ProgressPercent = (complatedOrderCount / deliveringOrderCount) * 100;
            }

            return data;
        }

        public async Task<List<AdminProductListItemModel>> GetHotProducts()
        {
            var productModels = new List<AdminProductListItemModel>();

            int curentMonth = DateTime.Now.Date.Month;

            var orders = await _uow.Orders.GetOrdersIncludeItemsDetailAsync(o => o.CreatedAt.Month >= curentMonth - 1, 1, 10000);

            var recentTopProducts = orders
                //.Where(o => o.CreatedAt >= DateTime.UtcNow.AddDays(-30)) // lọc đơn hàng gần đây
                .SelectMany(o => o.OrderItems) // gom tất cả OrderItem lại
                .GroupBy(item => new { item.ProductVariantOptionId })
                .Select(g => new
                {
                    PVOId = g.Key.ProductVariantOptionId,
                    SoldCount = g.Sum(x => x.Quantity),
                })
                .OrderByDescending(x => x.SoldCount)
                .Take(6)
                .ToList();

            var topPvoIds = recentTopProducts
            .Select(x => x.PVOId)
            .ToList();

                    var topOrderItems = orders
            .SelectMany(o => o.OrderItems)
            .Where(oi => topPvoIds.Contains(oi.ProductVariantOptionId))
            .ToList();

            if (topPvoIds.Count > 0)
            {
                foreach (var item in topOrderItems)
                {
                    var productModel = new AdminProductListItemModel
                    {
                        ProductVariantOptionId = item.ProductVariantOptionPublicId,
                        CategoryName = item.ProductVariantOption.ProductVariant.Product.Category.Name,
                        Name = item.ProductVariantOption.ProductVariant.Product.Name + " " + item.ProductVariantOption.ProductVariant.Name + " " + item.ProductVariantOption.Name,
                        SoldCount = item.ProductVariantOption.ProductVariant.SoldCount,
                        Price = item.ProductVariantOption.ProductVariant.Price,
                        MainImageUrl = item.ProductVariantOption.ImageUrl,
                        AverageRating = item.ProductVariantOption.ProductVariant.Product.AverageRating,
                        Stock = item.ProductVariantOption.Stock,
                        RatedCount = item.ProductVariantOption.ProductVariant.Product.RatedCount,
                    };
                    productModels.Add(productModel);
                }
            }

            return productModels;
        }

        public async Task<List<AdminProductListItemModel>> GetTopSoldProducts()
        {
            var productModels = new List<AdminProductListItemModel>();

            var topSoldProducts = await _uow.Products.GetTopSoldProductsAsync(7);

            foreach (var product in topSoldProducts)
            {
                productModels.Add(product.ToAdminProductListItem());
            }

            return productModels;
        }

        public async Task<List<AdminProductListItemModel>> GetTopRatedProductsAsync()
        {
            var productModels = new List<AdminProductListItemModel>();

            var top10RatedProducts = await _uow.Products.GetTopRatedProductsAsync(7);

            foreach (var product in top10RatedProducts)
            {
                productModels.Add(product.ToAdminProductListItem());
            }

            return productModels;
        }

        public async Task<List<UserListItemResponseModel>> GetLoyalCustomers()
        {
            var orders = await _uow.Orders.FindManyAsync(o => true);

            var userModels = new List<UserListItemResponseModel>();
            var top6LoyalCustomers = orders
                .GroupBy(o => o.CustomerId)
                .Select(g => new
                {
                    UserId = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .Take(6)
                .ToList();
            if (top6LoyalCustomers.Count > 0)
            {
                foreach (var item in top6LoyalCustomers)
                {
                    var user = await _uow.Users.FindOneAsync(x => x.Id == item.UserId);
                    if (user != null)
                    {
                        userModels.Add(user.ToUserListItemModel());
                    }
                }
            }
            return userModels;
        }

        public async Task<List<ActionModel>> GetActionsRecently()
        {
            var actionModels = new List<ActionModel>();

            var orders = await _uow.Orders.FindManyAsync(o => true);


            var newOrders = new ActionModel
            {
                Title = "Các đơn hàng mới",
                Description = await _uow.Orders.CountAsync(x => x.OrderStatus == EOrderStatus.Pending && x.CreatedAt.Date == DateTime.UtcNow.Date) + " đơn hàng được đặt",
                Time = TimeZoneHelper.GetUtcNow()
            };
            actionModels.Add(newOrders);

            var processingOrders = new ActionModel
            {
                Title = "Số đơn hàng được xử lý",
                Description = await _uow.Orders.CountAsync(x => x.OrderStatus == EOrderStatus.Processing && x.CreatedAt.Date == DateTime.UtcNow.Date) + " đơn hàng xử lý",
                Time = TimeZoneHelper.GetUtcNow()
            };
            actionModels.Add(processingOrders);

            var newUsers = new ActionModel
            {
                Title = "Số người đăng ký mới",
                Description = await _uow.Users.CountAsync(x => x.CreatedAt.Date == DateTime.UtcNow.Date) + " đăng ký mới tuần này",
                Time = TimeZoneHelper.GetUtcNow()
            };
            actionModels.Add(newUsers);

            var importProductsCount = new ActionModel
            {
                Title = "Nhập kho",
                Description = " hôm nay nhập kho 250 sản phẩm",
                Time = TimeZoneHelper.GetUtcNow()
            };
            actionModels.Add(importProductsCount);

            var deliveringOrders = new ActionModel
            {
                Title = "Số đơn hàng đâng vận chuyển",
                Description = await _uow.Orders.CountAsync(x => x.OrderStatus == EOrderStatus.Delivering) + " đơn hàng đang được giao",
                Time = TimeZoneHelper.GetUtcNow()
            };
            actionModels.Add(deliveringOrders);


            return actionModels;
        }

    }
}
