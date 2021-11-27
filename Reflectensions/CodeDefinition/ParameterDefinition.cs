namespace doob.Reflectensions.CodeDefinition
{
    public class ParameterSignature
    {
        public string? Name { get; set; }
        public TypeSignature Type { get; set; }
        public bool IsOut { get; set; }
        public bool ByRef { get; set; }
        public bool IsOptional { get; set; }
        public object? DefaultValue { get; set; }

        public ParameterSignature(ParameterInfo parameterInfo)
        {
            Name = parameterInfo.Name;
            Type = TypeSignature.Create(parameterInfo.ParameterType);
            IsOut = parameterInfo.IsOut;
            ByRef = parameterInfo.ParameterType.IsByRef;
            IsOptional = parameterInfo.IsOptional;
            DefaultValue = parameterInfo.DefaultValue;

        }

        public ParameterSignature(string parameterSignature)
        {
            var regex = new Regex(@"((?<ref>[a-z]+)?\s)?(?<type>.+)\s(?<name>[a-zA-Z0-9_\.]+)");

            var match = regex.Match(parameterSignature);

            if (!match.Success)
                throw new Exception($"can't parse '{parameterSignature}' to TypeDefinition");


            Name = match.Groups["name"].Value;
            
            Type = TypeSignature.Parse(match.Groups["type"].Value.Trim());

            var reference = match.Groups["ref"].Value.TrimToNull()?.ToLower();
            if (!String.IsNullOrWhiteSpace(reference))
            {
                
                if (reference == "out")
                {
                    Type.IsByRef = true;
                    IsOut = true;
                }
                else if (reference == "ref")
                {
                    Type.IsByRef = true;
                }
            }
        }

        public static ParameterSignature Create(ParameterInfo parameterInfo)
        {
            return new ParameterSignature(parameterInfo);
        }

        public static ParameterSignature Parse(string parameterSignature)
        {
            return new ParameterSignature(parameterSignature);
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
