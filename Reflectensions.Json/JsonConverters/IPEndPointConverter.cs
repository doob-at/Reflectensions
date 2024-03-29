﻿using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace doob.Reflectensions.JsonConverters
{
    public class IpEndPointConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPEndPoint));
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            IPEndPoint ep = (IPEndPoint)value!;
            writer.WriteStartObject();
            writer.WritePropertyName("Address");
            serializer.Serialize(writer, ep.Address);
            writer.WritePropertyName("Port");
            writer.WriteValue(ep.Port);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            IPAddress address = jo["Address"]?.ToObject<IPAddress>(serializer) ?? throw new ArgumentNullException("Address");
            int port = jo["Port"]?.Value<int>() ?? throw new ArgumentNullException("Address");
            return new IPEndPoint(address, port);
        }
    }
}
