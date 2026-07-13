$(document).ready(function () {

    $('.upload-file').on('change', function () {

        const $wrapper = $(this).closest('.upload-wrapper');
        const file = this.files[0];

        $wrapper.removeClass('success uploaded');

        if (!file) {
            $(this).removeClass('input-validation-error');
            return;
        }

        // Validate against accept attribute
        const accept = ($(this).attr('accept') || "").toLowerCase();

        if (accept) {

            const allowed = accept.split(',').map(x => x.trim());

            const fileName = file.name.toLowerCase();
            const mimeType = file.type.toLowerCase();

            const valid = allowed.some(type => {

                if (type.startsWith('.'))
                    return fileName.endsWith(type);

                if (type.endsWith('/*'))
                    return mimeType.startsWith(type.slice(0, -1));

                return mimeType === type;

            });

            if (!valid) {

                showNotification(
                    `"${file.name}" is not a supported file type.`,
                    'danger'
                );

                this.value = "";
                $wrapper.removeClass('success uploaded');

                return;
            }
        }

        // Valid file
        setTimeout(() => {
            $wrapper.addClass("uploaded");
        }, 600);

        setTimeout(() => {
            $wrapper.removeClass("uploaded").addClass("success");
        }, 1600);

    });

});