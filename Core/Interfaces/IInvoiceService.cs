namespace Reconciliation.Blazor;

public interface IInvoiceService
{
    Task<InvoiceUploadResponse> CreateAsync(InvoiceUploadRequest model, byte[] fileBytes, string fileName, string contentType);
    // Task<MerchantUploadResponse> GetAllMerchantUploadData();
    // Task<TransactionLineResponse> GetAllTransactionLines(string id);
    // Task<byte[]?> DownloadMerchantFile(int id);
}
