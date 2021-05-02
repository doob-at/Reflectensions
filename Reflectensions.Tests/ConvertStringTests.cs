using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reflectensions.ExtensionMethods;
using Xunit;
using Xunit.Abstractions;

namespace Reflectensions.Tests
{
    public class ConvertStringTests
    {

        private readonly ITestOutputHelper _output;

        public ConvertStringTests(ITestOutputHelper output) {
            this._output = output;
        }


        [Theory]
        [InlineData("")]
        [InlineData(@"\/Date(1198908717056)\/")]
        [InlineData("2012-03-19T07:22Z")]
        [InlineData("915148798.75")]
        [InlineData("December 17, 1995 03:24:00")]

        public void ConvertNullableToDateTime(string value) {

            var dt = value.ToNullableDateTime();

            _output.WriteLine(dt.ToString());
        }

        //[Theory]
        //[InlineData(@"\/Date(1198908717056)\/")]
        //[InlineData("2012-03-19T07:22Z")]
        //[InlineData("December 17, 1995 03:24:00")]

        //public void ConvertToDateTime(string value) {

        //    var dt = value.ToDateTime();

        //    _output.WriteLine(dt.ToString());
        //}
        
    }
}
