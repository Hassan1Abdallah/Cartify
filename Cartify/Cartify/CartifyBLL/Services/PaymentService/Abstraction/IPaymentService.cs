
namespace CartifyBLL.Services.PaymentService.Abstraction
{
    public interface IPaymentService
    {
        Task<string> CreateCheckoutSessionAsync(int orderId);
    }
}
