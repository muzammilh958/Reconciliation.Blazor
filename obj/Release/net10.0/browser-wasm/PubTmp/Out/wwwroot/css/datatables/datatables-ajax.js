// /**
//  * Template Name: UBold - Admin & Dashboard Template
//  * By (Author): Coderthemes
//  * Module/App (File Name): Datatables Ajax
//  */

export function loadDataTableAjax() {
    console.log("DataTable module loaded");

    const table = document.querySelector('[data-tables="basic"]');
    if (!table) {
        console.error("Table not found");
        return;
    }

    if ($.fn.dataTable.isDataTable(table)) {
        $(table).DataTable().destroy();
    }

    $(table).DataTable({
        responsive: true,
        paging: true,
        searching: true,
        ordering: true
    });
}

export function reloadDataTable() {
    if (window.dtInstance) {
        window.dtInstance.destroy();
    }

    window.dtInstance = document.querySelector('[data-tables="basic"]').DataTable({
        responsive: true
    });
}

export function refreshDataTable() {
    const table = document.querySelector('[data-tables="basic"]');
    if (!table) return;
    if ($.fn.dataTable.isDataTable(table)) {
        $(table).DataTable().destroy();
    }
    $(table).DataTable({
        responsive: true,
        paging: true,
        searching: true,
        ordering: true
    });
}

export function initDataTable() {
  
    initSingleTable('[data-tables="basic"]');

    initSingleTable('#merchantTable');
    initSingleTable('#invoiceTable');

    if ($.fn.DataTable.isDataTable('#reconTable')) {
        $('#reconTable').DataTable().destroy();
    }

    $('#reconTable').DataTable({
        responsive: true,
        pageLength: 25,
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
        dom: 'Bfrtip',
        buttons: ['copy', 'csv', 'excel', 'pdf', 'print'],
        order: [[6, 'desc']],
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search records..."
        },
        columnDefs: [
            { targets: [9], orderable: false }
        ]
    });
}

function initSingleTable(selector) {
    const table = document.querySelector(selector);
    if (!table) {
        console.warn(`DataTable skipped: ${selector} not in DOM yet`);
        return;
    }
    if ($.fn.dataTable.isDataTable(table)) {
        $(table).DataTable().destroy();
    }
    $(table).DataTable({
        responsive: true,
        paging: true,
        searching: true,
        ordering: true
    });
}

// manual-adjustment.js

export function initializeSelectAll() {
    const selectAllMerchant = document.getElementById('selectAllMerchant');
    if (selectAllMerchant) {
        selectAllMerchant.removeEventListener('change', handleMerchantSelectAll);
        selectAllMerchant.addEventListener('change', handleMerchantSelectAll);
    }

    const selectAllInvoice = document.getElementById('selectAllInvoice');
    if (selectAllInvoice) {
        selectAllInvoice.removeEventListener('change', handleInvoiceSelectAll);
        selectAllInvoice.addEventListener('change', handleInvoiceSelectAll);
    }
}

function handleMerchantSelectAll() {
    const checkboxes = document.querySelectorAll('.merchant-checkbox');
    checkboxes.forEach(cb => cb.checked = this.checked);
}

function handleInvoiceSelectAll() {
    const checkboxes = document.querySelectorAll('.invoice-checkbox');
    checkboxes.forEach(cb => cb.checked = this.checked);
}

export function setupIndividualCheckboxes() {

    Console.log("Setting up individual checkboxes...");
    document.querySelectorAll('.merchant-checkbox').forEach(cb => {
        cb.removeEventListener('change', updateMerchantSelectAllState);
        cb.addEventListener('change', updateMerchantSelectAllState);
    });

    document.querySelectorAll('.invoice-checkbox').forEach(cb => {
        cb.removeEventListener('change', updateInvoiceSelectAllState);
        cb.addEventListener('change', updateInvoiceSelectAllState);
    });
}

function updateMerchantSelectAllState() {
    const allCheckboxes = document.querySelectorAll('.merchant-checkbox');
    const checkedCheckboxes = document.querySelectorAll('.merchant-checkbox:checked');
    const selectAll = document.getElementById('selectAllMerchant');
    if (selectAll) {
        selectAll.checked = allCheckboxes.length > 0 && allCheckboxes.length === checkedCheckboxes.length;
    }
}

function updateInvoiceSelectAllState() {
    const allCheckboxes = document.querySelectorAll('.invoice-checkbox');
    const checkedCheckboxes = document.querySelectorAll('.invoice-checkbox:checked');
    Console.log(`Checked invoices: ${checkedCheckboxes.length} / ${allCheckboxes.length}`);
    const selectAll = document.getElementById('selectAllInvoice');
    if (selectAll) {
        selectAll.checked = allCheckboxes.length > 0 && allCheckboxes.length === checkedCheckboxes.length;
    }
}

