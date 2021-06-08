using System.Collections.Concurrent;
using doob.Reflectensions.CodeDefinition.Definitions;

namespace doob.Reflectensions.CodeDefinition
{
    public static class TypeDefinitionCache
    {
        private static ConcurrentDictionary<string, TypeDefinition> Cache { get; } = new();

        internal static void TryAdd(string name, TypeDefinition typeDefinition)
        {
            Cache.TryAdd(name, typeDefinition);
        }

        public static bool TryGetValue(string name, out TypeDefinition? typeDefinition)
        {
            return Cache.TryGetValue(name, out typeDefinition);
        }
    }
}
