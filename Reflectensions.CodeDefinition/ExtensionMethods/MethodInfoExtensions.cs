using System.Reflection;

namespace doob.Reflectensions.CodeDefinition.ExtensionMethods
{
    internal static class MethodInfoExtensions
    {
        internal static Modifier GetModifier(this MethodInfo methodInfo)
        {
            var attributes = methodInfo.Attributes;

            if ((attributes & MethodAttributes.Public) == MethodAttributes.Public)
            {
                return Modifier.Public;
            }

            if ((attributes & MethodAttributes.FamORAssem) == MethodAttributes.FamORAssem)
            {
                return Modifier.ProtectedInternal;
            }

            if ((attributes & MethodAttributes.Family) == MethodAttributes.Family)
            {
                return Modifier.Protected;
            }

            if ((attributes & MethodAttributes.Assembly) == MethodAttributes.Assembly)
            {
                return Modifier.Internal;
            }

            if ((attributes & MethodAttributes.FamANDAssem) == MethodAttributes.FamANDAssem)
            {
                return Modifier.Protected;
            }

            if ((attributes & MethodAttributes.Private) == MethodAttributes.Private)
            {
                return Modifier.Private;
            }

            return Modifier.None;
        }
    }
}
