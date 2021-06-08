using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using doob.Reflectensions.CodeDefinition.ExtensionMethods;
using doob.Reflectensions.ExtensionMethods;
using doob.Reflectensions.Helper;

namespace doob.Reflectensions.CodeDefinition.Definitions
{
    public class TypeDefinition
    {
        public string? Namespace { get; set; }
        public Modifier? Modifier { get; set; }
        public Kind? Kind { get; set; }
        public string Name { get; set; }

        public TypeDefinition[] GenericArguments { get; set; } = new TypeDefinition[0];
        public TypeDefinition? BaseType { get; set; }

        public TypeDefinition[] ImplementedInterfaces { get; set; } = new TypeDefinition[0];

        public bool IsGeneric { get; set; }
        public bool IsArray { get; set; }
        public int ArrayDimensions { get; set; }
        public bool IsNullable { get; set; }
        public bool IsByRef { get; set; }

        public bool IsValueType { get; set; }

        public static TypeDefinition Create(Type type)
        {
            var _type = type;
            if (TypeDefinitionCache.TryGetValue(_type.ToString(), out var typeDefinition))
            {
                return typeDefinition;
            }

            typeDefinition = new TypeDefinition();

            var isGenericTypeParameter = type.IsGenericTypeParameter();
            typeDefinition.IsNullable = type.IsNullableType();
            if (typeDefinition.IsNullable)
            {
                type = Nullable.GetUnderlyingType(type);
            }

            typeDefinition.IsValueType = _type.IsValueType;
            var typeName = type.Name;

            var arrayDepth = 0;
            while (typeName.EndsWith("[]"))
            {
                arrayDepth++;
                typeName = typeName.Remove(typeName.Length - 2);
            }

            typeDefinition.ArrayDimensions = arrayDepth;

            if (type.IsArray || type.IsByRef)
            {
                typeDefinition.IsArray = type.IsArray;
                typeDefinition.IsByRef = type.IsByRef;
                type = type.GetElementType();
                if (type.IsGenericTypeParameter())
                {
                    isGenericTypeParameter = true;
                }
            }


            if (!isGenericTypeParameter && !type.IsGenericParameter)
            {
                typeDefinition.Namespace = type.Namespace;
            }

            typeDefinition.Name = type.Name.Contains('`') ? type.Name.Remove(type.Name.IndexOf('`')) : type.Name;
            typeDefinition.Modifier = type.GetModifier();
            typeDefinition.Kind = type.GetKind();
            TypeDefinitionCache.TryAdd(_type.ToString(), typeDefinition);

            if (type.IsGenericType)
            {
                var tdinfo = type.GetTypeInfo();
                typeDefinition.GenericArguments = tdinfo.GenericTypeParameters.Any() ?
                    tdinfo.GenericTypeParameters.Select(t => TypeDefinition.Create(t)).ToArray() :
                    tdinfo.GenericTypeArguments.Select(t => TypeDefinition.Create(t)).ToArray();
            }

            if (type.HasInspectableBaseType())
            {
                typeDefinition.BaseType = TypeDefinition.Create(type.BaseType);
            }

            if (typeDefinition.Kind != CodeDefinition.Kind.Interface)
            {
                var directInterfaces = type.GetDirectInterfaces().ToList();
                typeDefinition.ImplementedInterfaces = directInterfaces.Select(Create).ToArray();
            }

            return typeDefinition;
        }


        public static TypeDefinition Parse(string definitionString)
        {

            Regex regex = new Regex(@"((?<modifier>[a-z]+)?\s)?((?<kind>[a-z]+)\s)?(?<fqn>[a-zA-Z0-9_\.]+)(`[\d+|\?])?([\[|<](?<genericArguments>.*)[\]|>])?");

            var match = regex.Match(definitionString);

            if (!match.Success)
                throw new Exception($"can't parse '{definitionString}' to TypeDefinition");

            var td = new TypeDefinition();
            var fqn = match.Groups["fqn"].Value;

            if (fqn.Contains('.'))
            {
                var lastDotIndex = fqn.LastIndexOf('.');
                td.Name = fqn.Substring(lastDotIndex + 1);
                td.Namespace = fqn.Substring(0, lastDotIndex);
            }
            else
            {
                td.Name = fqn;
                td.IsGeneric = true;
            }


            if (match.Groups["kind"].Value.TryGetEnum<Kind>(out var kind))
            {
                td.Kind = kind;
            }

            td.GenericArguments = match.Groups["genericArguments"].Value.TrimToNull()?.SplitGenericArguments().Select(g => TypeHelper.NormalizeTypeName(g))
                .Select(Parse).ToArray();
            //if (match.Groups["genericArguments"].Value.TrimToNull() != null)
            //{
            //    var parameterParts = Regex.Split(match.Groups["genericArguments"].Value, @"\,(?![^<]*>)").Select(p => p.TrimToNull()).Where(p => p != null).ToList();
            //    td.GenericArguments = parameterParts.Select(Parse).ToArray();
            //}


            return td;

        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(Namespace))
            {
                strBuilder.Append($"{Namespace}.");
            }

            strBuilder.Append(Name);

            if (GenericArguments?.Any() == true)
            {
                strBuilder.Append($"<{String.Join(", ", GenericArguments.Select(ga => ga.ToString()))}>");
            }

            for (int i = 0; i < ArrayDimensions; i++)
            {
                strBuilder.Append("[]");
            }

            return strBuilder.ToString();
        }
    }
}
