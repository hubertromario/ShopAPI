using Azure;
using MaxiShop.Application.ApplicationConstants;
using MaxiShop.Application.Common;
using MaxiShop.Application.Input_Models;
using MaxiShop.Application.Services;
using MaxiShop.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MaxiShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IAuthService _authService;
        protected APIResponse _apiResponse; 

        public UserController(IAuthService authService)
        {
            _authService = authService;
            _apiResponse = new APIResponse();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<APIResponse>> Register(Register register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _apiResponse.AddError(ModelState.ToString());
                    _apiResponse.AddWarning(CommonMessage.registrationFailed);
                    return _apiResponse;
                }

                var result = await _authService.register(register);
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.DisplayMessage = CommonMessage.registrationsuccess;
                _apiResponse.Result = result;
            }
            catch (Exception)
            {
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.AddError(CommonMessage.systemError);
            }
            return Ok(_apiResponse);

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<APIResponse>> Login(Login login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _apiResponse.AddError(ModelState.ToString());
                    _apiResponse.AddWarning(CommonMessage.loginFailed);
                    return _apiResponse;
                }

                var result = await _authService.login(login);

                if (result is string)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.DisplayMessage = CommonMessage.loginFailed;
                    _apiResponse.Result = result;
                    return _apiResponse;


                }
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.DisplayMessage = CommonMessage.loginSuccess;
                _apiResponse.Result = result;
               
            }
            catch (Exception)
            {
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.AddError(CommonMessage.systemError);
            }
            return Ok(_apiResponse);

        }
    }
}
