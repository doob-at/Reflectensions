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



# Invokehelper

Helper to invoke Methods in several ways

```csharp
public static void InvokeVoidMethod(object? instance, MethodInfo methodInfo, params object[] parameters);
public static T? InvokeMethod<T>(object? instance, MethodInfo methodInfo, params object[] parameters);
public static async Task InvokeVoidMethodAsync(object? instance, MethodInfo methodInfo, params object[] parameters);
public static async Task<T?> InvokeMethodAsync<T>(object? instance, MethodInfo methodInfo, params object[] parameters);

public static void InvokeGenericVoidMethod(object instance, MethodInfo methodInfo, IEnumerable<Type> genericArguments, params object[] parameters);
public static void InvokeGenericVoidMethod<TArg>(object instance, MethodInfo methodInfo, params object[] parameters);
public static void InvokeGenericVoidMethod<TArg1, TArg2>(object instance, MethodInfo methodInfo, params object[] parameters);
public static void InvokeGenericVoidMethod<TArg1, TArg2, TArg3>(object instance, MethodInfo methodInfo, params object[] parameters);

public static object? InvokeGenericMethod(object instance, MethodInfo methodInfo, IEnumerable<Type> genericArguments, params object[] parameters);
public static TResult? InvokeGenericMethod<TResult>(object instance, MethodInfo methodInfo, IEnumerable<Type> genericArguments, params object[] parameters);
public static TResult? InvokeGenericMethod<TArg, TResult>(object instance, MethodInfo methodInfo, params object[] parameters);
public static TResult? InvokeGenericMethod<TArg1, TArg2, TResult>(object instance, MethodInfo methodInfo, params object[] parameters);
public static TResult? InvokeGenericMethod<TArg1, TArg2, TArg3, TResult>(object instance, MethodInfo methodInfo, params object[] parameters);

public static Task InvokeGenericVoidMethodAsync(object instance, MethodInfo methodInfo, IEnumerable<Type> genericArguments, params object[] parameters);
public static Task InvokeGenericVoidMethodAsync<TArg>(object instance, MethodInfo methodInfo, params object[] parameters);
public static Task InvokeGenericVoidMethodAsync<TArg1, TArg2>(object instance, MethodInfo methodInfo, params object[] parameters);
public static Task InvokeGenericVoidMethodAsync<TArg1, TArg2, TArg3>(object instance, MethodInfo methodInfo, params object[] parameters);

public static Task<TResult?> InvokeGenericMethodAsync<TResult>(object instance, MethodInfo methodInfo, IEnumerable<Type> genericArguments, params object[] parameters);
public static Task<TResult?> InvokeGenericMethodAsync<TArg, TResult>(object instance, MethodInfo methodInfo, params object[] parameters);
public static Task<TResult?> InvokeGenericMethodAsync<TArg1, TArg2, TResult>(object instance, MethodInfo methodInfo, params object[] parameters);
public static Task<TResult?> InvokeGenericMethodAsync<TArg1, TArg2, TArg3, TResult>(object instance, MethodInfo methodInfo, params object[] parameters);
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


## IEnumerable\<MethodInfo\> Extensions

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

## IEnumerable\<Type\> Extensions

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

#region Get Operator Methods
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

## Object Extensions

ExtensionMethods for the `object` type are special.  
To prevent that those ExtensionMethods appears on all types which inherit from `object`(which are almost all...) there is ONE special Method called `Reflect()`.  

```csharp
public static ObjectReflection Reflect(this object reflectionObject);
```

From there you have access to the ExtensionMethods which are avsailable to `object` types.

```csharp
public static bool EqualsToAnyOf(this ObjectReflection objectReflection, params object[] equalsTo);
public static bool ToBoolean(this ObjectReflection objectReflection, params object[] trueValues);
public static bool IsImplicitCastableTo(this ObjectReflection objectReflection, Type type);
public static bool IsImplicitCastableTo<T>(this ObjectReflection objectReflection);

// tries to switch the type of an object
public static bool TryAs(this ObjectReflection objectReflection, Type type, out object? outValue);
public static bool TryAs<T>(this ObjectReflection objectReflection, out T? outValue);
public static object? As(this ObjectReflection objectReflection, Type type);
public static T? As<T>(this ObjectReflection objectReflection);

// tries to cast on object
// - use TryAs Methods to switch type if possible
// - uses an ExtensinMehtod from this lib to `convert` the object, fro example `string.ToGuid()`
// - search for a suitable implicit operator Method
public static bool TryTo(this ObjectReflection objectReflection, Type type, out object? outValue);
public static object? To(this ObjectReflection objectReflection, Type type, object? defaultValue);
public static object? To(this ObjectReflection objectReflection, Type type);
public static bool TryTo<T>(this ObjectReflection objectReflection, out T? outValue);
public static  T? To<T>(this ObjectReflection objectReflection, T? defaultValue);
public static  T? To<T>(this ObjectReflection objectReflection);

public static object? GetPropertyValue(this ObjectReflection objectReflection, string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance);
public static T? GetPropertyValue<T>(this ObjectReflection objectReflection, string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance);
public static void SetPropertyValue(this ObjectReflection objectReflection, string path, object value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance);

```


```csharp

var dtString = "2021-03-21T15:50:17+00:00";
DateTime date = dtString.Reflect().To<DateTime>();

```
