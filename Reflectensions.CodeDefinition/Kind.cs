using System.Runtime.Serialization;

namespace doob.Reflectensions.CodeDefinition
{
    public enum Kind
    {
        [EnumMember(Value = "")]
        None = 0,

        [EnumMember(Value = "class")]
        Class = 1,

        [EnumMember(Value = "struct")]
        Struct = 2,

        [EnumMember(Value = "interface")]
        Interface = 4,

        [EnumMember(Value = "enum")]
        Enum = 8
    }
}
