using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.VietQR;
using TechStore.Service.Interfaces;

namespace TechStore.Service.Implementations
{
    public class VietQrService : IVietQrService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public VietQrService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string?> GenerateQrAsync(decimal amount, string content)
        {
            var accountNo = _configuration["VietQrSettings:AccountNo"];
            var accountName = _configuration["VietQrSettings:AccountName"];
            var acqId = _configuration["VietQrSettings:AcqId"];

            if (accountNo == null || accountName == null || acqId == null)
            {
                return null;
            }

            var request = new
            {
                accountNo = accountNo,
                accountName = accountName,
                acqId = acqId,
                amount = amount,
                addInfo = content,
                format = "text",
                template = "compact"
            };

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.vietqr.io/v2/generate");

            httpRequest.Headers.Add(
                "x-client-id",
                _configuration["VietQrSettings:ClientId"]);

            httpRequest.Headers.Add(
                "x-api-key",
                _configuration["VietQrSettings:ApiKey"]);

            httpRequest.Content = JsonContent.Create(request);

            var response = await _httpClient.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<VietQrResponse>();

            return result!.Data!.QrDataURL;
        }

        //public async Task<IActionResult> Callback(VietQrCallback request)
        //{
        //    var payment = await _paymentRepository
        //        .FindByCodeAsync(request.PaymentCode);

        //    if (payment is null)
        //        return NotFound();

        //    payment.Status = PaymentStatus.Paid;
        //    payment.PaidAt = DateTime.UtcNow;

        //    var order = new Order
        //    {
        //        PaymentId = payment.Id,
        //        TotalPrice = payment.Amount,
        //        Status = OrderStatus.Pending
        //    };

        //    await _orderRepository.AddAsync(order);

        //    await _unitOfWork.SaveChangesAsync();

        //    return Ok();
        //}
    }
}
