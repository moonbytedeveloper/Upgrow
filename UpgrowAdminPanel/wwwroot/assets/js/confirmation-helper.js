/**
 * Reusable confirmation dialog utility using SweetAlert2
 */
(function (window) {
    'use strict';

    /**
     * Shows a confirmation dialog with customizable options
     * @param {Object} options - Configuration object
     * @param {string} options.title - Dialog title (default: "Are you sure?")
     * @param {string} options.text - Dialog message
     * @param {string} options.icon - Icon type: 'warning', 'info', 'success', 'error', 'question' (default: 'warning')
     * @param {string} options.confirmButtonText - Text for confirm button (default: "Yes, confirm!")
     * @param {string} options.cancelButtonText - Text for cancel button (default: "Cancel")
     * @param {string} options.confirmButtonColor - Confirm button color (default: "#3085d6")
     * @param {string} options.cancelButtonColor - Cancel button color (default: "#d33")
     * @param {Function} options.onConfirm - Callback when confirmed
     * @param {Function} options.onCancel - Callback when cancelled (optional)
     * @param {string} options.successTitle - Success dialog title (optional)
     * @param {string} options.successMessage - Success dialog message (optional)
     * @param {boolean} options.showSuccessMessage - Whether to show success message (default: true)
     */
    window.showConfirmation = function (options) {
        const defaults = {
            title: "Are you sure?",
            text: "Do you want to proceed with this action?",
            icon: "warning",
            confirmButtonText: "Yes, confirm!",
            cancelButtonText: "Cancel",
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            showCancelButton: true,
            showSuccessMessage: true
        };

        const config = Object.assign({}, defaults, options);

        Swal.fire({
            title: config.title,
            text: config.text,
            icon: config.icon,
            showCancelButton: config.showCancelButton,
            confirmButtonColor: config.confirmButtonColor,
            cancelButtonColor: config.cancelButtonColor,
            confirmButtonText: config.confirmButtonText,
            cancelButtonText: config.cancelButtonText
        }).then((result) => {
            if (result.isConfirmed) {
                // Execute confirm callback
                if (typeof config.onConfirm === 'function') {
                    config.onConfirm();
                }

                // Show success message if configured
                if (config.showSuccessMessage && (config.successTitle || config.successMessage)) {
                    Swal.fire(
                        config.successTitle || "Success!",
                        config.successMessage || "Action completed successfully.",
                        "success"
                    );
                }
            } else if (result.isDismissed) {
                // Execute cancel callback if provided
                if (typeof config.onCancel === 'function') {
                    config.onCancel();
                }
            }
        });
    };

    /**
     * Helper specifically for toggle confirmations
     * @param {boolean} newState - The new state (true = activate, false = deactivate)
     * @param {Object} options - Additional options
     * @param {string} options.entityName - Name of entity being toggled (e.g., "user", "gender")
     * @param {string} options.activateText - Custom text for activation (optional)
     * @param {string} options.deactivateText - Custom text for deactivation (optional)
     * @param {Function} options.onConfirm - Callback when confirmed
     * @param {Function} options.onCancel - Callback when cancelled
     */
    window.showToggleConfirmation = function (newState, options) {
        const entityName = options.entityName || "item";
        const action = newState ? "activate" : "deactivate";
        const actionCaps = newState ? "Activate" : "Deactivate";

        const activateText = options.activateText || `Do you want to activate this ${entityName}?`;
        const deactivateText = options.deactivateText || `Do you want to deactivate this ${entityName}?`;

        showConfirmation({
            title: "Are you sure?",
            text: newState ? activateText : deactivateText,
            icon: "warning",
            confirmButtonText: `Yes, ${action}!`,
            successTitle: "Updated!",
            successMessage: `${entityName.charAt(0).toUpperCase() + entityName.slice(1)} ${action}d successfully.`,
            onConfirm: options.onConfirm,
            onCancel: options.onCancel
        });
    };

})(window);