$(document).ready(function () {

    const DATE_FORMAT = "DD-MM-YYYY";

    function syncHasValue($input) {
        if ($.trim($input.val()) !== "") {
            $input.addClass("has-value");
        } else {
            $input.removeClass("has-value");
        }
    }

    function clearValidationUi($input) {
        if (!$input || $input.length === 0) return;

        const input = $input.get(0);

        $input.removeClass("input-validation-error is-invalid");

        const wrapper =
            input.closest(".inputGroup") ||
            input.closest(".upload-wrapper") ||
            input.closest(".formgroup") ||
            input.parentElement;

        if (wrapper) {
            wrapper.classList.remove("input-error");
            wrapper.querySelectorAll(".error-icon").forEach(icon => icon.classList.remove("show"));
        }

        const name = input.getAttribute("name");
        if (name) {
            const $form = $input.closest("form");
            const $msg = $form.find(`[data-valmsg-for="${name}"]`);
            if ($msg.length) {
                $msg.text("")
                    .removeClass("field-validation-error")
                    .addClass("field-validation-valid")
                    .hide();
            }
        }

        if (typeof $input.valid === "function") {
            $input.valid();
        }
    }

    function parseBool(value) {
        return String(value).toLowerCase() === "true";
    }

    function parseDateValue($input) {
        const value = $.trim($input.val());
        if (!value) return null;

        const parsed = moment(value, DATE_FORMAT, true);
        return parsed.isValid() ? parsed : null;
    }

    function normalizeSelector(rawSelector) {
        const selector = String(rawSelector || "").trim();
        if (!selector) return "";

        if (selector.startsWith("#") || selector.startsWith(".") || selector.startsWith("[")) {
            return selector;
        }

        return `#${selector}`;
    }

    function initializeDatePickers() {
        $(".datetimepicker").each(function () {
            const $wrapper = $(this);
            const $input = $wrapper.find("input.floating-input").first();
            if ($input.length === 0) return;

            const role = String($input.attr("data-date-role") || "single").toLowerCase();
            const linkToStart = normalizeSelector($input.attr("data-link-to-start"));
            const minDateToday = parseBool($input.attr("data-min-date-today"));
            const maxDateToday = parseBool($input.attr("data-max-date-today"));

            const options = {
                format: DATE_FORMAT,
                ignoreReadonly: true,
                allowInputToggle: true,
                useCurrent: false
            };

            if (minDateToday) {
                options.minDate = moment().startOf("day");
            }

            if (maxDateToday) {
                options.maxDate = moment().endOf("day");
            }

            $wrapper.datetimepicker(options);

            if (role === "end" && linkToStart) {
                let $start = $(linkToStart).first();

                if ($start.length > 0 && !$start.is("input")) {
                    $start = $start.find("input.floating-input").first();
                }

                const applyStartBoundary = function () {
                    const picker = $wrapper.data("DateTimePicker");
                    if (!picker) return;

                    const startDate = $start.length ? parseDateValue($start) : null;
                    const ownDate = parseDateValue($input);

                    if (startDate) {
                        picker.minDate(startDate.clone().startOf("day"));

                        if (ownDate && ownDate.isBefore(startDate, "day")) {
                            picker.date(startDate.clone());
                        }
                    } else if (minDateToday) {
                        picker.minDate(moment().startOf("day"));
                    } else {
                        picker.minDate(false);
                    }
                };

                applyStartBoundary();

                if ($start.length) {
                    $start.on("dp.change change input", applyStartBoundary);
                }
            }
        });
    }

    initializeDatePickers();

    // Initial sync for pre-filled values
    $(".floating-input").each(function () {
        syncHasValue($(this));
    });

    // When date selected from picker
    $(document).on("dp.change", ".datetimepicker", function () {
        const $input = $(this).find("input.floating-input");
        syncHasValue($input);
        clearValidationUi($input);
        $input.trigger("change");
    });

    // Fallback for any direct value changes
    $(document).on("change input", ".floating-input", function () {
        syncHasValue($(this));
    });
});