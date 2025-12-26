using server.Data;
using server.Entities;
using server.Interfaces.Repository;
using server.Repository;

namespace server.Reposistory
{
    public class ImageReposistory:GenericRepository<Image> ,IImageReposistory
    {
        private readonly DataContext context;

        public ImageReposistory(DataContext context):base(context) {
        {
            this.context = context;
        }
    }
    }

}
