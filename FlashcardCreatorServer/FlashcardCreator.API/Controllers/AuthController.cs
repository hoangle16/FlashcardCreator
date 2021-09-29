using AutoMapper;
using FlashcardCreator.Domain.Entities;
using FlashcardCreator.Domain.Helpers;
using FlashcardCreator.Domain.Interfaces;
using FlashcardCreator.Domain.Models.User;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlashcardCreator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        private static readonly HttpClient Client = new HttpClient();
        public AuthController(
            IAuthRepository authRepository, 
            IOptions<AppSettings> appSettings, 
            IMapper mapper)
        {
            _authRepository = authRepository;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> Signin([FromBody] UserLoginModel model)
        {
            var user = await _authRepository.Authenticate(model);
            if (user == null)
            {
                return BadRequest(new { message = "Email or Password are incorrect" });
            }

            var tokenString = AuthUtils.CreateJwtToken(user.Id, user.Email, _appSettings.JwtSecret);

            var userModel = _mapper.Map<UserModel>(user);

            Response.Headers.Add("Access-Control-Allow-Credentials", "true");

            Response.Cookies.Append("X-Token", tokenString, new CookieOptions() { HttpOnly = true, Path = "/", Secure = true, SameSite = SameSiteMode.None, Expires = DateTime.Now.AddDays(7) });

            return Ok(userModel);
        }
        [AllowAnonymous]
        [HttpPost("google-signin")]
        public async Task<IActionResult> GoogleSignin([FromBody] UserToken userToken)
        {
            try
            {
                var payload = GoogleJsonWebSignature.ValidateAsync(userToken.TokenId, new GoogleJsonWebSignature.ValidationSettings()).Result;

                var user = await _authRepository.GoogleAuthenticate(payload);

                //create jwt
                var tokenString = AuthUtils.CreateJwtToken(user.Id, user.Email, _appSettings.JwtSecret);

                var userModel = _mapper.Map<UserModel>(user);

                Response.Headers.Add("Access-Control-Allow-Credentials", "true");

                Response.Cookies.Append("X-Token", tokenString, new CookieOptions() { HttpOnly = true, Path = "/", Secure = true, SameSite = SameSiteMode.None, Expires = DateTime.Now.AddDays(7) });

                return Ok(userModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("facebook-signin")]
        public async Task<IActionResult> FacebookSignin([FromBody] UserToken userToken)
        {
            // tokenId == access token
            // 1.generate an app access token
            var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_appSettings.FacebookAppId}&client_secret={_appSettings.FacebookAppSecret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            // 2. validate the user access token
            var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={userToken.TokenId}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                return BadRequest(new { message = "login failure" });
            }

            // 3. we've got a valid token so we can request user data from fb
            var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={userToken.TokenId}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            // 4. ready to create the local user account (if necessary) and jwt
            var user = await _authRepository.FacebookAuthenticate(userInfo);
            //create jwt
            var tokenString = AuthUtils.CreateJwtToken(user.Id, user.Email, _appSettings.JwtSecret);

            var userModel = _mapper.Map<UserModel>(user);

            Response.Headers.Add("Access-Control-Allow-Credentials", "true");

            Response.Cookies.Append("X-Token", tokenString, new CookieOptions() { HttpOnly = true, Path = "/", Secure = true, SameSite = SameSiteMode.None, Expires = DateTime.Now.AddDays(7) });
            
            return Ok(userModel);
        }
        [AllowAnonymous]
        [HttpPost("email-signin")]
        public async Task<IActionResult> Signup([FromBody] UserRegisterModel model)
        {
            User user = new()
            {
                Email = model.Email,
                FullName = model.FullName
            };
            var userId = await _authRepository.SignupByEmail(user, model.Password);
            var tokenString = AuthUtils.CreateJwtToken(userId, user.Email, _appSettings.JwtSecret);

            var userModel = _mapper.Map<UserModel>(user);

            Response.Headers.Add("Access-Control-Allow-Credentials", "true");

            Response.Cookies.Append("X-Token", tokenString, new CookieOptions() { HttpOnly = true, Path = "/", Secure = true, SameSite = SameSiteMode.None, Expires = DateTime.Now.AddDays(7) });

            return Ok(userModel);
        }
        [Authorize]
        [HttpGet("signout")]
        public IActionResult Signout()
        {
            Request.Cookies.TryGetValue("X-Token", out string token);
            if (token == null)
                return BadRequest();

            Response.Headers.Add("Access-Control-Allow-Credentials", "true");

            Response.Cookies.Append("X-Token", "", new CookieOptions() { HttpOnly = true, Path = "/", Secure = true, SameSite = SameSiteMode.None, Expires = DateTime.Now.AddDays(-1) });
            return Ok();
        }
    }
}
