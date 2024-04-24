using ApiCatalogo.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiCatalogo.Services
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            //Obtendo a chave secreta
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ?? throw new InvalidOperationException("Invalid Secret Key");
            //Convertendo a chave para []bytes
            var privateKey = Encoding.UTF8.GetBytes(key);
            //Criando as credencias para criar o token
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);
            //Descrição utilizada para gerar o token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),
                Audience = _config.GetSection("JWT").GetValue<string>("ValidAudience"),
                Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),
                SigningCredentials = signingCredentials
            };
            //Criar e Validar os tokens
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return token;
        }

        public string GenerateRefreshToken()
        {
            //Armazenar bytes aletorios de forma segura
            var secureRandomBytes = new byte[128];
            //Gerador de números aleatórios
            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(secureRandomBytes);
            //Converter os bytes aleatorios em base64 string
            var refreshToken = Convert.ToBase64String(secureRandomBytes);

            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid Key");
            //Parametros de validação para o token expirado
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false
            };
            //Manipular o token
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                                !jwtSecurityToken.Header.Alg.Equals(
                                SecurityAlgorithms.HmacSha256,
                                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }
            return principal;
        }
    }
}
