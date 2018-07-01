//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Text;
//using Curnik.Models.Database;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;

//namespace Curnik.Controllers
//{
//    public class TokenController : Controller
//    {
//        private readonly IConfiguration _configuration;
//        private readonly DatabaseContext _dbContext;
//        public TokenController(IConfiguration configuration, DatabaseContext dbContext)
//        {
//            this._dbContext = dbContext;
//            this._configuration = configuration;
//        }

//        public IActionResult GenerateToken(string username, string password)
//        {
//            if (this._dbContext.Users.Any(cr => cr.UserName == username && cr.Password == password))
//            {
//                var jwtToken = JwtTokenBuilder();

//                return Ok(new { accessToken = jwtToken });
//            }
//            else
//            {
//                IActionResult response = Unauthorized();
//                return response;
//            }
//        }

//        private string JwtTokenBuilder()
//        {
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
//            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(issuer: this._configuration["JWT:issuer"], audience: this._configuration["JWT:audience"],
//                signingCredentials: credentials,
//                expires: DateTime.Now.AddMinutes(10));

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}