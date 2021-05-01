using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Reflectensions.ExtensionMethods {
    public static class MethodBaseExtensions {

        public static bool IsExtensionMethod(this MethodBase methodInfo) {
            return methodInfo.IsDefined(typeof(ExtensionAttribute), true);
        }
    }
}
