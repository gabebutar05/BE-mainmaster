using Microsoft.EntityFrameworkCore;

namespace API_Dinamis.Models
{
    //[Keyless]
    public class Authx
    {
        private int id1;

        public int id { get => id1; set => id1 = value; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpiryTime { get; set; } = DateTime.UtcNow;
        //public string sysname { get; set; } = null!;
        //public User(string userName, string password)
        //{
        //    UserName = userName;
        //    Password = password;
        //}
    }
}
