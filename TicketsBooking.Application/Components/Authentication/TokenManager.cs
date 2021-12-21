using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TicketsBooking.Crosscut.Settings;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Authentication
{
    public class TokenManager: ITokenManager
    {
        private readonly JwtSettings _jwtTokenConfig;
        private readonly byte[] _secret;

        public TokenManager(JwtSettings jwtTokenConfig)
        {
            _jwtTokenConfig = jwtTokenConfig;
            _secret = Encoding.UTF8.GetBytes(_jwtTokenConfig.Secret);
        }

        public string GenerateToken(User user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(_secret);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
