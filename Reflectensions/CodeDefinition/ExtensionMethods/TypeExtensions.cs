namespace doob.Reflectensions.CodeDefinition.ExtensionMethods
{
    internal static class TypeExtensions
    {
        public static Modifier GetModifier(this Type type)
        {
            var attributes = type.Attributes;

            if ((attributes & TypeAttributes.Public) == TypeAttributes.Public)
            {
                return Modifier.Public;
            }

            if ((attributes & TypeAttributes.NestedFamORAssem) == TypeAttributes.NestedFamORAssem)
            {
                return Modifier.ProtectedInternal;
            }

            if ((attributes & TypeAttributes.NestedFamily) == TypeAttributes.NestedFamily)
            {
                return Modifier.Protected;
            }

            if ((attributes & TypeAttributes.NestedAssembly) == TypeAttributes.NestedAssembly)
            {
                return Modifier.Internal;
            }

            if ((attributes & TypeAttributes.NestedFamANDAssem) == TypeAttributes.NestedFamANDAssem)
            {
                return Modifier.PrivateProtected;
            }

            if ((attributes & TypeAttributes.NestedPrivate) == TypeAttributes.NestedPrivate)
            {
                return Modifier.Private;
            }

            return Modifier.None;
        }

        public static Kind GetKind(this Type type)
        {
            if (type.IsClass)
            {
                return Kind.Class;
            }
            
            if (type.IsInterface)
            {
                return Kind.Interface;
            }
            
            if (type.IsEnum)
            {
                return Kind.Enum;
            }
            
            if (type.IsValueType)
            {
                return Kind.Struct;
            }

            return Kind.None;
        }


        public static bool IsGenericTypeParameter(this Type type)
        {
            return type.IsGenericParameter && (object)type.DeclaringMethod == null;
        }

    }
}
