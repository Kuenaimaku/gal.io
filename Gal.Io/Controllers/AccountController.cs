using Gal.Io.Interfaces;
using Gal.Io.Interfaces.DTOs;
using Gal.Io.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gal.Io.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private IConfiguration _config;
        private ILogger<AccountController> _logger;
        private IAccountService _accountService;

        public AccountController(IConfiguration config, ILogger<AccountController> logger, IAccountService accountService)
        {
            _config = config;
            _logger = logger;
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginDTO login)
        {
            IActionResult response = Unauthorized();
            UserDTO user = _accountService.Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString, user });
            }

            return response;
        }

        [AllowAnonymous]
        [HttpPut]
        public IActionResult Register([FromBody]RegisterDTO register)
        {
            IActionResult response = BadRequest();
            if (register.Password != register.Confirm)
                response = BadRequest("Passwords do not match");
            UserDTO user = _accountService.Register(register);
            if (user != null)
            {
                response = Ok(user);
            }
            return response;
        }

        private string BuildToken(UserDTO user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            claims.Add(new Claim("username", user.Username));
            claims.Add(new Claim("email", user.Email));
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims: claims,
              expires: DateTime.Now.AddHours(4),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}