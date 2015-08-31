namespace Delbert.Infrastructure.Abstrakt
{
    public interface IJsonAdapter
    {
        string Serialize(object obj);
        T Deserialize<T>(string json);
    }
}