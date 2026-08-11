namespace Reconciliation.Blazor;

public interface IInvoiceTypeService
{
    Task<InvoiceTypeList> GetAllAsync();
    Task<SingleInvoiceTypeResponse?> GetByIdAsync(int id);

    Task<InvoiceCreateResponse> CreateAsync(InvoiceType model);

    Task<InvoiceUpdateResponse> UpdateAsync(int id, InvoiceType request);

    Task<InvoiceDeleteResponse> DeleteAsync(string id);
    Task<InvoiceUploadedList> GetUploadedInvoicesAsync();

    Task<byte[]?> DownloadInvoiceFile(int id);

}
