namespace SimpleSocialAuth.Core
{
    public interface ISessionStorage
    {
        object Load(string key);
        void Store(string key, object value);
    }
}