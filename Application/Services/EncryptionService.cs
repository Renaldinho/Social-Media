using Application.Interfaces;

namespace Application.Services;

public class EncryptionService: IEncryptionService
{
    
    /// <summary>
    /// Generates a password hash and a cryptographic salt using the HMACSHA512 algorithm.
    /// </summary>
    /// <param name="password">The plaintext password to hash.</param>
    /// <param name="passwordHash">Output parameter that will contain the hashed password.</param>
    /// <param name="passwordSalt">Output parameter that will contain the cryptographic salt.</param>
    /// <remarks>
    /// The HMACSHA512 algorithm is used for hashing because it provides a secure way to hash passwords.
    /// It generates a unique cryptographic key (used as the salt) for each hashing operation, ensuring that
    /// the hash value is unique even for identical passwords. The salt and hash must be stored for later
    /// verification of passwords.
    /// </remarks>
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    /// <summary>
    /// Verifies a plaintext password against the stored hash and salt.
    /// </summary>
    /// <param name="password">The plaintext password to verify.</param>
    /// <param name="storedHash">The stored hash to compare against.</param>
    /// <param name="storedSalt">The stored salt used to hash the original password.</param>
    /// <returns>True if the password matches the stored hash; otherwise, false.</returns>
    /// <remarks>
    /// This method rehashes the provided password using the stored salt and compares the result
    /// to the stored hash. This is the secure way to verify passwords without storing them in plaintext.
    /// HMACSHA512 is used for the hashing algorithm to match the original hash creation.
    /// </remarks>
    public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return !computedHash.Where((t, i) => t != storedHash[i]).Any();
    }
}