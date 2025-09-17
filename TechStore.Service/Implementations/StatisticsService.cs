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
                TotalIncomeChartData = new List<ChartDecimalData>(),
                TotalOrdersChartData = new List<ChartData>(),
                HotProducts = new List<AdminProductListItemModel>(),
                BestSellProducts = new List<AdminProductListItemModel>(),
                BestRatedProducts = new List<AdminProductListItemModel>(),
                LoyalCustomer = new List<UserListItemResponseModel>(),
                RecentlyActions = new List<ActionModel>()
            };


            var orders = await _uow.Orders.GetOrdersIncludeItemsAsync(o => true);
            var products = await _uow.Products.GetAllAsync();
            var users = await _uow.Users.GetAllAsync();

            if (orders != null)
            {
                orders = await GetCategoryOfOrderItem(orders);

                var orderModels = orders.Select(o => o.ToOrderResponseModel()).ToList();
                overviewData.ProcessingOfPendingOrders = GetProcessingOfPendingOrders(orderModels);
                overviewData.DeliveringOfProcessingOrders = GetDeliveringOfProcessingOrders(orderModels);
                overviewData.ComplatedOfDeliveringOrders = GetComplatedOfDeliveringOrders(orderModels);
                overviewData.CategoryChartData = GetCategoryChartData(orderModels);
                overviewData.TotalRevenueChartData = GetMonthlyRevenueSeries(orderModels);
                overviewData.TotalIncomeChartData = GetMonthlyIncomeSeriesAsync(orderModels, products.ToList());
                overviewData.TotalOrdersChartData = GetMonthlyTotalOrdersSeries(orderModels);
                overviewData.HotProducts = GetHotProducts(orderModels, products.ToList());
                overviewData.BestSellProducts = GetBestSellProducts(products.ToList());
                overviewData.BestRatedProducts = GetBestRatedProducts(products.ToList());
                overviewData.LoyalCustomer = GetLoyalCustomers(orderModels, users.ToList());
                overviewData.RecentlyActions = GetActionsRecently(orderModels, users.ToList());
            }


            serviceResult.Data = overviewData;

            return serviceResult;
        }

        private List<ChartData> GetCategoryChartData(List<OrderResponseModel> orderModels)
        {
            var data = new List<ChartData>();

            var categoryStatistics = orderModels.SelectMany(order => order.Items).
                GroupBy(item => item.CategoryName)
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

        public List<ChartDecimalData> GetMonthlyRevenueSeries(List<OrderResponseModel> allOrderModels)
        {
            var orderModels = allOrderModels.Where(x => x.OrderStatus == EOrderStatus.Completed).ToList();

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

            foreach (var order in orderModels)
            {
                if (order.CreatedAt.Year == currentYear)
                {
                    int monthIndex = order.CreatedAt.Month - 1; // từ 0 đến 11
                    data[monthIndex].Value += order.TotalPrice;
                }
            }

            return data;
        }

        public List<ChartDecimalData> GetMonthlyIncomeSeriesAsync(List<OrderResponseModel> allOrderModels, List<Product> products)
        {
            var orderModels = allOrderModels.Where(x => x.OrderStatus == EOrderStatus.Completed).ToList();

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

            //foreach (var order in orderModels)
            //{
            //    if (order.CreatedAt.Year == currentYear)
            //    {
            //        int monthIndex = order.CreatedAt.Month - 1; // từ 0 đến 11

            //        decimal incomeOfOrderItem = 0m;

            //        foreach (var item in order.Items)
            //        {
            //            var product = products.Where(x => x.PublicId == item.ProductVariantOptionId).FirstOrDefault();

            //            if (product != null)
            //            {
            //                incomeOfOrderItem += item.TotalPrice - product.Variants.Where(v => v.Id == ).ImportPrice * item.Quantity;
            //            }
            //        }


            //        data[monthIndex].Value += incomeOfOrderItem;
            //    }
            //}

            return data;
        }

        public List<ChartData> GetMonthlyTotalOrdersSeries(List<OrderResponseModel> orderModels)
        {
            var data = new List<ChartData>();
            //var orders = await _orderService.GetAllOrdersAsync();

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

            foreach (var order in orderModels)
            {
                if (order.CreatedAt.Year == currentYear)
                {
                    int monthIndex = order.CreatedAt.Month - 1; // từ 0 đến 11

                    data[monthIndex].Value += 1;
                }
            }

            return data;
        }

        public RadialBarChartData GetProcessingOfPendingOrders(List<OrderResponseModel> orderModels)
        {
            var data = new RadialBarChartData
            {
                Goal = 0,
                Progress = 0,
                ProgressPercent = 0
            };

            var pendingOrderCount = orderModels.
                Where(x => x.OrderStatus == EOrderStatus.Pending).Count();

            var processingOrderCount = orderModels.
                Where(x => x.OrderStatus == EOrderStatus.Processing).Count();

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

        public RadialBarChartData GetDeliveringOfProcessingOrders(List<OrderResponseModel> orderModels)
        {
            var data = new RadialBarChartData
            {
                Goal = 0,
                Progress = 0,
                ProgressPercent = 0
            };

            var processingOrderCount = orderModels.
                Where(x => x.OrderStatus == EOrderStatus.Processing).Count();

            var deliveringOrderCount = orderModels.
                Where(x => x.OrderStatus == EOrderStatus.Delivering).Count();

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

        public RadialBarChartData GetComplatedOfDeliveringOrders(List<OrderResponseModel> orderModels)
        {
            var data = new RadialBarChartData
            {
                Goal = 0,
                Progress = 0,
                ProgressPercent = 0
            };

            var deliveringOrderCount = orderModels.
                Where(x => x.OrderStatus == EOrderStatus.Delivering).Count();

            var complatedOrderCount = orderModels.
                Where(x => x.OrderStatus == EOrderStatus.Completed).Count();

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

        public List<AdminProductListItemModel> GetHotProducts(List<OrderResponseModel> orders, List<Product> products)
        {
            var productModels = new List<AdminProductListItemModel>();

            //var recentTop15Products = orders
            //    .Where(o => o.CreatedAt >= DateTime.UtcNow.AddDays(-30)) // lọc đơn hàng gần đây
            //    .SelectMany(o => o.Items) // gom tất cả OrderItem lại
            //    .GroupBy(item => new { item.ProductVariantOptionId, item.ProductName, item.CategoryName })
            //    .Select(g => new
            //    {
            //        ProductId = g.Key.ProductId,
            //        Name = g.Key.ProductName,
            //        CategoryName = g.Key.CategoryName,
            //        SoldCount = g.Sum(x => x.Quantity)
            //    })
            //    .OrderByDescending(x => x.SoldCount)
            //    .Take(6)
            //    .ToList();

            //if (recentTop15Products.Count > 0)
            //{
            //    foreach (var item in recentTop15Products)
            //    {
            //        var product = products.Where(x => x.PublicId == item.ProductId).FirstOrDefault();

            //        if (product != null)
            //        {
            //            var productModel = new AdminProductListItemModel
            //            {
            //                ProductId = product.PublicId,
            //                CategoryName = item.CategoryName,
            //                Name = product.Name,
            //                SoldCount = item.SoldCount,
            //                Price = product.Price,
            //                MainImageUrl = product.MainImageUrl,
            //                AverageRating = product.AverageRating,
            //                Stock = product.Stock,
            //                RatedCount = product.RatedCount,
            //            };
            //            productModels.Add(productModel);
            //        }
            //    }
            //}

            return productModels;
        }

        public List<AdminProductListItemModel> GetBestSellProducts(List<Product> products)
        {
            var productModels = new List<AdminProductListItemModel>();

            var top10SoldProducts = products
            .OrderByDescending(p => p.SoldCount)
            .Take(7)
            .ToList();

            if (top10SoldProducts.Count > 0)
            {
                foreach (var product in top10SoldProducts)
                {
                    productModels.Add(product.ToAdminProductListItem());
                }
            }

            return productModels;
        }

        public List<AdminProductListItemModel> GetBestRatedProducts(List<Product> products)
        {
            var productModels = new List<AdminProductListItemModel>();

            var top10RatedProducts = products
            .OrderByDescending(p => p.AverageRating)
            .Take(7)
            .ToList();

            if (top10RatedProducts.Count > 0)
            {
                foreach (var product in top10RatedProducts)
                {
                    productModels.Add(product.ToAdminProductListItem());
                }
            }

            return productModels;
        }

        public List<UserListItemResponseModel> GetLoyalCustomers(List<OrderResponseModel> orders, List<User> users)
        {
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
                    var user = users.Where(x => x.PublicId == item.UserId).FirstOrDefault();
                    if (user != null)
                    {
                        userModels.Add(user.ToUserListItemModel());
                    }
                }
            }
            return userModels;
        }

        public List<ActionModel> GetActionsRecently(List<OrderResponseModel> orders, List<User> users)
        {
            var actionModels = new List<ActionModel>();

            var newOrders = new ActionModel
            {
                Title = "Các đơn hàng mới",
                Description = orders.Where(x => x.OrderStatus == EOrderStatus.Pending && x.CreatedAt.Date == DateTime.UtcNow.Date).Count() + " đơn hàng được đặt",
                Time = TimeZoneHelper.GetUtcNow()
            };
            actionModels.Add(newOrders);

            var processingOrders = new ActionModel
            {
                Title = "Số đơn hàng được xử lý",
                Description = orders.Where(x => x.OrderStatus == EOrderStatus.Processing && x.CreatedAt.Date == DateTime.UtcNow.Date).Count() + " đơn hàng xử lý",
                Time = TimeZoneHelper.GetUtcNow()
            };
            actionModels.Add(processingOrders);

            var newUsers = new ActionModel
            {
                Title = "Số người đăng ký mới",
                Description = users.Where(x => x.CreatedAt.Date == DateTime.UtcNow.Date).Count() + " đăng ký mới tuần này",
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
                Description = orders.Where(x => x.OrderStatus == EOrderStatus.Delivering).Count() + " đơn hàng đang được giao",
                Time = TimeZoneHelper.GetUtcNow()
            };
            actionModels.Add(deliveringOrders);


            return actionModels;
        }

        private async Task<List<Order>> GetCategoryOfOrderItem(List<Order> orders)
        {
            //foreach (var order in orders)
            //{
            //    foreach (var item in order.OrderItems)
            //    {
            //        if (item.Product != null)
            //        {
            //            var category = await _uow.Categories.FindOneAsync(c => c.Id == item.Product.CategoryId);
            //            if (category != null)
            //            {
            //                item.Product.Category = category;
            //            }
            //        }
            //    }
            //}
            return orders;
        }
    }
}
