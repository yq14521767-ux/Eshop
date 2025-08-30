using System.Security.Cryptography;
using System.Text;

namespace Eshop.Utils
{
    public static class PasswordHelper
    {
        //生成密码哈希 + 盐
        public static void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            using (var hmac =new HMACSHA256())  
            {
                passwordSalt = hmac.Key; //随机盐
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); //把明文密码转成 UTF-8 字节数组，再用 HMAC-SHA256 计算哈希，得到最终的 PasswordHash
            }
        }

        //验证密码
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA256(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        } 

    }
}
