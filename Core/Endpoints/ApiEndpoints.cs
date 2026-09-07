namespace Reconciliation.Blazor.Core.Endpoints
{
    public static class ApiEndpoints
    {
        public static string Base { get; set; } ="";

        public static void Initialize(string baseUrl)
        {
            Base = baseUrl.TrimEnd('/');
        }
        public static class Auth
        {
            public static  string Login => Base + "/auth/login";
            public static string SignUp => Base + "/auth/register";
            public static string ValidateToken => Base + "/auth/validate-token";
            public static string RefreshToken => Base + "/auth/refresh-token";
            public static string ForgetPassword => Base + "/auth/forgot-password";
            public static string ResetPassword => Base + "/auth/reset-password";
            public static string UserList => Base + "/UserManagement/all";
            public static string UserCreate => Base + "/UserManagement/create";
            public static string UserDelete => Base + "/UserManagement/delete";
            public static string UserUpdate => Base + "/UserManagement/update/";
            public static string RoleList => Base + "/Role/all";
        }

        public static class Batch
        {
            public static string GetAll => Base + "/batch/get-all";
            public static string GetById => Base + "/batch/get-by-id/";

            public static string Create => Base + "/batch/create";
            public static string Delete => Base + "/batch/delete/";
            public static string Update => Base + "/batch/update/";

        }
        public static class Store
        {
            public static string GetAll => Base + "/Store/get-all";

            public static string Create => Base + "/Store/create";
            public static string Delete => Base + "/Store/delete/";
            public static string Update => Base + "/Store/update/";
            public static string GetById => Base + "/Store/get-by-id/";                                                   

        }

        public static class Menu
        {
            public static string GetAll => Base + "/Menu/structure";

        }

        public static class Payment
        {
            public static string GetAll => Base + "/Payment/get-all";
            public static string Create => Base + "/Payment/create";
            public static string Delete => Base + "/Payment/delete/";
            public static string Update => Base + "/Payment/update/";
            public static string GetById => Base + "/Payment/get-by-id/";

        }

        public static class Settings
        {
            public static string GetAll => Base + "/GlobalSettings/get-all";

        }

        public static class Merchant
        {
            public static string Create => Base + "/Merchant/create";
            public static string MerchatUploadList => Base + "/Merchant/MerchatUploadList";
            public static string Download => Base + "/Merchant/download";
            public static string TransactionLines => Base + "/Merchant/TransactionLines/";

        }

        public static class ReconciliationAPI
        {
            public static string StartReconciliation => Base + "/Reconciliation/start";
            public static string GetException => Base + "/Reconciliation/GetException/";
            public static string DownloadReconciliationResult => Base + "/Reconciliation/download/";

            public static string batchLocked => Base + "/Batch/lockBatch/";
            public static  string DeleteReconciliationResult = Base + "/Reconciliation/DeleteReconciliationResult/";
            public static string GetManualReconciliationResults = Base + "/Reconciliation/GetManualTransactionForReconciliation";
            public static string SetManualTransactionForReconciliation = Base + "/Reconciliation/SetManualTransactionForReconciliation";

            public static string GetManualReconciliationApprovalList = Base + "/Reconciliation/GetManualReconciliationApprovalList";
            public static string SetManualReconciliationApprovalList = Base + "/Reconciliation/SetManualReconciliationApproval";
            public static string LockBatch = Base + "/Reconciliation/lockBatch/";

            public static string LockBatchReco = Base +"/Reconciliation/lockBatch/";



        }
         public static class Dashboard
        {
            public static string GetBatchCount => Base + "/Dashboard/get-batch-count";
        }

        public static class InvoiceType
        {
            public static string GetAll => Base + "/InvoiceType/get-all";
            public static string Create => Base + "/InvoiceType/create";
            public static string Delete => Base + "/InvoiceType/delete";
            public static string Update => Base + "/InvoiceType/update/";
            public static string GetById => Base + "/InvoiceType/get-by-id/";


        }
        public static class Invoice
        {
            public static string Create => Base + "/InvoiceType/InvoiceCreateDataUpload";
            public static string GetAll => Base + "/InvoiceType/InvoiceListDataUpload";
            public static string Download => Base + "/InvoiceType/DownloadInvoiceFile";
        }

        public static class Exception
        {
            public static string getAllException => Base + "/Reconciliation/GetException/";
        }
    }
}