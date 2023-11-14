using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.IO;
using System;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Services;
using TalabatAPIs.Dtos;
using TalabatAPIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Microsoft.Extensions.Logging;

namespace TalabatAPIs.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;

        public PaymentsController(IPaymentService paymentService, ILogger logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null) return BadRequest(new ApiResponse(400, "a Problem with your Basket"));
            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> Stripewebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], "");

                PaymentIntent intent;
                Order order;
                // Handle the event
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        order = await _paymentService.UpdatePaymentIntentToSucceededOrFalid(intent.Id, true);
                        _logger.LogInformation("Payment Succceeded ", order.Id , intent.Id);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        order = await _paymentService.UpdatePaymentIntentToSucceededOrFalid(intent.Id, false);
                        _logger.LogInformation("Payment Failed ", order.Id , intent.Id);
                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
