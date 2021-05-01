using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Reflectensions.ExtensionMethods;
using Reflectensions.Tests.TestClasses;
using Xunit;

namespace Reflectensions.Tests
{
    public class ActionTests
    {

        private Action<ActionInvokeTestClass<int>> action1 = 
            (first) =>
            {
                first.Value++;
            };

        private Action<ActionInvokeTestClass<int>, ActionInvokeTestClass<string>> action2 =
            (first, second) =>
            {
                first.Value++;
                second.Value = $"{second.Value}{first.Value}";
            };

        private Action<ActionInvokeTestClass<int>, ActionInvokeTestClass<string>, ActionInvokeTestClass<DateTime>> action3 =
            (first, second, third) =>
            {
                first.Value++;
                second.Value = $"{second.Value}{first.Value}";
                third.Value = third.Value.AddDays(first.Value);
            };


        [Fact]
        public void ActionWithOneParamTest()
        {

            var result1 = action1.InvokeAction();
            Assert.Equal(1, result1.Value);

            var instance11 = new ActionInvokeTestClass<int>(10);
            var result2 = action1.InvokeAction(instance11);

            Assert.Equal(11, instance11.Value);
            Assert.Equal(instance11, result2);
        }

        [Fact]
        public void ActionWithTwoParamTest()
        {

            var (first, second) = action2.InvokeAction();
            Assert.Equal(1, first.Value);
            Assert.Equal("1", second.Value);

            var instance21 = new ActionInvokeTestClass<int>(10);
            var instance22 = new ActionInvokeTestClass<string>("Test");
            var(first2, second2) = action2.InvokeAction(instance21, instance22);

            Assert.Equal(11, first2.Value);
            Assert.Equal(instance21, first2);

            Assert.Equal("Test11", second2.Value);
            Assert.Equal(instance22, second2);
        }

        [Fact]
        public void ActionWithThreeParamTest()
        {

            var (first, second, third) = action3.InvokeAction();
            Assert.Equal(1, first.Value);
            Assert.Equal("1", second.Value);
            Assert.Equal(new DateTime().AddDays(first.Value), third.Value);


            var instance21 = new ActionInvokeTestClass<int>(10);
            var instance22 = new ActionInvokeTestClass<string>("Test");
            
            var (first2, second2, third2) = action3.InvokeAction(instance21, instance22);

            Assert.Equal(11, first2.Value);
            Assert.Equal(instance21, first2);

            Assert.Equal("Test11", second2.Value);
            Assert.Equal(instance22, second2);

            Assert.Equal( new DateTime().AddDays(first2.Value), third2.Value);
            
        }
    }
}
