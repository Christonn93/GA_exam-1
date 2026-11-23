namespace Kursplassen.Core.Interfaces;

public interface IPassHashService
{
    // Hash a plaintext password for storage (never store plaintext)
    string Hash(string password);

    // Check a plaintext password against a stored hash
    bool Verify(string password, string passwordHash);
}