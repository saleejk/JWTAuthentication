using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        public UserController(IConfiguration config)
        {
            _config = config;
            
        }
        List<UserModel> users = new List<UserModel>()
        {
            new UserModel(){Id=1, Name="saleej",Password="1234",Role="admin"},
            new UserModel (){Id=2, Name="babi",Password="babi",Role="user"}
        };

        [HttpPost("logIn")]
        public IActionResult logIn([FromBody] UserModel data)
        {
            try
            {
                if (data == null)
                {
                    return BadRequest("No data Exist");
                }
                var checkUser = users.FirstOrDefault(u => u.Name == data.Name && u.Password == data.Password);
                if (checkUser == null)
                {
                    return BadRequest("User not Available");
                }
                var token = getToken(checkUser);
                return Ok(new { Token = token });
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error{ex.Message}");
            }
        }
        private string getToken(UserModel data)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {new Claim(ClaimTypes.NameIdentifier,data.Id.ToString()),
            new Claim (ClaimTypes.Name,data.Name),
            new Claim(ClaimTypes.Role,data.Role)};
            var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], audience: _config["Jwt:Audience"], claims: claims, signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
