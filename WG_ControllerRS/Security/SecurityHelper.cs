using System.Security.Cryptography;
using System.Text;

namespace WishGrid.Security
{
    public class SecurityHelper
    {

        /// <summary>
        /// verifies a value hashed and salted
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueHash"></param>
        /// <param name="valueSalt"></param>
        /// <returns></returns>
        public static bool VerifyEncryption(string value, byte[] valueHash, byte[] valueSalt)
        {
            using (var sha512 = new HMACSHA512(valueSalt))
            {
                var computedHash = sha512.ComputeHash(Encoding.UTF8.GetBytes(value));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != valueHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// create a value hashed and salted
        /// </summary>
        /// <param name="value">The value to encrypt</param>
        /// <param name="valueHash"></param>
        /// <param name="valueSalt"></param>
        public static void Encrypt(string value, out byte[] valueHash, out byte[] valueSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                valueSalt = hmac.Key;
                valueHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        //public static unsafe bool UnsafeCompare(byte[] value1, byte[] value2)
        //{
        //    if (value1 == value2) return true;
        //    if (value1 == null || value2 == null || value1.Length != value2.Length)
        //        return false;
        //    fixed (byte* p1 = value1, p2 = value2)
        //    {
        //        byte* x1 = p1, x2 = p2;
        //        int l = value1.Length;
        //        for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
        //            if (*((long*)x1) != *((long*)x2)) return false;
        //        if ((l & 4) != 0) { if (*((int*)x1) != *((int*)x2)) return false; x1 += 4; x2 += 4; }
        //        if ((l & 2) != 0) { if (*((short*)x1) != *((short*)x2)) return false; x1 += 2; x2 += 2; }
        //        if ((l & 1) != 0) if (*((byte*)x1) != *((byte*)x2)) return false;
        //        return true;
        //    }
        //}
    }
}
