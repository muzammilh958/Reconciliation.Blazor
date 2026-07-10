using Microsoft.AspNetCore.Components.Forms;

namespace Reconciliation.Blazor;

public interface IMerchantService
{

    Task<MerchantCreateResponse> CreateAsync(MerchantCreate model, byte[] fileBytes, string fileName, string contentType);
    Task<MerchantUploadResponse> GetAllMerchantUploadData();
    Task<TransactionLineResponse> GetAllTransactionLines(string id);
    Task<byte[]?> DownloadMerchantFile(int id);
}
