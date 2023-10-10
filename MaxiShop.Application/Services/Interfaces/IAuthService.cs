using MaxiShop.Application.Input_Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IEnumerable<IdentityError>> register(Register register);

        Task<object> login(Login login);
    }
}
