namespace BaseLib.Core.Serialization
{
    /// <summary>
    /// Static class provides added core functionality to serialize/deserialize objects or value types to/from JSON
    /// </summary>
    public static class CoreSerializer
    {
        static ICoreSerializer? serializer;

        public static void Initialize(ICoreSerializer s)
        {
            serializer = s;
        }

        public static string Serialize<T>(T value)
        {
            if (serializer == null) throw new NullReferenceException("Serializer not present, invoke static Initialize(ICoreSerializer)");
            return serializer.Serialize(value);
        }

        public static T? Deserialize<T>(string json)
        {
            if (serializer == null) throw new NullReferenceException("Serializer not present, invoke static Initialize(ICoreSerializer)");
            return serializer.Deserialize<T>(json);
        }
    }
}