# Reflectensions

Reflectensions is a library to make it easier to work with C# Reflection and various Types.  
Initially i have created this library to make ma life easier and speed up my daily coding...  
Probably it's not a big player for everyone, but maybe it is helpful for one or the other.

# Typehelper

## NormalizeTypeName

```csharp
public static string NormalizeTypeName(string typeName);
public static string NormalizeTypeName(string typeName, IDictionary<string, string> customTypeMapping);
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
public static Type? FindType(string typename);
public static Type? FindType(string typename, IDictionary<string, string> customTypeMapping);

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


# ExtensionMethods

There are several ExtensionMethods for a few Types.  
Most of them are self explained because aof their names...

## Action Extensions

```csharp
public static T InvokeAction<T>(this Action<T> action, T? instance = default);
public static (T1, T2) InvokeAction<T1, T2>(this Action<T1, T2> action, T1? firstInstance = default, T2? secondInstance = default);
public static (T1, T2, T3) InvokeAction<T1, T2, T3>(this Action<T1, T2, T3> action, T1? firstInstance = default, T2? secondInstance = default, T3? thirdInstance = default)
```

If no parameter is provided, a 'default' instance is created, so you don't have to create it by yourself.  
Just works for simple Types which have a constructor with no parameters!  
Or you can provide one or more parameters, if the Type doesn't have a constructor with no parameters;


## MethodBase Extensions

```csharp
public static bool IsExtensionMethod(this MethodBase methodInfo);
```

Currently just one Method which tells you if the Method is an ExtensionMethod.  
Which means that the Method is declared in a static class, and the first parameter of this Method has the `this` modifier.  


## IEnumerable<MethodInfo> Extensions

```csharp
public static IEnumerable<MethodInfo> WithName(this IEnumerable<MethodInfo> methodInfos, string name, StringComparison stringComparison = StringComparison.CurrentCulture);
public static IEnumerable<MethodInfo> WithReturnType<T>(this IEnumerable<MethodInfo> methodInfos);
public static IEnumerable<MethodInfo> WithReturnType(this IEnumerable<MethodInfo> methodInfos, Type returnType);
public static IEnumerable<MethodInfo> WithParametersOfType(this IEnumerable<MethodInfo> methodInfos, params Type[] types);
public static IEnumerable<MethodInfo> WithAttribute<T>(this IEnumerable<MethodInfo> methodInfos, bool inherit = false);
public static IEnumerable<MethodInfo> WithAttribute(this IEnumerable<MethodInfo> methodInfos, Type attributeType, bool inherit = false);
public static IEnumerable<(MethodInfo MethodInfo, T Attribute)> WithAttributeExpanded<T>(this IEnumerable<MethodInfo> methodInfos, bool inherit = false) where T : Attribute;
public static IEnumerable<(MethodInfo MethodInfo, Attribute Attribute)> WithAttributeExpanded(this IEnumerable<MethodInfo> methodInfos, Type attributeType, bool inherit = false)
```

## MethodInfo Extensions

```csharp
public static bool HasName(this MethodInfo methodInfo, string name, StringComparison stringComparison = StringComparison.CurrentCulture);
public static bool HasParametersLengthOf(this MethodInfo methodInfo, int parameterLength, bool includeOptional = false);
public static bool HasParametersOfType(this MethodInfo methodInfo, Type[] types);
public static bool HasAttribute<T>(this MethodInfo methodInfo, bool inherit = false) where T : Attribute;
public static bool HasAttribute(this MethodInfo methodInfo, Type attributeType, bool inherit = false);
public static bool HasReturnType<T>(this MethodInfo methodInfo);
public static bool HasReturnType(this MethodInfo methodInfo, Type returnType);
```


## PropertyInfo Extensions

```csharp
public static bool IsIndexerProperty(this PropertyInfo propertyInfo);
public static bool IsPublic(this PropertyInfo propertyInfo);
public static IEnumerable<PropertyInfo> WhichIsIndexerProperty(this IEnumerable<PropertyInfo> propertyInfos);
```


## String Extensions

```csharp
public static string Repeat(this string value, int times);
public static string[] Split(this string value, string split, StringSplitOptions options);
public static string[] Split(this string value, string split, bool removeEmptyEntries = false);
public static string Trim(this string value, params string[] trimCharacters);
public static string? TrimToNull(this string? value);
public static string RemoveEmptyLines(this string value);

