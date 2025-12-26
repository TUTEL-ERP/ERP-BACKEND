using AutoMapper;
using server.Dto;
using server.Entities;
using server.Interfaces.Repository;
using server.Interfaces.Services;

namespace server.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryReposistory categoryReposistory;
        private readonly IBrandReposistory brandReposistory;
        private readonly IImageServies imageServies;
        private readonly IMapper mapper;  // inject mapper

        public CatalogService(
            IProductRepository productRepository,
            ICategoryReposistory categoryReposistory,
            IBrandReposistory brandReposistory,
            IImageServies imageServies,
            IMapper mapper)   // <-- add IMapper here
        {
            this.productRepository = productRepository;
            this.categoryReposistory = categoryReposistory;
            this.brandReposistory = brandReposistory;
            this.imageServies = imageServies;
            this.mapper = mapper;  // now not null
        }

        public async Task<Brand> CreateBrand(CreateBrandReq inData)
        {
            Image image = await this.imageServies.SaveImageAsync(inData.Image);
            Brand brand = mapper.Map<Brand>(inData);
            brand.Image = image;
            return await brandReposistory.AddAsync(brand);
        }

        //public async Task<Categories> CreateCategory(CreateCategoryReq inData)
        //{
        //    Image image = await this.imageServies.SaveImageAsync(inData.Image);
        //    Categories categories = mapper.Map<Categories>(inData);
        //    categories.Image = image;
        //    return await categoryReposistory.AddAsync(categories);
        //}


        public async Task<Categories> CreateCategory(CreateCategoryReq inData)
        {
            Image? image = null;
            string imageUrl = ""; 

            if (inData.Image != null && inData.Image.Length > 0)
            {
                image = await this.imageServies.SaveImageAsync(inData.Image);
            }

            Categories categories = new Categories
            {
                Name = inData.Name,
                Image = image,
                ImageId = image?.Id,
                ImageUrl = imageUrl
            };

            return await categoryReposistory.AddAsync(categories);
        }




        public async Task<Product> CreateProduct(CreateProductReq inData)
        {
            Categories? categories = await this.categoryReposistory.GetByIdAsync(inData.CategoryId);
            Brand? brand = await this.brandReposistory.GetByIdAsync(inData.BrandId);

            if (categories == null) throw new ArgumentNullException($"Invalid Category Id {inData.CategoryId}");
            if (brand == null) throw new ArgumentNullException($"Invalid Brand Id {inData.BrandId}");

            Image image = await this.imageServies.SaveImageAsync(inData.Thumbnail);

            Product newProduct = mapper.Map<Product>(inData);
            newProduct.Brand = brand;
            newProduct.Categories = categories;
            newProduct.Thumbnail = image;

            return await productRepository.AddAsync(newProduct);
        }

        public async Task DeleteBrand(int brandId)
        {
            Brand? brand = await brandReposistory.GetByIdAsync(brandId);
            if (brand == null) throw new ArgumentNullException($"Invalid Brand Id {brandId}");

            if (brand.ImageId.HasValue)
                await imageServies.DeleteImageAsync(brand.ImageId.Value);

            await brandReposistory.DeleteAsync(brand);
        }

        public async Task DeleteCategory(int categoryId)
        {
            Categories? categories = await categoryReposistory.GetByIdAsync(categoryId);
            if (categories == null) throw new ArgumentNullException($"Invalid Category Id {categoryId}");

            if (categories.ImageId.HasValue)
                await imageServies.DeleteImageAsync(categories.ImageId.Value);

            await categoryReposistory.DeleteAsync(categories);
        }

        public async Task DeleteProduct(int productId)
        {
            Product? product = await productRepository.GetByIdAsync(productId);
            if (product == null) throw new ArgumentNullException($"Invalid Product Id {productId}");

            if (product.ThumbnailId.HasValue)
                await imageServies.DeleteImageAsync(product.ThumbnailId.Value);

            await productRepository.DeleteAsync(product);
        }

        public async Task<IEnumerable<Brand>> GetAllBrand()
        {
            return await brandReposistory.GetAllIncludingImage();
        }

        public async Task<IEnumerable<Categories>> GetAllCategory()
        {
            return await categoryReposistory.GetAllIncludingImage();
        }

        public async Task<ProductPagination> GetAllProducts(CatalogSpec inData)
        {
            return await productRepository.GetAllIncludingChildEntites(inData);
        }
    }
}
