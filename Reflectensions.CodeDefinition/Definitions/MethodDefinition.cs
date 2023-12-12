using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using doob.Reflectensions.CodeDefinition.ExtensionMethods;

namespace doob.Reflectensions.CodeDefinition.Definitions
{
    public class MethodDefinition
    {
        public Modifier? Modifier { get; set; }
        public TypeDefinition? ReturnType { get; set; }
        public string Name { get; set; } = default!;
        public List<TypeDefinition> GenericArguments { get; set; } = new List<TypeDefinition>();
        public List<ParameterDefinition> Parameters { get; set; } = new List<ParameterDefinition>();
        public TypeDefinition DeclaringType { get; set; }


        public static MethodDefinition Create(MethodInfo methodInfo)
        {
            var methodDefinition = new MethodDefinition();
            methodDefinition.Modifier = methodInfo.GetModifier();
            methodDefinition.Name = methodInfo.Name;
            methodDefinition.ReturnType = TypeDefinition.Create(methodInfo.ReturnType);
            methodDefinition.GenericArguments = methodInfo.GetGenericArguments().Select(t => TypeDefinition.Create(t)).ToList();
            methodDefinition.DeclaringType = TypeDefinition.Create(methodInfo.DeclaringType);
            methodDefinition.Parameters = methodInfo.GetParameters().Select(p => ParameterDefinition.Create(p)).ToList();

            return methodDefinition;
        }

        public static MethodDefinition Parse(string methodString)
        {

            var regex = new Regex(@"(?<modifier>.*)? (?<returnType>\S+)\s(?<name>[a-zA-Z0-9_]+)(<(?<genericArguments>.*)>)?\((?<parameters>.*)?\)");

            var match = regex.Match(methodString);

            if (!match.Success)
                throw new Exception($"can't parse '{methodString}' to MethodDefinition");

            var md = new MethodDefinition();

            md.Name = match.Groups["name"]?.Value ?? default!;

            md.GenericArguments = match.Groups["genericArguments"]?.Value
                .Split(',')
                .Where(s => !String.IsNullOrWhiteSpace(s))
                .Select(TypeDefinition.Parse)
                .ToList() ?? new List<TypeDefinition>();

            var returnType = match.Groups["returnType"]?.Value.TrimToNull();
            md.ReturnType = returnType != null ? TypeDefinition.Parse(returnType) : null;


           md.Parameters = match.Groups["parameters"].Value.TrimToNull()?.SplitGenericArguments().Select(ParameterDefinition.Parse)
                .ToList() ?? new List<ParameterDefinition>();

            return md;
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
