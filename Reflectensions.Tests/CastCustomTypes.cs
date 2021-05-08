using System;
using doob.Reflectensions.ExtensionMethods;
using doob.Reflectensions.Tests.TestClasses;
using doob.Reflectensions.Tests.TestEnums;
using Xunit;

namespace doob.Reflectensions.Tests
{
    public class CastCustomTypes
    {


        [Theory]
        [InlineData(typeof(Camaro), typeof(Truck))]
        public void From_IsCastableTo(Type from, Type to) {
            var isCastable = from.IsImplicitCastableTo(to);
            Assert.True(isCastable);
        }


        [Theory]
        [InlineData(typeof(Truck), typeof(Camaro))]

        public void From_IsNOTCastableTo(Type from, Type to) {
            var isCastable = from.IsImplicitCastableTo(to);
            Assert.False(isCastable);
        }

        [Fact]
        public void FromEmptyStringToNullableDateTime() {

            var str = "2018-03-21T15:50:17+00:00";

            DateTime? nullDate = str.Reflect().To<DateTime?>();
            DateTime date = str.Reflect().To<DateTime>();

            var str2 = "";

            DateTime? nullDate2 = str2.Reflect().To<DateTime?>(null);
            DateTime date2 = str2.Reflect().To<DateTime>(DateTime.Now);

        }


        [Theory]
        [InlineData("1")]
        [InlineData("12345")]
        public void FromStringToInt(string value) {

           

            int? _nullInt = value.Reflect().To<int?>();
            int _int = value.Reflect().To<int>();

           Assert.Equal(_nullInt, int.Parse(value));
           Assert.Equal(_int, int.Parse(value));
        }

        [Theory]
        [InlineData("1,123")]
        [InlineData("12345,123")]
        public void FromStringToDouble(string value) {

            double? _nullInt = value.Reflect().To<double?>();
            double _int = value.Reflect().To<double>();

            Assert.Equal(_nullInt, double.Parse(value));
            Assert.Equal(_int, double.Parse(value));
        }

        [Theory]
        [InlineData("a5ac3789-a5ef-4a49-81bb-fc24a6e47244")]
        public void FromStringToGuid(string value) {

            Guid? nullg = $"{value}!!".Reflect().To<Guid?>(null);
            Guid g = value.Reflect().To<Guid>();
            
        }

        [Theory]
        [InlineData("ja", true)]
        [InlineData("yes", true)]
        [InlineData("si", false)]
        [InlineData("hai", true)]
        [InlineData("oui", false)]
        [InlineData(Simplest.Third, true)]
        [InlineData(NoFlags.Eins | NoFlags.Drei, true)]
        [InlineData(NoFlags.Drei, false)]
        [InlineData(WithFlags.Two, true)]
        public void ReflectToBoolean(object value, bool assert)
        {

            var truevalues = new object[]
            {
                "ja",
                "yes",
                "hai",
                Simplest.Third,
                NoFlags.Eins | NoFlags.Drei,
                WithFlags.Two
            };

            var val = value.Reflect().ToBoolean(truevalues);

            Assert.Equal(assert, val);

        }
    }
}
