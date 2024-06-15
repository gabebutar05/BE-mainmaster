namespace API_Dinamis.Dto
{
    public class AuthDto
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; } = DateTime.UtcNow;
    }
}
