using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Entities;
using server.Interfaces.Services;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService catalogService;
        private readonly IMapper mapper;

        public CatalogController(ICatalogService catalogService, IMapper mapper)
        {
            this.catalogService = catalogService;
            this.mapper = mapper;
        }

        // =====================================================================
        // 🔵 PRODUCT SECTION
        // =====================================================================

        [HttpPost("product/getall")]
        public async Task<ActionResult<ResponceDto>> GetAllProducts([FromBody] CatalogSpec req)
        {
            var response = new ResponceDto();

            var res = await catalogService.GetAllProducts(req);

            response.Data = new ProductPaginationRes
            {
                PageIndex = res.PageIndex,
                PageSize = res.PageSize,
                Data = mapper.Map<IReadOnlyList<ProductResDto>>(res.Data),
                count = res.count,
                MinPrice = res.MinPrice,
                MaxPrice = res.MaxPrice
            };

            return Ok(response);
        }

        [HttpPost("product/create")]
        [Consumes("multipart/form-data")]

        public async Task<ActionResult> CreateProduct([FromForm] CreateProductReq req)
        {
            var product = await catalogService.CreateProduct(req);
            return Ok(product);
        }

     


        [HttpDelete("product/delete/{productId}")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            await catalogService.DeleteProduct(productId);
            return Ok();
        }

        // =====================================================================
        // 🟣 BRAND SECTION (FILE UPLOAD FIXED)
        // =====================================================================

        [HttpGet("brand/getall")]
        public async Task<ActionResult<ResponceDto>> GetAllBrands()
        {
            var response = new ResponceDto();

            var brands = await catalogService.GetAllBrand();
            response.Data = mapper.Map<IEnumerable<BrandResDto>>(brands);

            return Ok(response);
        }

        [HttpPost("brand/create")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateBrand([FromForm] CreateBrandReq req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var brand = await catalogService.CreateBrand(req);
            return Ok(brand);
        }

            
    
        [HttpDelete("brand/delete/{brandId}")]
        public async Task<ActionResult> DeleteBrand(int brandId)
        {
            await catalogService.DeleteBrand(brandId);
            return Ok();
        }

        // =====================================================================
        // 🟣 CATEGORY SECTION
        // =====================================================================

        [HttpGet("category/getall")]
        public async Task<ActionResult<ResponceDto>> GetAllCategories()
        {
            var response = new ResponceDto();

            var categories = await catalogService.GetAllCategory();
            response.Data = mapper.Map<IEnumerable<Categories>>(categories);

            return Ok(response);
        }

        [HttpPost("category/create")]   
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateCategory([FromForm] CreateCategoryReq req)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = await catalogService.CreateCategory(req);
            return Ok(category);
        }




        [HttpDelete("category/delete/{categoryId}")]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            await catalogService.DeleteCategory(categoryId);
            return Ok();
        }
    }
}
