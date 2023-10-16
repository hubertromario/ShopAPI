using MaxiShop.Application.Common;
using MaxiShop.Application.Input_Models;
using MaxiShop.Application.Services.Interfaces;
using MaxiShop.Application.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services
{

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser applicationUser;
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            applicationUser = new ApplicationUser();
        }

        public async Task<object> login(Login login)
        {
            applicationUser = await _userManager.FindByEmailAsync(login.Email);  //to find user exists using email id

            if(applicationUser == null)
            {
                return "Invalid Email Address";
            }

            var result = await _signInManager.PasswordSignInAsync(applicationUser, login.Password, isPersistent: true, lockoutOnFailure: true);

            var isvalidCredentials = await _userManager.CheckPasswordAsync(applicationUser, login.Password);

      
            if (result.Succeeded)
            {
                var token = await GenerateToken();
                LoginResponse loginResponse = new LoginResponse
                {
                    UserId = applicationUser.Id,
                    Token = token

                };

                return loginResponse;
            }
            else
            {
                if(result.IsLockedOut) {
                    return "your Account is Locked,contact system admin";
                }
                if(result.IsNotAllowed)
                {
                    return "please verify Email Address";
                }
                if(isvalidCredentials == false)
                {
                    return "Invalid Password";
                }
                else
                {
                    return "Login Failed";
                }
            }


           
        }

        public async Task<IEnumerable<IdentityError>> register(Register register)
        {
            applicationUser.FirstName = register.firstName; 
            applicationUser.LastName = register.lastName;
            applicationUser.Email = register.email;
            applicationUser.UserName = register.email;

            var result = await _userManager.CreateAsync(applicationUser,register.password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(applicationUser, "ADMIN");
            }
            return result.Errors;
        }

        public async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(applicationUser);

            var roleCliams = roles.Select(x=> new Claim(ClaimTypes.Role,x)).ToList();

            List<Claim> cliams = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email)
            }.Union(roleCliams).ToList();

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings.Audience"],
                claims:cliams,
                signingCredentials:signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JwtSettings.DurationInMinute"]))
                );

            return new JwtSecurityTokenHandler().WriteToken(token);



        }


    }
}
