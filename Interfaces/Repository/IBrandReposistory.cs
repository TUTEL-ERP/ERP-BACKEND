using server.Entities;
using server.Reposistory;

namespace server.Interfaces.Repository
{
    public interface IBrandReposistory:IGenericReposistroy<Brand>
    {
        Task<IEnumerable<Brand>> GetAllIncludingImage();
    }
}
