using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
