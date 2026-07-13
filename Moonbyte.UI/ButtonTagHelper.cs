
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Moonbyte.UI
{
    [HtmlTargetElement("form-button")]
    public sealed class ButtonTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = default!;

        [HtmlAttributeName("label")]
        public string Label { get; set; } = "Button";

        [HtmlAttributeName("type")]
        public ButtonType Type { get; set; } = ButtonType.Submit;

        [HtmlAttributeName("url")]
        public string? Url { get; set; }

        [HtmlAttributeName("id")]
        public string? Id { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            output.TagMode = TagMode.StartTagAndEndTag;

            var html = Type switch
            {
                ButtonType.Submit => GenerateSubmitButton(),
                ButtonType.Cancel => GenerateCancelButton(),
                ButtonType.Add => GenerateAddButton(),
                _ => GenerateSubmitButton()
            };

            output.Content.AppendHtml(html);
        }

        private string GetIdAttribute()
        {
            return string.IsNullOrWhiteSpace(Id)
                ? string.Empty
                : $@" id=""{Id}""";
        }

        private string GenerateSubmitButton()
        {
            return $@"
<button type=""submit""
        data-form-button=""submit""
        {GetIdAttribute()}
        class=""btn btn-primary btn-round theme-submit-button"">
    {Label}
</button>";
        }

        private string GenerateCancelButton()
        {
            var href = Url ?? "#";

            return $@"
<a href=""{href}"" type=""button""{GetIdAttribute()} class=""btn btn-round theme-cancel-button"">
    {Label}
</a>";
        }

        private string GenerateAddButton()
        {
            var href = Url ?? "#";

            return $@"
<a href=""{href}"" type=""button""{GetIdAttribute()} class=""btn btn-primary btn-round theme-add-button"">
    {Label}
</a>";
        }
    }

    public enum ButtonType
    {
        Submit,
        Cancel,
        Add
    }
}


