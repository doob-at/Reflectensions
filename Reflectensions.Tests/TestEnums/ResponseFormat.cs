using System.Runtime.Serialization;

namespace Reflectensions.Tests.TestEnums
{
    public enum ResponseFormat {
        [EnumMember(Value = "*/*")]
        Unknown,

        [EnumMember(Value = "application/json")]
        Json,

        [EnumMember(Value = "application/xml")]
        Xml,

        [EnumMember(Value = "text/plain")]
        Text,

    }
}