export function initManualAdjustment() {
    initializeSelectAll();
    setupIndividualCheckboxes();
}

export function initManualAdjustmentTables() {
    if (document.querySelector('#merchantTable')) {
        if ($.fn.DataTable.isDataTable('#merchantTable')) {
            $('#merchantTable').DataTable().clear().destroy();
        }
        $('#merchantTable').DataTable({
            responsive: true,
            destroy: true
        });
    }

    if (document.querySelector('#invoiceTable')) {
        if ($.fn.DataTable.isDataTable('#invoiceTable')) {
            $('#invoiceTable').DataTable().clear().destroy();
        }
        $('#invoiceTable').DataTable({
            responsive: true,
            destroy: true
        });
    }
}

export function destroyDataTabls() {
    console.log("Destroying manual adjustment DataTables...");

    if ($.fn.DataTable.isDataTable('#merchantTable')) {
        $('#merchantTable').DataTable().clear().destroy();
    }

    if ($.fn.DataTable.isDataTable('#invoiceTable')) {
        $('#invoiceTable').DataTable().clear().destroy();
    }
}

export function initExceptionTable(batchId, dotNetHelper) {
    const table = document.getElementById('exceptionTable');

    if (!table) {
        console.warn("Exception table not found");
        return;
    }

    if ($.fn.DataTable.isDataTable(table)) {
        $(table).DataTable().destroy();
    }

    $(table).DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        pageLength: 10,
        lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
        ajax: function (data, callback, settings) {
            const draw = data.draw;
            const start = data.start;
            const length = data.length;
            const order = data.order && data.order.length > 0 ? data.order[0] : null;
            const sortColumn = order ? order.column : null;
            const sortDirection = order ? order.dir : 'asc';

            dotNetHelper.invokeMethodAsync('GetExceptionData', batchId, draw, start, length, sortColumn, sortDirection)
                .then(result => {
                    callback(result);
                })
                .catch(error => {
                    console.error('Error fetching exception data:', error);
                    callback({
                        draw: draw,
                        recordsTotal: 0,
                        recordsFiltered: 0,
                        data: []
                    });
                });
        },
        columns: [
            { data: 'batchName' },
            { data: 'invoiceId' },
            { data: 'paymentId' },
            { data: 'exceptionType' },
            { data: 'description' }
        ],
        language: {
            paginate: {
                first: '<i class="ti ti-chevrons-left"></i>',
                previous: '<i class="ti ti-chevron-left"></i>',
                next: '<i class="ti ti-chevron-right"></i>',
                last: '<i class="ti ti-chevrons-right"></i>'
            },
            lengthMenu: '_MENU_ records per page',
            info: 'Showing _START_ to _END_ of _TOTAL_ records',
            infoEmpty: 'No records available',
            processing: '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>'
        }
    });
}

export function destroyDataTables() {
    $('table.dataTable').each(function () {
        if ($.fn.DataTable.isDataTable(this)) {
            $(this).DataTable().destroy();
        }
    });
}

