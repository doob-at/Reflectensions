using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace doob.Reflectensions.CodeDefinition.ExtensionMethods
{
    internal static class EnumExtensions
    {
        public static string AsString(this Enum value)
        {
            if (value == null)
                return null;

            var val = value.ToString("F");
            var field = value.GetType()
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == val);

            return field?.GetCustomAttribute<EnumMemberAttribute>(false)?.Value ?? field?.GetCustomAttribute<DescriptionAttribute>(false)?.Description ?? val;
        }

        public static bool TryGetEnum<T>(this string value, out T result, bool ignoreCase = false) where T: Enum
        {
            var enumType = typeof(T);

            var fields = enumType.GetFields();

            var enumValues = value.Split(',').Select(v => v.Trim()).Where(v => !String.IsNullOrWhiteSpace(v)).ToList();

            if (!enumValues.Any())
            {
                result = default;
                return false;
            }

            var enums = new List<string>();

            var comparsion = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;

            foreach (var val in enumValues)
            {
                foreach (var field in fields)
                {
                    var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
                    if (attribute != null)
                    {
                        if (attribute.Value?.Equals(val, comparsion) == true)
                        {
                            enums.Add(field.Name);
                            continue;
                        }

                    }

                    var descAttribute = field.GetCustomAttribute<DescriptionAttribute>();
                    if (descAttribute != null)
                    {
                        if (descAttribute.Description.Equals(val, comparsion) == true)
                        {
                            enums.Add(field.Name);
                            continue;
                        }

                    }

                    if (field.Name?.Equals(val, comparsion) == true)
                    {
                        enums.Add(field.Name);
                    }
                }
            }


            result = (T)Enum.Parse(enumType, String.Join(",", enums), ignoreCase);
            return true;
        }

    }
}
