using System.Text;

using UrlShortener.Interfaces;

namespace UrlShortener.Concretes.Hashing;

public class MD5HashProvider : IHashProvider<string, byte[]>
{
    public byte[] ComputeHash(string toHash)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(toHash);
        return inputBytes; 
    }
}