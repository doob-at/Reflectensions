using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Reflectensions.Exceptions;
using Reflectensions.ExtensionMethods;

namespace Reflectensions.Classes
{
    public class ObjectReflection
    {
        private object Value { get; }

        public ObjectReflection(object value)
        {
            Value = value;
        }

        public bool EqualsToAnyOf(params object[] equalsTo)
        {
            var value = Value;

            foreach (var trueValue in equalsTo)
            {
                if (value == trueValue)
                {
                    return true;
                }
            }

            return false;
        }


        public bool ToBoolean(params object[] trueValues)
        {
            var value = Value;

            if (value == null)
                return false;

            if (trueValues.Any())
            {
                return EqualsToAnyOf(trueValues);
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


        public bool IsImplicitCastableTo(Type type)
        {
            return Value.GetType().IsImplicitCastableTo(type);
        }
        public bool IsImplicitCastableTo<T>()
        {
            return Value.GetType().IsImplicitCastableTo<T>();
        }



        public bool TryTo(Type type, out object? outValue)
        {



            if (type == typeof(bool))
            {
                outValue = ToBoolean();
                return true;
            }



            if (TryAs(type, out var _outValue))
            {
                outValue = _outValue;
                return true;
            }


            if (type.IsNullableType())
            {
                var nt = Nullable.GetUnderlyingType(type);
                if (nt == null)
                {
                    outValue = _outValue;
                    return false;
                }

                type = nt;
            }


            var value = Value;

            if (type == typeof(Guid))
            {
                if (value is string str)
                {
                    if (str.IsGuid())
                    {
                        outValue = str.ToGuid();
                        return true;
                    }
                    else
                    {
                        outValue = null;
                        return false;
                    }
                }
            }

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

            if (value.GetType().ImplementsInterface<IConvertible>() && type.ImplementsInterface<IConvertible>())
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
                outValue = method.Invoke(null, new object[] {
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


        public bool TryTo<T>(out T? outValue)
        {

            var result = TryTo(typeof(T), out var _outValue);

            outValue = _outValue != null ? (T)_outValue : default;
            return result;
        }

        public object? To(Type type, bool throwOnError = true, object? returnOnError = null)
        {

            var result = TryTo(type, out var outValue);
            if (result)
                return outValue;


            if (!throwOnError)
            {
                if (returnOnError != null)
                {
                    return returnOnError;
                }
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }
            throw new InvalidCastException($"Can't cast object of Type '{Value?.GetType()}' to '{type}'.");

        }

        public T? To<T>(bool throwOnError = true, T? returnOnError = default)
        {
            return (T?)To(typeof(T), throwOnError, returnOnError);
        }



        public bool TryAs(Type type, out object? outValue)
        {
            var value = Value;

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

            var method = t.GetImplicitCastMethodTo(type);

            if (method != null)
            {
                outValue = method.Invoke(null, new object[] {
                    value
                });
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

        public bool TryAs<T>(out T? outValue)
        {
            var result = TryAs(typeof(T), out var _outValue);

            outValue = _outValue != null ? (T)_outValue : default;
            return result;
        }

        public object? As(Type type)
        {

            var result = TryAs(type, out var outValue);
            return result ? outValue : null;

        }

        public T? As<T>()
        {

            var result = TryAs<T>(out var outValue);
            return result ? outValue : default;

        }


        public override bool Equals(object? obj)
        {
            return Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
#pragma warning disable CS8603 // Possible null reference return.
            return Value.ToString();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public new Type GetType()
        {
            return Value.GetType();
        }


        public object? GetPropertyValue(string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            return GetPropertyValue<object>(name, bindingFlags);
        }
        public T? GetPropertyValue<T>(string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {

            var parts = name.Split('.');

            var currentObject = Value;

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

        public void SetPropertyValue(string path, object value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            var parts = path.Split('.');

            var currentObject = Value;
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