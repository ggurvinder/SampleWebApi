using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Data.Repositories;
using SampleWebApi.Models.Domain;
using SampleWebApiDto;
using SampleWebApiDto.Models.DTO;
using System.Security.Claims;

namespace SampleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        }
        //POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var appUser = new ApplicationUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName,

            };


            var identityUser = await _userManager.CreateAsync(appUser, registerRequestDto.Password);

            if (identityUser.Succeeded)
            {
                // Add Role to User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {

                    identityUser = await _userManager.AddToRolesAsync(appUser, registerRequestDto.Roles);

                    if (identityUser.Succeeded)
                    {
                        return Ok("User was registred successfully! Please login.");
                    }

                }

            }

            return BadRequest("Something went wrong!");
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {

                var checkPwdResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);



                if (checkPwdResult)
                {
                    //Get roles for this user

                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null && roles.Any())
                    {
                        //Create Tocken
                        var jwtToken = _tokenRepository.CreateJwtToken(user, roles.ToList());
                        var response = new LoginResponseDto();
                        response.JwtToken = jwtToken;


                        return Ok(response);
                    }
                }



            }


            return BadRequest("Username or password incorrent");

        }

        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            // Retrieve user ID from claims
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string s = $"UserId: {userId} , Email: {userEmail}";
            return Ok(s);

        }

    }
}
