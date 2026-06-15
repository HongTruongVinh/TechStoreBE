using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Models;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Invoice;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;

        public InvoiceService(IUnitOfWork uow,
            SequenceGeneratorService sequenceService
            )
        {
            _uow = uow;
            _sequenceService = sequenceService;
        }

        public async Task<ServiceResult<List<ListItemInvoiceModel>>> GetAllItems()
        {
            var serviceResult = new ServiceResult<List<ListItemInvoiceModel>>
            {
                Data = new List<ListItemInvoiceModel>(),
                IsSuccess = true,
                Message = Messenger.GetDataSuccessful
            };

            var invoices = await _uow.Invoices.GetAllAsync();

            if (invoices == null || !invoices.Any())
            {
                serviceResult.IsSuccess = false;
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }
            else
            {
                foreach (var invoice in invoices)
                {
                    var order = await _uow.Orders.GetByInternalIdAsync(invoice.OrderId);

                    if (order != null)
                    {
                        serviceResult.Data.Add(invoice.ToListItemInvoiceModel(order));
                    }
                }
            }

            return serviceResult;
        }

        public async Task<ServiceResult<ListItemInvoiceModel>> GetItemById(string id)
        {
            var serviceResult = new ServiceResult<ListItemInvoiceModel>
            {
                Data = null,
                IsSuccess = true,
                Message = Messenger.GetDataSuccessful
            };

            var invoice = await _uow.Invoices.GetByIdAsync(id);

            if (invoice == null)
            {
                serviceResult.IsSuccess = false;
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }
            else
            {
                var order = await _uow.Orders.GetByInternalIdAsync(invoice.OrderId);

                if (order != null)
                {
                    serviceResult.Data = invoice.ToListItemInvoiceModel(order);
                }
                else
                {
                    serviceResult.IsSuccess = false;
                    serviceResult.Message = Messenger.NoExitData;
                }
            }

            return serviceResult;
        }
    }
}
