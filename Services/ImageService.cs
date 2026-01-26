using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using server.Entities;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using server.Reposistory;

namespace server.Services
{
    public class ImageService : IImageServies
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IImageReposistory _imageReposistory;

        public ImageService(IWebHostEnvironment environment, IImageReposistory imageReposistory)
        {
            _environment = environment;
            _imageReposistory = imageReposistory;
        }

        public async Task DeleteImageAsync(int id)
        {
            var image = await _imageReposistory.GetByIdAsync(id);
            if (image == null) throw new ArgumentNullException($"No Image Found With Id {id}");

            // delete file from disk
            var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads");
            var filePath = Path.Combine(uploadsPath, image.ImageUrl);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // remove from db
            await _imageReposistory.DeleteAsync(image); // keep using your repo method name
        }

        public async Task<Image> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException(nameof(file), "file is empty");

            var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);
            var originalFileName = Path.GetFileName(file.FileName);      
            var extension = Path.GetExtension(originalFileName) ?? "";   
            var savedFileName = $"{Guid.NewGuid()}{extension}";         
            var savedFilePath = Path.Combine(uploadsPath, savedFileName);
            await using (var stream = new FileStream(savedFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var image = new Image
            {
                ImageUrl = savedFileName,          
                ImageName = originalFileName,
                ImageExtension = extension.TrimStart('.') 
            };
            await _imageReposistory.AddAsync(image); 
            return image;
        }
    }
}