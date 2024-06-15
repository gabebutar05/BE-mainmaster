using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_Dinamis.Repository;
using AutoMapper;
using System.Security.Cryptography;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Dto;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authrepository;
        IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IKeyRepository _keys;

        public AuthController(IAuthRepository authrepository, IConfiguration _configuration, IMapper mapper, IPasswordHasher passwordHasher, IKeyRepository keys)
        {
            _authrepository = authrepository;
            this._configuration = _configuration;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _keys = keys;
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult Auth([FromBody] User user)
        //{
        //    IActionResult response = Unauthorized();

        //    if (user != null)
        //    {
        //        if (user.UserName.Equals("test@gmail.com") && user.Password.Equals("a"))
        //        {
        //            var issuer = configuration["Jwt:Issuer"];
        //            var audience = configuration["Jwt:Audience"];
        //            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
        //            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        //            var subject = new ClaimsIdentity(new[] { new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //                                                new Claim(JwtRegisteredClaimNames.Email, user.UserName),});

        //            var expires = DateTime.UtcNow.AddMinutes(15);

        //            var tokenDescriptor = new SecurityTokenDescriptor
        //            {
        //                Subject = subject,
        //                Expires = expires,
        //                //Expires = DateTime.UtcNow.AddMinutes(15),
        //                Issuer = issuer,
        //                Audience = audience,
        //                SigningCredentials = signingCredentials
        //            };

        //            var tokenHandler = new JwtSecurityTokenHandler();
        //            var token = tokenHandler.CreateToken(tokenDescriptor);
        //            var jwtToken = tokenHandler.WriteToken(token);

        //            return Ok(jwtToken);
        //        }
        //    }

        //    return response;
        //}

        [HttpPost("CreateLoginUser/")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateLoginUser([FromBody] AuthDto auth)
        {
            if (!_keys.KeyExists())
            {
                string inputString = "Hello, Todays date is " + DateTime.Now;
                byte[] byteArray = Encoding.UTF8.GetBytes(inputString);
                //BitConverter.ToString(byteArray).Replace("-", "");

                //var key_ = _keys.GetKeys();

                var key_ = new Key();

                var create_key = _mapper.Map<Key>(key_);
                create_key.Keys = BitConverter.ToString(byteArray).Replace("-", "");

                _keys.CreateKey(create_key);
            }

            if (auth != null)
            {
                var passHash = _passwordHasher.Hash(auth.Password);

                auth.Password = passHash;

                var authMap = _mapper.Map<Authx>(auth);

                _authrepository.UpdateAuth(authMap);

                return Ok();
            }
            else
            {
                return BadRequest("No Data Posted");
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Auth([FromBody] AuthDto auth)
        {
            if (auth != null)
            {
                //================================================================================================================
                //var passHash = _passwordHasher.Hash(auth.Password);

                //auth.Password = passHash;

                //var authMap = _mapper.Map<Authx>(auth);

                //_authrepository.UpdateAuth(authMap);

                //return Ok();
                //================================================================================================================


                //================================================================================================================
                if (!_authrepository.AuthExists(auth.UserName))
                {
                    throw new Exception("username or password invalid");
                    //return NotFound("username or password invalid");
                }

                var auth_id_hash = _authrepository.Getid2(auth.UserName);

                var result = _passwordHasher.Verify(auth_id_hash.Password, auth.Password);

                if (!result)
                {
                    throw new Exception("username or password invalid");
                }

                var refreshToken = GenerateRefreshToken();
                int _id = auth_id_hash.id;
                string _user = auth_id_hash.UserName;
                string _pass = auth_id_hash.Password;

                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("userid", _id.ToString()),
                        new Claim("refreshtoken", refreshToken)
                    };

                string fixedString = GenerateFixedString(_keys.GetKey());

                //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(fixedString));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: signIn);

                var _token = new JwtSecurityTokenHandler().WriteToken(token);

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                auth.id = _id;
                auth.AccessToken = _token;
                auth.RefreshToken = refreshToken;
                auth.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);
                auth.UserName = _user;
                auth.Password = _pass;

                var authMap = _mapper.Map<Authx>(auth);

                _authrepository.UpdateAuth(authMap);

                return Ok(_token);

                //return Ok();
                //================================================================================================================


                //================================================================================================================
                ////if (auth.UserName.Equals("admin") && auth.Password.Equals("admin"))
                //if (_authrepository.AuthExists(auth.UserName, auth.Password))
                //{
                //    var refreshToken = GenerateRefreshToken();
                //    var auth_id = _authrepository.Getid(auth.UserName, auth.Password);
                //    int _id = auth_id.id;
                //    string _user = auth_id.UserName;
                //    string _pass = auth_id.Password;

                //    var claims = new[] {
                //        //new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //        //new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                //        new Claim(ClaimTypes.Name, _id.ToString()),
                //        //new Claim("UserName", auth.UserName),
                //        //new Claim("Password", auth.Password),
                //        new Claim(ClaimTypes.Actor, refreshToken)
                //    };

                //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                //    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                //    var token = new JwtSecurityToken(
                //        _configuration["Jwt:Issuer"],
                //        _configuration["Jwt:Audience"],
                //        claims,
                //        expires: DateTime.UtcNow.AddMinutes(15),
                //        signingCredentials: signIn);


                //    //auth.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);

                //    var _token = new JwtSecurityTokenHandler().WriteToken(token);

                //    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                //    auth.id = _id;
                //    auth.AccessToken = "";
                //    auth.RefreshToken = refreshToken;
                //    auth.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);
                //    auth.UserName = _user;
                //    auth.Password = _pass;

                //    var authMap = _mapper.Map<Authx>(auth);

                //    _authrepository.UpdateAuth(authMap);

                //    return Ok(_token);
                //}
                //else
                //{
                //    return BadRequest("User or password invalid");
                //}
                //================================================================================================================
            }
            else
            {
                return BadRequest("No Data Posted");
            }
        }

        [HttpPost("refreshtoken/")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RefreshToken([FromBody] AuthDto auth)
        {
            if (auth is null)
            {
                return BadRequest("Invalid client request");
            }

            var principal = GetPrincipalFromExpiredToken(auth.AccessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            //string username = principal.Identity.Name;
            string username = principal.FindFirst("userid")?.Value;
            string refresh = principal.FindFirst("refreshtoken")?.Value;
            var user = _authrepository.Getid_id(int.Parse(username));

            if (user == null || user.RefreshToken != refresh || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var refreshToken = GenerateRefreshToken();

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("userid", username),
                        new Claim("refreshtoken", refreshToken)
                    };

            string fixedString = GenerateFixedString(_keys.GetKey());

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(fixedString));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: signIn);

            var _token = new JwtSecurityTokenHandler().WriteToken(token);

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            auth.UserName = user.UserName;
            auth.Password = user.Password;
            auth.id = int.Parse(username);
            auth.AccessToken = _token;
            auth.RefreshToken = refreshToken;
            auth.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);

            var authMap = _mapper.Map<Authx>(auth);

            _authrepository.UpdateAuth(authMap);

            return Ok(_token);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            string fixedString = GenerateFixedString(_keys.GetKey());

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"])),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(fixedString)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        static byte[] GenerateKeyBytes(string customValue)
        {
            // Generate a random 128-bit key
            byte[] randomBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            // Convert the custom value to bytes
            byte[] customBytes = Encoding.UTF8.GetBytes(customValue);

            // Combine the random bytes with the custom bytes
            for (int i = 0; i < randomBytes.Length; i++)
            {
                randomBytes[i] ^= customBytes[i % customBytes.Length];
            }

            return randomBytes;
        }

        static string GenerateFixedString(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Take the first 16 bytes (128 bits) to get a 128-bit string
                byte[] resultBytes = new byte[16];
                Array.Copy(hashBytes, resultBytes, 16);

                // Convert the byte array to a hexadecimal string
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in resultBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}