export function initManualAdjustmentServerTables(dotNetHelper, batchname) {
    
    function setupTable(tableId, type) {
        // Handle checkbox changes using event delegation
        $(document).on('change', `${tableId} .row-checkbox`, function() {
            const id = parseInt($(this).val());
            const isChecked = $(this).prop('checked');
            
            // Send to C#
            dotNetHelper.invokeMethodAsync(
                type === 'merchant' ? 'OnMerchantSelectionChanged' : 'OnInvoiceSelectionChanged',
                id, 
                isChecked
            );
        });
        
        // Handle "Select All"
        const selectAllId = type === 'merchant' ? '#selectAllMerchant' : '#selectAllInvoice';
        $(document).on('change', selectAllId, function() {
            const isChecked = $(this).prop('checked');
            $(`${tableId} .row-checkbox`).each(function() {
                $(this).prop('checked', isChecked).trigger('change');
            });
        });
    }


    initManualServerTable(
        '#merchantTable',
        'merchant',
        dotNetHelper,
        [
            {
                data: null,
                orderable: false,
                searchable: false,
                render: (data, type, row) =>
                    `<input type="checkbox" class="row-checkbox merchant-checkbox" value="${row.id}" />`
            },
            // { data: 'id', visible: false },
            {
                data: 'batchName', render: function (data, type, row) {
                  return batchname || '';
                }
            },
            { data: 'orderId' },
            { data: 'storeId' },
            { data: 'amount' },
            { data: 'tenderType' },
            { data: 'authCode' },
            {
                data: 'transactionDate',
                render: function (data, type, row) {
                    if (!data) return '';

                    const date = new Date(data);

                    if (isNaN(date.getTime())) return data;

                    const year = date.getFullYear();
                    const month = String(date.getMonth() + 1).padStart(2, '0');
                    const day = String(date.getDate()).padStart(2, '0');

                    const hour24 = date.getHours();
                    const minutes = String(date.getMinutes()).padStart(2, '0');

                    const ampm = hour24 >= 12 ? 'PM' : 'AM';
                    const displayHours = String(hour24 % 12 || 12).padStart(2, '0');

                    return `${year}-${month}-${day} ${displayHours}:${minutes} ${ampm}`;
                }
            },
            {
                data: 'businessDay',
                render: function (data, type, row) {
                    if (!data) return "";

                    const date = new Date(data);

                    if (isNaN(date.getTime())) return data;

                    const year = date.getFullYear();
                    const month = String(date.getMonth() + 1).padStart(2, "0");
                    const day = String(date.getDate()).padStart(2, "0");

                    return `${year}-${month}-${day}`;
                }
            }
        ]
    );

    initManualServerTable(
        '#invoiceTable',
        'invoice',
        dotNetHelper,
        [
            {
                data: null,
                orderable: false,
                searchable: false,
                render: (data, type, row) =>
                    `<input type="checkbox" class="row-checkbox invoice-checkbox" value="${row.id}" />`
            },
            // { data: 'id', visible: false },
           {
                data: 'batchName', render: function (data, type, row) {
                  return batchname || '';
                }
            },
            { data: 'orderId' },
            { data: 'storeId' },
            { data: 'amount' },
            { data: 'tenderType' },
            { data: 'authCode' },
            {
                data: 'transactionDate',
                render: function (data, type, row) {
                    if (!data) return '';

                    const date = new Date(data);

                    if (isNaN(date.getTime())) return data;

                    const year = date.getFullYear();
                    const month = String(date.getMonth() + 1).padStart(2, '0');
                    const day = String(date.getDate()).padStart(2, '0');

                    const hour24 = date.getHours();
                    const minutes = String(date.getMinutes()).padStart(2, '0');

                    const ampm = hour24 >= 12 ? 'PM' : 'AM';
                    const displayHours = String(hour24 % 12 || 12).padStart(2, '0');

                    return `${year}-${month}-${day} ${displayHours}:${minutes} ${ampm}`;
                }
            },
            {
                data: 'businessDay',
                render: function (data, type, row) {
                    if (!data) return "";

                    const date = new Date(data);

                    if (isNaN(date.getTime())) return data;

                    const year = date.getFullYear();
                    const month = String(date.getMonth() + 1).padStart(2, "0");
                    const day = String(date.getDate()).padStart(2, "0");

                    return `${year}-${month}-${day}`;
                }
            },
            { data: 'varianceAmount' }
        ]
    );

    setupTable('#merchantTable', 'merchant');
    setupTable('#invoiceTable', 'invoice');
}

function initManualServerTable(selector, tableType, dotNetHelper, columns) {
    const table = document.querySelector(selector);

    if (!table) {
        console.log("Table not found:", selector);
        return;
    }

    if ($.fn.DataTable.isDataTable(table)) {
        $(table).DataTable().destroy();
    }

    $(table).DataTable({
        processing: true,
        serverSide: true,
        pageLength: 10,
        columns: columns, 
        search: {
            regex: false,
            smart: true
        },
        ajax: function (data, callback, settings) {
           
           const draw = data.draw;
            const pageNumber = Math.floor(data.start / data.length) + 1;
            const pageSize = data.length;
            const searchValue = data.search.value; 

             const columnSearches = {};
        data.columns.forEach((col, index) => {
            if (col.search.value) {
                columnSearches[index] = col.search.value;
            }
        });

            dotNetHelper.invokeMethodAsync(
                'GetManualAdjustmentPage',
                tableType,
                pageNumber,
                pageSize,
                draw  ,
                searchValue
            )
                .then(result => {
                    callback({
                        draw: draw,
                        recordsTotal: result.recordsTotal,
                        recordsFiltered: result.recordsFiltered,
                        data: result.data
                    });
                })
                .catch(error => {
                    console.error(error);
                    callback({
                        draw: draw,
                        recordsTotal: 0,
                        recordsFiltered: 0,
                        data: []
                    });
                });
        }
    });
}