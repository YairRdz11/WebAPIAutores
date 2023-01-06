using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIAutores.DTOs;
using WebAPIAutores.Services;

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountsController(UserManager<IdentityUser> userManager, 
            IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }


        [HttpPost("register", Name = "registerUser")]
        public async Task<ActionResult<ResponseAuthentication>> Register(UserCredentials userCredential)
        {
            var user = new IdentityUser { UserName = userCredential.Email, Email = userCredential.Email };
            var result = await userManager.CreateAsync(user, userCredential.Password);

            if (result.Succeeded)
            {
                return await BuildToken(userCredential);
            } 
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login", Name = "loginUser")]
        public async Task<ActionResult<ResponseAuthentication>> Login(UserCredentials userCredentials)
        {
            var result = await signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, false, false);

            if (result.Succeeded)
            {
                return await BuildToken(userCredentials);
            }
            else
            {
                return BadRequest("Login invalid");
            }
        }

        [HttpGet("renovate-token", Name = "renovateToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ResponseAuthentication>> Renovate()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var userCredentials = new UserCredentials()
            {
                Email = email
            };

            return await BuildToken(userCredentials);
        }

        [HttpPost("get-admin", Name = "getAdmin")]
        public async Task<ActionResult> GetAdmin(EditAdminDTO editAdminDTO)
        {
            var user = await userManager.FindByEmailAsync(editAdminDTO.Email);
            await userManager.AddClaimAsync(user, new Claim("isAdmin", "1"));

            return NoContent();
        }

        [HttpPost("remove-admin", Name = "revokeAdmin")]
        public async Task<ActionResult> RemoveAdmin(EditAdminDTO editAdminDTO)
        {
            var user = await userManager.FindByEmailAsync(editAdminDTO.Email);
            await userManager.RemoveClaimAsync(user, new Claim("isAdmin", "1"));

            return NoContent();
        }

        private async Task<ResponseAuthentication> BuildToken(UserCredentials userCredential)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userCredential.Email)
            };

            var user = await userManager.FindByEmailAsync(userCredential.Email);
            var claimsDB = await userManager.GetClaimsAsync(user);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var securiyToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            return new ResponseAuthentication()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securiyToken),
                Expiration = expiration
            };
        }
    }
}
