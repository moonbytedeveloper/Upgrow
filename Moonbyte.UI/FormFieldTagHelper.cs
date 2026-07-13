using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Moonbyte.UI
{
    [HtmlTargetElement("form-field", Attributes = "asp-for")]
    public sealed class FormFieldTagHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;

        public FormFieldTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; } = default!;

        [HtmlAttributeName("view-link-path")]
        public string? ViewLinkPath { get; set; }

        [HtmlAttributeName("url")]
        public string? Url { get; set; }

        [HtmlAttributeName("accept")]
        public string? Accept { get; set; }

        public string? Label { get; set; }
        public bool Required { get; set; }
        public string? Placeholder { get; set; }

        [HtmlAttributeName("multiple")]
        public bool Multiple { get; set; }

        [HtmlAttributeName("searchable")]
        public bool Searchable { get; set; } = true;
        public FormFieldType Type { get; set; } = FormFieldType.Text;

        [HtmlAttributeName("asp-items")]
        public IEnumerable<SelectListItem>? Items { get; set; }

        [HtmlAttributeName("date-role")]
        public string DateRole { get; set; } = "single"; // start | end | single

        [HtmlAttributeName("link-to-start")]
        public string LinkToStart { get; set; } = "";    // e.g. #Start_Date

        [HtmlAttributeName("min-date-today")]
        public bool MinDateToday { get; set; } = false;

        [HtmlAttributeName("max-date-today")]
        public bool MaxDateToday { get; set; } = false;

        [HtmlAttributeName(DictionaryAttributePrefix = "attr-")]
        public IDictionary<string, object?> HtmlAttributes { get; set; }
            = new Dictionary<string, object?>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            output.TagMode = TagMode.StartTagAndEndTag;

            var labelText = Label ?? For.Metadata.DisplayName ?? For.Name;

            var fullName = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
            var id = TagBuilder.CreateSanitizedId(fullName, "_");

            var requiredStar = Required ? "<span class=\"required-star\" style=\"color: red;\">*</span>" : "";

            switch (Type)
            {
                case FormFieldType.Checkbox:
                    output.Content.AppendHtml(GenerateCheckboxHtml(labelText));
                    break;

                case FormFieldType.TextArea:
                    output.Content.AppendHtml(GenerateTextAreaHtml(labelText, fullName, id, requiredStar));
                    break;

                case FormFieldType.CKEditor:
                    output.Content.AppendHtml(GenerateCKEditorHtml(labelText, fullName, id, requiredStar));
                    break;

                case FormFieldType.Select:
                case FormFieldType.MultiSelect:
                    output.Content.AppendHtml(
                        GenerateSelectHtml(labelText, fullName, id, requiredStar)
                    );
                    break;

                case FormFieldType.File:
                    output.Content.AppendHtml(GenerateFileHtml(labelText, fullName, id, requiredStar));
                    break;

                case FormFieldType.Date:
                    output.Content.AppendHtml(GenerateDateHtml(labelText, fullName, id, requiredStar));
                    break;

                default:
                    output.Content.AppendHtml(GenerateInputHtml(labelText, fullName, id, requiredStar));
                    break;
            }

            // IMPORTANT: do NOT append validation message here anymore.
            // It's now rendered inside each field template next to the icon.
        }

        private TagBuilder GenerateValidationTooltipTag(string fullName)
        {
            // Let MVC emit field-validation-error / field-validation-valid based on ModelState.
            var tag = _generator.GenerateValidationMessage(
                ViewContext,
                For.ModelExplorer,
                fullName,
                message: null,
                tag: "span",
                htmlAttributes: null);

            // Keep custom tooltip styling
            tag.AddCssClass("error-tooltip");
            return tag;
        }



        private string GenerateInputHtml(string labelText, string fullName, string id, string requiredStar)
        {
            var inputType = Type switch
            {
                FormFieldType.Number => "number",
                FormFieldType.Email => "email",
                FormFieldType.Password => "password",
                FormFieldType.Url => "url",
                _ => "text"
            };

            var input = GenerateInputTag(inputType, fullName, id);
            var inputHtml = RenderTagBuilder(input);

            var validationTooltipHtml = RenderTagBuilder(GenerateValidationTooltipTag(fullName));

            var valueText = Convert.ToString(For.Model);
            var hasValue = !string.IsNullOrWhiteSpace(valueText);

            var wrapperClass = hasValue ? "inputGroup formgroup filled" : "inputGroup formgroup";

            var passwordToggle = inputType == "password"
    ? $@"<span class=""password-toggle"">
            <i class=""fas fa-eye""></i>
        </span>"
    : "";

            return $@"
<div class=""{wrapperClass}"">
    {inputHtml}
    {passwordToggle}
    <i class=""error-icon fas fa-exclamation-circle""></i>
    {validationTooltipHtml}
    <label for=""{id}"">{labelText} {requiredStar}</label>
</div>";
        }

        private string GenerateTextAreaHtml(string labelText, string fullName, string id, string requiredStar)
        {
            var textarea = GenerateTextAreaTag(fullName, id);
            var textareaHtml = RenderTagBuilder(textarea);

            var validationTooltipHtml = RenderTagBuilder(GenerateValidationTooltipTag(fullName));

            var valueText = Convert.ToString(For.Model);
            var hasValue = !string.IsNullOrWhiteSpace(valueText);
            var wrapperClass = hasValue ? "inputGroup formgroup filled" : "inputGroup formgroup";

            return $@"
<div class=""{wrapperClass}"">
    {textareaHtml}
    <i class=""error-icon fas fa-exclamation-circle""></i>
    {validationTooltipHtml}
    <label for=""{id}"">{labelText} {requiredStar}</label>
</div>";
        }

        private string GenerateSelectHtml(string labelText, string fullName, string id, string requiredStar)
        {
            if (Items == null)
                throw new InvalidOperationException("Items must be provided for Select type.");

            var select = GenerateSelectTag(fullName, id);
            var selectHtml = RenderTagBuilder(select);
            var validationTooltipHtml = RenderTagBuilder(GenerateValidationTooltipTag(fullName));

            var hasValue = Multiple
                ? For.Model is IEnumerable<string> values && values.Any(v => !string.IsNullOrWhiteSpace(v))
                : !string.IsNullOrWhiteSpace(Convert.ToString(For.Model));

            var wrapperClass = hasValue ? "inputGroup formgroup filled" : "inputGroup formgroup";

            return $@"
<div class=""{wrapperClass}"">
    {selectHtml}
    <i class=""error-icon fas fa-exclamation-circle""></i>
    {validationTooltipHtml}
    <label for=""{id}"">{labelText} {requiredStar}</label>
<span class=""clear-select"" style=""display: none;"">✕</span>
</div>";
        }

        private string GenerateCKEditorHtml(string labelText, string fullName, string id, string requiredStar)
        {
            var textarea = GenerateTextAreaTag(fullName, id);
            textarea.AddCssClass("ckeditor-textarea");
            var textareaHtml = RenderTagBuilder(textarea);

            var validationTooltipHtml = RenderTagBuilder(GenerateValidationTooltipTag(fullName));

            return $@"
<div class=""ckeditor-wrapper"">
    <label for=""{id}"">{labelText} {requiredStar}</label>
    <i class=""error-icon fas fa-exclamation-circle""></i>
    {validationTooltipHtml}
    {textareaHtml}
</div>";
        }

        //        private string GenerateSelectHtml(string labelText, string fullName, string id, string requiredStar)
        //        {
        //            if (Items == null)
        //                throw new InvalidOperationException("Items must be provided for Select type.");

        //            var select = GenerateSelectTag(fullName, id);
        //            var selectHtml = RenderTagBuilder(select);
        //            var validationTooltipHtml = RenderTagBuilder(GenerateValidationTooltipTag(fullName));

        //            return $@"
        //<div class=""inputGroup formgroup"">
        //    {selectHtml}
        //    <i class=""error-icon fas fa-exclamation-circle""></i>
        //    {validationTooltipHtml}
        //    <label for=""{id}"">{labelText} {requiredStar}</label>
        //<span class=""clear-select"" style=""display: none;"">✕</span>
        //</div>";
        //        }

        private string GenerateDateHtml(string labelText, string fullName, string id, string requiredStar)
        {
            var input = GenerateDateInputTag(fullName, id);
            var inputHtml = RenderTagBuilder(input);
            var validationTooltipHtml = RenderTagBuilder(GenerateValidationTooltipTag(fullName));

            var valueText = Convert.ToString(For.Model);
            var hasValue = !string.IsNullOrWhiteSpace(valueText);
            var wrapperClass = hasValue ? "inputGroup formgroup floating-group datetimepicker filled" : "inputGroup formgroup floating-group datetimepicker";

            return $@"
<div class=""{wrapperClass}""> 
    {inputHtml}   
    <i class=""error-icon fas fa-exclamation-circle""></i>
    {validationTooltipHtml}
<label class=""floating-label"" style="" padding: 0 15px !important; "" for=""{id}"" >{labelText} {requiredStar}</label>
</div>";
        }

        private TagBuilder GenerateDateInputTag(string fullName, string id)
        {
            string? formattedValue = null;
            if (For.Model is DateTime dateValue && dateValue != default)
            {
                formattedValue = dateValue.ToString("dd-MM-yyyy");
            }
            else if (For.Model is DateOnly dateOnlyValue && dateOnlyValue != default)
            {
                formattedValue = dateOnlyValue.ToString("dd-MM-yyyy");
            }

            var hasValueClass = string.IsNullOrWhiteSpace(formattedValue) ? "" : " has-value";

            var normalizedDateRole = string.IsNullOrWhiteSpace(DateRole)
                ? "single"
                : DateRole.Trim().ToLowerInvariant();

            if (normalizedDateRole is not ("single" or "start" or "end"))
                normalizedDateRole = "single";

            var attrs = new Dictionary<string, object?>
            {
                ["type"] = "text",
                ["autocomplete"] = "off",
                ["readonly"] = "readonly",
                ["class"] = $"form-control floating-input formgroup{hasValueClass}",
                ["placeholder"] = " ",
                ["name"] = fullName,
                ["id"] = id,
                ["data-date-role"] = normalizedDateRole,
                ["data-link-to-start"] = LinkToStart ?? string.Empty,
                ["data-min-date-today"] = MinDateToday ? "true" : "false",
                ["data-max-date-today"] = MaxDateToday ? "true" : "false"
            };

            if (Required)
            {
                attrs["required"] = "required";
                attrs["data-val"] = "true";
                attrs["data-val-required"] = $"{(Label ?? For.Metadata.DisplayName ?? For.Name)} is required.";
            }

            MergeAttributes(attrs);

            return _generator.GenerateTextBox(
                ViewContext,
                For.ModelExplorer,
                fullName,
                formattedValue ?? string.Empty,
                format: null,
                htmlAttributes: attrs
            );
        }

        private string GenerateCheckboxHtml(string labelText)
        {
            var checkbox = GenerateCheckboxTag();
            var checkboxHtml = RenderTagBuilder(checkbox);

            return $@"
<label class=""CheckBoxcontainer"">
    {checkboxHtml}
    <svg viewBox=""0 0 64 64"" height=""1em"" width=""1em"">
        <path d=""M 0 16 V 56 A 8 8 90 0 0 8 64 H 56 A 8 8 90 0 0 64 56 V 8 A 8 8 90 0 0 56 0 H 8 A 8 8 90 0 0 0 8 V 16 L 32 48 L 64 16 V 8 A 8 8 90 0 0 56 0 H 8 A 8 8 90 0 0 0 8 V 56 A 8 8 90 0 0 8 64 H 56 A 8 8 90 0 0 64 56 V 16"" pathLength=""575.0541381835938"" class=""path""></path>
    </svg>
    {labelText}
</label>";
        }

        private TagBuilder GenerateFileTag(string fullName, string id)
        {
            var attrs = new Dictionary<string, object?>
            {
                ["type"] = "file",
                ["class"] = "upload-file",
                ["name"] = fullName,
                ["id"] = id
            };

            if (!string.IsNullOrWhiteSpace(Accept))
            {
                attrs["accept"] = Accept;
            }

            if (Multiple)
            {
                attrs["multiple"] = "multiple";
            }

            if (Required)
            {
                attrs["required"] = "required";
                attrs["data-val"] = "true";
                attrs["data-val-required"] = $"{(Label ?? For.Metadata.DisplayName ?? For.Name)} is required.";
            }

            MergeAttributes(attrs);

            return _generator.GenerateTextBox(
                ViewContext,
                For.ModelExplorer,
                fullName,
                value: null,
                format: null,
                htmlAttributes: attrs
            );
        }

        //Updated by : Krishna (03-04-2026)
        private string GenerateFileHtml(string labelText, string fullName, string id, string requiredStar)
        {
            var file = GenerateFileTag(fullName, id);
            var fileHtml = RenderTagBuilder(file);

            var validationTooltipHtml = RenderTagBuilder(GenerateValidationTooltipTag(fullName));

            var viewLinkHtml = "";
            if (!string.IsNullOrWhiteSpace(ViewLinkPath))
            {
                var urlHelperFactory = ViewContext.HttpContext.RequestServices
                    .GetRequiredService<IUrlHelperFactory>();
                var urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
                var href = urlHelper.Content(ViewLinkPath);

                // CHANGED: class file-view-link (not file-upload-name)
                viewLinkHtml = $@"<a data-src=""{href}"" class=""file-view-link file-upload-name"" 
                style=""margin-bottom: 0px; 
                position: absolute;                
                bottom: 75%; 
                right: 3%; 
                max-width: 70%;
    margin-bottom: 0;
    white-space: nowrap;
    overflow: hidden;
                text-overflow: ellipsis;
                cursor: pointer""
                data-bs-toggle=""tooltip"">view document</a>";
            }

            return $@"
    <div class=""upload-wrapper"">
        {fileHtml}
        <i class=""error-icon fas fa-exclamation-circle""></i>
        {validationTooltipHtml}
        <svg version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" preserveAspectRatio=""xMidYMid meet"" viewBox=""224.3881704980842 176.8527621722847 221.13266283524905 178.8472378277154"" width=""221.13"" height=""178.85"">
            <defs>
                <path class=""svg-color"" d=""M357.38 176.85C386.18 176.85 409.53 204.24 409.53 238.02C409.53 239.29 409.5 240.56 409.42 241.81C430.23 246.95 445.52 264.16 445.52 284.59C445.52 284.59 445.52 284.59 445.52 284.59C445.52 309.08 423.56 328.94 396.47 328.94C384.17 328.94 285.74 328.94 273.44 328.94C246.35 328.94 224.39 309.08 224.39 284.59C224.39 284.59 224.39 284.59 224.39 284.59C224.39 263.24 241.08 245.41 263.31 241.2C265.3 218.05 281.96 199.98 302.22 199.98C306.67 199.98 310.94 200.85 314.93 202.46C324.4 186.96 339.88 176.85 357.38 176.85Z"" id=""b1aO7LLtdW""></path>
                <path d=""M306.46 297.6L339.79 297.6L373.13 297.6L339.79 255.94L306.46 297.6Z"" id=""c4SXvvMdYD""></path>
                <path d=""M350.79 293.05L328.79 293.05L328.79 355.7L350.79 355.7L350.79 293.05Z"" id=""b11si2zUk""></path>
            </defs>
            <g>
                <g>
                    <g>
                        <use xlink:href=""#b1aO7LLtdW"" opacity=""1"" class=""svg-color"" fill-opacity=""1""></use>
                    </g>
                    <g>
                        <g>
                            <use xlink:href=""#c4SXvvMdYD"" opacity=""1"" class=""icon-svg-color"" fill-opacity=""1""></use>
                        </g>
                        <g>
                            <use xlink:href=""#b11si2zUk"" opacity=""1"" class=""icon-svg-color"" fill-opacity=""1""></use>
                        </g>
                    </g>
                </g>
            </g>
        </svg>
        <span class=""file-upload-text"">{labelText} {requiredStar}</span>
        <div class=""file-success-text"">
            <svg viewBox=""0 0 100 100"">
                <circle class=""sucesssvg-fill"" style=""stroke:#ffffff;stroke-width:10;stroke-miterlimit:10;"" cx=""49.799"" cy=""49.746"" r=""44.757""/>
                <polyline style=""fill:rgba(0,0,0,0);stroke:#ffffff;stroke-width:10;stroke-linecap:round;stroke-linejoin:round;"" points=""27.114,51 41.402,65.288 72.485,34.205""/>
            </svg>
            <span>Successfully Uploaded</span>
        </div>
    </div>
    {viewLinkHtml}
   
<p class=""file-upload-name-text"" style=""margin-bottom:0;position:absolute;bottom:70%;right:3%;max-width:70%;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;cursor:pointer"" data-bs-toggle=""tooltip""></p>";
        }


        private TagBuilder GenerateInputTag(string type, string fullName, string id)
        {
            var attrs = new Dictionary<string, object?>
            {
                ["type"] = type,
                ["autocomplete"] = "off",
                ["name"] = fullName,
                ["id"] = id,
                ["placeholder"] = string.IsNullOrWhiteSpace(Placeholder) ? " " : Placeholder
            };

            if (Required)
            {
                attrs["required"] = "required";
                attrs["data-val"] = "true";
                attrs["data-val-required"] = $"{(Label ?? For.Metadata.DisplayName ?? For.Name)} is required.";
            }

            if (type == "url")
            {
                attrs["data-val"] = "true";
                attrs["data-val-url"] = "Please enter a valid URL.";
            }

            if (type == "number")
            {
                attrs["onkeydown"] = "return !['e','E','+','-'].includes(event.key)";
            }

            MergeAttributes(attrs);

            return _generator.GenerateTextBox(
                ViewContext,
                For.ModelExplorer,
                fullName,
                For.Model,
                format: null,
                htmlAttributes: attrs
            );
        }

        private TagBuilder GenerateTextAreaTag(string fullName, string id)
        {
            var attrs = new Dictionary<string, object?>
            {
                ["autocomplete"] = "off",
                ["name"] = fullName,
                ["id"] = id,
                ["placeholder"] = string.IsNullOrWhiteSpace(Placeholder) ? " " : Placeholder
            };

            if (Required)
            {
                attrs["required"] = "required";
                attrs["data-val"] = "true";
                attrs["data-val-required"] = $"{(Label ?? For.Metadata.DisplayName ?? For.Name)} is required.";
            }

            MergeAttributes(attrs);

            return _generator.GenerateTextArea(
                ViewContext,
                For.ModelExplorer,
                fullName,
                rows: 2,
                columns: 0,
                htmlAttributes: attrs
            );
        }

        private TagBuilder GenerateSelectTag(string fullName, string id)
        {
            var attrs = new Dictionary<string, object?>
            {
                ["id"] = id
            };

            if (Required)
            {
                attrs["required"] = "required";
                attrs["data-val"] = "true";
                attrs["data-val-required"] = $"{(Label ?? For.Metadata.DisplayName ?? For.Name)} is required.";
            }

            if (Searchable)
                attrs["class"] = Multiple
                    ? "form-select2 searchable-select js-multi-select"
                    : "form-select2 searchable-select js-single-select";

            if (Multiple)
                attrs["multiple"] = "multiple";

            MergeAttributes(attrs);

            var select = new TagBuilder("select");

            select.Attributes["name"] = fullName;
            select.Attributes["id"] = id;

            foreach (var kv in attrs)
                select.Attributes[kv.Key] = Convert.ToString(kv.Value) ?? "";

            var currentValue = Convert.ToString(For.Model);
            var hasModelValue = !string.IsNullOrWhiteSpace(currentValue);

            // Keep control empty initially (hidden, not shown in dropdown list)
            if (!Multiple)
            {
                var emptyOption = new TagBuilder("option");
                emptyOption.Attributes["value"] = "";
                emptyOption.Attributes["hidden"] = "hidden";

                if (!hasModelValue)
                    emptyOption.Attributes["selected"] = "selected";

                select.InnerHtml.AppendHtml(emptyOption);
            }

            if (Items != null)
            {
                var modelValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                if (Multiple && For.Model is IEnumerable<string> values)
                    modelValues = values.Where(v => !string.IsNullOrWhiteSpace(v)).ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var item in Items)
                {
                    var opt = new TagBuilder("option");
                    var value = item.Value ?? item.Text ?? "";

                    opt.Attributes["value"] = value;

                    if (item.Disabled)
                        opt.Attributes["disabled"] = "disabled";

                    
                    var isSelected =
                        Multiple
                            ? modelValues.Contains(value)
                            : hasModelValue && string.Equals(currentValue, value, StringComparison.OrdinalIgnoreCase);

                    if (isSelected)
                        opt.Attributes["selected"] = "selected";

                    opt.InnerHtml.Append(item.Text ?? "");
                    select.InnerHtml.AppendHtml(opt);
                }
            }

            return select;
        }

        /*private TagBuilder GenerateFileTag()
        {
            var attrs = new Dictionary<string, object?>
            {
                ["type"] = "file",
                ["class"] = "upload-file"
            };


            if (!string.IsNullOrWhiteSpace(Accept))
            {
                attrs["accept"] = Accept;
            }

            if (Required)
            {
                attrs["required"] = "required";
                attrs["data-val"] = "true";
                attrs["data-val-required"] = "Required!";
            }

            MergeAttributes(attrs);

            return _generator.GenerateTextBox(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                value: null,
                format: null,
                htmlAttributes: attrs
            );
        }*/

        private TagBuilder GenerateCheckboxTag()
        {
            var attrs = new Dictionary<string, object?>();
            MergeAttributes(attrs);

            return _generator.GenerateCheckBox(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                isChecked: (bool?)For.Model,
                htmlAttributes: attrs
            );
        }

        private void MergeAttributes(IDictionary<string, object?> target)
        {
            foreach (var attr in HtmlAttributes)
            {
                if (target.ContainsKey(attr.Key))
                {
                    if (attr.Key == "class")
                        target[attr.Key] = $"{target[attr.Key]} {attr.Value}";
                }
                else
                {
                    target[attr.Key] = attr.Value;
                }
            }
        }

        private static string RenderTagBuilder(TagBuilder tagBuilder)
        {
            using var writer = new StringWriter();
            tagBuilder.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}