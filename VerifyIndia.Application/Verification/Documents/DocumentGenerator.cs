using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Interfaces.Notification;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Verification.Documents
{
    public class DocumentGenerator
        : IDocumentGenerator
    {
        private readonly ITemplateRepository
            _templateRepository;

        private readonly ITemplateRenderer
            _templateRenderer;

        private readonly IHtmlToPdfConverter
            _htmlToPdfConverter;

        public DocumentGenerator(
            ITemplateRepository templateRepository,
            ITemplateRenderer templateRenderer,
            IHtmlToPdfConverter htmlToPdfConverter)
        {
            _templateRepository =
                templateRepository;

            _templateRenderer =
                templateRenderer;

            _htmlToPdfConverter =
                htmlToPdfConverter;
        }

        public async Task<string> GenerateBase64Async(
            DocumentGenerationContext context,
            CancellationToken cancellationToken)
        {
            var template =
                await _templateRepository
                    .GetTemplateAsync(
                        context.VerificationCode,
                        cancellationToken);

            var variables =
                BuildVariables(
                    context.ProviderResponse);

            foreach (var item in context.AdditionalVariables)
            {
                variables[item.Key] =
                    item.Value ?? string.Empty;
            }

            var html =
                _templateRenderer
                    .Render(
                        template,
                        variables);

            var pdf =
                await _htmlToPdfConverter
                    .ConvertAsync(
                        html,
                        cancellationToken);

            return Convert.ToBase64String(
                pdf);
        }

        private static Dictionary<string, object> BuildVariables(
            object model)
        {
            var variables =
                new Dictionary<string, object>(
                    StringComparer.OrdinalIgnoreCase);

            AddProperties(
                variables,
                model,
                null);

            return variables;
        }

        private static void AddProperties(
            IDictionary<string, object> variables,
            object? value,
            string? prefix)
        {
            if (value == null)
            {
                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    variables[prefix] =
                        string.Empty;
                }

                return;
            }

            var type =
                value.GetType();

            if (IsSimpleType(type))
            {
                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    variables[prefix] =
                        value;
                }

                return;
            }

            foreach (var property in type.GetProperties(
                         BindingFlags.Public |
                         BindingFlags.Instance))
            {
                var propertyValue =
                    property.GetValue(value);

                var propertyName =
                    string.IsNullOrWhiteSpace(prefix)
                        ? property.Name
                        : $"{prefix}.{property.Name}";

                AddProperties(
                    variables,
                    propertyValue,
                    propertyName);
            }
        }

        private static bool IsSimpleType(
            Type type)
        {
            type =
                Nullable.GetUnderlyingType(type)
                ?? type;

            return
                type.IsPrimitive
                || type.IsEnum
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                || type == typeof(TimeSpan)
                || type == typeof(Guid);
        }
    }
}