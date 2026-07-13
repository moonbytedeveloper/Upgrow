using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Moonbyte.UI
{
    [HtmlTargetElement("custom-form-field")]
    public class CustomFormFieldTagHelper : TagHelper
    {
        public FormFieldType Type { get; set; } = FormFieldType.Text;

        public string? Label { get; set; }

        public string? Id { get; set; }

        public string? Name { get; set; }

        public bool Required { get; set; }

        public string? InputMode { get; set; }

        public string? Pattern { get; set; }

        public int? MaxLength { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            var requiredStar = Required
                ? "<span class='required-star' style='color:red;'>*</span>"
                : "";

            if (Type == FormFieldType.File)
            {
                output.Content.SetHtmlContent($@"
<div class='upload-wrapper formgroup'>
    <input type='file'
           id='{Id}'
           name='{Name}'
           data-verification-field='true'
           class='upload-file'
           {(Required ? "required" : "")}>

    <svg version='1.1'
         xmlns='http://www.w3.org/2000/svg'
         xmlns:xlink='http://www.w3.org/1999/xlink'
         preserveAspectRatio='xMidYMid meet'
         viewBox='224.3881704980842 176.8527621722847 221.13266283524905 178.8472378277154'
         width='221.13'
         height='178.85'>
        <defs>
            <path class='svg-color'
                  d='M357.38 176.85C386.18 176.85 409.53 204.24 409.53 238.02C409.53 239.29 409.5 240.56 409.42 241.81C430.23 246.95 445.52 264.16 445.52 284.59C445.52 309.08 423.56 328.94 396.47 328.94C384.17 328.94 285.74 328.94 273.44 328.94C246.35 328.94 224.39 309.08 224.39 284.59C224.39 263.24 241.08 245.41 263.31 241.2C265.3 218.05 281.96 199.98 302.22 199.98C306.67 199.98 310.94 200.85 314.93 202.46C324.4 186.96 339.88 176.85 357.38 176.85Z'
                  id='cloudShape'>
            </path>

            <path d='M306.46 297.6L339.79 297.6L373.13 297.6L339.79 255.94L306.46 297.6Z'
                  id='arrowTop'>
            </path>

            <path d='M350.79 293.05L328.79 293.05L328.79 355.7L350.79 355.7L350.79 293.05Z'
                  id='arrowBottom'>
            </path>
        </defs>

        <g>
            <use xlink:href='#cloudShape'
                 class='svg-color'>
            </use>

            <use xlink:href='#arrowTop'
                 class='icon-svg-color'>
            </use>

            <use xlink:href='#arrowBottom'
                 class='icon-svg-color'>
            </use>
        </g>
    </svg>

    <span class='file-upload-text'>
        {Label}
        {requiredStar}
    </span>

    <div class='file-success-text'>
        <span>Successfully Uploaded</span>
    </div>
</div>

<p class='file-upload-name-text'
   style='margin-bottom:0;
          position:absolute;
          bottom:70%;
          right:3%;
          max-width:70%;
          white-space:nowrap;
          overflow:hidden;
          text-overflow:ellipsis;
          cursor:pointer'>
</p>");
            }
            else
            {
                var inputType = Type switch
                {
                    FormFieldType.Number => "number",
                    FormFieldType.Email => "email",
                    FormFieldType.Password => "password",
                    FormFieldType.Date => "date",
                    _ => "text"
                };

                output.Content.SetHtmlContent($@"
<div class='inputGroup formgroup'>
    <input type='{inputType}'
       id='{Id}'
       name='{Name}'
       data-verification-field='true'
       {(Required ? "required" : "")}
       {(MaxLength.HasValue ? $"maxlength='{MaxLength.Value}'" : "")}
       {(!string.IsNullOrWhiteSpace(Pattern) ? $"pattern='{Pattern}'" : "")}
       {(!string.IsNullOrWhiteSpace(InputMode) ? $"inputmode='{InputMode}'" : "")}
       autocomplete='off'
       placeholder=' ' />

    <label for='{Id}'>
        {Label}
        {requiredStar}
    </label>

    <i class='error-icon fas fa-exclamation-circle'></i>

    <span class='error-tooltip'>
        Field is required
    </span>
</div>");
            }
        }
    }
}