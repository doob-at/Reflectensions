using System;
using System.Collections.Generic;
using doob.Reflectensions.CodeDefinition;
using doob.Reflectensions.Helper;
using doob.Reflectensions.Tests.TestClasses;
using Xunit;

namespace doob.Reflectensions.Tests.TypeTests {
    public class CodeSignatureTests {


        [Theory]
        [InlineData("System.Object", 2, 1)]
        public void StringToTypeSignature(string typeString, int genArgumentsLength, int arrayLength)
        {

            var t = new List<Dictionary<string, object[,][]>>();

            var ta = t.GetType();
            var ts = ta.ToString();

            var typeSignature = new TypeSignature(ts);
            var str = typeSignature.ToString();

            Assert.Equal(typeSignature.GenericArguments.Length, genArgumentsLength);
            Assert.Equal(typeSignature.ArrayDimensions.Length, arrayLength);
            
        }


    }
}
