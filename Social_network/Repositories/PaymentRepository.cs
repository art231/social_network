using Social_network.Models.Entities;

namespace Social_network.Repositories
{
    public class PaymentRepository
    {
        // TODO: Implement payment repository methods
        public Task<Payment?> CreatePaymentAsync(Payment payment)
        {
            return Task.FromResult<Payment?>(null);
        }

        public Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
            return Task.FromResult<Payment?>(null);
        }

        public Task<bool> UpdatePaymentStatusAsync(int paymentId, string status, string externalId)
        {
            return Task.FromResult(true);
        }

        public Task<Payment?> GetPaymentByExternalIdAsync(string externalId)
        {
            return Task.FromResult<Payment?>(null);
        }
    }
}
