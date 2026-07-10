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