using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class PaymentTransaction : BaseEntity
    {
        /// <summary>
        /// Mã giao dịch duy nhất từ phía ngân hàng/SEPay.
        /// </summary>
        public required string TransactionId { get; set; } // example: 67532173

        /// <summary>
        /// Mã nội dung thanh toán dùng để xác định PaymentSnapshot.
        /// Ví dụ: mã Order/Snapshot mà khách chuyển khoản vào.
        /// </summary>
        public required string PaymentCode { get; set; } // example: STORE20260710158671

        /// <summary>
        /// Số tiền thực tế nhận được từ ngân hàng.
        /// </summary>
        public required decimal Amount { get; set; }

        /// <summary>
        /// Thời điểm giao dịch xảy ra tại ngân hàng.
        /// </summary>
        public required string TransactionDate { get; set; }

        /// <summary>
        /// Mã tham chiếu giao dịch ngân hàng.
        /// </summary>
        public string? ReferenceCode { get; set; } // example: 6bea03d5-4acf-66a6-8cd3-10c6789efeb3

        /// <summary>
        /// Ngân hàng gửi tiền.
        /// </summary>
        public string? Gateway { get; set; }

        /// <summary>
        /// Nội dung chuyển khoản.
        /// </summary>
        public string? TransferContent { get; set; }

        /// <summary>
        /// Snapshot mà hệ thống xác định giao dịch này thuộc về.
        /// Có thể null nếu không tìm thấy Snapshot.
        /// </summary>
        public Guid? PaymentSnapshotId { get; set; }

        public PaymentSnapshot? PaymentSnapshot { get; set; }

        /// <summary>
        /// Payment được tạo sau khi giao dịch được xác nhận.
        /// Có thể null nếu giao dịch hết hạn, sai tiền, cần refund...
        /// </summary>
        public Guid? PaymentId { get; set; }

        public Payment? Payment { get; set; }

        /// <summary>
        /// Trạng thái xử lý giao dịch.
        /// </summary>
        public EPaymentTransactionStatus Status { get; set; }

        /// <summary>
        /// Ghi chú nội bộ.
        /// </summary>
        public string? Note { get; set; }
        public string? RawPayload { get; set; }
    }
}
