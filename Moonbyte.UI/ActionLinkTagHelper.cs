using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonbyte.UI
{
    [HtmlTargetElement("action-link")]
    public sealed class ActionLinkTagHelper : TagHelper
    {        
            [HtmlAttributeName("id")]
            public string Id { get; set; } = "actionLink";

            [HtmlAttributeName("label")]
            public string Label { get; set; } = "Action";

            [HtmlAttributeName("type")]
            public ActionLinkType Type { get; set; } = ActionLinkType.Edit;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var isDisplay = true;
            output.TagName = null;
            output.TagMode = TagMode.StartTagAndEndTag;

            var html = string.Empty;
            if (isDisplay)
            {
                html = $@"<li><a href=""javascript:void(0)"" id=""{Id}"" data-link-type=""{Type.ToString().ToLowerInvariant()}"">
    {Label}
</a></li>"; 
            }

            output.Content.AppendHtml(html);
            }
        }
    }
