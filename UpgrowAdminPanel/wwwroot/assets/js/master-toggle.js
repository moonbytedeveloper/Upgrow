/**
 * Initializes toggle switch handlers for master entity status updates
 * Should be included in _ViewScripts partial or layout
 */
(function () {
    'use strict';

    // Use event delegation to handle dynamically loaded toggle switches
    $(document).on('change', '.toggle-status', function (e) {
        e.stopPropagation();

        const $checkbox = $(this);
        const uuid = $checkbox.data('uuid');
        const toggleUrl = $checkbox.data('toggle-url');
        const entityName = $checkbox.data('entity-name') || 'item'; // Allow custom entity name
        const currentState = !$checkbox.is(':checked'); // Previous state (before change)
        const newState = $checkbox.is(':checked'); // New intended state

        // Immediately revert the checkbox to prevent visual toggle before confirmation
        $checkbox.prop('checked', currentState);

        // Show confirmation dialog
        showToggleConfirmation(newState, {
            entityName: entityName,
            onConfirm: function () {
                // User confirmed - proceed with toggle
                performToggle($checkbox, uuid, toggleUrl, newState);
            },
            onCancel: function () {
                // User cancelled - checkbox already reverted, do nothing
            }
        });
    });

    /**
     * Performs the actual AJAX toggle request
     */
    function performToggle($checkbox, uuid, toggleUrl, newState) {
        // Disable the checkbox during request
        $checkbox.prop('disabled', true);

        $.ajax({
            url: toggleUrl,
            type: 'POST',
            data: { uuid: uuid },
            success: function (response) {
                if (response.success) {
                    // Update checkbox state based on server response
                    $checkbox.prop('checked', response.isActive);

                    // Show success notification (using toastr if available)
                    if (typeof toastr !== 'undefined') {
                        toastr.success(response.message);
                    }
                } else {
                    // Revert checkbox state on failure
                    $checkbox.prop('checked', !newState);

                    if (typeof toastr !== 'undefined') {
                        toastr.error(response.message);
                    } else {
                        Swal.fire("Error", response.message, "error");
                    }
                }
            },
            error: function (xhr, status, error) {
                // Revert checkbox state on error
                $checkbox.prop('checked', !newState);

                const errorMsg = 'An error occurred while updating status.';
                if (typeof toastr !== 'undefined') {
                    toastr.error(errorMsg);
                } else {
                    Swal.fire("Error", errorMsg, "error");
                }
            },
            complete: function () {
                // Re-enable the checkbox
                $checkbox.prop('disabled', false);
            }
        });
    }
})();