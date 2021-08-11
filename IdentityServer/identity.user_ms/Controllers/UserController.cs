using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace identity.user_ms.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET api/values
        [HttpGet("getUser")]
        public ActionResult<IEnumerable<User>> Get()
        {
            User user = GetDummyData();
            return Ok(user);
        }
        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Get(string name,string password)
        {
            if(name == "cp" && password == "123")
            {
                var now = DateTime.UtcNow;
                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,name),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub,now.ToUniversalTime().ToString(),ClaimValueTypes.Integer64),
                };
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is the secret key to add some default jwt token, lets see how it works"));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidIssuer = "sohail",
                    ValidateAudience = true,
                    ValidAudience = "shahzad",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };
                var jwt = new JwtSecurityToken(
                    issuer: "sohail",
                    audience: "shahzad",
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials:new SigningCredentials(signingKey,SecurityAlgorithms.HmacSha256)
               );

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var responseJson = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)TimeSpan.FromMinutes(2000).TotalSeconds
                };
                return Ok(responseJson);
            }
            else
            {
                return BadRequest("User name or password is incorrect");
            }
            
        }
        private User GetDummyData()
        {
            User user = new User
            {
                Id = 1,
                Name = "CP Variyani",
                Email = "cpvariyani90@gmail.com"
            };
            return user;
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
}
