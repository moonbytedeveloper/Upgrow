using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Moonbyte.UI;

  [HtmlTargetElement("dependent-dropdown", Attributes = "asp-for,key")]
public sealed class DependentDropdownTagHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;

        public DependentDropdownTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; } = default!;

        /// <summary>
        /// Dropdown key registered in DropdownRegistry
        /// Example: State, DistrictByState
        /// </summary>
        [HtmlAttributeName("key")]
        public string Key { get; set; } = default!;

        /// <summary>
        /// Parent dropdown semantic name (NOT id)
        /// Example: State
        /// </summary>
        [HtmlAttributeName("parent")]
        public string? Parent { get; set; }

        /// <summary>
        /// Label text (optional)
        /// </summary>
        public string? Label { get; set; }

        public bool Required { get; set; }      

        [HtmlAttributeName("loading-text")]
        public string LoadingText { get; set; } = "Loading...";

        [HtmlAttributeName("searchable")]
        public bool Searchable { get; set; } = true;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;

        var fullName = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
        var id = TagBuilder.CreateSanitizedId(fullName, "_");

        var labelText = Label ?? For.Metadata.DisplayName ?? For.Name;
        var requiredStar = Required ? "<span class=\"required-star\" style=\"color:red;\">*</span>" : "";

        var selectClass = Searchable
            ? "form-select2 searchable-select js-single-select dependent-dropdown"
            : "form-select dependent-dropdown";

        var existingValue = Convert.ToString(For.Model);
        var hasExistingValue = !string.IsNullOrWhiteSpace(existingValue);
        var wrapperClass = hasExistingValue ? "inputGroup formgroup filled" : "inputGroup formgroup";

        var select = new TagBuilder("select");
        select.Attributes["id"] = id;
        select.Attributes["name"] = fullName;
        select.Attributes["data-dd"] = string.IsNullOrWhiteSpace(Parent) ? Key : $"{Key}({Parent})";
        select.Attributes["data-loading"] = LoadingText;
        // IMPORTANT: intentionally not setting data-placeholder => keep visually empty
        select.AddCssClass(selectClass);

        if (Required)
        {
            select.Attributes["required"] = "required";
            select.Attributes["data-val"] = "true";
            select.Attributes["data-val-required"] = $"{labelText} is required.";
        }

        var emptyOption = new TagBuilder("option");
        emptyOption.Attributes["value"] = "";
        emptyOption.Attributes["hidden"] = "hidden";
        if (!hasExistingValue)
            emptyOption.Attributes["selected"] = "selected";

        select.InnerHtml.AppendHtml(emptyOption);

        if (hasExistingValue)
            select.Attributes["data-value"] = existingValue;

        var selectHtml = RenderTag(select);

        var validation = _generator.GenerateValidationMessage(
            ViewContext,
            For.ModelExplorer,
            fullName,
            message: null,
            tag: "span",
            htmlAttributes: new { @class = "error-tooltip field-validation-valid" });

        var validationHtml = RenderTag(validation);

        output.Content.SetHtmlContent(
            $@"<div class=""{wrapperClass}"">{selectHtml}<i class=""error-icon fas fa-exclamation-circle""></i>{validationHtml}<label for=""{id}"">{labelText} {requiredStar}</label></div>");
    }

    private static string RenderTag(TagBuilder tag)
    {
        using var writer = new StringWriter();
        tag.WriteTo(writer, HtmlEncoder.Default);
        return writer.ToString();
    }
}

