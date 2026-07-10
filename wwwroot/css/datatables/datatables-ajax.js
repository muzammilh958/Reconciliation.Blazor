// /**
//  * Template Name: UBold - Admin & Dashboard Template
//  * By (Author): Coderthemes
//  * Module/App (File Name): Datatables Ajax
//  */

// window.loadDataTableAjax = function () {
//     const tableElement = document.getElementById('datatables-ajax')
//     if (tableElement) {
//         new DataTable(tableElement, {
//             ajax: '/data/datatables.json',
//             processing: true,
//             columns: [
//                 {data: 'company'},
//                 {data: 'symbol'},
//                 {data: 'price'},
//                 {data: 'change'},
//                 {data: 'volume'},
//                 {data: 'market_cap'},
//                 {data: 'rating'},
//                 {
//                     data: 'status',
//                     render: (data, type, row) => {
//                         const isBullish = data === 'Bullish';
//                         return `<span class="badge badge-label badge-soft-${isBullish ? 'success' : 'danger'}">${data}</span>`;
//                     }
//                 }
//             ],
//             language: {
//                 paginate: {
//                     first: '<i class="ti ti-chevrons-left"></i>',
//                     previous: '<i class="ti ti-chevron-left"></i>',
//                     next: '<i class="ti ti-chevron-right"></i>',
//                     last: '<i class="ti ti-chevrons-right"></i>'
//                 },
//                 lengthMenu: '_MENU_ Companies per page',
//                 info: 'Showing <span class="fw-semibold">_START_</span> to <span class="fw-semibold">_END_</span> of <span class="fw-semibold">_TOTAL_</span> Companies'
//             }
//         });
//     }
// };
export function loadDataTableAjax() {

    console.log("DataTable module loaded");

    const table = document.querySelector('[data-tables="basic"]');

    if (!table) {
        console.error("Table not found");
        return;
    }

    // prevent double init (Blazor navigation safe)
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
if ($.fn.DataTable.isDataTable('#reconTable')) {
        $('#reconTable').DataTable().destroy();
    }
    // Initialise with professional options
    $('#reconTable').DataTable({
        responsive: true,
        pageLength: 25,
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
        dom: 'Bfrtip',   // requires Buttons extension
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        order: [[6, 'desc']], // order by date
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search records..."
        },
        columnDefs: [
            { targets: [9], orderable: false } // disable sorting on Matched Line
        ]
    });
    const table = document.querySelector('[data-tables="basic"]');

    if (!table) {
        console.warn("DataTable skipped: table not in DOM yet");
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