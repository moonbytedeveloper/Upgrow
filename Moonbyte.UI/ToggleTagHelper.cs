using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Moonbyte.UI
{  

    /// <summary>
    /// Renders a reusable toggle switch for IsActive status in DataTables
    /// Usage: <toggle-switch uuid="@dto.UUID" is-active="@dto.IsActive" toggle-url="/Admin/Master/ToggleGender" entity-name="gender" />
    /// </summary>
    [HtmlTargetElement("toggle-switch")]
    public sealed class ToggleTagHelper : TagHelper
    {
        [HtmlAttributeName("uuid")]
        public string UUID { get; set; } = string.Empty;

        [HtmlAttributeName("is-active")]
        public bool IsActive { get; set; }

        [HtmlAttributeName("toggle-url")]
        public string ToggleUrl { get; set; } = string.Empty;

        [HtmlAttributeName("entity-name")]
        public string EntityName { get; set; } = "item";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            output.TagMode = TagMode.StartTagAndEndTag;

            var checkedAttr = IsActive ? "checked" : "";

            var html = $@"
<div class=""form-switch-success d-flex"">
    <div id=""button-3"" class=""button r"">
        <input type=""checkbox"" 
               class=""checkbox customcheckbox toggle-status"" 
               data-uuid=""{UUID}"" 
               data-toggle-url=""{ToggleUrl}""
               data-entity-name=""{EntityName}"" 
               {checkedAttr}>
        <div class=""knobs""></div>
        <div class=""layer""></div>
    </div>
</div>";

            output.Content.AppendHtml(html);
        }

        /// <summary>
        /// Generates toggle HTML for use in DataTable column mapping (server-side rendering)
        /// </summary>
        public static string GenerateHtml(string uuid, bool isActive, string toggleUrl, string entityName = "item")
        {
            var checkedAttr = isActive ? "checked" : "";

            return $@"<div class=""form-switch-success d-flex"">
    <div id=""button-3"" class=""button r"">
        <input type=""checkbox"" 
               class=""checkbox customcheckbox toggle-status"" 
               data-uuid=""{uuid}"" 
               data-toggle-url=""{toggleUrl}""
               data-entity-name=""{entityName}"" 
               {checkedAttr}>
        <div class=""knobs""></div>
        <div class=""layer""></div>
    </div>
</div>";
        }
    }
}

