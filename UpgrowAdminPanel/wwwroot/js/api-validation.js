document.addEventListener("DOMContentLoaded", function () {

    /* =========================================================
       GLOBAL HELPERS
    ========================================================== */

    function getWrapper(field) {
        return field.closest(".inputGroup")
            || field.closest(".formgroup")
            || field.closest(".floating-group")
            || field.parentElement;
    }

    function showError(field, message) {

        if (!field) return;

        // IMPORTANT FIX
        const wrapper =
            field.closest('.inputGroup') ||
            field.closest('.formgroup') ||
            field.parentElement;

        if (!wrapper) return;

        // FORCE CLASS
        wrapper.classList.add('input-error');

        // textarea specific fix
        if (field.tagName === "TEXTAREA") {
            field.classList.add("input-error");
        }

        // icon
        const icon = wrapper.querySelector('.error-icon');

        if (icon) {
            icon.classList.add('show');
        }

        // validation message
        const name = field.getAttribute('name');

        const msg =
            wrapper.querySelector(`[data-valmsg-for="${name}"]`) ||
            document.querySelector(`[data-valmsg-for="${name}"]`);

        if (msg) {

            msg.textContent = message;

            msg.classList.remove('field-validation-valid');

            msg.classList.add('field-validation-error');

            msg.style.display = 'block';
        }

        console.log('Validation Applied:', {
            field: field.name,
            wrapper: wrapper,
            classes: wrapper.className
        });
    }

    function clearErrors() {

        form.querySelectorAll('.input-error')
            .forEach(x => x.classList.remove('input-error'));

        form.querySelectorAll('.error-icon')
            .forEach(x => x.classList.remove('show'));

        form.querySelectorAll('textarea')
            .forEach(x => x.classList.remove('input-error'));

        form.querySelectorAll('[data-valmsg-for]')
            .forEach(x => {

                x.textContent = '';

                x.classList.remove('field-validation-error');

                x.classList.add('field-validation-valid');

                x.style.display = 'none';
            });
    }

    window.clearFieldError = function (field) {

        if (!field) return;

        const wrapper = getWrapper(field);
        if (!wrapper) return;

        wrapper.classList.remove("input-error");

        const icon = wrapper.querySelector(".error-icon");
        if (icon) {
            icon.classList.remove("show");
        }

        const name = field.getAttribute("name");

        let msg =
            wrapper.querySelector(`[data-valmsg-for="${name}"]`) ||
            document.querySelector(`[data-valmsg-for="${name}"]`);

        if (msg) {
            msg.textContent = "";
            msg.classList.remove("field-validation-error");
            msg.classList.add("field-validation-valid");
            msg.style.display = "";
        }
    };

    window.clearFormErrors = function (form) {

        if (!form) return;

        form.querySelectorAll(".input-error")
            .forEach(x => x.classList.remove("input-error"));

        form.querySelectorAll(".error-icon")
            .forEach(x => x.classList.remove("show"));

        form.querySelectorAll("[data-valmsg-for]").forEach(x => {

            x.textContent = "";

            x.classList.remove("field-validation-error");

            x.classList.add("field-validation-valid");

            x.style.display = "";
        });
    };

    function shake(form) {

        const card =
            form.closest(".card")
            || document.querySelector(".card");

        if (!card) return;

        card.classList.add("shake");

        setTimeout(() => {
            card.classList.remove("shake");
        }, 300);
    }

    /* =========================================================
       LIVE VALIDATION REMOVE
    ========================================================== */

    $(document).on(
        "input change",
        "input, textarea, select",
        function () {

            const field = this;

            let valid = true;

            if (field.type === "checkbox") {
                valid = field.checked;
            }
            else if (field.type === "radio") {

                const radios = document.querySelectorAll(
                    `[name="${field.name}"]`
                );

                valid = [...radios].some(r => r.checked);

                if (valid) {
                    radios.forEach(r => clearFieldError(r));
                }

                return;
            }
            else {
                valid = (field.value || "").trim() !== "";
            }

            if (valid) {
                clearFieldError(field);
            }
        }
    );

    /* =========================================================
       SELECT2 SUPPORT
    ========================================================== */

    if (window.jQuery && $.fn.select2) {

        $('.searchable-select').select2({
            allowClear: false,
            width: '100%'
        });

        $('.searchable-select').on('change', function () {

            clearFieldError(this);

            const select2Selection =
                this.nextElementSibling
                    ?.querySelector(".select2-selection");

            select2Selection?.classList.remove("input-error");
        });
    }

    /* =========================================================
       FORM INITIALIZATION
    ========================================================== */

    document.querySelectorAll("form").forEach(initForm);

    function initForm(form) {

        const submitBtn = form.querySelector(
            'button[type="submit"], input[type="submit"]'
        );

        if (!submitBtn) return;

        /* =====================================================
           VALIDATION
        ====================================================== */

        function validate() {

            let valid = true;

            const fields = form.querySelectorAll(
                "input[required], select[required], textarea[required]"
            );

            fields.forEach(field => {

                let invalid = false;

                /* TEXTAREA */
                if (field.tagName === "TEXTAREA") {

                    invalid = !(field.value || "").trim();
                }

                /* CHECKBOX */
                else if (field.type === "checkbox") {

                    invalid = !field.checked;
                }

                /* RADIO */
                else if (field.type === "radio") {

                    const radios = form.querySelectorAll(
                        `[name="${field.name}"]`
                    );

                    invalid = ![...radios].some(r => r.checked);

                    if (invalid) {
                        radios.forEach(r =>
                            showError(r, "This field is required")
                        );
                    }

                    return;
                }

                /* SELECT */
                else if (field.tagName === "SELECT") {

                    invalid =
                        !field.value ||
                        field.value === "0";

                    if (invalid) {

                        const select2Selection =
                            field.nextElementSibling
                                ?.querySelector(".select2-selection");

                        select2Selection?.classList.add("input-error");
                    }
                }

                /* INPUT */
                else {

                    invalid = !(field.value || "").trim();
                }

                if (invalid) {

                    showError(field, "This field is required");

                    valid = false;
                }
            });

            return valid;
        }

        /* =====================================================
           SERVER ERROR HANDLER
        ====================================================== */

        function handleErrors(xhr) {

            const res = xhr.responseJSON;

            if (!res) return;

            if (res.errors) {

                Object.keys(res.errors).forEach(key => {

                    const field = form.querySelector(
                        `[name="${key}"]`
                    );

                    showError(field, res.errors[key]);
                });

                shake(form);

                return;
            }

            if (res.error) {
                alert(res.error);
            }
        }

        /* =====================================================
           SUBMIT HANDLER
        ====================================================== */

        submitBtn.addEventListener("click", function (e) {

            clearFormErrors(form);

            const isValid = validate();

            if (!isValid) {

                e.preventDefault();

                shake(form);

                return;
            }

            const isAjax = form.dataset.ajax === "true";

            if (isAjax) {

                e.preventDefault();

                const formData = new FormData(form);

                $.ajax({

                    url: form.action,

                    type: "POST",

                    data: formData,

                    processData: false,

                    contentType: false,

                    success: function (res) {

                        alert(res.message || "Success");
                    },

                    error: function (xhr) {

                        handleErrors(xhr);
                    }
                });
            }
        });

        /* =====================================================
           SERVER SIDE VALIDATION LOAD
        ====================================================== */

        form.querySelectorAll("[data-valmsg-for]").forEach(e => {

            const msg = e.textContent?.trim();

            if (!msg) return;

            const name = e.getAttribute("data-valmsg-for");

            const field = form.querySelector(
                `[name="${name}"]`
            );

            showError(field, msg);
        });
    }
});