using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Data.UnitOfWork;

namespace TechStore.Service.Implementations
{
    public class SequenceGeneratorService
    {
        private readonly IUnitOfWork _uow;
        public SequenceGeneratorService(IUnitOfWork uow) 
        { 
            _uow = uow;
        }
        public async Task<long> GetNextSequenceValueAsync(string sequenceName)
        {
            var sequence = await _uow.Sequences.FindOneAsync(sequence => sequence.Id == sequenceName);
            //var sequence =  _uow.Sequences.FindTracked(sequence => sequence.Id == sequenceName);

            if (sequence == null)
            {
                await _uow.Sequences.AddAsync(new Data.Entities.Sequence { Id = sequenceName, Value = 1 });
                //await _uow.CommitAsync();
                return 1;
            }

            sequence.Value += 1;

            _uow.Sequences.Update(sequence);

            return sequence.Value;
        }

        public async Task<bool> ResetSequenceAsync()
        {
            try
            {

                await _uow.Sequences.DeleteAllAsync();
                return true;
            }
            catch
            {

                return false;
            }
        }

        public async Task<string> GetNextCategoryIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.Category);
            var Id = $"CATE-{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextProductIdAsync()
        {
            //var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.Product);
            //var Id = $"PROD-{sequenceNumber:D4}";

            var sequenceNumber = await _uow.Products.CountAsync() + 1;
            var Id = $"{sequenceNumber}";
            return Id;
        }

        public async Task<string> GetNextProductVariantIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.ProductVariant);
            var Id = $"PV-{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextProductVariantOptionIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.ProductVariantOption);
            var Id = $"PVO-{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextCartItemIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.CartItem);
            var Id = $"CART-{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextOrderIdAsync()
        {
            var today = DateTime.UtcNow.Date;

            //var countToday = await _uow.Orders
            //    .CountAsync(o => o.CreatedAt.Date == today);

            //return $"ORD-{today:yyyyMMdd}-{(countToday + 1):D6}";

            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.Order);
            //var Id = $"ORD-{sequenceNumber:D4}";

            var Id = $"ORD-{today:yyyyMMdd}{(sequenceNumber + 1):D3}";
            return Id;
        }

        public async Task<string> GetNextOrderItemIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.OrderItem);
            var Id = $"ORDI-{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextPaymentIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.Payment);
            var Id = $"PAYM{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextReportIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.Report);
            var Id = $"REPT-{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextUserIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.User);
            var Id = $"USER{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextVoucherIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.Voucher);
            var Id = $"VOUC-{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextInvoiceIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.Invoice);
            var Id = $"INV{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextBrandIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.Brand);
            var Id = $"BRAND{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextCommentIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.Comment);
            var Id = $"CMT-{sequenceNumber:D4}";
            return Id;
        }

        public async Task<string> GetNextShipperIdAsync()
        {
            var sequenceNumber = await GetNextSequenceValueAsync(CollectionName.Shipper);
            var Id = $"SHIP{sequenceNumber:D4}";
            return Id;
        }
    }
}
