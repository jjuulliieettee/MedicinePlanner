using MedicinePlanner.WebApi.Auth.Configs;
using MedicinePlanner.WebApi.Auth.Dtos;
using MedicinePlanner.WebApi.Auth.Services.Interfaces;
using MedicinePlanner.WebApi.Dtos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedicinePlanner.WebApi.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthConfigsManager _authConfigsManager;
        public AuthService(AuthConfigsManager authConfigsManager)
        {
            _authConfigsManager = authConfigsManager;
        }

        public LoginResponseDto Login(UserReadDto user)
        {
            ClaimsIdentity identity = GetIdentity(user);

            DateTime now = DateTime.UtcNow;

            JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: _authConfigsManager.GetIssuer(),
                    audience: _authConfigsManager.GetAudience(),
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(_authConfigsManager.GetLifetime())),
                    signingCredentials: new SigningCredentials(_authConfigsManager.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new LoginResponseDto
            {
                AccessToken = encodedJwt
            };
        }

        private ClaimsIdentity GetIdentity(UserReadDto user)
        {
            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim("UserId", user.Id),
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
