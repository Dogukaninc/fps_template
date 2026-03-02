using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SaveSystem
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting            = Formatting.Indented,
            TypeNameHandling      = TypeNameHandling.Auto,   
            NullValueHandling     = NullValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = new List<JsonConverter> 
            {
                new StringEnumConverter(),
                new Vector2Converter(),
                new Vector3Converter(),
                new Vector4Converter(),
                new QuaternionConverter(),
                new ColorConverter(),
            }
        };

        public static string Serialize<T>(T obj)   => JsonConvert.SerializeObject(obj, Settings);
        public static T      Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, Settings);

        // ─── Garanti Çalışan Non-Generic Dönüştürücüler ──────────────────────────────────────────

        private class Vector2Converter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => objectType == typeof(Vector2);
            
            public override void WriteJson(JsonWriter w, object val, JsonSerializer s)
            { 
                Vector2 v = (Vector2)val;
                w.WriteStartObject(); w.WritePropertyName("x"); w.WriteValue(v.x); w.WritePropertyName("y"); w.WriteValue(v.y); w.WriteEndObject(); 
            }
            
            public override object ReadJson(JsonReader r, Type t, object e, JsonSerializer s)
            { 
                if (r.TokenType == JsonToken.Null) return Vector2.zero;
                var j = JObject.Load(r); 
                return new Vector2((float?)j["x"] ?? 0f, (float?)j["y"] ?? 0f); 
            }
        }

        private class Vector3Converter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => objectType == typeof(Vector3);
            
            public override void WriteJson(JsonWriter w, object val, JsonSerializer s)
            { 
                Vector3 v = (Vector3)val;
                w.WriteStartObject(); w.WritePropertyName("x"); w.WriteValue(v.x); w.WritePropertyName("y"); w.WriteValue(v.y); w.WritePropertyName("z"); w.WriteValue(v.z); w.WriteEndObject(); 
            }
            
            public override object ReadJson(JsonReader r, Type t, object e, JsonSerializer s)
            { 
                if (r.TokenType == JsonToken.Null) return Vector3.zero;
                var j = JObject.Load(r); 
                return new Vector3((float?)j["x"] ?? 0f, (float?)j["y"] ?? 0f, (float?)j["z"] ?? 0f); 
            }
        }
        
        private class Vector4Converter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => objectType == typeof(Vector4);

            public override void WriteJson(JsonWriter w, object val, JsonSerializer s)
            { 
                Vector4 v = (Vector4)val;
                w.WriteStartObject(); w.WritePropertyName("x"); w.WriteValue(v.x); w.WritePropertyName("y"); w.WriteValue(v.y); w.WritePropertyName("z"); w.WriteValue(v.z); w.WritePropertyName("w"); w.WriteValue(v.w); w.WriteEndObject(); 
            }
            
            public override object ReadJson(JsonReader r, Type t, object e, JsonSerializer s)
            { 
                if (r.TokenType == JsonToken.Null) return Vector4.zero;
                var j = JObject.Load(r); 
                return new Vector4((float?)j["x"] ?? 0f, (float?)j["y"] ?? 0f, (float?)j["z"] ?? 0f, (float?)j["w"] ?? 0f); 
            }
        }

        private class QuaternionConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => objectType == typeof(Quaternion);

            public override void WriteJson(JsonWriter w, object val, JsonSerializer s)
            { 
                Quaternion v = (Quaternion)val;
                w.WriteStartObject(); w.WritePropertyName("x"); w.WriteValue(v.x); w.WritePropertyName("y"); w.WriteValue(v.y); w.WritePropertyName("z"); w.WriteValue(v.z); w.WritePropertyName("w"); w.WriteValue(v.w); w.WriteEndObject(); 
            }
            
            public override object ReadJson(JsonReader r, Type t, object e, JsonSerializer s)
            { 
                if (r.TokenType == JsonToken.Null) return Quaternion.identity;
                var j = JObject.Load(r); 
                return new Quaternion((float?)j["x"] ?? 0f, (float?)j["y"] ?? 0f, (float?)j["z"] ?? 0f, (float?)j["w"] ?? 1f); 
            }
        }

        private class ColorConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => objectType == typeof(Color);

            public override void WriteJson(JsonWriter w, object val, JsonSerializer s)
            { 
                Color v = (Color)val;
                w.WriteStartObject(); w.WritePropertyName("r"); w.WriteValue(v.r); w.WritePropertyName("g"); w.WriteValue(v.g); w.WritePropertyName("b"); w.WriteValue(v.b); w.WritePropertyName("a"); w.WriteValue(v.a); w.WriteEndObject(); 
            }
            
            public override object ReadJson(JsonReader r, Type t, object e, JsonSerializer s)
            { 
                if (r.TokenType == JsonToken.Null) return Color.white;
                var j = JObject.Load(r); 
                return new Color((float?)j["r"] ?? 1f, (float?)j["g"] ?? 1f, (float?)j["b"] ?? 1f, (float?)j["a"] ?? 1f); 
            }
        }
    }
}