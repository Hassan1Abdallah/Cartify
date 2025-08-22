

using CartifyBLL.Services.OrderService.Abstraction;
using CartifyBLL.Services.PaymentService.Abstraction;
using CartifyDAL.Entities.payment;
using CartifyDAL.Repo.Abstraction;
using CartifyDAL.Repo.paymentRepo.Abstraction;
using CartifyDAL.Repo.productRepo.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

[Authorize] 
public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private readonly IOrderService _orderService;
    private readonly IPaymentRepo _paymentRepo;
    private readonly IProductRepo _productRepo;
    private readonly IOrderRepo _orderRepo;

    public PaymentController(
        IPaymentService paymentService,
        IOrderService orderService,
        IPaymentRepo paymentRepo,
        IProductRepo productRepo,
        IOrderRepo orderRepo)
    {
        _paymentService = paymentService;
        _orderService = orderService;
        _paymentRepo = paymentRepo;
        _productRepo = productRepo;
        _orderRepo = orderRepo;
    }


    [HttpGet]
    public async Task<IActionResult> CreateCheckoutSession(int orderId)
    {
        try
        {
            var checkoutUrl = await _paymentService.CreateCheckoutSessionAsync(orderId);
            return Redirect(checkoutUrl);
        }
        catch (Exception ex)
        {
           
            TempData["ErrorMessage"] = "Could not connect to payment gateway. Please try again.";
            return RedirectToAction("Index", "Cart"); 
        }
    }

    
    public async Task<IActionResult> Success(string session_id)
    {
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(session_id);

        
        if (session.PaymentStatus == "paid")
        {
            
            var orderId = int.Parse(session.Metadata["order_id"]);

            var (order, orderError) =await _orderRepo.GetByIdWithItemsAsync(orderId);

            foreach (var item in order.OrderItems)
            {
                await _productRepo.ReduceStockAsync(item.ProductId, item.Quantity);
            }

            var systemUserId = "SYSTEM_PAYMENT_GATEWAY"; 
            var (success, error) = _orderService.ChangeOrderStatus(orderId, "Paid", systemUserId);

            
            var transaction = await _paymentRepo.GetByGatewayTransactionIdAsync(session.Id);
            if (transaction != null)
            {
                transaction.Status = Payment.TransactionStatus.Success;
                transaction.LastUpdatedAt = DateTime.UtcNow;
                await _paymentRepo.UpdateAsync(transaction);
            }

           
            ViewBag.OrderId = orderId;
            return View("Success");
        }

        
        return RedirectToAction("Cancel");
    }

    
    public IActionResult Cancel()
    {
        TempData["ErrorMessage"] = "Payment was cancelled. You can try again from your cart.";
        return RedirectToAction("Cancal", "Payment");
    }
}
