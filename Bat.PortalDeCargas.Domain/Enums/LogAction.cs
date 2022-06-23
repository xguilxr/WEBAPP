using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bat.PortalDeCargas.Domain.Enums
{
    public class LogAction : Enumeration<LogAction>
    {
        public static readonly LogAction Create = new LogAction(1, "Create");
        public static readonly LogAction Update = new LogAction(2, "Update");
        public static readonly LogAction Delete = new LogAction(3, "Delete");

        private LogAction(int value, string name) : base(value, name)
        { }
        public class LogActionEnumerationConverter : JsonConverter<LogAction>
        {
            public override LogAction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TryGetInt32(out var value))
                {
                    return (LogAction)value;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }

            public override void Write(Utf8JsonWriter writer, LogAction value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteString("name", value);
                writer.WriteNumber("value", (int)value);
                writer.WriteEndObject();
            }
        }
    }

    // public enum LogAction
    // {
    //     Create = 1,
    //     Update = 2,
    //     Delete = 3
    // }
}
