using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopAppAPI.Apps.AdminApp.Dtos.UserDto;
using ShopAppAPI.Entities;
using ShopAppAPI.Services.Interfaces;
using ShopAppAPI.Settings;
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
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;
        public AuthController(IOptions<JwtSettings> jwtSettings,UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _tokenService = tokenService;
            _jwtSettings=jwtSettings.Value;
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
            var roles=await _userManager.GetRolesAsync(user);
            var audience = _jwtSettings.Audience;
            var issuer = _jwtSettings.Issuer;
            var secretKey = _jwtSettings.SecretKey;
            return Ok(new {token= _tokenService.GetToken(secretKey, audience, issuer, user, roles) }); 
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserGetDto>(user));
        }
    }
}
