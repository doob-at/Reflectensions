namespace doob.Reflectensions.CodeDefinition;

public class TypeSignature
{

    private static ConcurrentDictionary<string, TypeSignature> Cache { get; } = new();

    public string? Namespace { get; set; }
    public Modifier? Modifier { get; set; }
    public Kind? Kind { get; set; }
    public string Name { get; set; }

    public TypeSignature[]? GenericArguments { get; set; }
    public TypeSignature? BaseType { get; set; }

    public TypeSignature[] ImplementedInterfaces { get; set; }

    public bool IsGeneric { get; set; }
    public bool IsArray { get; set; }
    public bool IsJaggedArray { get; set; }
    public int[] ArrayDimensions { get; set; }

    public bool IsNullable { get; set; }
    public bool IsByRef { get; set; }

    public bool IsValueType { get; set; }


    public TypeSignature(Type type)
    {
        var _type = type;

        var isGenericTypeParameter = type.IsGenericTypeParameter();
        IsNullable = type.IsNullableType();
        if (IsNullable)
        {
            type = Nullable.GetUnderlyingType(type)!;
        }

        IsValueType = _type.IsValueType;

        Regex regex = new Regex(@"(\[(?<array>[,]*)\])*");

        var match = regex.Match(type.Name);
        if (match.Success)
        {
            var arrays = match.Groups["array"].Captures;
            var arrayDimensions = new List<int>();
            foreach (Capture c in arrays)
            {
                var l = c.Value.Split(',').Length;
                arrayDimensions.Add(l);
            }

            ArrayDimensions = arrayDimensions.ToArray();
        }


        if (type.IsArray || type.IsByRef)
        {
            IsArray = type.IsArray;
            IsByRef = type.IsByRef;
            type = type.GetElementType()!;
            if (type.IsGenericTypeParameter())
            {
                isGenericTypeParameter = true;
            }
        }


        if (!isGenericTypeParameter && !type.IsGenericParameter)
        {
            Namespace = type.Namespace;
        }

        Name = type.Name.Contains('`') ? type.Name.Remove(type.Name.IndexOf('`')) : type.Name;
        Modifier = type.GetModifier();
        Kind = type.GetKind();
        Cache.TryAdd(_type.ToString(), this);

        if (type.IsGenericType)
        {
            var tdinfo = type.GetTypeInfo();
            GenericArguments = tdinfo.GenericTypeParameters.Any() ?
                tdinfo.GenericTypeParameters.Select(t => TypeSignature.Create(t)).ToArray() :
                tdinfo.GenericTypeArguments.Select(t => TypeSignature.Create(t)).ToArray();
        }
        else
        {
            GenericArguments = Array.Empty<TypeSignature>();
        }

        if (TypeHelper.HasInspectableBaseType(type))
        {
            BaseType = TypeSignature.Create(type.BaseType!);
        }

        if (Kind != CodeDefinition.Kind.Interface)
        {
            var directInterfaces = type.GetDirectInterfaces().ToList();
            ImplementedInterfaces = directInterfaces.Select(Create).ToArray();
        }
        else
        {
            ImplementedInterfaces = Array.Empty<TypeSignature>();
        }

    }

    public TypeSignature(string signatureString)
    {
        //Regex regex = new Regex(@"((?<modifier>[a-z]+)?\s)?((?<kind>[a-z]+)\s)?(?<name>[a-zA-Z0-9_\.]+)(`[\d+|\?])?([\[|<](?<genericArguments>(([a-zA-Z0-9_\.\s]+)[,]*)+)[\]|>])?(\[(?<array>[,]*)\])*");

        var normalizeRegex = new Regex(@"(\[(?<gen>(.*)[^|\[]\])\])");
        signatureString = normalizeRegex.Replace(signatureString, "<${gen}>");

       


        Regex regex = new Regex(@"
((?<modifier>[a-z]+)?\s)?
((?<kind>[a-z]+)\s)?
(?<name>[a-zA-Z0-9_\.]+){1}
(`[\d+|\?])?
(<(?<genericArguments>.*)>)*
(\[(?<array>[,]*)\])*
", RegexOptions.IgnorePatternWhitespace);

        var match = regex.Match(signatureString);

        if (!match.Success)
            throw new Exception($"can't parse '{signatureString}' to TypeSignature");


        var name = match.Groups["name"].Value;

        if (name.Contains('.'))
        {
            var lastDotIndex = name.LastIndexOf('.');
            Name = name.Substring(lastDotIndex + 1);
            Namespace = name.Substring(0, lastDotIndex);
        }
        else
        {
            Name = name;
        }

        if (match.Groups["modifier"].Value.TryGetEnum<Modifier>(out var modifier))
        {
            Modifier = modifier;
        }

        if (match.Groups["kind"].Value.TryGetEnum<Kind>(out var kind))
        {
            Kind = kind;
        }

        var val = match.Groups["genericArguments"].Value;
        GenericArguments = val.TrimToNull()?.SplitGenericArguments().Select(g =>
        {
            return Parse(g);
        }).ToArray() ?? Array.Empty<TypeSignature>();

        IsGeneric = GenericArguments.Length > 0;

        var arrays = match.Groups["array"].Captures;
        IsArray = arrays.Count > 0;
        IsJaggedArray = arrays.Count > 1;

        if (IsArray)
        {
            var arrayDimensions = new List<int>();
            foreach (Capture c in arrays)
            {
                var l = c.Value.Split(',').Length;
                arrayDimensions.Add(l);
            }

            ArrayDimensions = arrayDimensions.ToArray();
        }
        else
        {
            ArrayDimensions = Array.Empty<int>();
        }

    }

    public static TypeSignature Create(Type type)
    {
        return Cache.TryGetValue(type.ToString(), out var typeDefinition) ? typeDefinition : new TypeSignature(type);
    }


    public static TypeSignature Parse(string definitionString)
    {

        return new TypeSignature(definitionString);

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


        foreach (var arrayDimension in ArrayDimensions)
        {
            var delimiters = new string(',', arrayDimension-1);
            strBuilder.Append($"[{delimiters}]");
        }
        //for (int i = 0; i < ArrayDimensions.Length; i++)
        //{
        //    strBuilder.Append("[]");
        //}

        return strBuilder.ToString();
    }
}

