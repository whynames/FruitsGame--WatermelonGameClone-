using System.IO;
using System.Security.Cryptography;

namespace _Assets.Scripts.Misc
{
    public static class HashHelper
    {
        public static byte[] GetFileHash(string filePath)
        {
            HashAlgorithm sha1 = HashAlgorithm.Create();
            using(FileStream stream = new FileStream(filePath,FileMode.Open,FileAccess.Read))
                return sha1.ComputeHash(stream);
        }
    }
}