window.initializeMerchantUpload = function() {
    // Initialize FilePond
    if (typeof FilePond !== 'undefined') {
        FilePond.parse(document.body);
    }
    
    // Initialize Dropzone if needed
    if (typeof Dropzone !== 'undefined') {
        Dropzone.autoDiscover = false;
        // Your Dropzone initialization here
    }
}

window.initMerchantSelect2 = (dotNetHelper) => {
     
    window._merchantDotNetHelper = dotNetHelper;
    // Destroy first in case of re-render, so we don't double-init
    if ($('#batchSelect').hasClass("select2-hidden-accessible")) {
        $('#batchSelect').select2('destroy');
    }
    if ($('#paymentSelect').hasClass("select2-hidden-accessible")) {
        $('#paymentSelect').select2('destroy');
    }
    if ($('#invoiceSelect').hasClass("select2-hidden-accessible")) {
        $('#invoiceSelect').select2('destroy');
    }
    

    $('#batchSelect').select2({
        width: '100%',
        placeholder: "Select Batch",
        allowClear: false
    }).on('change', function () {
       // dotNetHelper.invokeMethodAsync('OnBatchSelected', parseInt($(this).val()));
         if (window._merchantDotNetHelper) {
            window._merchantDotNetHelper.invokeMethodAsync('OnBatchSelected', parseInt($(this).val()));
        } else {
            console.warn('DotNetHelper not available for batch selection');
        }
    });

    $('#paymentSelect').select2({
        width: '100%',
        placeholder: "Select Payment",
        allowClear: false
    }).on('change', function () {
        dotNetHelper.invokeMethodAsync('OnPaymentSelected', parseInt($(this).val()));
    });

    $('#invoiceSelect').select2({
        width: '100%',
        placeholder: "Select Invoice",
        allowClear: false
    }).on('change', function () {
        dotNetHelper.invokeMethodAsync('OnInvoiceSelected', parseInt($(this).val()));
    });
};

window.destroyMerchantSelect2 = () => {
    if ($('#batchSelect').hasClass("select2-hidden-accessible")) {
        $('#batchSelect').select2('destroy');
    }
    if ($('#paymentSelect').hasClass("select2-hidden-accessible")) {
        $('#paymentSelect').select2('destroy');
    }
};

window.openInvoiceModal = function (data) {

    document.getElementById("modalBatchId").innerText = data.batchId ?? "";
    document.getElementById("modalAmount").innerText = data.amount ?? "";
    document.getElementById("modalAuthCode").innerText = data.authCode ?? "";
    document.getElementById("modalTender").innerText = data.tender ?? "";
    document.getElementById("modalStoreId").innerText = data.storeId ?? "";
    document.getElementById("modalInvoiceId").innerText = data.invoiceId ?? "";

    const modalElement = document.getElementById("invoiceModal");

    const modal = bootstrap.Modal.getOrCreateInstance(modalElement);

    modal.show();
};

window.initUserSelect2 = function (dotNetRef) {
    $('#roleSelect').select2({
        placeholder: "Select Role",
        width: '100%'
    }).on('change', function () {
        const value = $(this).val();
        dotNetRef.invokeMethodAsync('OnRoleSelected', value || "");
    });
};

window.initUserEditSelect2 = function (dotNetRef, currentRoleId) {
    const $select = $('#roleSelect').select2({
        placeholder: "-- Select a role --",
        width: '100%'
    });

    // Pre-select the user's current role, since Select2 needs to be told
    // explicitly after taking over the native <select>
    if (currentRoleId) {
        $select.val(currentRoleId).trigger('change.select2');
    }

    $select.on('change', function () {
        const value = $(this).val();
        dotNetRef.invokeMethodAsync('OnRoleSelected', value || "");
    });
};