using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace StripeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        public StripeController()
        {
            StripeConfiguration.ApiKey = "sk_test_51OAJ7xCz1Gug3YHbI7RhCCJgZJ2XbuTTgb3zBYtAATy9bN3C1aTTH7IoTalduUZdedlQExqNRfEXOeaUpkYFHuKC008CkDdvWD";
        }
        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession()
        {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
        {
          new SessionLineItemOptions
          {
            PriceData = new SessionLineItemPriceDataOptions
            {
              UnitAmount = 2000,
              Currency = "usd",
              ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                    Name = "T-shirt",
                    },
                    },
                    Quantity = 1,
                     },
                },
                Mode = "payment",
                UiMode = "embedded",
                ReturnUrl = "http://localhost:4200/Return?session_id={CHECKOUT_SESSION_ID}",
            };

            var service = new SessionService();
            Session session = service.Create(options);
            
            return Ok(new { clientSecret = session.ClientSecret });
        }
        [HttpGet]
        public ActionResult SessionStatus([FromQuery] string session_id)
        {
            var sessionService = new SessionService();
            Session session = sessionService.Get(session_id);

            return Ok(new { status = session.Status, customer_email = session.CustomerDetails.Email });
        }
    }

}

