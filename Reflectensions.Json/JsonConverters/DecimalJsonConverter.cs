using System;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace doob.Reflectensions.JsonConverters {
    public class DecimalJsonConverter : JsonConverter {

        public override bool CanRead => false;

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType) {
            return (objectType == typeof(decimal) || objectType == typeof(float) || objectType == typeof(double) || objectType == typeof(int) || objectType == typeof(long));
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
            if (DecimalJsonConverter.IsWholeValue(value)) {
                writer.WriteRawValue(JsonConvert.ToString(Convert.ToInt64(value)));
            } else {
                writer.WriteRawValue(JsonConvert.ToString(value));
            }
        }

        private static bool IsWholeValue(object? value) {
            switch (value)
            {
                case int:
                    return true;
                case decimal decimalValue:
                    return decimalValue == Math.Round(decimalValue, 0, MidpointRounding.AwayFromZero);
                case float floatValue:
                    return floatValue == Math.Truncate(floatValue);
                case double doubleValue:
                    return doubleValue == Math.Truncate(doubleValue);
                default:
                    return false;
            }
        }
    }

}
