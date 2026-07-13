using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VerifyIndia.Infrastructure.Common
{
    public class DateOnlyJsonConverter
        : JsonConverter<DateOnly>
    {
        // =====================================
        // SUPPORTED FORMATS
        // =====================================

        private static readonly string[]
            FORMATS =
            {
                "yyyy-MM-dd",
                "dd-MM-yyyy"
            };

        // =====================================
        // READ
        // =====================================

        public override DateOnly Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var value =
                reader.GetString();

            if (DateOnly.TryParseExact(
                    value,
                    FORMATS,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
            {
                return date;
            }

            throw new FormatException(
                $"Invalid DateOnly format: {value}");
        }

        // =====================================
        // WRITE
        // =====================================

        public override void Write(
            Utf8JsonWriter writer,
            DateOnly value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(
                value.ToString("yyyy-MM-dd"));
        }
    }
}
