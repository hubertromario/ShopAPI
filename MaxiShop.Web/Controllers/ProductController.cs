using MaxiShop.Application.ApplicationConstants;
using MaxiShop.Application.Common;
using MaxiShop.Application.DTO.Product;
using MaxiShop.Application.Input_Models;
using MaxiShop.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MaxiShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        public readonly IProductService _ProductService;
        protected APIResponse _response;

        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
            _response = new APIResponse();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Get()
        {
            try
            {
                var products = await _ProductService.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = products;
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
        [Route("Get Pagination")]
        public async Task<ActionResult<APIResponse>> GetPagination(PaginationInputModel pagination)
        {
            try
            {
                var products = await _ProductService.GetPagination(pagination);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = products;
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
        [Route("Filter")]
        public async Task<ActionResult<APIResponse>> GetByFilter(int? categoryId,int? brandId)
        {
            try
            {
                var products = await _ProductService.GetAllByFilterAsync(categoryId,brandId);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = products;
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
        public async Task<ActionResult<APIResponse>> Create([FromBody] CreateProductDTO dto)
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
                    var entity = await _ProductService.CreateAsync(dto);
                    _response.StatusCode = HttpStatusCode.Created;
                    _response.DisplayMessage = CommonMessage.createOperationSuccess;
                    _response.IsSuccess = true;
                    _response.Result = entity;
                }
            }
            catch (Exception)
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
                var Product = await _ProductService.GetByIdAsync(id);
                if (Product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.recordNotFound;

                    return Ok(_response);

                }
                else
                {
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Result = Product;
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
        public async Task<ActionResult<APIResponse>> Update([FromBody] UpdateProductDTO Productdto)
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
                var Product = await _ProductService.GetByIdAsync(Productdto.Id);
                if (Product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.updateOperationFailed;
                }

                await _ProductService.UpdateAsync(Productdto);

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
                var Product = await _ProductService.GetByIdAsync(id);
                if (Product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.deleteOperationFailed;
                    return Ok(_response);
                }
                await _ProductService.DeleteAsync(id);

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
