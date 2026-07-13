// Loader Js

window.addEventListener("load", function () {



    const loader = document.querySelector(".loading");



    // Small delay so animation looks smooth

    setTimeout(() => {

        loader.classList.add("fade-out");



        // After fade animation completes → remove completely

        setTimeout(() => {

            loader.style.display = "none";

        }, 600);



    }, 300);



});

// Loader Js

//-------------------------------------------------------------------------------------------------------------------------------------------------//







//-------------------------------------------------------------------------------------------------------------------------------------------------//

// Darkmode Js

function applySavedTheme() {



    const body = document.body;

    const moonIcon = document.getElementById("moonIcon");

    const sunIcon = document.getElementById("sunIcon");

    const colorlogo = document.getElementById("colorlogo");

    const whitelogo = document.getElementById("whitelogo");
    const smallwhite = document.getElementById("smallwhitelogo");
    const smallcolor = document.getElementById("smallcolorlogo");



    if (localStorage.getItem("theme") === "dark") {

       

        body.classList.add("dark-mode");
        moonIcon && moonIcon.classList.add("d-none");
        sunIcon && sunIcon.classList.remove("d-none");
        colorlogo && colorlogo.classList.add("d-none");
        whitelogo && whitelogo.classList.remove("d-none");
        smallcolor.classList.remove("d-none");
        smallwhite.classList.add("d-none");

    } else {
       
        body.classList.remove("dark-mode");
        sunIcon && sunIcon.classList.add("d-none");
        moonIcon && moonIcon.classList.remove("d-none");
        whitelogo && whitelogo.classList.add("d-none");
        colorlogo && colorlogo.classList.remove("d-none");
        smallcolor.classList.add("d-none");
        smallwhite.classList.remove("d-none");

    }

}





// ------------------------------

// TOGGLE THEME (Event Delegation)

// ------------------------------

document.addEventListener("click", function (e) {



    if (e.target.closest("#themeToggle")) {



        const body = document.body;



        if (body.classList.contains("dark-mode")) {

            localStorage.setItem("theme", "light");

        } else {

            localStorage.setItem("theme", "dark");

        }



        applySavedTheme();

    }



});

//Darkmode js

//-------------------------------------------------------------------------------------------------------------------------------------------------//





// ------------------------------

// MODIFY YOUR addHTML FUNCTION

// ------------------------------

function addHTML() {

    var el, i, domEl, fileName, xmlHttp;



    el = document.getElementsByTagName("*");



    for (i = 0; i < el.length; i++) {



        domEl = el[i];

        fileName = domEl.getAttribute("w3-include-html");



        if (fileName) {



            xmlHttp = new XMLHttpRequest();



            xmlHttp.onreadystatechange = function () {



                if (this.readyState == 4) {



                    if (this.status == 200) {

                        domEl.innerHTML = this.responseText;

                    }



                    if (this.status == 404) {

                        domEl.innerHTML = "Page not found.";

                    }



                    domEl.removeAttribute("w3-include-html");



                    addHTML();



                    // When all includes are done, apply theme

                    if (!document.querySelector("[w3-include-html]")) {

                        applySavedTheme();

                    }

                }

            }



            xmlHttp.open("GET", fileName, true);

            xmlHttp.send();

            return;

        }

    }

}



// Start loading sidebar

addHTML();



//-------------------------------------------------------------------------------------------------------------------------------------------------//





// Dropdown js for Clear data and On Select border color change 



function clearSelect(el) {



    const select = el.parentElement.querySelector("select");

    const group = select.closest(".inputGroup");



    if ($(select).hasClass("select2-hidden-accessible")) {

        $(select).val(null).trigger("change");   // Proper Select2 clear

    } else {

        select.value = "";

        select.dispatchEvent(new Event("change"));

    }



    group.classList.remove("filled");

}





$(document).on('select2:select select2:clear select2:unselect', function (e) {

    const select = e.target;

    const group = select.closest(".inputGroup");



    if (!group) return;



    if ($(select).val() && $(select).val().length !== 0) {

        group.classList.add("filled");

    } else {

        group.classList.remove("filled");

    }

});

//-------------------------------------------------------------------------------------------------------------------------------------------------//

