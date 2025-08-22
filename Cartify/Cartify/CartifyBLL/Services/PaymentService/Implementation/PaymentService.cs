using CartifyBLL.Services.PaymentService.Abstraction;
using CartifyDAL.Entities.payment; 

using CartifyDAL.Repo.Abstraction;
using CartifyDAL.Repo.paymentRepo.Abstraction;
using CartifyDAL.Repo.userRepo.Abstraction;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;


public class PaymentService : IPaymentService
{
    private readonly IConfiguration _config;
    private readonly IOrderRepo _orderRepo;
    private readonly IUserRepo _userRepo;
    private readonly IPaymentRepo _paymentRepo;

    public PaymentService(
        IConfiguration config,
        IOrderRepo orderRepo,
        IUserRepo userRepo,
        IPaymentRepo paymentRepo)
    {
        _config = config;
        _orderRepo = orderRepo;
        _userRepo = userRepo;
        _paymentRepo = paymentRepo;
        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
    }

    public async Task<string> CreateCheckoutSessionAsync(int orderId)
    {
        var order =  _orderRepo.GetById(orderId);
        var user = await _userRepo.GetByIdAsync(order.Order.UserId);

        if (string.IsNullOrEmpty(user.StripeCustomerId))
        {
            var customerService = new CustomerService();
            var customer = await customerService.CreateAsync(new CustomerCreateOptions { Email = user.Email, Name = user.FullName });
            user.StripeCustomerId = customer.Id;
            await _userRepo.UpdateAsync(user);
        }

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            Mode = "payment",
            Customer = user.StripeCustomerId,
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(order.Order.TotalAmount * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions { Name = $"Order #{order.Order.OrderId}" }
                    },
                    Quantity = 1
                }
            },
            Metadata = new Dictionary<string, string> { { "order_id", order.Order.OrderId.ToString() } },
            SuccessUrl = $"{_config["AppSettings:AppBaseUrl"]}/Payment/Success?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{_config["AppSettings:AppBaseUrl"]}/Cart"
        };

        var sessionService = new SessionService();
        var session = await sessionService.CreateAsync(options);

        var transaction = new Payment
        {
            OrderId = order.Order.OrderId,
            Amount = order.Order.TotalAmount,
            Status = Payment.TransactionStatus.Pending,
            GatewayName = "Stripe",
            GatewayTransactionId = session.Id,
            CreatedAt = DateTime.UtcNow
        };
        await _paymentRepo.AddAsync(transaction);

        return session.Url;
    }
}
