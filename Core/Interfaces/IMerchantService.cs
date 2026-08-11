using Microsoft.AspNetCore.Components.Forms;
using static Reconciliation.Blazor.Pages.Merchant.MerchantUpload;

namespace Reconciliation.Blazor;

public interface IMerchantService
{

    Task<MerchantCreateResponse> CreateAsync(MerchantCreate model, List<byte[]> fileBytes, List<string> fileNames, List<string> contentTypes);
    Task<MerchantUploadResponse> GetAllMerchantUploadData();
    Task<TransactionLineResponse> GetAllTransactionLines(string id);
    Task<byte[]?> DownloadMerchantFile(int id);
}
