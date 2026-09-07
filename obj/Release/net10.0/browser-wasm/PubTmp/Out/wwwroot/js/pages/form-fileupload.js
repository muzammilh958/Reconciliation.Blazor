/**
 * Template Name: UBold - Admin & Dashboard Template
 * By (Author): Coderthemes
 * Module/App (File Name): Form Fileupload
 */

class FileUpload {
    constructor() {
        this.init();
    }

    init() {
        if (typeof Dropzone === 'undefined') {
            console.warn("Dropzone is not loaded.");
            return;
        }

        Dropzone.autoDiscover = false;

        const dropzones = document.querySelectorAll('[data-plugin="dropzone"]');
        if (dropzones) {
            dropzones.forEach(dropzoneEl => {
                const actionUrl = dropzoneEl.getAttribute('action') || '/';
                const previewContainer = dropzoneEl.dataset.previewsContainer;
                const uploadPreviewTemplate = dropzoneEl.dataset.uploadPreviewTemplate;

                const options = {
                    url: actionUrl,
                    acceptedFiles: 'image/*',
                };

                if (previewContainer) {
                    options.previewsContainer = previewContainer;
                }

                if (uploadPreviewTemplate) {
                    const template = document.querySelector(uploadPreviewTemplate);
                    if (template) {
                        options.previewTemplate = template.innerHTML;
                    }
                }

                try {
                    new Dropzone(dropzoneEl, options);
                } catch (e) {
                    console.error("Dropzone initialization failed:", e);
                }
            });
        }
    }
}

// window.loadFormFileUpload = function () {
//     new FileUpload();

//     if (typeof FilePond !== 'undefined') {
//         // FilePond Plugins
//         try {
//             FilePond.registerPlugin(FilePondPluginImagePreview);
//         } catch (e) {
//             console.warn("FilePond plugins registration failed:", e);
//         }

//         // multiple-file inputs
//         const multiInputs = document.querySelectorAll("input.filepond-input-multiple");
//         multiInputs.forEach(input => {
//             FilePond.create(input);
//         });

//         // circle-style FilePond inputs
//         const circleInputs = document.querySelectorAll("input.filepond-input-circle");
//         circleInputs.forEach(input => {
//             FilePond.create(input, {
//                 imageCropAspectRatio: "1:1",
//                 imageResizeTargetWidth: 200,
//                 imageResizeTargetHeight: 200,
//                 stylePanelLayout: "compact circle",
//                 styleLoadIndicatorPosition: "center bottom",
//                 styleProgressIndicatorPosition: "right bottom",
//                 styleButtonRemoveItemPosition: "left bottom",
//                 styleButtonProcessItemPosition: "right bottom",
//                 allowImagePreview: true,
//                 imagePreviewHeight: 100,
//                 labelIdle: `<i class="fs-32 text-muted ti ti-camera"></i>`,
//             });
//         });
//     } else {
//         console.warn("FilePond is not loaded.");
//     }
// };

// wwwroot/js/pages/form-fileupload.js
// wwwroot/js/pages/form-fileupload.js
// wwwroot/js/pages/form-fileupload.js

// Check if already defined to prevent duplicate declaration
// wwwroot/js/pages/form-fileupload.js

