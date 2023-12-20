using System.Text.Json;
using System.Text.Json.Serialization;
using BaseLib.Core.Models;

namespace BaseLib.Core.Json
{
    /// <summary>
    /// Only to Flatten the response to support older consumers
    /// </summary>
    public class FlattenCoreReasonCodeConverter : JsonConverter<CoreServiceResponseBase>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            var canConvert = typeof(CoreServiceResponseBase).IsAssignableFrom(typeToConvert);
            return canConvert;
        }

        public override CoreServiceResponseBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, CoreServiceResponseBase value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            writer.WriteStartObject();


            // loop through properties
            foreach (var prop in type.GetProperties())
            {
                var propValue = prop.GetValue(value);
                if (propValue is CoreReasonCode reasonCode)
                {
                    // Flatten CoreReasonCode properties
                    writer.WriteNumber(prop.Name, reasonCode.Value);
                    var reasonName = options.PropertyNamingPolicy == null ? "Reason" : "reason";
                    writer.WriteString(reasonName, reasonCode.Description);
                }
                else
                {
                    writer.WritePropertyName(prop.Name);
                    JsonSerializer.Serialize(writer, propValue, options);
                }
            }
            writer.WriteEndObject();
        }
    }
}