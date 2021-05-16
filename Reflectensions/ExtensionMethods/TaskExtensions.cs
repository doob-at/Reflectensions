using System.Threading.Tasks;
using doob.Reflectensions.ExtensionMethods;

namespace doob.Reflectensions.Internal
{
    public static class TaskExtensions
    {
        public static async Task<TResult?> ConvertToTaskOf<TResult>(this Task task) =>
            await task.ContinueWith(t => 
                t.Reflect().GetPropertyValue<TResult>("Result")
            );
    }
}
