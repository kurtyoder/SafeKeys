namespace SafeKeys.Library
{
    public interface ICrypto
    {
        string Decrypt(string cipherText, byte[] key, string iv);
        string[] Encrypt(string plainText, byte[] key, string iv = null);
        string GenerateAuthHash(string plaintext);
        byte[] GenerateCrptoKey(string password, byte[] salt, int iterations = 32768, int length = 32);
        byte[] GenerateSalt(int length = 32);
        bool VerifyAuthHash(string plaintext, string hash);
    }
}
