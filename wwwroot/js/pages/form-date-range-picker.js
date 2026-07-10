/**
 * Template Name: UBold - Admin & Dashboard Template
 * By (Author): Coderthemes
 * Module/App (File Name): Form Daterangepicker
 */
window.initDatePickers = () => {
    if (typeof flatpickr === "undefined") {
        console.error("Flatpickr library not loaded");
        return;
    }
    document.querySelectorAll(".flatpickr").forEach(el => {
        if (!el._flatpickr) {
            flatpickr(el, {
                dateFormat: "Y-m-d"
            });
        }
    });
};
window.loadDateRangePicker = function () {
    if (typeof flatpickr === "undefined") {
        console.error("Flatpickr library not loaded");
        return;
    }
    if (jQuery().daterangepicker) {
        const start = moment().subtract(29, 'days');
        const end = moment();

        // Default config for range pickers
        const defaultRangeOptions = {
            startDate: start,
            endDate: end,
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [
                    moment().subtract(1, 'month').startOf('month'),
                    moment().subtract(1, 'month').endOf('month')
                ]
            },
            locale: {
                format: 'MM/DD/YYYY'
            },
            cancelClass: 'btn-light',
            applyButtonClasses: 'btn-success'
        };

        // Initialize range picker
        $('[data-toggle="date-picker-range"]').each(function () {
            const $this = $(this);
            const dataOptions = $this.data();

            const options = $.extend(true, {}, defaultRangeOptions, dataOptions);
            const targetSelector = $this.attr('data-target-display');

            $this.daterangepicker(options, function (start, end) {
                if (targetSelector) {
                    $(targetSelector).html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
                }
            });

            // Set initial display value
            if (targetSelector) {
                $(targetSelector).html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
            }
        });

        // Default config for single date pickers
        const defaultSingleOptions = {
            singleDatePicker: true,
            showDropdowns: true,
            locale: {
                format: 'MM/DD/YYYY'
            },
            cancelClass: 'btn-light',
            applyButtonClasses: 'btn-success'
        };

        // Initialize single date pickers
        $('[data-toggle="date-picker"]').each(function () {
            const $this = $(this);
            const dataOptions = $this.data();
            const options = $.extend(true, {}, defaultSingleOptions, dataOptions);

            // Handle stringified locale object safely
            if (typeof options.locale === 'string') {
                try {
                    options.locale = JSON.parse(options.locale.replace(/'/g, '"'));
                } catch (e) {
                    console.warn('Invalid JSON format in data-locale:', e);
                }
            }

            $this.daterangepicker(options);
        });
    }

};

window.initDatePickers = function() {
    // Initialize flatpickr with date format
    flatpickr('.flatpickr', {
        dateFormat: "Y-m-d",
        allowInput: true,
        onChange: function(selectedDates, dateStr, instance) {
            // Trigger Blazor update
            var input = instance.input;
            var event = new Event('change', { bubbles: true });
            input.dispatchEvent(event);
        }
    });
};

window.setDatePickerValues = function(fromDate, toDate) {
    var inputs = document.querySelectorAll('.flatpickr');
    if (inputs.length >= 2) {
        if (fromDate) {
            inputs[0].value = fromDate;
            inputs[0]._flatpickr?.setDate(fromDate);
        }
        if (toDate) {
            inputs[1].value = toDate;
            inputs[1]._flatpickr?.setDate(toDate);
        }
    }
};