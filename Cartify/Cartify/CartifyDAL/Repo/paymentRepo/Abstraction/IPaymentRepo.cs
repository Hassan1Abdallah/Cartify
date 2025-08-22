using CartifyDAL.Entities.payment;

namespace CartifyDAL.Repo.paymentRepo.Abstraction
{
    public interface IPaymentRepo
    {
        Task AddAsync(Payment payment);
        Task<Payment> GetByGatewayTransactionIdAsync(string gatewayTransactionId);
        Task UpdateAsync(Payment payment);
    }
}
