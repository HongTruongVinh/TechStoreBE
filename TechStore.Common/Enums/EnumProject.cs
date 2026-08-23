using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Common.Enums
{

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
        Unpaid,
        PartiallyPaid,
        Paid,
        Refunded,
        Cancelled
    }

    public enum EPaymentStatus
    {
        Pending,   // Chờ thanh toán
        Paid,      // Đã thanh toán
        Failed,    // Thanh toán thất bại
        Refunded,  // Đã hoàn tiền
        Canceled,
    }

    public enum EPaymentSnapshotStatus
    {
        PendingPayment = 1,
        Paid = 2,
        Expired = 3,
        Cancelled = 4
    }

    public enum EPaymentMethod
    {
        COD,
        DomesticBank,
        Cash,
        VoucherOrFree
    }

    public enum EShippingStatus
    {
        Preparing,   // Đang chuẩn bị
        Shipping,    // Đang giao hàng
        Delivered,   // Đã giao hàng
        Failed,      // Giao hàng thất bại
        Canceled     // Đã hủy giao hàng
    }

    public enum EDiscountType
    {
        Percentage,
        FixedAmount
    }

    public enum EVoucherStatus
    {
        Draft,
        Active,
        Expired,
        Disabled
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

    public enum EDeviceType
    {
        Mobile,
        Desktop
    }

    public enum EPaymentTransactionStatus
    {
        Received = 1,

        Processed = 2,

        Duplicate = 3,

        ExpiredSnapshot = 4,

        IncorrectAmount = 5,

        InvalidSnapshot = 6,

        RefundPending = 7,

        Refunded = 8,

        ManualReview = 9
    }

    public enum StockReservationStatus
    {
        Reserved = 1,

        Confirmed = 2,

        Released = 3,

        Expired = 4
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

    public enum EErrorType
    {
        BadRequest,
        SystemError,
        NotFound,
        ConfictData,
        Unauthorized,
        Forbidden,
        Status500InternalServerError,

        IdempotencyKeyConflict
    }
}
