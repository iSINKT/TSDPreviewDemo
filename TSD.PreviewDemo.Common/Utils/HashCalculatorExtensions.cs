using System.IO;
using System.Security.Cryptography;
using System.Text;
using TSD.PreviewDemo.Common.Extensions;

namespace TSD.PreviewDemo.Common.Utils
{
    public static class HashCalculatorExtensions
    {
        public static string Sha256Hash(this Stream stream)
        {
            stream.ThrowIfNull(nameof(stream));

            var hash = string.Empty;

            if (!stream.CanRead) return hash;

            if (stream.Position != 0)
                stream.Position = 0L;

            using (var mySha256 = SHA256.Create())
            {
                var hashValue = mySha256.ComputeHash(stream);
                var sb = new StringBuilder();
                foreach (var t in hashValue)
                {
                    sb.Append($"{t:X2}");
                }
                hash = sb.ToString();
            }

            return hash.ToLower();
        }
    }
}
