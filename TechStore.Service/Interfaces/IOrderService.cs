using System;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.RequestModel;

namespace TechStore.Service.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<List<OrderDetailResponseModel>>> GetListOrdersByStatusIdAsync(EOrderStatus statusId);
        Task<ServiceResult<List<OrderResponseModel>>> GetAllOrdersAsync();
        Task<ServiceResult<List<OrderListItemModel>>> GetOnlineOrdersAsync();
        Task<ServiceResult<string>> CreateCODOnlineOrderAsync(string userId, OrderCreateModel createOrderRequest);
        Task<ServiceResult<string>> CreatePrePayOnlineOrderAsync(string userId, OrderCreateModel createOrderRequest);
        Task<ServiceResult<string>> CreateInStoreOrderAsync(string createdByCashierId, InStoreOrderCreateModel createOrderRequest);
        
        Task<ServiceResult<List<OrderListItemModel>>> GetInStoreOrdersAsync();
        Task<ServiceResult<InStoreOrderResponseModel>> GetInStoreOrderAsync(string id);
        Task<ServiceResult<bool>> ConfirmInStoreOrder(string id);
        Task<ServiceResult<bool>> CheckoutInStoreOrderAsync(string id);
        Task<ServiceResult<bool>> UpdateInStoreOrderAsync(string updatedByCashierId, string orderId);

        Task<ServiceResult<OrderDetailResponseModel>> SeedDataOrderAsync(string userId, OrderCreateModel createOrderRequest);
        Task<ServiceResult<OrderDetailResponseModel>> GetOrderByIdAsync(string orderId);
        Task<ServiceResult<bool>> UpdateOrderStatusToProcessingAsync(string updateByUserId, string orderId);
        Task<ServiceResult<bool>> UpdateOrderStatusToDeliveringAsync(string updateByUserId, string orderId, UpdateOrderToDeliveringModel shipperId);
        Task<ServiceResult<bool>> UpdateOrderStatusToCompletedAsync(string updateByUserId, string orderId);
        Task<ServiceResult<bool>> UpdateOrderStatusToCanceledAsync(string updateByUserId, string orderId, CancelOrderModel orderUpdateStatusModel);
        Task<ServiceResult<bool>> UpdateOrderStatusToRefundedAsync(string updateByUserId, string orderId, OrderUpdateStatusModel orderUpdateStatusModel);
        Task<ServiceResult<bool>> UpdateOrderStatusToFailedAsync(string updateByUserId, string orderId, OrderUpdateStatusModel orderUpdateStatusModel);
        Task<ServiceResult<bool>> DeleteOrderAsync(string orderId);

        Task<ServiceResult<List<OrderResponseModel>>> GetCustomerOrdersAsync(string customerId);
    }
}
