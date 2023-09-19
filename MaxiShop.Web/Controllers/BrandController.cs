﻿using MaxiShop.Application.ApplicationConstants;
using MaxiShop.Application.Common;
using MaxiShop.Application.DTO.Brand;
using MaxiShop.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MaxiShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Get()
        {
            try
            {
                var categories = await _BrandService.GetAllAsync();
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
        public async Task<ActionResult<APIResponse>> Create([FromBody] CreateBrandDTO dto)
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
                    var entity = await _BrandService.CreateAsync(dto);
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


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<ActionResult<APIResponse>> Update([FromBody] UpdateBrandDTO Branddto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommonMessage.updateOperationFailed;
                    _response.AddError(ModelState.ToString());
                    return Ok(_response);
                }
                var Brand = await _BrandService.GetByIdAsync(Branddto.Id);
                if (Brand == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.updateOperationFailed;
                }

                await _BrandService.UpdateAsync(Branddto);

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
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommonMessage.deleteOperationFailed;
                    return Ok(_response);
                }
                var Brand = await _BrandService.GetByIdAsync(id);
                if (Brand == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.deleteOperationFailed;
                    return Ok(_response);
                }
                await _BrandService.DeleteAsync(id);

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
