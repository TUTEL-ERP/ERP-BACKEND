using server.Entities;

namespace server.Interfaces.Repository
{
    public interface ICategoryReposistory:IGenericReposistroy<Categories>
    {
        Task<IEnumerable<Categories>> GetAllIncludingImage();

    }
}
