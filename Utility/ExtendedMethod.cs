using Scholar.Utility;

namespace System
{
    public static class StringExtendedMethod
    {
        /// <summary>
        /// 将十六进制形式表示的文本转换成数组
        /// </summary>
        /// <returns></returns>
        public static byte[] ToByteArray(this string str)
        {
            return Util.Hex2ByteArray(str);
        }
    }
    public static class ByteArrayExtendedMethod
    {
        /// <summary>
        /// 将数组转换成十六进制形式表示的文本
        /// </summary>
        /// <returns></returns>
        public static string ToHexString(this byte[] str)
        {
            return Util.ByteArray2Hex(str);
        }
    }
}
