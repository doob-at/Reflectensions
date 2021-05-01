# Reflectensions

Reflectensions is a library to make it easier to work with C# Reflection and various Types.



# Typehelper

## NormalizeTypeName

```csharp
public static string NormalizeTypeName(string typeName)
public static string NormalizeTypeName(string typeName, IDictionary<string, string> customTypeMapping)
```

Normalizing complex type names to runtime friendly C# type names.
following flavours are supported  
* Normal Type Names: `System.String`, `System.DateTime`, `CustomNamespace.MyClass`...
* C# keywords: `string`, `double`, `dynamic`, `int` ...
* Arrays: `int[]`, `int[][]`, `System.Object[][][]`...
* Custom Type mappings, for example if you want to normalize `number` to `double`
* Generic Type Names, and also nested Generic Type Names
  * ```System.Collections.Generic.Dictionary`2[System.String, System.Object]```
  * ```System.Collections.Generic.List<string>```
  * ```System.Collections.Generic.Dictionary<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<number>>, System.Collections.Generic.IReadOnlyCollection<Guid[]>>```
* Mixed flavours from this list

If a typename has no namespace, like just `Guid[]` in the above list. It is assumed that this type is in the System namespace.  
  
Examples:  
```csharp
var a = "System.Collections.Generic.Dictionary`2[string, dynamic]";
TypeHelper.NormalizeTypeName(a);
// System.Collections.Generic.Dictionary`2[System.String, System.Object]


var b = "System.Collections.Generic.List<string>";
TypeHelper.NormalizeTypeName(b);
// System.Collections.Generic.List`1[System.String]


var customTypeMapping = new Dictionary<string, string>
{
    ["number"] = "double"
};
var c = "System.Collections.Generic.Dictionary<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<number>>, System.Collections.Generic.IReadOnlyCollection<Guid[]>>";
TypeHelper.NormalizeTypeName(c,customTypeMapping);
// System.Collections.Generic.Dictionary`2[System.Collections.Generic.Dictionary`2[System.String, System.Collections.Generic.List`1[System.Double]], System.Collections.Generic.IReadOnlyCollection`1[System.Guid[]]]
```



## FindType

```csharp
public static Type? FindType(string typename)
public static Type? FindType(string typename, IDictionary<string, string> customTypeMapping)

```

Uses the above 'NormalizeTypeName' Methods to at first normalizes the type name, and then find the C# Type in the loaded Assemblies.

With the customTypeMapping parameter, its possible to modify the normalization behaviour.  
I use this for example in another library to map TypeScript Syntax to C# Types.
```csharp
var cMap = new Dictionary<string, string>
{
    ["number"] = "double"
};
var typeString = "System.Collections.Generic.Dictionary<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<number>>, System.Collections.Generic.IReadOnlyCollection`1[Guid[]]>";
var type = TypeHelper.FindType(typeString, cMap);
// which resolves to a type of
// Dictionary<Dictionary<string, List<double>>, IReadOnlyCollection<Guid[]>>

```

