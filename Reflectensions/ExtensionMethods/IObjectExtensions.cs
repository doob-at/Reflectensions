using doob.Reflectensions.Common.Classes;
using doob.Reflectensions.Internal;

namespace doob.Reflectensions.ExtensionMethods
{
    public static class IObjectExtensions
    {

        public static IObjectReflection Reflect(this IObjectReflection reflectionObject)
        {
            return reflectionObject;
        }

        public static IObjectReflection Reflect(this object reflectionObject)
        {
            return new ObjectReflection(reflectionObject);
        }

    }
}
