window.ChangePasswordModal = (function () {
    let changePwdValidator = null;
    let config = {
        getUrl: '',
        postUrl: ''
    };

    function getPwdUi(fieldId) {
        const el = document.getElementById(fieldId);
        const wrapper = el?.closest('.inputGroup, .formgroup, .form-group, .col-12');
        const icon = wrapper?.querySelector('i.error-icon');
        return { el, wrapper, icon };
    }

    function setPwdErrorUi(fieldId, showError) {
        const { el, wrapper, icon } = getPwdUi(fieldId);
        if (!el) return;

        if (showError) {
            wrapper?.classList.add('input-error');
            el.classList.add('input-validation-error');
            if (icon) icon.style.display = 'block';
        } else {
            wrapper?.classList.remove('input-error');
            el.classList.remove('input-validation-error');
            if (icon) icon.style.display = '';
        }
    }

    function clearPwdFieldError(fieldId) {
        setPwdErrorUi(fieldId, false);

        const $input = $('#' + fieldId);
        if (!$input.length) return;

        const name = $input.attr('name') || fieldId;
        const $msg = $("[data-valmsg-for='" + name + "']");

        $msg.removeClass('field-validation-error')
            .addClass('field-validation-valid')
            .text('');

        if (changePwdValidator) {
            delete changePwdValidator.invalid[name];
            delete changePwdValidator.submitted[name];
        }
    }

    function initValidation() {
        const $form = $('#bannerForm');
        if (!$form.length) return false;

        $form.removeData('validator');
        $form.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse($form);

        changePwdValidator = $form.validate();
        changePwdValidator.settings.ignore = [];
        return true;
    }

    function processForm($form) {
        if (!changePwdValidator && !initValidation()) return;

        const current = ($('#CurrentPassword').val() || '').trim();
        const newPwd = ($('#NewPassword').val() || '').trim();
        const confirm = ($('#ConfirmPassword').val() || '').trim();
        const reason = ($('#Reason').val() || '').trim();

        changePwdValidator.resetForm();
        setPwdErrorUi('CurrentPassword', false);
        setPwdErrorUi('NewPassword', false);
        setPwdErrorUi('ConfirmPassword', false);
        setPwdErrorUi('Reason', false);

        let hasError = false;

        if (!current) {
            changePwdValidator.showErrors({ CurrentPassword: 'Current Password is required.' });
            setPwdErrorUi('CurrentPassword', true);
            hasError = true;
        }

        if (!newPwd) {
            changePwdValidator.showErrors({ NewPassword: 'New Password is required.' });
            setPwdErrorUi('NewPassword', true);
            hasError = true;
        }

        if (!confirm) {
            changePwdValidator.showErrors({ ConfirmPassword: 'Confirm Password is required.' });
            setPwdErrorUi('ConfirmPassword', true);
            hasError = true;
        }

        if (!reason) {
            changePwdValidator.showErrors({ Reason: 'Reason is required.' });
            setPwdErrorUi('Reason', true);
            hasError = true;
        }

        if (newPwd && confirm && newPwd !== confirm) {
            changePwdValidator.showErrors({ ConfirmPassword: 'Passwords do not match.' });
            setPwdErrorUi('ConfirmPassword', true);
            hasError = true;
        }

        if (current && newPwd && current === newPwd) {
            changePwdValidator.showErrors({ NewPassword: 'New password must be different from current password.' });
            setPwdErrorUi('NewPassword', true);
            hasError = true;
        }

        if (hasError) return;

        if (!$form.valid()) {
            ['CurrentPassword', 'NewPassword', 'ConfirmPassword', 'Reason'].forEach(function (field) {
                if ($('#' + field).hasClass('input-validation-error')) {
                    setPwdErrorUi(field, true);
                }
            });
            return;
        }

        $.ajax({
            url: config.postUrl,
            type: 'POST',
            data: $form.serialize(),
            success: function (response) {
                if (typeof response === 'object' && response.success !== undefined) {
                    if (response.success) {
                        $('#changePasswordModal').modal('hide');
                        Swal.fire({ icon: 'success', title: 'Success', html: response.message });
                    } else {

                        const field = response.field || "NewPassword";

                        changePwdValidator.showErrors({ [field]: response.message });
                        setPwdErrorUi(field, true);
                    }
                    return;
                }

                $('#changePasswordModalBody').html(response);
                initValidation();
            },
            error: function (xhr) {
                Swal.fire(
                    'Error',
                    xhr.status === 400
                        ? 'Invalid request token. Please reopen Change Password.'
                        : 'Something went wrong',
                    'error');
            }
        });
    }

    function bindEvents() {
        $(document).off('click.changePwd', '#btnChangePassword');
        $(document).off('submit.changePwd', '#bannerForm');
        $(document).off('input.changePwd', '#CurrentPassword, #NewPassword, #ConfirmPassword, #Reason');

        $(document).on('click.changePwd', '#btnChangePassword', function (e) {
            e.preventDefault();
            processForm($('#bannerForm'));
        });

        $(document).on('submit.changePwd', '#bannerForm', function (e) {
            e.preventDefault();
            processForm($(this));
        });

        $(document).on('input.changePwd', '#CurrentPassword, #NewPassword, #ConfirmPassword, #Reason', function () {
            if ((this.value || '').trim()) {
                clearPwdFieldError(this.id);
            }
        });
    }

    function init(options) {
        config = {
            ...config,
            ...options
        };

        bindEvents();
    }

    function open(uuid) {
        $.get(config.getUrl, { uuid: uuid }, function (html) {
            $('#changePasswordModalBody').html(html);
            initValidation();
            $('#changePasswordModal').modal('show');
        });
    }

    return {
        init,
        open
    };
})();
