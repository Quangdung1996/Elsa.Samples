using Elsa.Samples.Models;

namespace Elsa.Samples.Interfaces
{
    public interface IPasswordHasher
    {
        HashedPassword HashPassword(string password);

        HashedPassword HashPassword(string password, byte[] salt);
    }
}