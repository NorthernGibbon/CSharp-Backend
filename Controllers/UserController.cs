using JacobCorp.Data;
using JacobCorp.Models;
using JacobCorp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JacobCorp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenService _jwtTokenService;

        public UserController(IUserService userService, JwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = _jwtTokenService.GenerateToken(user.Email);
            return Ok(new { Token = token });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = await _userService.Register(model);

            var token = _jwtTokenService.GenerateToken(user.Email);
            return Ok(new { Token = token });
        }
    }
}
