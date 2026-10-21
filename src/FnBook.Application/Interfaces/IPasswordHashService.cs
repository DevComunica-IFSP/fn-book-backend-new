namespace FnBook.Application.Interfaces;

public interface IPasswordHashService
{
    string Hash(string password);
    bool Verify(string hash, string password);
}
