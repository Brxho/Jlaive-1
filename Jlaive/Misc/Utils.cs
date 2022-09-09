using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;

namespace Jlaive
{
    internal class Utils
    {
        public static byte[] GetEmbeddedResource(string name)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            using (var ms = new MemoryStream())
            {
                using (var stream = asm.GetManifestResourceStream(name))
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        public static string GetEmbeddedString(string name)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            using (var stream = new StreamReader(asm.GetManifestResourceStream(name)))
            {
                return stream.ReadToEnd();
            }
        }

        public static byte[] Encrypt(byte[] input, byte[] key, byte[] iv)
        {
            using (var aes = new AesManaged { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 })
            {
                using (var encryptor = aes.CreateEncryptor(key, iv))
                {
                    return encryptor.TransformFinalBlock(input, 0, input.Length);
                }
            }
        }

        public static byte[] Compress(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    {
                        msi.CopyTo(gs);
                        return mso.ToArray();
                    }
                }
            }
        }

        public static bool IsAssembly(string path)
        {
            try
            {
                AssemblyName.GetAssemblyName(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
