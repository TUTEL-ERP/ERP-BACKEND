using server.Dto;
using server.Entities;

namespace server.Interfaces.Repository
{
    public interface IProductRepository:IGenericReposistroy<Product>
    {
        Task<ProductPagination> GetAllIncludingChildEntites(CatalogSpec inData);
        Task<Product?> GetByIdIncludingChildEntities(int id);


    }
}
