using MaxiShop.Application.ApplicationConstants;
using MaxiShop.Application.Common;
using MaxiShop.Application.DTO.Category;
using MaxiShop.Application.Services.Interfaces;
using MaxiShop.Domain.Contracts;
using MaxiShop.Domain.Models;
using MaxiShop.InfraStructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MaxiShop.Web.Controllers.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CategoryController : ControllerBase
    {
        public readonly ICategoryService _categoryService;
        protected APIResponse _response;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _response = new APIResponse();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Get()
        {
            try {
                var categories = await _categoryService.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = categories;
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommonMessage.systemError);
            }
             return Ok(_response);

        }

        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> Create([FromBody]CreateCategoryDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommonMessage.createOperationFailed;
                    _response.AddError(ModelState.ToString());
                    return Ok(_response);   
                }
                else
                {
                    var entity = await _categoryService.CreateAsync(dto);
                    _response.StatusCode = HttpStatusCode.Created;
                    _response.DisplayMessage = CommonMessage.createOperationSuccess;
                    _response.IsSuccess = true;
                    _response.Result = entity;
                }
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.DisplayMessage = CommonMessage.createOperationFailed;
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
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    _response.StatusCode =HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.recordNotFound;

                    return Ok(_response);

                }
                else
                {
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Result = category;
                }
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommonMessage.systemError);
            }


            return Ok(_response);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<ActionResult<APIResponse>> Update([FromBody] UpdateCategoryDTO categorydto)
        {
            try {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommonMessage.updateOperationFailed;
                    _response.AddError(ModelState.ToString());
                    return Ok(_response);
                }
                var category = await _categoryService.GetByIdAsync(categorydto.Id);
                if (category == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.updateOperationFailed;
                }

                await _categoryService.UpdateAsync(categorydto);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                _response.Result = CommonMessage.updateOperationSuccess;
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.DisplayMessage = CommonMessage.updateOperationFailed;
                _response.AddError(CommonMessage.systemError);
            }


            return Ok(_response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommonMessage.deleteOperationFailed;
                    return Ok(_response);
                }
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.deleteOperationFailed;
                    return Ok(_response);
                }
                await _categoryService.DeleteAsync(id);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = CommonMessage.deleteOperationSuccess;
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.DisplayMessage = CommonMessage.deleteOperationFailed;
                _response.AddError(CommonMessage.systemError);
            }

            return Ok(_response);
        }


    }
}
