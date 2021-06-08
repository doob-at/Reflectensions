using System;
using System.Reflection;
using System.Text.RegularExpressions;
using doob.Reflectensions.CodeDefinition.ExtensionMethods;

namespace doob.Reflectensions.CodeDefinition.Definitions
{
    public class ParameterDefinition
    {
        public string Name { get; set; }
        public TypeDefinition Type { get; set; }
        public bool IsOut { get; set; }
        public bool ByRef { get; set; }
        public bool IsOptional { get; set; }
        public object DefaultValue { get; set; }

        public static ParameterDefinition Create(ParameterInfo parameterInfo)
        {
            var pDefinition = new ParameterDefinition();
            pDefinition.Name = parameterInfo.Name;
            pDefinition.Type = TypeDefinition.Create(parameterInfo.ParameterType);
            pDefinition.IsOut = parameterInfo.IsOut;
            pDefinition.ByRef = parameterInfo.ParameterType.IsByRef;
            pDefinition.IsOptional = parameterInfo.IsOptional;
            pDefinition.DefaultValue = parameterInfo.DefaultValue;
            return pDefinition;
        }

        public static ParameterDefinition Parse(string definitionString)
        {
            var regex = new Regex(@"((?<ref>[a-z]+)?\s)?(?<type>.+)\s(?<name>[a-zA-Z0-9_\.]+)");

            var match = regex.Match(definitionString);

            if (!match.Success)
                throw new Exception($"can't parse '{definitionString}' to TypeDefinition");

            var pd = new ParameterDefinition();

            pd.Name = match.Groups["name"].Value;
            
            pd.Type = TypeDefinition.Parse(match.Groups["type"].Value.TrimToNull());

            var reference = match.Groups["ref"].Value.TrimToNull()?.ToLower();
            if (!String.IsNullOrWhiteSpace(reference))
            {
                
                if (reference == "out")
                {
                    pd.Type.IsByRef = true;
                    pd.IsOut = true;
                }
                else if (reference == "ref")
                {
                    pd.Type.IsByRef = true;
                }
            }

            return pd;
        }
        

        public override string ToString()
        {
            var refText = ByRef ? IsOut ? "out" : "ref" : null;
            return $"{refText} {Type} {Name}".Trim();
        }

        public string AsMethodParamterString() {
            var refText = ByRef ? IsOut ? "out " : "ref " : "";

            
            if (IsOptional) {
                var NullableText = Type.IsNullable ? "?" : null;
                if (DefaultValue == null) {
                    return $"{refText}{Type}{NullableText} {Name} = null";
                } else {
                    return $"{refText}{Type}{NullableText} {Name} = {DefaultValue}";
                }
            } 
            
            return $"{refText}{Type} {Name}";
        }
    }
}
