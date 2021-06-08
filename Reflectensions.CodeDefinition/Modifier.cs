using System.Runtime.Serialization;

namespace doob.Reflectensions.CodeDefinition
{
    public enum Modifier
    {
        [EnumMember(Value = "")]
        None = 0,

        [EnumMember(Value = "public")]
        Public = 1,

        [EnumMember(Value = "protected internal")]
        ProtectedInternal = 2,

        [EnumMember(Value = "protected")]
        Protected = 4,

        [EnumMember(Value = "internal")]
        Internal = 8,

        [EnumMember(Value = "private protected")]
        PrivateProtected = 16,

        [EnumMember(Value = "private")]
        Private = 32,
        
    }
}
