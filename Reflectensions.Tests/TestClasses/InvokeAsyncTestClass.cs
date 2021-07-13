using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doob.Reflectensions.ExtensionMethods;
using doob.Reflectensions.Helper;

namespace doob.Reflectensions.Tests.TestClasses
{
    public class InvokeAsyncTestClass
    {


        public async Task<object> GetNameAsync() {
            var methodInfo = typeof(InvokeAsyncTestClass).GetMethods()
                .WithName(nameof(GetNameAsyncAsObject)).First();
            var generic = methodInfo.MakeGenericMethod(typeof(string));
            var res = await InvokeHelper.InvokeMethodAsync(this, generic, new object[0]);
            return res;
        }

        public Task<T> GetNameAsyncAsObject<T>() {
            return Task.FromResult<T>(default);
        }
    }
}
