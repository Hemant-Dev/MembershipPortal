using MembershipPortal.API.ErrorHandling;
using MembershipPortal.DTOs;
using MembershipPortal.IServices;
using MembershipPortal.Models;
using MembershipPortal.Services;
using Microsoft.AspNetCore.Mvc;
using static MembershipPortal.DTOs.ProductDTO;
using static MembershipPortal.DTOs.UserDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MembershipPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly string tableName = "Product";


        public ProductController (IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/<ProductController>
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<GetProductDTO>>> Get()
        //{
        //   try
        //    {
        //        var product = await _productService.GetProductsAsync();
        //        return Ok(product);
        //        //if (product.Count() != 0)
        //        //{

        //        //    return Ok(product);
        //        //}
        //        //else
        //        //{
        //        //    return NotFound(MyException.DataNotFound(tableName));
        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(500, MyException.DataProcessingError(ex.Message));
        //    }
        //}
        public async Task<ActionResult<IEnumerable<GetProductDTO>>> Get(string? sortColumn, string? sortOrder)
        {
            try
            {
                var getProductsDTOList = await _productService.GetAllSortedProducts(sortColumn, sortOrder);
                if (getProductsDTOList != null)
                {
                    return Ok(getProductsDTOList);
                }
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[HttpGet("paginated")]
        //public async Task<ActionResult<Paginated<GetProductDTO>>> GetPaginatedProductData(int page, int pageSize)
        //{
        //    var paginatedProductDTOAndTotalPages = await _productService.GetAllPaginatedProductAsync(page, pageSize);
        //    var result = new Paginated<GetProductDTO>()
        //    {
        //        dataArray = paginatedProductDTOAndTotalPages.Item1,
        //        totalPages = paginatedProductDTOAndTotalPages.Item2
        //    };
        //    return Ok(result);
        //}
        [HttpPost("paginated")]
        public async Task<ActionResult<Paginated<GetProductDTO>>> GetPaginatedProductData(int page, int pageSize,[FromBody] GetProductDTO product)
        {
            try
            {
                var paginatedProductDTOAndTotalPages = await _productService.GetAllPaginatedProductAsync(page, pageSize, new Product()
                {
                    ProductName = product.ProductName,
                    Price = product.Price
                });
                var result = new Paginated<GetProductDTO>
                {
                    dataArray = paginatedProductDTOAndTotalPages.Item1,
                    totalPages = paginatedProductDTOAndTotalPages.Item2
                };
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductDTO>> Get(long id)
        {

            try
            {
                var product = await _productService.GetProductAsync(id);
                if (product == null)
                {
                    return NotFound(MyException.DataWithIdNotPresent(id, tableName));
                }
                return Ok(product);
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<GetProductDTO>> Post([FromBody] CreateProductDTO createProductDTO)
        {
            try
            {
                var product = await _productService.CreateProductAsync(createProductDTO);
               if(product ==null)
                {
                return StatusCode(500, $"Product is null");

                }

                return Ok(product);
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GetProductDTO>> Put(long id, [FromBody] UpdateProductDTO updateProductDTO)
        {

            if (id != updateProductDTO.Id)
            {
                return BadRequest(MyException.IdMismatch());
            }
            try
            {
                var product = await _productService.UpdateProductAsync(id, updateProductDTO);
                if (product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return NotFound(MyException.DataWithIdNotPresent(id, tableName));
                }

            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

            // DELETE api/<ProductController>/5
            [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(long id)
        {
            try
            {
                var isDeleted = await _productService.DeleteProductAsync(id);
                return Ok(isDeleted);
                //if (isDeleted)
                //{
                //    return StatusCode(200, MyException.DataDeletedSuccessfully(tableName));
                //}
                //else
                //{
                //    return NotFound(MyException.DataWithIdNotPresent(id, tableName));
                //}
            }
            catch (Exception ex)
            {

                return StatusCode(500, MyException.DataProcessingError(ex.Message));
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetProductDTO>>> GetProductSearchAsync(string find)
        {
            try
            {
                var productInfo = await _productService.GetProductSearchAsync(find);
                return Ok(productInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while searching product info : {ex.Message}");


            }

        }


        [HttpPost("advancesearch")]
        public async Task<ActionResult<IEnumerable<GetProductDTO>>> GetProductAdvanceSearchAsync(GetProductDTO getProductDTO)
        {
            try
            {
                var filterData = await _productService.GetProductAdvanceSearchAsync(getProductDTO);
                return Ok(filterData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving  advance search mobile info : {ex.Message}");

            }
        }

        
    }
}
