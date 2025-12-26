using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Entities;
using server.Interfaces.Services;
using server.Services;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageServies _imageService;
        private readonly IMapper mapper;

        public ImageController(IImageServies imageService,IMapper mapper)
        {
           _imageService = imageService;
            this.mapper = mapper;

        }

        [HttpPost]
        
        public async Task<ActionResult<Image>> Upload(IFormFile file) {
            Image image  =await _imageService.SaveImageAsync(file);
            return Ok(mapper.Map<ImageDtoRes>(image));
    }

        [HttpDelete("Id")]
        public async Task<ActionResult<Image>> Delete(int id)
        {
            await _imageService.DeleteImageAsync(id);
            return Ok();
        }
    }


}