// Use a flag to track initialization
if (!window._fileUploadInitialized) {

    function initializeFileUpload() {
       

        // Check if all dependencies are loaded
        let dependenciesLoaded = true;
        let missingDependencies = [];

        // Check for FileUpload class
        if (typeof FileUpload === 'undefined') {
            dependenciesLoaded = false;
            missingDependencies.push('FileUpload');
        }

        // Check for Dropzone (if FileUpload depends on it)
        if (typeof Dropzone === 'undefined') {
            console.log("Dropzone is not loaded - FileUpload may not work correctly");
            // Continue anyway as FileUpload might handle this internally
        }

        // Check for FilePond
        if (typeof FilePond === 'undefined') {
            console.log("FilePond is not loaded - FilePond features will not work");
        }

        // Initialize FileUpload class if it exists
        if (typeof FileUpload !== 'undefined') {
            try {
                new FileUpload();
              
            } catch (e) {
                console.error("FileUpload initialization failed:", e);
            }
        } else {
            console.log("FileUpload class is not available");
        }

        // Initialize FilePond
        initializeFilePond();
    }

    function initializeFilePond() {
        if (typeof FilePond === 'undefined') {
            console.warn("FilePond is not loaded.");
            return;
        }

        // FilePond Plugins
        try {
            if (typeof FilePondPluginImagePreview !== 'undefined') {
                FilePond.registerPlugin(FilePondPluginImagePreview);
                console.log("FilePond plugins registered");
            }
        } catch (e) {
            console.warn("FilePond plugins registration failed:", e);
        }

        // multiple-file inputs
        const multiInputs = document.querySelectorAll("input.filepond-input-multiple");
        multiInputs.forEach(input => {
            try {
                FilePond.create(input, {
                    acceptedFileTypes: [
                            '.xls',
                            '.xlsx',
                            'application/vnd.ms-excel',
                            'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                        ]
                });
            } catch (e) {
                console.warn("Failed to create FilePond for input:", input, e);
            }
        });

        // circle-style FilePond inputs
        const circleInputs = document.querySelectorAll("input.filepond-input-circle");
        circleInputs.forEach(input => {
            try {
                FilePond.create(input, {
                    imageCropAspectRatio: "1:1",
                    imageResizeTargetWidth: 200,
                    imageResizeTargetHeight: 200,
                    stylePanelLayout: "compact circle",
                    styleLoadIndicatorPosition: "center bottom",
                    styleProgressIndicatorPosition: "right bottom",
                    styleButtonRemoveItemPosition: "left bottom",
                    styleButtonProcessItemPosition: "right bottom",
                    allowImagePreview: true,
                    imagePreviewHeight: 100,
                    labelIdle: `<i class="fs-32 text-muted ti ti-camera"></i>`,
                });
            } catch (e) {
                console.log("Failed to create circle FilePond for input:", input, e);
            }
        });
    }

    // Define the function on window
    window.loadFormFileUpload = initializeFileUpload;

    // Mark as initialized
    window._fileUploadInitialized = true;

    window.getFilePondFiles = async function () {
        const pond = FilePond.find(document.querySelector('.filepond'));
        if (!pond) return null;

        const files = pond.getFiles();
        const fileData = [];

        for (let file of files) {
            const buffer = await file.file.arrayBuffer();
            fileData.push({
                name: file.file.name,
                contentType: file.file.type,
                bytes: Array.from(new Uint8Array(buffer))
            });
        }
        return fileData;
    };
window.getFilePondFileBase64 = async function (fileName) {
    try {
        const pondElement = document.querySelector('.filepond');
        if (!pondElement) {
            console.error("FilePond element not found");
            return null;
        }

        const pond = FilePond.find(pondElement);
        if (!pond) {
            console.error("FilePond instance not found");
            return null;
        }

        const files = pond.getFiles();
        const file = files.find(f => f.filename === fileName || f.file.name === fileName);
        
        if (!file) {
            console.error("File not found in pond:", fileName);
            return null;
        }

        // Convert File/Blob to base64
        const arrayBuffer = await file.file.arrayBuffer();
        const bytes = new Uint8Array(arrayBuffer);
        
        let binary = '';
        for (let i = 0; i < bytes.byteLength; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return btoa(binary);
    } catch (err) {
        console.error("Error getting base64:", err);
        return null;
    }
};
    window.getFilePondFileBytes = async function (fileName) {
    const pond = FilePond.find(document.querySelector('.filepond'));
    if (!pond) return null;

    const file = pond.getFiles().find(f => f.filename === fileName);
    if (!file) return null;

    const arrayBuffer = await file.file.arrayBuffer();
    return Array.from(new Uint8Array(arrayBuffer)); // or handle as byte[]
};

   
} else {
    console.warn("FileUpload already initialized, skipping");
}


window.initSearchableSelects = function () {
    var selects = document.querySelectorAll('.searchable-select');
    selects.forEach(function (select) {
        if (select.choices) return;
        new Choices(select, {
            searchEnabled: true,
            searchFields: ['label'],
            itemSelectText: '',
            placeholderValue: select.getAttribute('data-placeholder') || 'Select...',
            noResultsText: 'No results found',
            noChoicesText: 'No choices available',
            position: 'bottom',
            removeItemButton: false,
        });
    });
};