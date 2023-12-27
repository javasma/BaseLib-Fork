namespace BaseLib.Core.Serialization
{
    public interface ICoreSerializer
    {
        string Serialize<T>(T value);
        T? Deserialize<T>(string json);
    }
}