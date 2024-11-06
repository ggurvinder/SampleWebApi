using Microsoft.IdentityModel.Tokens;
using SampleWebApi.Models.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleWebApi.Data.Repositories
{
    public interface ITokenRepository
    {
        string CreateJwtToken(ApplicationUser user, List<string> roles);
    }
    public class TokenRepository : ITokenRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;
        private readonly IConfiguration _configuration;

        public TokenRepository(NZWalksDbContext nZWalksDbContext,IConfiguration configuration)
        {
            this._nZWalksDbContext = nZWalksDbContext;
            this._configuration = configuration;
        }
        public string CreateJwtToken(ApplicationUser user, List<string> roles)
        {
            //create claims

            var claims=new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            foreach (var role in roles) {
                claims.Add(new Claim(ClaimTypes.Role,role));
            
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires:DateTime.Now.AddMinutes(15),
                signingCredentials:credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
    }
}
