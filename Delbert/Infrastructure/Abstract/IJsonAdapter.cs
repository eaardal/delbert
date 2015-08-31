namespace Delbert.Infrastructure.Abstract
{
    public interface IJsonAdapter
    {
        string Serialize(object obj);
        T Deserialize<T>(string json);
    }
}