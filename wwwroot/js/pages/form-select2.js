window.loadFormSelect = function() {
    console.log('Initializing select2...');
    
    // Destroy existing select2 instances
    $('.select2').each(function() {
        if ($(this).data('select2')) {
            $(this).select2('destroy');
        }
    });
    
    // Initialize all select2 elements
    $('.select2').each(function() {
        const $select = $(this);
        const placeholder = $select.find('option:first').text() || 'Select an option';
        
        $select.select2({
            placeholder: placeholder,
            allowClear: true,
            width: '100%',
            dropdownAutoWidth: true,
            // Important for dynamic data
            language: {
                noResults: function() {
                    return 'No results found';
                }
            },
            // Custom matcher for better search
            matcher: function(params, data) {
                // If there are no search terms, return all of the data
                if ($.trim(params.term) === '') {
                    return data;
                }
                
                // Skip if there is no 'text' property
                if (typeof data.text === 'undefined') {
                    return null;
                }
                
                // `params.term` should be the term that is used for searching
                // `data.text` is the text that is displayed for the data object
                if (data.text.toLowerCase().indexOf(params.term.toLowerCase()) > -1) {
                    return data;
                }
                
                // Return `null` if the term should not be displayed
                return null;
            }
        });
        
        // Handle Blazor binding - trigger change event
        $select.on('change', function() {
            // This triggers Blazor's @bind to update
            $(this).trigger('input');
        });
    });
    
    console.log('Select2 initialized successfully');
};


// Initialize Select2
function initializeSelect2(element) {
    if (!element) return;
    
    // Check if Select2 is already initialized
    if ($(element).hasClass('select2-hidden-accessible')) {
        $(element).select2('destroy');
    }
    
    $(element).select2({
        placeholder: 'Search and select...',
        allowClear: false,
        width: '100%',
        dropdownAutoWidth: true,
        language: {
            noResults: function() {
                return 'No results found';
            },
            searching: function() {
                return 'Searching...';
            }
        }
    });
}

// Set up change event for Select2
function setupSelect2Change(element, dotNetHelper, methodName) {
    if (!element) return;
    
    $(element).off('change.select2');
    $(element).on('change.select2', function(e) {
        const value = $(this).val();
        if (dotNetHelper) {
            dotNetHelper.invokeMethodAsync(methodName, value);
        }
    });
}

// Helper function to update Select2 value from Blazor
function updateSelect2Value(element, value) {
    if (!element) return;
    $(element).val(value).trigger('change');
}

// Cleanup function
function destroySelect2(element) {
    if (!element) return;
    if ($(element).hasClass('select2-hidden-accessible')) {
        $(element).select2('destroy');
    }
}