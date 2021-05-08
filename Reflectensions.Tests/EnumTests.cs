using System;
using doob.Reflectensions.ExtensionMethods;
using Reflectensions.Tests.TestEnums;
using Xunit;
using Xunit.Abstractions;

namespace Reflectensions.Tests
{
    public class EnumTests
    {

        private readonly ITestOutputHelper _output;

        public EnumTests(ITestOutputHelper output) {
            this._output = output;
        }


        [Theory]
        [InlineData(NoFlags.Eins)]
        [InlineData(NoFlags.Zwei)]
        [InlineData(NoFlags.Drei)]
        [InlineData(NoFlags.Eins | NoFlags.Zwei)]
        [InlineData(NoFlags.Zwei | NoFlags.Eins | NoFlags.Drei)]
        [InlineData(WithFlags.One)]
        [InlineData(WithFlags.Two)]
        [InlineData(WithFlags.Three)]
        [InlineData(WithFlags.One | WithFlags.Two)]
        [InlineData(WithFlags.Two | WithFlags.One | WithFlags.Three)]
        [InlineData(Simplest.First)]
        [InlineData(Simplest.Second)]
        [InlineData(Simplest.Third)]
        [InlineData(Simplest.First | Simplest.Second | Simplest.Zero)]
        [InlineData(Simplest.Second | Simplest.First | Simplest.Third)]

        public void GetEnumNames(Enum value) {


            var names = value.GetName();
            
            _output.WriteLine($"GetName() - '{names}'");

            value.GetType().TryFind(names, out var ens);
            _output.WriteLine($"Parsed - {((Enum)ens).ToString("F")}");
        }

        [Fact]
        public void ParseEnumNames() {

            var find = "";

           
           //var tryp = Enum.TryParse(typeof(Simplest), find, out var tens);
           // _output.WriteLine($"TryParse - {tryp}, {((Enum)tens)?.ToString("F")}");

            
            var tryf = typeof(Simplest).TryFind(find, out var ens);
            _output.WriteLine($"TryFind - {tryf}, {((Enum)ens)?.ToString("F")}");
        }
    }
}
