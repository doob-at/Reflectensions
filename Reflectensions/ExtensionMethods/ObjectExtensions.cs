using doob.Reflectensions.Classes;

namespace doob.Reflectensions.ExtensionMethods
{
    public static class ObjectExtensions
    {

        public static ObjectReflection Reflect(this ObjectReflection reflectionObject)
        {
            return reflectionObject;
        }

        public static ObjectReflection Reflect(this object reflectionObject)
        {
            return new ObjectReflection(reflectionObject);
        }

    }
}
