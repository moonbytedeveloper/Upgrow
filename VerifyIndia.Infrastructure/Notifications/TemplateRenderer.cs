using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Interfaces.Notification;

namespace VerifyIndia.Infrastructure.Notifications;

public sealed class TemplateRenderer
    : ITemplateRenderer
{
    public string Render(
        string template,
        IReadOnlyDictionary<string, object> variables)
    {
        if (string.IsNullOrWhiteSpace(template))
        {
            return template;
        }

        foreach (var variable in variables)
        {
            var placeholder = "{{" + variable.Key + "}}";

            template = template.Replace(
                placeholder,
                variable.Value?.ToString() ?? string.Empty,
                StringComparison.OrdinalIgnoreCase);
        }

        return template;
    }
}
