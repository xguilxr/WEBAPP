using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Services.Files;

namespace Bat.PortalDeCargas.Domain.Enums
{
    public class FileType : Enumeration<FileType>
    {
        public static FileType Csv = new FileType(3, ".cvs", "text/plain", () => new ExcelTemplateGenerator());

        public static FileType Xls = new FileType(2, ".xls", "application/vnd.ms-excel",
            () => new ExcelTemplateGenerator());

        public static FileType Xlsx = new FileType(1, ".xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", () => new ExcelTemplateGenerator());

        private readonly Lazy<IFileGeneratorService<TemplateDimension>> _fileGenerator;

        private FileType
            (int value, string name, string contentType, Func<IFileGeneratorService<TemplateDimension>> factory) : base(
            value, name)
        {
            ContentType = contentType;
            _fileGenerator = new Lazy<IFileGeneratorService<TemplateDimension>>(factory);
        }

        public string ContentType { get; }
        public IFileGeneratorService<TemplateDimension> FileGenerator => _fileGenerator.Value;

        public class FileTypeEnumerationConverter : JsonConverter<FileType>
        {
            public override FileType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TryGetInt32(out var value))
                {
                    return (FileType)value;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }

            public override void Write(Utf8JsonWriter writer, FileType value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteString("name", value);
                writer.WriteNumber("value", (int)value);
                writer.WriteEndObject();
            }
        }
    }
}
