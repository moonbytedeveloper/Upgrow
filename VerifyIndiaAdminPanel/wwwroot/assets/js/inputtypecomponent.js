class CustomTextbox extends HTMLElement {
    connectedCallback() {
        const label = this.getAttribute("label") || "";
        const placeholder = this.getAttribute("placeholder") || "";
        const name = this.getAttribute("name") || "";
        const type = this.getAttribute("type") || "text";
        const required = this.hasAttribute("required") ? "required" : "";
        const redirectionurl = this.getAttribute("redirectionurl") || "#";
        

        switch (type) {

            case "textarea":
                this.innerHTML = `
                    <div class="inputGroup formgroup">
                        <textarea type="${type}" ${required} id="${name}" name="${name}" autocomplete="off"></textarea>
                         <i class="error-icon fas fa-exclamation-circle"></i>
                         <span class="error-tooltip text-area-error">
            First name must be at least 2 characters
        </span>
                      <label for="${name}">${label} ${required ? ' <span class="required-star" style="color: red;">*</span>' : ''} </label>
                    </div>
                  `;
                break;
            case "ckeditor":
                this.innerHTML = `
                 
                      <label for="${name}">${label} ${required ? ' <span class="required-star" style="color: red;">*</span>' : ''} </label>
                       <i class="error-icon fas fa-exclamation-circle"></i>
                         <span class="error-tooltip">
            First name must be at least 2 characters
        </span>
                        <textarea type="${type}" class="ckeditor" ${required} id="${name}" name="${name}" autocomplete="off" ></textarea>
                  `;
                break;
            case "checkbox":
                this.innerHTML = `
                    <label class="CheckBoxcontainer">
                      <input type="checkbox">
                      <svg viewBox="0 0 64 64" height="1em" width="1em">
                        <path d="M 0 16 V 56 A 8 8 90 0 0 8 64 H 56 A 8 8 90 0 0 64 56 V 8 A 8 8 90 0 0 56 0 H 8 A 8 8 90 0 0 0 8 V 16 L 32 48 L 64 16 V 8 A 8 8 90 0 0 56 0 H 8 A 8 8 90 0 0 0 8 V 56 A 8 8 90 0 0 8 64 H 56 A 8 8 90 0 0 64 56 V 16" pathLength="575.0541381835938" class="path">
                        </path>
                    </svg>
                    ${label}
                    </label>
                  `;
                break;
            case "customedate":
                this.innerHTML = `<!-- DATE PICKER -->
<div class="inputGroup formgroup date-wrapper">

    <input type="text" class="dateInput" autocomplete="off">
  <i class="error-icon fas fa-exclamation-circle"></i>
                         <span class="error-tooltip">
            Date is required
        </span>
 <label>${label} ${required ? ' <span class="required-star" style="color: red;">*</span>' : ''} </label>

    <div class="datecontainer">
        <div class="datecard">
            <div class="datefront">
                <div class="datecontentfront">

                    <div class="month">
                        <div class="month-nav">
                            <span class="prev">&#10094;</span>
                            <span class="monthTitle"></span>
                            <span class="next">&#10095;</span>
                        </div>

                        <table>
                            <thead>
                                <tr>
                                    <th>M</th>
                                    <th>T</th>
                                    <th>W</th>
                                    <th>T</th>
                                    <th>F</th>
                                    <th>S</th>
                                    <th>S</th>
                                </tr>
                            </thead>
                            <tbody class="calendar"></tbody>
                        </table>
                    </div>

                    <div class="date">
                        <div class="dateNumber"></div>
                        <div class="dayName"></div>
                        <div class="monthYear"></div>
                    </div>

                </div>
            </div>
        </div>
    </div>

</div>

<!-- You can duplicate this block multiple times -->
`;
                break;
            case "dropdown": {
                const optionsAttr = this.getAttribute("options");
                let options = [];

                if (optionsAttr) {
                    try {
                        options = JSON.parse(optionsAttr);
                    } catch (e) {
                        console.error("Invalid options JSON", e);
                    }
                }

                const optionsHTML = options.map(opt =>
                    `<option value="${opt.value}">${opt.text}</option>`
                ).join("");


                this.innerHTML = `
                        <div class="inputGroup formgroup">
                            <select class="searchable-select"  name="${name}" ${required ? "required" : ""} autocomplete="off">
                                <option value="" hidden></option>
                                ${optionsHTML}
                            </select>
                             <i class="error-icon fas fa-exclamation-circle"></i>
                         <span class="error-tooltip">
            Field is required
        </span>
                            <label for="${name}">
                                -- ${label} --
                                ${required ? '<span class="required-star" style="color:red">*</span>' : ''}
                            </label>
                            <span class="clear-select" onclick="clearSelect(this)">✕</span>
                            </div>
                            `
                break;
            }

            case "multiselectdropdown": {
                const optionsAttr = this.getAttribute("options");
                let options = [];

                if (optionsAttr) {
                    try {
                        options = JSON.parse(optionsAttr);
                    } catch (e) {
                        console.error("Invalid options JSON", e);
                    }
                }

                const optionsHTML = options.map(opt =>
                    `<option value="${opt.value}">${opt.text}</option>`
                ).join("");


                this.innerHTML = `
                        <div class="inputGroup formgroup">
                            <select class="searchable-select" multiple  name="${name}" ${required ? "required" : ""} autocomplete="off">
                                <option value="" hidden></option>
                                ${optionsHTML}
                            </select>
                             <i class="error-icon fas fa-exclamation-circle"></i>
                         <span class="error-tooltip">
            Field is required
        </span>
                            <label for="${name}">
                                -- ${label} --
                                ${required ? '<span class="required-star" style="color:red">*</span>' : ''}
                            </label>
                            <span class="clear-select" onclick="clearSelect(this)">✕</span>
                            </div>
                            `
                break;
            };

            case "file":
                this.innerHTML = ` <div class="upload-wrapper">
                
                                        <input type="file" id="upload-file" class="upload-file" ${required} />
                                        <svg version="1.1" xmlns="http://www.w3.org/2000/svg"
                                            xmlns:xlink="http://www.w3.org/1999/xlink"
                                            preserveAspectRatio="xMidYMid meet"
                                            viewBox="224.3881704980842 176.8527621722847 221.13266283524905 178.8472378277154"
                                            width="221.13" height="178.85">
                                            <defs>
                                                <path class="svg-color"
                                                    d="M357.38 176.85C386.18 176.85 409.53 204.24 409.53 238.02C409.53 239.29 409.5 240.56 409.42 241.81C430.23 246.95 445.52 264.16 445.52 284.59C445.52 284.59 445.52 284.59 445.52 284.59C445.52 309.08 423.56 328.94 396.47 328.94C384.17 328.94 285.74 328.94 273.44 328.94C246.35 328.94 224.39 309.08 224.39 284.59C224.39 284.59 224.39 284.59 224.39 284.59C224.39 263.24 241.08 245.41 263.31 241.2C265.3 218.05 281.96 199.98 302.22 199.98C306.67 199.98 310.94 200.85 314.93 202.46C324.4 186.96 339.88 176.85 357.38 176.85Z"
                                                    id="b1aO7LLtdW"></path>
                                                <path
                                                    d="M306.46 297.6L339.79 297.6L373.13 297.6L339.79 255.94L306.46 297.6Z"
                                                    id="c4SXvvMdYD"></path>
                                                <path
                                                    d="M350.79 293.05L328.79 293.05L328.79 355.7L350.79 355.7L350.79 293.05Z"
                                                    id="b11si2zUk"></path>
                                            </defs>
                                            <g>
                                                <g>
                                                    <g>
                                                        <use xlink:href="#b1aO7LLtdW" opacity="1" class="svg-color"
                                                            fill-opacity="1"></use>
                                                    </g>
                                                    <g>
                                                        <g>
                                                            <use xlink:href="#c4SXvvMdYD" opacity="1" class="icon-svg-color"
                                                                fill-opacity="1"></use>
                                                        </g>
                                                        <g>
                                                            <use xlink:href="#b11si2zUk" opacity="1" class="icon-svg-color"
                                                                fill-opacity="1"></use>
                                                        </g>
                                                    </g>
                                                </g>
                                            </g>
                                        </svg>
                                         
                                        <span class="file-upload-text">${label} ${required ? ' <span class="required-star" style="color: red;">*</span>' : ''}</span>
                                            <div class="upload-error">
        <i class="error-icon fas fa-exclamation-circle"></i>
        <span class="error-tooltip">
            File should be jpg, png or pdf.
        </span>
    </div>
                                        <div class="file-success-text">
                                            <svg version="1.1" id="check" xmlns="http://www.w3.org/2000/svg"
                                                xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
                                                viewBox="0 0 100 100" xml:space="preserve">
                                                <circle class="sucesssvg-fill"
                                                    style="stroke:#ffffff;stroke-width:10;stroke-miterlimit:10;"
                                                    cx="49.799" cy="49.746" r="44.757" />
                                                <polyline
                                                    style="fill:rgba(0,0,0,0);stroke:#ffffff;stroke-width:10;stroke-linecap:round;stroke-linejoin:round;stroke-miterlimit:10;"
                                                    points="
                    27.114,51 41.402,65.288 72.485,34.205 " />
                                            </svg> <span>Successfully Uploaded</span>
                                        </div>
                                    </div>
                                    <a href="#"  data-src="http://127.0.0.1:5501/assets/images/users/user-4.jpg" 
   data-caption="Snow Image" class="file-upload-name" style="margin-bottom: 0px; position: absolute; bottom: 70%; right: 3%;  max-width: 70%;
    margin-bottom: 0;

    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;cursor: pointer" data-bs-toggle="tooltip" >view image</a>
                                    <p class="file-upload-name" style="margin-bottom: 0px; position: absolute; bottom: 70%; right: 3%;  max-width: 70%;
    margin-bottom: 0;

    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;cursor: pointer" data-bs-toggle="tooltip" ></p>
`;
                break;

            case "submitbutton":
                this.innerHTML = `<button class="submitbutton" type="submit">
    <span class="submitbutton-decor"></span>
    <div class="submitbutton-content">
        <div class="submitbutton__icon">
        <?xml version="1.0" encoding="utf-8"?>
<!-- Generator: Adobe Illustrator 18.1.1, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->
<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
	 viewBox="0 0 402 405" enable-background="new 0 0 402 405" xml:space="preserve" width="24" height="24" class="submitbutton-svg-color">
<circle cx="200.5" cy="202.5" r="198.1"/>
<path class="submiticon" d="M329.8,175.6c-67.7-27.5-135.4-55-203.1-82.5c-10.5-4.3-19.4-2.7-26.1,4.4c-6.8,7.2-8.1,16.5-3.6,26.8
	c9.8,22.7,19.7,45.4,29.2,68.2c1.1,2.6,0.9,6.3-0.1,8.9c-3,8-6.6,15.7-10,23.6c-6.6,15.4-13.3,30.7-19.8,46.1
	c-6.6,15.8,3.5,31.7,20,32.3c3.1-0.8,6.3-1.2,9.2-2.4c68.2-27.6,136.4-55.3,204.6-83c9.5-3.9,14.8-10.9,15-21.3
	C345.4,187.5,339.6,179.6,329.8,175.6z M108.7,286.7c11.1-25.9,21.4-50,32.1-74.1c0.7-1.6,3.8-3,5.8-3c17.4-0.2,34.8,0.2,52.2-0.4
	c4-0.1,8.6-2.2,11.7-4.9c3.7-3.2,4-8.3,1.7-12.9c-2.6-5.2-7.2-7.3-12.9-7.2c-16.6,0-33.3-0.3-49.9,0.2c-5.8,0.2-8.4-1.7-10.6-7
	c-9.5-23-19.6-45.8-30.1-70.3c74.1,30.1,146.9,59.6,221.3,89.8C255.6,227,182.9,256.5,108.7,286.7z"/>
</svg>

        </div>
        <span class="submitbutton__text">${label}</span>
    </div>
</button></a>`;
                break;

            case "cancelbutton":
                this.innerHTML = `<a href="${redirectionurl}" class="cancelbutton">
    <span class="cancelbutton-decor"></span>
    <div class="cancelbutton-content">
        <span class="cancelbutton__text">${label}</span>
        <div class="cancelbutton__icon">
           <?xml version="1.0" encoding="utf-8"?>
<!-- Generator: Adobe Illustrator 18.1.1, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->
<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
	 viewBox="0 0 411 400" enable-background="new 0 0 411 400" xml:space="preserve" width="24" height="24" class="cancelbutton-svg-color">
<circle cx="205.5" cy="200" r="188.6"/>
<path display="none" d="M-501.6,0c103.8-0.3,188,84.2,188.6,189c0.5,102.8-84.6,188-187.9,188.2c-104.2,0.2-189-83.9-189.3-187.8
	C-690.6,84.9-606.3,0.3-501.6,0z M-501.7,349.7c87.6,0.3,160.5-71.8,161.2-158.7c0.7-90-70.7-162.4-158.4-163.3
	c-91.1-0.9-163.5,70.6-163.8,160.8C-663,277.2-590.9,349.4-501.7,349.7z"/>
<text transform="matrix(0.7036 0.7106 -0.7106 0.7036 -6.5843 192.0166)" class="cancelbutton-text" font-family="'Nunito" font-size="510">+</text>
</svg>

        </div>
        
    </div>
</a>`;
                break;

            case "addbutton":
                this.innerHTML = `<a href="${redirectionurl}" class="submitbutton addbutton">
    <span class="submitbutton-decor"></span>
    <div class="submitbutton-content">
        <div class="submitbutton__icon">
         <?xml version="1.0" encoding="utf-8"?>
<!-- Generator: Adobe Illustrator 18.1.1, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->
<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
	 viewBox="0 0 411 400" enable-background="new 0 0 411 400" xml:space="preserve"  width="24" height="24" class="addbutton-svg-color">
<circle cx="205.5" cy="200" r="188.6"/>
<path display="none" d="M-501.6,0c103.8-0.3,188,84.2,188.6,189c0.5,102.8-84.6,188-187.9,188.2c-104.2,0.2-189-83.9-189.3-187.8
	C-690.6,84.9-606.3,0.3-501.6,0z M-501.7,349.7c87.6,0.3,160.5-71.8,161.2-158.7c0.7-90-70.7-162.4-158.4-163.3
	c-91.1-0.9-163.5,70.6-163.8,160.8C-663,277.2-590.9,349.4-501.7,349.7z"/>
<path display="none" d="M-501.4,206.3c-14.1,14.2-27.1,27.3-40.1,40.5c-6.6,6.6-13.1,13.3-19.8,19.8c-4.9,4.8-11.4,5.6-16.4,2.5
	c-7.3-4.6-8.4-14.2-2-20.6c15.5-15.6,31-31.2,46.7-46.6c4.2-4.2,8.8-8,14.3-13c-9.5-9.2-18.1-17.3-26.5-25.5
	c-11.2-11-22.2-22.1-33.3-33.1c-6.1-6.1-7.1-13.3-2.6-18.7c5.6-6.8,13.9-6.8,20.8,0.2c11,10.9,21.9,21.9,32.8,32.9
	c8.4,8.5,16.7,17.1,26.2,26.9c6.4-6.8,11.9-13.2,17.9-19.2c13.4-13.6,27.1-27.1,40.6-40.7c6.1-6.1,14.1-6.7,19.4-1.4
	c5.6,5.6,5.2,13.5-1.2,19.9c-18,18-36.1,36-54.1,54c-1.4,1.4-2.7,2.9-4.9,5.1c7.6,7.4,15,14.4,22.3,21.6
	c12.3,12.2,24.5,24.5,36.8,36.7c4.5,4.4,6.3,9.4,4,15.3c-3.5,8.8-14.2,10.8-21.4,3.8c-15.3-15-30.5-30.3-45.6-45.5
	C-492,216.5-496.3,211.7-501.4,206.3z"/>
<text transform="matrix(1 0 0 1 53.1671 345.1663)" font-family="'Nunito" font-size="510" class="addbutton-text">+</text>
</svg>


        </div>
        <span class="submitbutton__text">${label}</span>
    </div>
</a>`;

                break;
            case "actiontoggle":
                this.innerHTML = `<div class="form-switch-success d-flex">
                                                <div id="button-3" class="button r">
                                                    <input type="checkbox" class="checkbox customcheckbox" checked>
                                                    <div class="knobs"></div>
                                                    <div class="layer"></div>
                                                </div>
                                                <label
                                                    class="form-check-label theme-text-color labelforcheckbox">${label}</label>
                                            </div>`;
                break;
            case "print":
                {
                    this.innerHTML = `<button class="print-button"><span class="print-icon"></span></button>`;
                }
                break;
            default:
                this.innerHTML = `
      <div class="inputGroup formgroup">
        <input type="${type}" ${required} autocomplete="off">
         <i class="error-icon fas fa-exclamation-circle"></i>
                         <span class="error-tooltip">
            Required field cannot be empty.
        </span>
        <label for="${name}">${label} ${required ? ' <span class="required-star" style="color: red;">*</span>' : ''} </label>
      </div>
    `;
                break;



        }





    }
}

customElements.define("form-field", CustomTextbox);







