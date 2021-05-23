using System.IO;

namespace doob.Reflectensions.ExtensionMethods {
    public static class StreamExtensions {

        private static T? AsJsonToObject<T>(this Stream stream)
        {
            var json = new Json();
            return json.FromJsonStreamToObject<T>(stream);
        }

    }
}
