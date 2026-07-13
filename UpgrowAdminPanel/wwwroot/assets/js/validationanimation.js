function showValidationAnimation(formOrSelector) {
    const form =
        typeof formOrSelector === "string"
            ? document.querySelector(formOrSelector)
            : formOrSelector;

    if (!form) return;

    const invalidInputs = form.querySelectorAll(`
                .input-validation-error,
                .is-invalid,
                [aria-invalid="true"]
            `);

    let firstInvalidWrapper = null;

    invalidInputs.forEach(input => {
        const wrapper =
            input.closest(".inputGroup") ||
            input.closest(".upload-wrapper") ||
            input.closest(".formgroup") ||
            input.parentElement;

        wrapper?.classList.add("input-error");
        wrapper?.querySelector(".error-icon")?.classList.add("show");

        if (input.tagName === "SELECT") {
            input.nextElementSibling
                ?.querySelector(".select2-selection")
                ?.classList.add("input-error");
        }

        if (!firstInvalidWrapper && wrapper) {
            firstInvalidWrapper = wrapper;
        }
    });

    if (invalidInputs.length > 0) {
        const card = document.querySelector(".card");
        card?.classList.add("shake");
        setTimeout(() => card?.classList.remove("shake"), 350);

        firstInvalidWrapper?.scrollIntoView({ behavior: "smooth", block: "center" });
    }
}

