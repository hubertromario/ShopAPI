using MaxiShop.Application.Common;
using MaxiShop.Application.Input_Models;
using MaxiShop.Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services
{

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser applicationUser;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                return true;
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
    }
}
