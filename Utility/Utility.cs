﻿using System;
using System.IO;
using System.Text;
using zlib;

namespace Scholar.Utility
{
    public class Util
    {
        public static Random Random = new Random();


        public static byte[] Hex2ByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        public static string ByteArray2Hex(byte[] bs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        public static byte[] RandByteArray(int len)
        {
            byte[] ret = new byte[len];
            Random.NextBytes(ret);
            return ret;
        }
        public static string Md5(string paramString)
        {
            System.Security.Cryptography.MD5 MD5Instance = System.Security.Cryptography.MD5.Create();
            char[] arrayOfChar;
            byte[] arrayOfByte1;
            byte[] arrayOfByte2;
            arrayOfChar = paramString.ToCharArray();
            arrayOfByte1 = new byte[arrayOfChar.Length];
            for (int j = 0; j < arrayOfChar.Length; ++j)
                arrayOfByte1[j] = (byte)arrayOfChar[j];
            arrayOfByte2 = MD5Instance.ComputeHash(arrayOfByte1);

            return arrayOfByte2.ToHexString();
        }

        private static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }
        /// <summary>
        /// 压缩字节数组
        /// </summary>
        /// <param name="sourceByte">需要被压缩的字节数组</param>
        /// <returns>压缩后的字节数组</returns>
        public static byte[] ZlibCompres(byte[] bin)
        {
            Stream sourceStream = new MemoryStream(bin);
            MemoryStream streamOut = new MemoryStream();
            ZOutputStream streamZOut = new ZOutputStream(streamOut, zlibConst.Z_DEFAULT_COMPRESSION);
            try
            {
                CopyStream(sourceStream, streamZOut);
                return streamOut.ToArray();
            }
            finally
            {
                sourceStream.Dispose();
                streamOut.Dispose();
                streamZOut.Dispose();
            }
          
        }
        private static byte[] ZlibDecompress(byte[] bin)
        {
            Stream sourceStream = new MemoryStream(bin);
            MemoryStream outStream = new MemoryStream();
            ZOutputStream outZStream = new ZOutputStream(outStream);
            try
            {
                CopyStream(sourceStream, outZStream);
                return outStream.ToArray();
            }
            finally
            {
                sourceStream.Dispose();
                outStream.Dispose();
                outZStream.Dispose();
            }
        }

        public static byte[] StreamToBytes(Stream stream)

        {

            byte[] bytes = new byte[2000];

            stream.Write(bytes, 0, 2000);

            return bytes;
        }
    }
}
