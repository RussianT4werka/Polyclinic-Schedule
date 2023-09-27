using Polyclinic_Schedule.Model;
using System.Security.Cryptography;
using System.Text;

namespace Polyclinic_Schedule.Tools
{
    public class Hash
    {
        public static string HashPassword(UserData user)
        {
            string hashPass;
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            hashPass = Encoding.UTF8.GetString(hash);
            return hashPass;
        }
    }
}
