namespace Habr.Security.Helpers.Interfaces
{
    public interface IPasswordHasher
    {
        byte[] GetHash(string password);

        string GetHashString(string password);
    }
}
