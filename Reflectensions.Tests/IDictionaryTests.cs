﻿using System;
using System.Collections.Generic;
using doob.Reflectensions.ExtensionMethods;
using doob.Reflectensions.Tests.TestClasses;
using Newtonsoft.Json;
using Xunit;

namespace doob.Reflectensions.Tests {
    public class IDictionaryTests {
        [Fact]
        public void Merge() {

            var exp1 = new Expandable1();
            exp1.Name = "Bernhard";
            exp1.Age = 99;
            exp1["Ok"] = true;

            var dict = new Dictionary<string, object>
            {
                ["Key1"] = "Value1",
                ["Key2"] = "Value2"
            };

            var dict2 = new Dictionary<string, object>
            {
                ["Key1"] = "NewValue1",
                ["Key3"] = "NewValue3"
            };

            var mergedDict = dict.Merge(dict2);


            Assert.Equal("NewValue1", mergedDict["Key1"]);
            Assert.Equal("NewValue3", mergedDict["Key3"]);
            Assert.Equal("Value2", mergedDict["Key2"]);

        }

   
    }
}