document.addEventListener("DOMContentLoaded", function () {

    const form = document.getElementById("bannerForm");
    if (!form) return;

    const submitBtn = form.querySelector('button[type="submit"], input[type="submit"]');
    if (!submitBtn) return;

    /* ==============================
       🔍 SELECT2 INITIALIZATION
    =============================== */

    if (window.jQuery && $.fn.select2) {

        $('.searchable-select').select2({
            allowClear: false,
            width: '100%'
        });

        $('.searchable-select').on('change', function () {

            const wrapper = this.closest(".inputGroup");

            wrapper?.classList.remove("input-error");
            wrapper?.querySelector(".error-icon")?.classList.remove("show");

            const select2Selection =
                this.nextElementSibling?.querySelector(".select2-selection");

            select2Selection?.classList.remove("input-error");

            runUnobtrusiveValidation();
        });
    }

    function runUnobtrusiveValidation() {
        const $ = window.jQuery;
        if (!$) return null;

        const $form = $(form);

        if ($.validator && $.validator.unobtrusive) {
            if (!$form.data("validator")) {
                $.validator.unobtrusive.parse($form);
            }
        }

        if ($form.data("validator") && typeof $form.valid === "function") {
            return $form.valid();
        }

        return null;
    }

    function markFieldErrorUi(field) {
        if (!field) return null;

        const wrapper =
            field.closest(".inputGroup") ||
            field.closest(".upload-wrapper") ||
            field.closest(".formgroup") ||
            field.parentElement;

        wrapper?.classList.add("input-error");
        wrapper?.querySelector(".error-icon")?.classList.add("show");

        if (field.tagName === "SELECT") {
            const select2Selection =
                field.nextElementSibling?.querySelector(".select2-selection");
            select2Selection?.classList.add("input-error");
        }

        return wrapper;
    }

    function clearValidationForField(field) {
        if (!field) return;

        field.classList.remove("input-validation-error", "is-invalid");

        const ancestors = [
            field.closest(".inputGroup"),
            field.closest(".upload-wrapper"),
            field.closest(".formgroup")
        ];
        ancestors.forEach(a => {
            if (a) {
                a.classList.remove("input-error");
                a.querySelectorAll(".error-icon").forEach(icon => icon.classList.remove("show"));
            }
        });

        const name = field.getAttribute("name");
        if (name) {
            const msg = form.querySelector(`[data-valmsg-for="${name}"]`);
            if (msg) {
                msg.textContent = "";
                msg.classList.remove("field-validation-error");
                msg.classList.add("field-validation-valid");
                msg.style.display = "none";
            }
        }

        const select2Selection = field.nextElementSibling?.querySelector?.(".select2-selection");
        if (select2Selection) select2Selection.classList.remove("input-error");

        // Also clear CKEditor container error UI if this textarea is bound to an editor.
        if (window.CKEDITOR && field.id && CKEDITOR.instances && CKEDITOR.instances[field.id]) {
            const editorInstance = CKEDITOR.instances[field.id];
            editorInstance?.container?.$?.classList.remove("input-error");
        }

        const $ = window.jQuery;
        if ($) {
            const $form = $(form);
            const validator = $form.data("validator");
            if (validator && name && validator.invalid) {
                delete validator.invalid[name];
            }

            if (typeof $(field).valid === "function") {
                $(field).valid();
            }
        }
    }

    function applyUiFromUnobtrusiveErrors() {
        const invalidInputs = form.querySelectorAll(".input-validation-error");
        let firstInvalidWrapper = null;

        invalidInputs.forEach(input => {
            if (input.type === "file" && input.files && input.files.length > 0) {
                input.classList.remove("input-validation-error", "is-invalid");
                return;
            }

            const wrapper = markFieldErrorUi(input);
            if (!firstInvalidWrapper && wrapper) firstInvalidWrapper = wrapper;
        });

        if (invalidInputs.length > 0) {
            shakeCard();
            firstInvalidWrapper?.scrollIntoView({ behavior: "smooth", block: "center" });
        }
    }

    // NEW: read server-rendered ModelState messages and mark field/icon UI
    function applyUiFromServerErrors() {
        const errorSpans = form.querySelectorAll("[data-valmsg-for]");
        let firstInvalidWrapper = null;
        let hasErrors = false;

        errorSpans.forEach(span => {
            const message = (span.textContent || "").trim();
            if (!message) return; // message text is the most reliable server-side signal

            const fieldName = span.getAttribute("data-valmsg-for");
            if (!fieldName) return;

            const safeName =
                window.CSS && typeof CSS.escape === "function"
                    ? CSS.escape(fieldName)
                    : fieldName.replace(/"/g, '\\"');

            const field = form.querySelector(`[name="${safeName}"]`);
            if (!field) return;

            const wrapper = markFieldErrorUi(field);
            if (!firstInvalidWrapper && wrapper) firstInvalidWrapper = wrapper;

            span.classList.remove("field-validation-valid");
            span.classList.add("field-validation-error");
            span.style.display = "";

            hasErrors = true;
        });

        if (hasErrors) {
            shakeCard();
            firstInvalidWrapper?.scrollIntoView({ behavior: "smooth", block: "center" });
        }
    }

    function shakeCard() {
        const card = document.querySelector(".card");
        card?.classList.add("shake");
        setTimeout(() => card?.classList.remove("shake"), 350);
    }

    function clearErrors() {
        form.querySelectorAll(".input-error").forEach(el =>
            el.classList.remove("input-error")
        );

        form.querySelectorAll(".error-icon").forEach(icon =>
            icon.classList.remove("show")
        );
    }

    submitBtn.addEventListener("click", function (e) {

        clearErrors();

        const unobtrusiveResult = runUnobtrusiveValidation();

        if (unobtrusiveResult === false) {
            e.preventDefault();
            applyUiFromUnobtrusiveErrors();
            applyUiFromServerErrors();
            return;
        }

        if (unobtrusiveResult === true) {
            return;
        }

        let valid = true;

        const requiredFields = form.querySelectorAll(`
            input[required],
            select[required],
            textarea[required]
        `);

        requiredFields.forEach(field => {

            const wrapper =
                field.closest(".inputGroup") ||
                field.closest(".upload-wrapper") ||
                field.closest(".formgroup") ||
                field.parentElement;

            let isInvalid = false;

            // FIX: handle any CKEditor instance created for this textarea id (nested models included).
            if (window.CKEDITOR && field.tagName === "TEXTAREA" && field.id && CKEDITOR.instances && CKEDITOR.instances[field.id]) {
                const editorInstance = CKEDITOR.instances[field.id];
                const editorData = editorInstance.getData().trim();

                if (!editorData) {
                    isInvalid = true;
                    editorInstance.container.$.classList.add("input-error");

                    const fieldName = field.getAttribute("name");
                    const requiredMsg = field.getAttribute("data-val-required");
                    if (fieldName && requiredMsg) {
                        const msgSpan = form.querySelector(`[data-valmsg-for="${fieldName}"]`);
                        if (msgSpan) {
                            msgSpan.textContent = requiredMsg;
                            msgSpan.classList.remove("field-validation-valid");
                            msgSpan.classList.add("field-validation-error");
                            msgSpan.style.display = "";
                        }
                    }
                } else {
                    editorInstance.container.$.classList.remove("input-error");
                }
            }
            else if (field.type === "file") {
                isInvalid = !field.files || field.files.length === 0;
            }
            else if (field.tagName === "SELECT") {
                if (!field.value || field.value === "0") {
                    isInvalid = true;

                    const select2Selection =
                        field.nextElementSibling?.querySelector(".select2-selection");

                    select2Selection?.classList.add("input-error");
                }
            }
            else {
                if (!field.value.trim()) {
                    isInvalid = true;
                }
            }

            if (isInvalid) {
                wrapper?.classList.add("input-error");
                wrapper?.querySelector(".error-icon")?.classList.add("show");
                valid = false;
            }
        });

        if (!valid) {
            e.preventDefault();
            shakeCard();
            return;
        }

        // 🔥 Redirect support (designer feature)
        const redirectUrl = submitBtn.getAttribute("redirectionurl");
        if (redirectUrl) {
            e.preventDefault();
            window.location.href = redirectUrl;
        }
    });

    form.addEventListener("input", function (e) {
        const field = e.target;
        if (!field || !field.matches("input, textarea")) return;

        if (field.type !== "file" && field.value.trim() !== "") {
            clearValidationForField(field);
        }
    });

    form.addEventListener("change", function (e) {
        const field = e?.target;
        if (!field || !field.matches("input, textarea, select")) {
            runUnobtrusiveValidation();
            return;
        }

        if (field.type === "file") {
            const hasFile = field.files && field.files.length > 0;
            const wrapper =
                field.closest(".inputGroup") ||
                field.closest(".upload-wrapper") ||
                field.closest(".formgroup") ||
                field.parentElement;

            if (hasFile) {
                clearValidationForField(field);

                const success = wrapper?.querySelector(".file-success-text");
                if (success) success.style.display = "flex";

                const namePlace = wrapper?.parentElement?.querySelector(".file-upload-name") || wrapper?.querySelector(".file-upload-name");
                if (namePlace) namePlace.textContent = field.files[0]?.name || "";

                runUnobtrusiveValidation();
            } else {
                wrapper?.classList.remove("input-error");
                wrapper?.querySelector(".error-icon")?.classList.remove("show");

                const success = wrapper?.querySelector(".file-success-text");
                if (success) success.style.display = "none";

                const namePlace = wrapper?.parentElement?.querySelector(".file-upload-name") || wrapper?.querySelector(".file-upload-name");
                if (namePlace) namePlace.textContent = "";
            }

            return;
        }

        if (field.tagName === "SELECT") {
            if (field.value && field.value !== "" && field.value !== "0") {
                clearValidationForField(field);
            }
            runUnobtrusiveValidation();
            return;
        }

        runUnobtrusiveValidation();
    });

    /* ==============================
       🔍 CKEDITOR CHANGE EVENT HANDLER
       Remove error icon when user adds content
    =============================== */
    if (window.CKEDITOR) {
        CKEDITOR.on('instanceReady', function (event) {
            const editor = event.editor;
            const textarea = document.getElementById(editor.name);

            if (!textarea) return;

            // Listen for content changes in the editor
            editor.on('change', function () {
                const editorData = editor.getData().trim();

                if (editorData) {
                    // Content exists - remove error UI
                    const wrapper = textarea.closest(".ckeditor-wrapper");
                    if (wrapper) {
                        wrapper.classList.remove("input-error");
                        wrapper.querySelector(".error-icon")?.classList.remove("show");
                    }

                    // Also clear the error message
                    clearValidationForField(textarea);
                }
            });
        });
    }

    // NEW: handle server-side errors right after page loads (postback case)
    applyUiFromServerErrors();
    applyUiFromUnobtrusiveErrors();
});