using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Common.Enums
{
    public enum EOrderType
    {
        Online,
        InStore // mua tại quầy
    }

    public enum EOrderStatus
    {
        Pending,     // Đang chờ xử lý
        Processing,  // Đang xử lý
        Delivering,
        Completed,   // Hoàn thành
        Canceled,     // Đã hủy
        Refunded,    // Đã hoàn tiền
        Failed       // Thất bại
    }

    public enum EInvoiceStatus
    {
        Paid,    // Đã thanh toán
        Unpaid,  // Chưa thanh toán 
        Canceled,
        Refunded
    }

    public enum EPaymentStatus
    {
        Pending,   // Chờ thanh toán
        Paid,      // Đã thanh toán
        Failed,    // Thanh toán thất bại
        Refunded,  // Đã hoàn tiền
        Canceled,
    }

    public enum EPaymentMethod
    {
        CreditCard,
        Momo,
        PayPal,
        COD,
        Cash
    }

    public enum EShippingDetailStatus
    {
        Preparing,   // Đang chuẩn bị
        Shipping,    // Đang giao hàng
        Delivered,   // Đã giao hàng
        Failed,      // Giao hàng thất bại
        Canceled     // Đã hủy giao hàng
    }

    public enum EVoucherStatus
    {
        Active,
        Inactive,
    }
    public enum EUserStatus
    {
        Active,
        Inactive,
        Banned,
        Deleted
    }


    public enum ERole
    {
        Admin,
        Staff,
        Customer
    }

    public enum EGender
    {
        Other,
        Male,
        Female
    }

    public enum EEntityStatus
    {
        Active,
        Deleted,
    }

    public enum EPhotoType
    {
        Product,
        Category,
        Brand,
        Shipper
    }

    public enum EQRCodeType
    {
        Payment,
        OrderTracking
    }

    public enum ERegisterType
    {
        Email,
        PhoneNumber,
        //SocialMedia // Mạng xã hội
    }

    public enum ERetCode
    {
        Successfull,

        BadRequest,

        SystemError,

        LoginSuccess,

        LoginError,

        ExitAccount,

        ErrorCookie,

        PasswordNotSame,

        NoExitData,

        ConfictData,

        NoPermission
    }
}
