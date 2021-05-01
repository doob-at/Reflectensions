using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Reflectensions.Classes;

namespace Reflectensions.ExtensionMethods
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
