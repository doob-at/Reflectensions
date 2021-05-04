using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reflectensions.ExtensionMethods;

namespace Reflectensions.Internal
{
    internal static class TaskExtensions
    {

        public static async Task<TResult?> ConvertToTaskOf<TResult>(this Task task, bool throwOnError = true, TResult? returnOnError = default)
        {

            if (task == null)
                return returnOnError;

            return await task.ContinueWith(t => {
                return t.Reflect().GetPropertyValue<TResult>("Result");
            });

        }

    }
}
