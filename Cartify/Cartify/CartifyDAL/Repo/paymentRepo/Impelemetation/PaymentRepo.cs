using Cartify.DAL.DataBase;
using CartifyDAL.Entities.payment;
using CartifyDAL.Repo.paymentRepo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CartifyDAL.Repo.paymentRepo.Impelemetation
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly CartifyDbContext cartifyDb;

        public PaymentRepo(CartifyDbContext cartifyDb)
        {
            this.cartifyDb = cartifyDb;
        }

        public async Task AddAsync(Payment payment)
        {
            await cartifyDb.Payment.AddAsync(payment);
            await cartifyDb.SaveChangesAsync();
        }

        public async Task<Payment> GetByGatewayTransactionIdAsync(string gatewayTransactionId)
        {
            return await cartifyDb.Payment.FirstOrDefaultAsync(t => t.GatewayTransactionId == gatewayTransactionId);
        }

        public async Task UpdateAsync(Payment payment)
        {
            cartifyDb.Payment.Update(payment);
            await cartifyDb.SaveChangesAsync();
        }
    }
}
