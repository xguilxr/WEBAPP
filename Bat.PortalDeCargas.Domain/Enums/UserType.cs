using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bat.PortalDeCargas.Domain.Enums
{
    public enum UserType
    {
        Administrador =1,
        Regular
    }

    public static class UserTypeExtension
    {
        public static string RoleName(this UserType userType)
        {
            switch (userType)
            {
                case UserType.Administrador:
                    return "admin";
                case UserType.Regular:
                    return "regular";
                default:
                    throw new ArgumentOutOfRangeException(nameof(userType), userType, null);
            }
        }
    }
    /*
    public class UserType : Enumeration<UserType>
    {
        static UserType()
        {
            Administrador = new UserType(1, "Administrador", "admin");
            Regular = new UserType(2, "Regular", "regular");
        }
        
        public static UserType Administrador;// = new UserType(1, "Administrador", "admin");
        public static UserType Regular;// = new UserType(2, "Regular", "regular");

        private UserType(int value, string name, string roleName) : base(value, name)
        {
            RoleName = roleName;
        }

        public string RoleName { get; }

        public class UserTypeEnumerationConverter : JsonConverter<UserType>
        {
            public override UserType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TryGetInt32(out var value))
                {
                    return (UserType)value;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }

            public override void Write(Utf8JsonWriter writer, UserType value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteString("name", value);
                writer.WriteNumber("value", (int)value);
                writer.WriteEndObject();
            }
        }
    }
*/
}
