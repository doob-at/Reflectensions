using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflectensions.Tests.TestClasses
{
    public class ActionInvokeTestClass<T>
    {
        public T Value { get; set; }

        public ActionInvokeTestClass() {}
        public ActionInvokeTestClass(T value)
        {
            Value = value;
        }
    }
}
