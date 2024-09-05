using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopAppAPI.Apps.AdminApp.Dtos.UserDto;
using ShopAppAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopAppAPI.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto rDto)
        {
            var existUser = await _userManager.FindByNameAsync(rDto.UserName);
            if (existUser != null) return BadRequest();
            AppUser user = new()
            {
                Email = rDto.Email,
                UserName = rDto.UserName,
                FullName = rDto.FullName,
            };
            var result = await _userManager.CreateAsync(user, rDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            _userManager.AddToRoleAsync(user, "member");
            return StatusCode(201);
        }
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            if (!await _roleManager.RoleExistsAsync("member"))
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "member",
                });
            }
            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "admin",
                });
            }
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto lDto)
        {
            var user = await _userManager.FindByNameAsync(lDto.UserName);
            if (user == null) return BadRequest();
            var result = await _userManager.CheckPasswordAsync(user, lDto.Password);
            if (!result) return BadRequest();
            var handler = new JwtSecurityTokenHandler();
            var privateKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var credentials = new SigningCredentials(
              new SymmetricSecurityKey(privateKey),
              SecurityAlgorithms.HmacSha256);

            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("id", user.Id));
            ci.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, user.FullName));
            ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            var roles = await _userManager.GetRolesAsync(user);
            ci.AddClaims(roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList());

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1),
                Subject = ci,
                Audience=_configuration.GetSection("Jwt:Audience").Value,
                Issuer=_configuration.GetSection("Jwt:Issuer").Value,
                NotBefore=DateTime.UtcNow.AddHours(1),
            };
            var token = handler.CreateToken(tokenDescriptor);
           
            return Ok(new { token = handler.WriteToken(token) });
        }
    }
}
