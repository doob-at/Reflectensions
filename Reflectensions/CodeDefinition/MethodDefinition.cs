namespace doob.Reflectensions.CodeDefinition
{
    public class MethodSignature
    {
        public Modifier? Modifier { get; set; }
        public TypeSignature? ReturnType { get; set; }
        public string Name { get; set; }
        public TypeSignature[] GenericArguments { get; set; }
        public ParameterSignature[] Parameters { get; set; }
        public TypeSignature? DeclaringType { get; set; }


        public MethodSignature(MethodInfo methodInfo)
        {
            Modifier = methodInfo.GetModifier();
            Name = methodInfo.Name;
            ReturnType = TypeSignature.Create(methodInfo.ReturnType);
            GenericArguments = methodInfo.GetGenericArguments().Select(t => TypeSignature.Create(t)).ToArray();
            DeclaringType = methodInfo.DeclaringType != null ? TypeSignature.Create(methodInfo.DeclaringType) : null;
            Parameters = methodInfo.GetParameters().Select(p => ParameterSignature.Create(p)).ToArray();

        }

        public MethodSignature(string methodSignature)
        {
            var regex = new Regex(@"(?<modifier>.*)? (?<returnType>\S+)\s(?<name>[a-zA-Z0-9_]+)(<(?<genericArguments>.*)>)?\((?<parameters>.*)?\)");

            var match = regex.Match(methodSignature);

            if (!match.Success)
                throw new Exception($"can't parse '{methodSignature}' to MethodSignature");


            Name = match.Groups["name"].Value;

            GenericArguments = match.Groups["genericArguments"]?.Value
                .Split(',')
                .Where(s => !String.IsNullOrWhiteSpace(s))
                .Select(TypeSignature.Parse)
                .ToArray() ?? Array.Empty<TypeSignature>();

            var returnType = match.Groups["returnType"]?.Value.TrimToNull();
            ReturnType = returnType != null ? TypeSignature.Parse(returnType) : null;


            Parameters = match.Groups["parameters"].Value.TrimToNull()?.SplitGenericArguments().Select(ParameterSignature.Parse)
                .ToArray() ?? Array.Empty<ParameterSignature>();
        }

        public static MethodSignature Create(MethodInfo methodInfo)
        {
            return new MethodSignature(methodInfo);
        }

        public static MethodSignature Parse(string methodString)
        {
            return new MethodSignature(methodString);
        }


        public override string ToString()
        {
            var genericArguments = GenericArguments?.Any() == true
                ? $"<{String.Join(", ", GenericArguments.Select(ga => ga.ToString()))}>" : null;
            var parameters = Parameters?.Any() == true ? String.Join(", ", Parameters.Select(p => p.AsMethodParamterString())) : null;
            return $"{Name}{genericArguments}({parameters})".Trim();
        }
    }
}
