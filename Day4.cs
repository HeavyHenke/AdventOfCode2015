using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApplication5
{
    public class Day4
    {
        public void Calculate()
        {
            string input = "yzbqklnj";
            var md5 = MD5.Create();

            for (int i = 0; true; i++)
            {
                string code = input + i;
                var inputBuffer = Encoding.Default.GetBytes(code);
                byte[] hash = md5.ComputeHash(inputBuffer, 0, inputBuffer.Length);
                string hexHash = ToHex(hash, 6);
                if (hexHash.StartsWith("000000"))
                {
                    Console.WriteLine("Found it: " + i);
                    return;
                }
            }
        }

        public string ToHex(byte[] data, int numChars)
        {
            char[] idxToHex = "0123456789ABCDEF".ToArray();
            int i = 0;

            StringBuilder sb = new StringBuilder(numChars);
            foreach (var b in data)
            {
                sb.Append(idxToHex[(b&0xf0) >> 4]);
                sb.Append(idxToHex[b & 0xF]);
                i += 2;
                if (i >= numChars)
                    return sb.ToString();
            }

            return sb.ToString();
        }
    }
}