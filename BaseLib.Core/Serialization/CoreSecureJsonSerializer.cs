using System.Text.Json;
using BaseLib.Core.Models;
using BaseLib.Core.Security;

namespace BaseLib.Core.Serialization
{
    public class CoreSecureJsonSerializer : ICoreSerializer
    {
        private readonly JsonSerializerOptions options;

        public CoreSecureJsonSerializer(IEncryptionKeyProvider keyProvider)
        {
            this.options = new JsonSerializerOptions();
            this.options.Converters.Add(new SecurePolymorphicConverter<CoreRequestBase>(keyProvider));
            this.options.Converters.Add(new SecurePolymorphicConverter<CoreResponseBase>(keyProvider));
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