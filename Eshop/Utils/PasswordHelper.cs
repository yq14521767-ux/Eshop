using System.Security.Cryptography;
using System.Text;

namespace Eshop.Utils
{
    public static class PasswordHelper
    {
        //生成密码哈希 + 盐
        public static void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            using (var rng = RandomNumberGenerator.Create())   //创建加密安全的随机数生成器
            {
                passwordSalt = new byte[16]; // 创建一个长度为16字节的字节数组（用于存储盐值）
                rng.GetBytes(passwordSalt); // 生成随机盐，填充数组
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, passwordSalt, 100000, HashAlgorithmName.SHA256)) //实现 PBKDF2 算法
            {
                passwordHash = pbkdf2.GetBytes(32); //256位哈希
            }
        }

        //验证密码
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password,storedSalt,100000,HashAlgorithmName.SHA256))
            {
                var computedHash = pbkdf2.GetBytes(32);//// 长度与 CreatePasswordHash 保持一致
                return computedHash.SequenceEqual(storedHash);
            }
        } 

    }
}
