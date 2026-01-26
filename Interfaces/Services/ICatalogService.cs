using server.Dto;
using server.Entities;

namespace server.Interfaces.Services
{
    public interface ICatalogService
    {
        // Product
        Task<ProductPagination> GetAllProducts(CatalogSpec inData);
        Task<Product> GetProductById(int productId);


        Task<Product> CreateProduct(CreateProductReq inData);
        Task DeleteProduct(int productId);

        // Brand
        Task<IEnumerable<Brand>> GetAllBrand();
        Task<Brand> CreateBrand(CreateBrandReq inData);
        Task DeleteBrand(int brandId);

        // Category
        Task<IEnumerable<Categories>> GetAllCategory();
        Task<Categories> CreateCategory(CreateCategoryReq inData);
        Task DeleteCategory(int categoryId);

    }
}
