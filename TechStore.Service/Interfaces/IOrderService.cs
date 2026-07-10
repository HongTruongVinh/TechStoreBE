using System;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Payment;

namespace TechStore.Service.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<PagedResult<OrderDetailResponseModel>>> GetListOrdersByStatusIdAsync(EOrderStatus statusId, int page, int pageSize);
        Task<ServiceResult<PagedResult<ListItemOrderModel>>> GetOrdersAsync(OrderSearchQuery query);
        Task<ServiceResult<string>> CreatePrePayOnlineOrderAsync(string userId, PaymentSnapshot ps, PaymentForSnapshot request);
        Task<ServiceResult<string>> CreateCODOnlineOrderAsync(string userId, OrderCreateModel createOrderRequest);
        Task<ServiceResult<string>> CreateInStoreOrderAsync(string createdByCashierId, string paymentId, InStoreOrderCreateModel createOrderRequest);
        
        Task<ServiceResult<List<ListItemOrderModel>>> GetInStoreOrdersAsync();
        Task<ServiceResult<InStoreOrderResponseModel>> GetInStoreOrderAsync(string id);
        Task<ServiceResult<bool>> ConfirmInStoreOrder(string id);
        Task<ServiceResult<bool>> CheckoutInStoreOrderAsync(string id);
        Task<ServiceResult<bool>> UpdateInStoreOrderAsync(string updatedByCashierId, string orderId);

        Task<ServiceResult<OrderDetailResponseModel>> GetOrderByIdAsync(string userId, string orderId);
        Task<ServiceResult<OrderDetailResponseModel>> AdminGetOrderByIdAsync(string userId, string orderId);
        Task<ServiceResult<bool>> UpdateOrderStatusToProcessingAsync(string updateByUserId, string orderId);
        Task<ServiceResult<bool>> UpdateOrderStatusToDeliveringAsync(string updateByUserId, string orderId, UpdateOrderToDeliveringModel shipperId);
        Task<ServiceResult<bool>> UpdateOrderStatusToCompletedAsync(string updateByUserId, string orderId);
        Task<ServiceResult<bool>> CancelOrderByAdminAsync(string updateByUserId, string orderId, CancelOrderModel orderUpdateStatusModel);
        Task<ServiceResult<bool>> UpdateOrderStatusToRefundedAsync(string updateByUserId, string orderId, OrderUpdateStatusModel orderUpdateStatusModel);
        Task<ServiceResult<bool>> UpdateOrderStatusToFailedAsync(string updateByUserId, string orderId, OrderUpdateStatusModel orderUpdateStatusModel);
        Task<ServiceResult<bool>> DeleteOrderAsync(string orderId);

        Task<ServiceResult<List<ListItemOrderModel>>> GetCustomerOrdersAsync(string customerId, int page, int pageSize);
        Task<ServiceResult<bool>> CancelOrderByCustomerAsync(string updateByUserId, string orderId, CancelOrderModel model);
        Task<ServiceResult<bool>> UpdateOrderByCustomerAsync(string updateByUserId, string orderId, UpdateOrderModel model);
    }
}
