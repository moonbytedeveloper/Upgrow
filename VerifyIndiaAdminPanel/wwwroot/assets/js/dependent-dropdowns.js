(function (window, $) {
    window.DependentDropdowns = window.DependentDropdowns || {};

    /**
     * Initialize API -> Provider dependent dropdown behavior.
     * @param {string} apiSelector - jQuery selector for API select (eg '#ApiProvider_ApiUUID')
     * @param {string} providerSelector - jQuery selector for Provider select (eg '#ApiProvider_ProviderUUID')
     * @param {Array} providerList - array of { Value, Text } entries for provider dropdown
     * @param {Array} mappings - array of { ApiUUID, ProviderUUID } entries
     * @param {Object} options - optional config: { placeholder, preserveOnLoad (true/false) }
     * @returns {Object} - { refresh(), populate(apiValue, preserveSelected) }
     */
    window.DependentDropdowns.initApiProviderDependency = function (apiSelector, providerSelector, providerList, mappings, options) {
        options = options || {};
        var placeholder = options.placeholder || ' ';
        var preserveOnLoad = options.preserveOnLoad !== undefined ? options.preserveOnLoad : true;

        // Normalize incoming arrays - keep original case for ProviderUUID
        var normMappings = (mappings || []).map(function (m) {
            return {
                ApiUUID: ((m.ApiUUID || '') + '').toString().trim().toLowerCase(),
                ProviderUUID: ((m.ProviderUUID || '') + '').toString().trim() // Preserve case
            };
        });

        var normProviders = (providerList || []).map(function (p) {
            return {
                Value: ((p.Value || '') + '').toString().trim(), // Preserve case
                Text: (p.Text || '') + ''
            };
        });

        var $api = $(apiSelector);
        var $prov = $(providerSelector);
        if ($api.length === 0 || $prov.length === 0) return null;

        function populate(apiVal, preserveSelected) {
            var previousSelected = preserveSelected ? ($prov.val() || '') : '';

            // Clear provider options and add placeholder
            $prov.empty();
            $prov.append($('<option/>').val('').text(placeholder));

            // Build array of selected APIs (support single or array)
            var selectedApis = [];
            if (apiVal == null || apiVal === '') {
                selectedApis = [];
            } else if (Array.isArray(apiVal)) {
                selectedApis = apiVal.map(function (v) { return ((v || '') + '').toString().trim().toLowerCase(); });
            } else {
                selectedApis = [((apiVal || '') + '').toString().trim().toLowerCase()];
            }

            if (selectedApis.length === 0) {
                // No API selected -> leave only placeholder
                $prov.trigger('change.select2');
                return;
            }

            // Determine allowed providers for selected API(s)
            var allowedSet = {};
            normMappings.forEach(function (m) {
                if (selectedApis.indexOf(m.ApiUUID) !== -1) {
                    allowedSet[m.ProviderUUID] = true;
                }
            });

            // Populate provider options based on allowed set
            normProviders.forEach(function (p) {
                if (allowedSet[p.Value]) {
                    $prov.append($('<option/>').val(p.Value).text(p.Text));
                }
            });

            // Preserve previous selection if still present and requested
            if (preserveSelected && previousSelected) {
                if ($prov.find('option[value="' + previousSelected + '"]').length > 0) {
                    $prov.val(previousSelected);
                } else {
                    $prov.val('');
                }
            }

            // Notify Select2 (if used)
            $prov.trigger('change.select2');
        }

        // Initialize Select2 if available and not already initialized
        try {
            if ($.fn.select2 && !$api.data('select2')) $api.select2({ width: '100%' });
            if ($.fn.select2 && !$prov.data('select2')) $prov.select2({ width: '100%' });
        } catch (e) {
            // ignore select2 init errors
        }

        // Bind change event
        $api.off('change.depd').on('change.depd', function () {
            populate($(this).val(), false);
        });

        // initial population (edit/initial load)
        populate($api.val(), preserveOnLoad);

        return {
            refresh: function () { populate($api.val(), true); },
            populate: populate
        };
    };
})(window, jQuery);