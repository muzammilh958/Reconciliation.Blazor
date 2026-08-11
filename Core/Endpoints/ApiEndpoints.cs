namespace Reconciliation.Blazor.Core.Endpoints
{
    public static class ApiEndpoints
    {
        public const string Base = "http://localhost:5186/api";
        public static class Auth
        {
            public const string Login = Base + "/auth/login";
            public const string SignUp = Base + "/auth/register";
            public const string RefreshToken = Base + "/auth/refresh-token";
            public const string ForgetPassword = Base + "/auth/forgot-password";
            public const string ResetPassword = Base + "/auth/reset-password";
            public const string UserList = Base + "/UserManagement/all";
            public const string UserCreate = Base + "/UserManagement/create";
            public const string UserDelete = Base + "/UserManagement/delete";
            public const string UserUpdate = Base + "/UserManagement/update/";
            public const string RoleList = Base + "/Role/all";
        }

        public static class Batch
        {
            public const string GetAll = Base + "/batch/get-all";
            public const string GetById = Base + "/batch/get-by-id/";

            public const string Create = Base + "/batch/create";
            public const string Delete = Base + "/batch/delete/";
            public const string Update = Base + "/batch/update/";

        }
        public static class Store
        {
            public const string GetAll = Base + "/Store/get-all";

            public const string Create = Base + "/Store/create";
            public const string Delete = Base + "/Store/delete/";
            public const string Update = Base + "/Store/update/";
            public const string GetById = Base + "/Store/get-by-id/";                                                   

        }

        public static class Menu
        {
            public const string GetAll = Base + "/Menu/structure";

        }

        public static class Payment
        {
            public const string GetAll = Base + "/Payment/get-all";
            public const string Create = Base + "/Payment/create";
            public const string Delete = Base + "/Payment/delete/";
            public const string Update = Base + "/Payment/update/";
            public const string GetById = Base + "/Payment/get-by-id/";

        }

        public static class Settings
        {
            public const string GetAll = Base + "/GlobalSettings/get-all";

        }

        public static class Merchant
        {
            public const string Create = Base + "/Merchant/create";
            public const string MerchatUploadList = Base + "/Merchant/MerchatUploadList";
            public const string Download = Base + "/Merchant/download";
            public const string TransactionLines = Base + "/Merchant/TransactionLines/";

        }

        public static class ReconciliationAPI
        {
            public const string StartReconciliation = Base + "/Reconciliation/start";
            public const string GetException = Base + "/Reconciliation/GetException/";

        }
         public static class Dashboard
        {
            public const string GetBatchCount = Base + "/Dashboard/get-batch-count";
        }

        public static class InvoiceType
        {
            public const string GetAll = Base + "/InvoiceType/get-all";
            public const string Create = Base + "/InvoiceType/create";
            public const string Delete = Base + "/InvoiceType/delete";
            public const string Update = Base + "/InvoiceType/update/";
            public const string GetById = Base + "/InvoiceType/get-by-id/";


        }
        public static class Invoice
        {
            public const string Create = Base + "/InvoiceType/InvoiceCreateDataUpload";
            public const string GetAll = Base + "/InvoiceType/InvoiceListDataUpload";
            public const string Download = Base + "/InvoiceType/DownloadInvoiceFile";
        }
    }
}