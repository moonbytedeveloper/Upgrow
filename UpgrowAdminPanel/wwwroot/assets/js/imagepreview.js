document.addEventListener("DOMContentLoaded", function () {
    function ensureModal() {
        let modal = document.getElementById("imagemodal");
        if (modal) return modal;

        modal = document.createElement("div");
        modal.id = "imagemodal";
        modal.className = "modal";
        modal.style.display = "none";

        const close = document.createElement("span");
        close.className = "close";
        close.innerHTML = "&times;";

        const img = document.createElement("img");
        img.className = "modal-content";
        img.id = "imagepath";

        modal.appendChild(close);
        modal.appendChild(img);
        document.body.appendChild(modal);

        close.addEventListener("click", () => (modal.style.display = "none"));
        modal.addEventListener("click", (e) => {
            if (e.target === modal) modal.style.display = "none";
        });

        return modal;
    }

    document.addEventListener("click", function (e) {
        const link = e.target.closest(".file-upload-name");
        if (!link) return;

        e.preventDefault();
        const imgSrc = link.getAttribute("data-src");
        if (!imgSrc) return;

        const modal = ensureModal();
        const modalImg = modal.querySelector("#imagepath");
        modalImg.src = imgSrc;
        /*modal.style.display = "block";*/
        modal.style.display = "flex";
    });
});