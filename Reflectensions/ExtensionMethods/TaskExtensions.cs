using System.Threading.Tasks;

namespace doob.Reflectensions.ExtensionMethods
{
    public static class TaskExtensions
    {
        public static async Task<TResult?> ConvertToTaskOf<TResult>(this Task task) =>
            await task.ContinueWith(t => 
                t.Reflect().GetPropertyValue<TResult>("Result")
            );
    }
}
