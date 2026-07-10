namespace Reconciliation.Blazor;

public interface IPaymentService
{
    Task<List<PaymentData>> GetAllAsync();
    Task<PaymentData?> GetByIdAsync(int id);

    Task<PaymentCreateResponse> CreateAsync(PaymentData model);

    Task<PaymentUpdateResponse> UpdateAsync(int id, PaymentData request);

    Task<PaymentDeleteResponse> DeleteAsync(string id);
}
