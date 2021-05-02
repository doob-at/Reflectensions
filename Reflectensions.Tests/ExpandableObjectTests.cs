using System;
using System.Collections.Generic;
using Reflectensions.Classes;
using Reflectensions.Tests.TestClasses;
using Xunit;

namespace Reflectensions.Tests {
    public class ExpandableObjectTests {
       
        [Fact]
        public void AsDictionary() {

            var exp2 = new Expandable2();
            exp2.Name = "Bernhard";
            exp2.Age = 99;
            exp2["Ok"] = true;

            var idict = exp2 as IDictionary<string, object>;
            
            Assert.Equal(true, idict["Ok"]);
            Assert.Equal("Bernhard", idict["Name"]);
            Assert.Equal("99", idict["Age"].ToString());

        }

        [Fact]
        public void FromDictionary() {


            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict["test"] = "123";
            var exp = new ExpandableObject(dict);

            Assert.Equal("123", exp.GetValue<string>("test"));
            
        }
    }
}
