using MaxiShop.Application.ApplicationConstants;
using MaxiShop.Application.Common;
using MaxiShop.Application.DTO.Brand;
using MaxiShop.Application.Exceptions;
using MaxiShop.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MaxiShop.Web.Controllers.V2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class BrandController : ControllerBase
    {
        public readonly IBrandService _BrandService;
        protected APIResponse _response;

        public BrandController(IBrandService BrandService)
        {
            _BrandService = BrandService;
            _response = new APIResponse();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration =60)]
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Get()
        {
            try
            {
                var brands = await _BrandService.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = brands;
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommonMessage.systemError);
            }
            return Ok(_response);

        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult<APIResponse>> Get(int id)
        {
            try
            {
                var Brand = await _BrandService.GetByIdAsync(id);
                if (Brand == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.recordNotFound;

                    return Ok(_response);

                }
                else
                {
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Result = Brand;
                }
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommonMessage.systemError);
            }


            return Ok(_response);
        }


       


    }
}
