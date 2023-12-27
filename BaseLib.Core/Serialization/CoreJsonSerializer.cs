using System.Text.Json;
using BaseLib.Core.Models;

namespace BaseLib.Core.Serialization
{
    public class CoreJsonSerializer : ICoreSerializer
    {
        private readonly JsonSerializerOptions options;

        public CoreJsonSerializer()
        {
            this.options = new JsonSerializerOptions();
            this.options.Converters.Add(new PolymorphicConverter<CoreRequestBase>());
            this.options.Converters.Add(new PolymorphicConverter<CoreResponseBase>());
        }

        public string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value, options);
        }

        public T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}