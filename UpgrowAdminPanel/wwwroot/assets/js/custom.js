
document.querySelector('.cross-icon')?.addEventListener('click', function () {
    const card = this.closest('.notificatoncard');
    card.style.animation = 'none';
    card.style.right = '-420px';
    card.style.opacity = '0';
    setTimeout(() => card.remove(), 300);
  });




   $(document).ready(function () {

    $('.upload-file').on('change', function () {

        const $wrapper = $(this).closest('.upload-wrapper');
        const filename = this.files.length ? this.files[0].name : "";

        // Update only THIS component
        $wrapper.next('.file-upload-name').text(filename);

        if (filename !== "") {

            $wrapper.removeClass('success uploaded');

            setTimeout(function () {
                $wrapper.addClass("uploaded");
            }, 600);

            setTimeout(function () {
                $wrapper.removeClass("uploaded").addClass("success");
            }, 1600);
        }
    });

});