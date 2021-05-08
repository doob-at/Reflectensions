using System;

namespace doob.Reflectensions.Classes
{
    public class ObjectReflection
    {
        internal object? Value { get; }

        internal ObjectReflection(object? value)
        {
            Value = value;
        }


        public override bool Equals(object? obj)
        {
            return Value!.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value!.GetHashCode();
        }

        public override string? ToString()
        {
            return Value!.ToString();
        }

        public new Type GetType()
        {
            return Value!.GetType();
        }

        public static bool operator ==(ObjectReflection first, ObjectReflection second)
        {
            return first.Value == second.Value;
        }

        public static bool operator !=(ObjectReflection first, ObjectReflection second)
        {
            return first.Value != second.Value;
        }

    }
}