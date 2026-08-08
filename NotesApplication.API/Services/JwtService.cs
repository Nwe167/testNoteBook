using Microsoft.IdentityModel.Tokens;
using NotesApplication.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace NotesApplication.API.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;


        public JwtService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.UserId.ToString()
                ),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email
                ),

                new Claim(
                    "FullName",
                    user.FullName
                )
            };



            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["Jwt:Key"]!
                    )
                );


            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                );



            var token =
                new JwtSecurityToken(

                    issuer:
                    _configuration["Jwt:Issuer"],


                    audience:
                    _configuration["Jwt:Audience"],


                    claims: claims,


                    expires:
                    DateTime.Now.AddHours(2),


                    signingCredentials:
                    credentials
                );



            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}