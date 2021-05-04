using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflectensions {
    public static class ExpandableObjectExtensions {

        public static Dictionary<string, object?> AsDictionary(this ExpandableObject expandableObject, bool omitNullValues = false) {
            var props = expandableObject.GetProperties();
            if (omitNullValues) {
                props = props.Where(p => p.Value != null);
            }
            return props.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static T SwitchType<T>(this ExpandableObject expandableObject) where T : ExpandableObject {

            var exp = Activator.CreateInstance<T>();
            foreach (var keyValuePair in expandableObject.AsDictionary()) {
                exp[keyValuePair.Key] = keyValuePair.Value!;
            }

            return exp;
        }

        public static IEnumerable<T> SwitchType<T>(this IEnumerable<ExpandableObject> expandableObjects) where T : ExpandableObject {
            return expandableObjects.Select(exo => exo.SwitchType<T>());
        }

    }
}
