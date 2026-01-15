using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Interfaces.Repository;

namespace server.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserReposistory _userRepository;

        public UserController(IUserReposistory userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<ResponceDto>> GetAll()
        {
            ResponceDto responseDto = new ResponceDto();

            var users = await _userRepository.GetAllUsers();

            responseDto.IsSuccessed = true;
            responseDto.Data = users;

            return Ok(responseDto);
        }
    }
}
