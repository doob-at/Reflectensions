using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using doob.Reflectensions.Classes;
using doob.Reflectensions.Exceptions;

namespace doob.Reflectensions.ExtensionMethods
{
    public static class ObjectReflectionExtensions
    {

        public static bool EqualsToAnyOf(this ObjectReflection objectReflection, params object[] equalsTo)
        {
            var value = objectReflection.Value;

            foreach (var trueValue in equalsTo)
            {
                if (value?.Equals(trueValue) == true)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ToBoolean(this ObjectReflection objectReflection, params object[] trueValues)
        {
            var value = objectReflection.Value;

            if (value == null)
                return false;

            if (trueValues.Any())
            {
                return EqualsToAnyOf(objectReflection, trueValues);
            }


            if (value is bool boolValue)
                return boolValue;



            var str = value.ToString();
            if (int.TryParse(str, out var numb))
            {
                return numb > 0;
            }

            if (bool.TryParse(str, out var ret))
            {
                return ret;
            }


            if (str?.ToLower() == "yes")
            {
                return true;
            }

            return ret;
        }


        public static bool IsImplicitCastableTo(this ObjectReflection objectReflection, Type type)
        {
            return objectReflection.GetType().IsImplicitCastableTo(type);
        }
        
        public static bool IsImplicitCastableTo<T>(this ObjectReflection objectReflection)
        {
            return objectReflection.GetType().IsImplicitCastableTo<T>();
        }


        public static bool TryTo(this ObjectReflection objectReflection, Type type, out object? outValue)
        {



            if (type == typeof(bool))
            {
                outValue = ToBoolean(objectReflection);
                return true;
            }



            if (TryAs(objectReflection, type, out outValue))
            {
                return true;
            }


            if (type.IsNullableType())
            {
                var nt = Nullable.GetUnderlyingType(type);
                if (nt == null)
                {
                    outValue = null;
                    return false;
                }

                type = nt;
            }


            var value = objectReflection.Value;

            if (type == typeof(Guid))
            {
                if (value is string str)
                {
                    if(Guid.TryParse(str, out var g))
                    {
                        outValue = g;
                        return true;
                    }
                    else
                    {
                        outValue = null;
                        return false;
                    }
                }
            }

            //if (type == typeof(DateTime))
            //{
            //    if (value is string str)
            //    {
            //        if (str.IsDateTime())
            //        {
            //            outValue = str.ToDateTime();
            //            return true;
            //        }
            //        else
            //        {
            //            outValue = null;
            //            return false;
            //        }
            //    }
            //}

            if (type.IsNumericType())
            {
                try
                {
                    outValue = Convert.ChangeType(value, type);
                    return true;
                }
                catch
                {
                    // ignored
                }
            }

            if (value!.GetType().ImplementsInterface<IConvertible>() && type.ImplementsInterface<IConvertible>())
            {
                try
                {
                    outValue = Convert.ChangeType(value, type);
                    return true;
                }
                catch
                {
                    // ignored
                }
            }


            var method = value.GetType().GetImplicitCastMethodTo(type);

            if (method != null)
            {
                outValue = method.Invoke(null, new[] {
                    value
                });
                return true;
            }

            //if (JsonHelpers.IsAvailable())
            //{
            //    try
            //    {
            //        //var json = JsonHelpers.Json().ToJson(value);
            //        outValue = JsonHelpers.Json().ToObject(value, type);
            //        return true;
            //    }
            //    catch
            //    {
            //        // ignored
            //    }
            //}

            outValue = null;
            return false;

        }

        public static bool TryTo<T>(this ObjectReflection objectReflection, out T? outValue)
        {

            var result = TryTo(objectReflection, typeof(T), out var _outValue);

            outValue = _outValue != null ? (T)_outValue : default;
            return result;
        }

        public static object? To(this ObjectReflection objectReflection, Type type, object? defaultValue)
        {

            var result = TryTo(objectReflection, type, out var outValue);
            if (result)
                return outValue;

            if (defaultValue != null)
            {
                return defaultValue;
            }

            return type.IsValueType ? Activator.CreateInstance(type) : null;

        }

        public static object? To(this ObjectReflection objectReflection, Type type)
        {

            var result = TryTo(objectReflection, type, out var outValue);
            if (result)
                return outValue;

            throw new InvalidCastException($"Can't cast object of Type '{objectReflection.GetType()}' to '{type}'.");
        }

        public static T? To<T>(this ObjectReflection objectReflection, T? defaultValue)
        {
            return (T?)To(objectReflection, typeof(T), defaultValue);
        }

        public static  T? To<T>(this ObjectReflection objectReflection)
        {
            return (T?)To(objectReflection, typeof(T));
        }


        public static bool TryAs(this ObjectReflection objectReflection, Type type, out object? outValue)
        {
            var value = objectReflection.Value;

            if (value == null)
            {
                outValue = null;
                return false;
            }

            var t = value.GetType();

            if (t == type)
            {
                outValue = value;
                return true;
            }

            if (t.ImplementsInterface(type) || t.InheritFromClass(type))
            {
                outValue = value;
                return true;
            }


            if (type.IsNullableType())
            {
                var underlingType = Nullable.GetUnderlyingType(type);
                if (underlingType != null && new ObjectReflection(value).TryAs(underlingType, out var innerValue))
                {
                    outValue = innerValue;
                    return true;
                }
            }

            outValue = null;
            return false;

        }

        public static bool TryAs<T>(this ObjectReflection objectReflection, out T? outValue)
        {
            var result = TryAs(objectReflection, typeof(T), out var _outValue);

            outValue = _outValue != null ? (T)_outValue : default;
            return result;
        }

        public static object? As(this ObjectReflection objectReflection, Type type)
        {

            var result = TryAs(objectReflection, type, out var outValue);
            return result ? outValue : null;

        }

        public static T? As<T>(this ObjectReflection objectReflection)
        {

            var result = TryAs<T>(objectReflection, out var outValue);
            return result ? outValue : default;

        }


        public static object? GetPropertyValue(this ObjectReflection objectReflection, string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            return GetPropertyValue<object>(objectReflection, name, bindingFlags);
        }

        public static T? GetPropertyValue<T>(this ObjectReflection objectReflection, string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            if (objectReflection.Value == null)
                throw new ArgumentNullException();

            var parts = name.Split('.');

            var currentObject = objectReflection.Value;

            var processedPaths = new List<string>();
            foreach (var part in parts)
            {

                processedPaths.Add(part);
                var currentPropertyInfo = currentObject.GetType().GetProperty(part, bindingFlags);


                if (currentPropertyInfo == null)
                    throw new Exception($"Path not found '{string.Join(".", processedPaths)}'");

                currentObject = currentPropertyInfo.GetValue(currentObject);

                if (currentObject == null)
                    throw new PropertyNotFoundException($"Path not found '{string.Join(".", processedPaths)}'");

            }

            return currentObject.Reflect().To<T>();

        }

        public static void SetPropertyValue(this ObjectReflection objectReflection, string path, object value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            var parts = path.Split('.');

            var currentObject = objectReflection.Value;
            var processedPaths = new List<string>();

            for (var i = 0; i < parts.Length; i++)
            {
                if (currentObject == null)
                    break;

                var part = parts[i];
                var isLast = i == parts.Length - 1;

                processedPaths.Add(part);
                var currentPropertyInfo = currentObject.GetType().GetProperty(part, bindingFlags);


                if (currentPropertyInfo == null)
                    throw new PropertyNotFoundException($"Path not found '{string.Join(".", processedPaths)}'");

                if (isLast)
                {
                    currentPropertyInfo.SetValue(currentObject, value);
                    return;
                }
                else
                {
                    currentObject = currentPropertyInfo.GetValue(currentObject);
                }

            }

        }



    }
}