#region StringIs
public static bool IsNullOrWhiteSpace(this string value);
public static bool IsNumeric(this string value);
public static bool IsInt(this string value);
public static bool IsLong(this string value);
public static bool IsDouble(this string value);
public static bool IsDateTime(this string value, string? customFormat = null);
public static bool IsBoolean(this string value);
public static bool IsValidIp(this string value);
public static bool IsBase64Encoded(this string value);
public static bool IsLowerCase(this string value);
public static bool IsUpperCase(this string value);
private static string DomainMapper(Match match);
public static Boolean IsValidEmailAddress(this string value);
public static bool IsGuid(this string value);
#endregion;

#region StringTo
public static string? ToNull(this string value);
public static int ToInt(this string value);
public static int? ToNullableInt(this string? value);
public static decimal ToDecimal(this string value);
public static decimal? ToNullableDecimal(this string value);
public static float ToFloat(this string value);
public static float? ToNullableFloat(this string value);
public static long ToLong(this string value);
public static long? ToNullableLong(this string value);
public static double ToDouble(this string value);
public static double? ToNullableDouble(this string value);
public static bool ToBoolean(this string value);
public static DateTime? ToNullableDateTime(this string value, string? customFormat = null);
public static DateTime ToDateTime(this string value, string? customFormat = null);
public static string EncodeToBase64(this string value);
public static string DecodeFromBase64(this string value);
public static Guid ToGuid(this string value);
#endregion
```

## IEnumerable<Type> Extensions

```csharp
public static IEnumerable<Type> WithAttribute<T>(this IEnumerable<Type> types, bool inherit = false) where T : Attribute;
public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> types, Type attributeType, bool inherit = false);
public static IEnumerable<KeyValuePair<Type, T>> WithAttributeExpanded<T>(this IEnumerable<Type> types, bool inherit = false) where T : Attribute;
public static IEnumerable<KeyValuePair<Type, Attribute>> WithAttributeExpanded(this IEnumerable<Type> types, Type attributeType, bool inherit = false);
public static IEnumerable<Type> WhichIsGenericTypeOf(this IEnumerable<Type> types, Type of);
public static IEnumerable<Type> WhichIsGenericTypeOf<T>(this IEnumerable<Type> types);
public static IEnumerable<Type> WhichInheritFromClass(this IEnumerable<Type> types, Type from);
public static IEnumerable<Type> WhichInheritFromClass<T>(this IEnumerable<Type> types);
```

## Type Extensions

```csharp
#region Check Type
public static bool IsNumericType(this Type type);
public static bool IsGenericTypeOf(this Type type, Type genericType);
public static bool IsGenericTypeOf<T>(this Type type);
public static bool IsNullableType(this Type type);
public static bool IsEnumerableType(this Type type);
public static bool IsDictionaryType(this Type type);
public static bool ImplementsInterface(this Type type, Type interfaceType);
public static bool ImplementsInterface<T>(this Type type);
public static bool InheritFromClass<T>(this Type type);
public static bool InheritFromClass(this Type type, string from);
public static bool InheritFromClass(this Type type, Type from);
public static bool IsImplicitCastableTo<T>(this Type type);
public static bool IsImplicitCastableTo(this Type type, Type to);
public static bool Equals<T>(this Type type);
public static bool NotEquals<T>(this Type type);
public static bool HasAttribute<T>(this Type type, bool inherit = false) where T : Attribute;
public static bool HasAttribute(this Type type, Type attributeType, bool inherit = false);
public static bool IsStatic(this Type type);
#endregion

#region Get Operator Methods;
public static IEnumerable<MethodInfo> GetImplicitOperatorMethods(this Type type, bool throwOnError = true);
public static IEnumerable<MethodInfo> GetExplicitOperatorMethods(this Type type, bool throwOnError = true);
public static IEnumerable<MethodInfo> GetImplicitOperatorMethods<T>(bool throwOnError = true);
public static IEnumerable<MethodInfo> GetExplicitOperatorMethods<T>(bool throwOnError = true);
public static MethodInfo? GetImplicitCastMethodTo<T>(this Type fromType);
public static MethodInfo? GetImplicitCastMethodTo(this Type fromType, Type toType);
public static MethodInfo? GetExplicitCastMethodTo<T>(this Type fromType);
public static MethodInfo? GetExplicitCastMethodTo(this Type fromType, Type toType);
#endregion

public static int InheritFromClassLevel(this Type type, Type from, int? maximumLevel = null);
public static int InheritFromClassLevel(this Type type, string from, int? maximumLevel = null);
public static int InheritFromClassLevel<T>(this Type type, int? maximumLevel = null);

```