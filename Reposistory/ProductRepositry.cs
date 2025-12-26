using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Entities;
using server.Interfaces.Repository;
using server.Repository;
using System.Diagnostics.Metrics;

namespace server.Reposistory
{
    public class ProductRepositry : GenericRepository<Product>, IProductRepository
    {

        private  readonly DataContext context;
        public ProductRepositry(DataContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<ProductPagination> GetAllIncludingChildEntites(CatalogSpec inData)
        {
            IQueryable<Product> product = context.Products
                .Include(p => p.Categories).Include(p => p.Brand).Include(p => p.Thumbnail).AsQueryable();

            if (!string.IsNullOrEmpty(inData.search))
            {
                product = product.Where(p => p.Name.Contains(inData.search));
            }
            if (inData.minPrice.HasValue)
            {
                product = product.Where(p => p.OrignalPrice >=(inData.minPrice));
            }
            if (inData.maxPrice.HasValue)
            {
                product = product.Where(p => p.OrignalPrice <= (inData.maxPrice));
            }
            if (inData.inStock.HasValue)
            {
                if (inData.inStock==true)
                {
                    product = product.Where(p => p.StockQuantity > 0);
                }
                else
                {
                    product = product.Where(p => p.StockQuantity == 0 );
                }

                if (inData.categoryId != null && inData.categoryId.Length > 0)
                {
                    product = product.Where(p => inData.categoryId.Contains(p.CategoryId));
                }

                if (inData.brandId != null && inData.brandId.Length > 0)
                {
                    product = product.Where(p => inData.brandId.Contains(p.BrandId));

                }
                if (!string.IsNullOrWhiteSpace(inData.sort))
                {
                    if (inData.sort.ToLower() == "price_htl")
                    {
                        product = product.OrderByDescending(p => p.OrignalPrice);
                    }

                    if (inData.sort.ToLower() == "price_lth")
                    {
                        product = product.OrderBy(p => p.OrignalPrice);
                    }   

                    if (inData.sort.ToLower() == "featured")
                    {
                        product = product.OrderByDescending(p => p.IsFeatured);
                    }

                    if (inData.sort.ToLower() == "rating")
                    {
                        product = product.OrderBy(p => p.IsFeatured);
                    }

                    if (inData.sort.ToLower() == "newest")
                    {
                        product = product.OrderByDescending(p => p.Created);
                    }

                }
            }

            // Safe Min/Max using nullable cast
            decimal? minPrice = await context.product.MinAsync(p => (decimal?)p.OrignalPrice);
            decimal? maxPrice = await context.product.MaxAsync(p => (decimal?)p.OrignalPrice);

            return new ProductPagination()
            {
                PageIndex = inData.pageIndex,
                PageSize = inData.pageSize,
                Data = await product
                    .Skip((inData.pageIndex - 1) * inData.pageSize)
                    .Take(inData.pageSize)
                    .ToListAsync(),
                count = await product.CountAsync(),
                MinPrice = minPrice ?? 0,   // fallback if table empty
                MaxPrice = maxPrice ?? 0
            };


            //return await product
            //    .Skip((inData.pageIndex - 1) * inData.pageSize)
            //    .Take(inData.pageSize)
            //    .ToListAsync(); 


        }
    }
}
