﻿using System;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace doob.Reflectensions.JsonConverters
{
    public sealed class ExpandoObjectConverter : CustomCreationConverter<ExpandoObject>
    {

        public override ExpandoObject Create(Type objectType)
        {
            return new ExpandoObject();
        }

        public override bool CanConvert(Type objectType)
        {
            // in addition to handling ExpandoObject
            // we want to handle the deserialization of dict value
            // which is of type object
            return objectType == typeof(object) || base.CanConvert(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject
                || reader.TokenType == JsonToken.Null)
                return base.ReadJson(reader, objectType, existingValue, serializer);

            // if the next token is not an object
            // then fall back on standard deserializer (strings, numbers etc.)
            return serializer.Deserialize(reader);
        }
    }
}
