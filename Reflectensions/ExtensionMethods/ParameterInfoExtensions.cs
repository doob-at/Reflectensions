using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace doob.Reflectensions.ExtensionMethods
{
    public static class ParameterInfoExtensions
    {
        public static bool HasAttribute(this ParameterInfo parameterInfo, Type attributeType, bool inherit = false)
        {
            if (parameterInfo == null)
            {
                throw new ArgumentNullException(nameof(parameterInfo));
            }

            if (!attributeType.InheritFromClass<Attribute>())
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' has to be an Attribute Type!");
            }

            return parameterInfo.GetCustomAttribute(attributeType, inherit) != null;
        }

        public static bool HasAttribute(this ParameterInfo parameterInfo, string attributeName, bool inherit = false)
        {
            if (parameterInfo == null)
            {
                throw new ArgumentNullException(nameof(parameterInfo));
            }

            return parameterInfo.GetCustomAttributes().Any(attr => attr.GetType().Name.Equals(attributeName));
        }
    }
}
