using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Payment;
using TechStore.Service.Interfaces;

namespace TechStoreAPI.Controllers
{
    [Route(RouterControllerName.Payments)]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<PaymentResponseModel>>> GetPayments()
        {
            var serviceResult = await _paymentService.GetPayments();

            if (serviceResult.IsSuccess)
            {
                return new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new()
                {
                    PartnerCode = Messenger.NoExitData,
                    RetCode = ERetCode.NoExitData,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<PaymentResponseModel>> GetPayment(string id)
        {
            var serviceResult = await _paymentService.GetPayment(id);

            if (serviceResult.IsSuccess)
            {
                return new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new()
                {
                    PartnerCode = Messenger.NoExitData,
                    RetCode = ERetCode.NoExitData,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }

        [HttpPost]
        public async Task<ApiResponse<string>> AddPayment(PaymentCreateModel paymentCreateModel)
        {
            var serviceResult = await _paymentService.AddPayment(paymentCreateModel);

            if (serviceResult.IsSuccess)
            {
                return new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new()
                {
                    PartnerCode = Messenger.NoExitData,
                    RetCode = ERetCode.NoExitData,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }

        [HttpPut("checkout/{id}")]
        public async Task<ApiResponse<bool>> CheckoutPayment(string id)
        {
            var serviceResult = await _paymentService.CheckoutPayment(id);

            if (serviceResult.IsSuccess)
            {
                return new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new()
                {
                    PartnerCode = Messenger.NoExitData,
                    RetCode = ERetCode.NoExitData,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }
    }
}
