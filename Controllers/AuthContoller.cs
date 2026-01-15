using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Entities;
using server.Enums;
using server.Helpers;
using server.Interfaces.Repository;
using server.Reposistory;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserReposistory _userRepository;
        private readonly IJwtHelper _helper;

        public AuthController(IUserReposistory userRepository, IJwtHelper helper)
        {
            _userRepository = userRepository;
            _helper = helper;
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginUserReqDto req)
        //{
        //    ResponceDto res = new ResponceDto();
        //    // Get user by email
        //    User? user = await _userRepository.GetUserByEmail(req.Email);
        //    if (user == null)
        //    {
        //        res.IsSuccessed = false;
        //        res.Message = "User not found";
        //        return BadRequest(res);
        //    }

        //    // Use EnhancedVerify because we stored password using EnhancedHashPassword
        //    if (!BCrypt.Net.BCrypt.EnhancedVerify(req.Password, user.Password))
        //    {
        //        res.IsSuccessed = false;
        //        res.Message = "Invalid password";
        //        return BadRequest(res);
        //    }

        //    var RefreshToken = _helper.GenerateRefreshToken();
        //    user.RefreshToken = RefreshToken;
        //    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        //    await _userRepository.UpdateUser(user);
        //    // Generate JWT token
        //    LoginUserResDto userDetail = new LoginUserResDto
        //    {
        //        AccessToken = _helper.GenerateJwtToken(user),
        //        RefreshToken = RefreshToken
        //    };

        //    res.IsSuccessed = true;
        //    res.Message = "Login successful";
        //    res.Data = userDetail;

        //    return Ok(res);
        //}




        [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginUserReqDto req)
{
    try
    {
        ResponceDto res = new ResponceDto();

        User? user = await _userRepository.GetUserByEmail(req.Email);
        if (user == null)
        {
            res.IsSuccessed = false;
            res.Message = "User not found";
            return BadRequest(res);
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(req.Password, user.Password))
        {
            res.IsSuccessed = false;
            res.Message = "Invalid password";
            return BadRequest(res);
        }

        var RefreshToken = _helper.GenerateRefreshToken();
        user.RefreshToken = RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userRepository.UpdateUser(user);

        LoginUserResDto userDetail = new LoginUserResDto
        {
            AccessToken = _helper.GenerateJwtToken(user),
            RefreshToken = RefreshToken,
            Username = user.Username,
            UserId = user.UserId,
            Role = user.Role
        };

        res.IsSuccessed = true;
        res.Message = "Login successful";
        res.Data = userDetail;

        return Ok(res);
    }
    catch(Exception ex)
    {
        return StatusCode(500, new { message = ex.Message, stack = ex.StackTrace });
    }
}





        [HttpPost("register")]
        public async Task<ActionResult<ResponceDto>> Register([FromBody] RegisterUserDto req)
        {
            ResponceDto res = new ResponceDto();

            // Check if email already exists
            User? user = await _userRepository.GetUserByEmail(req.Email);
            if (user != null)
            {
                res.IsSuccessed = false;
                res.Message = $"User with email {req.Email} already exists";
                return BadRequest(res);
            }

            // Create new user with EnhancedHashPassword
            User newUser = new User
            {
                Username = req.UserName,
                Email = req.Email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(req.Password),
                Role = UserRole.USER.ToString()
            };

            bool result = await _userRepository.AddUser(newUser);
            if (!result)
            {
                res.IsSuccessed = false;
                res.Message = "User registration failed";
                return BadRequest(res);
            }

            res.IsSuccessed = true;
            res.Message = "User registered successfully";
            return Ok(res);
        }



        [HttpPost("register-Admin")]
        public async Task<ActionResult<ResponceDto>> RegisterAdmin([FromBody] RegisterUserDto req)
        {
            ResponceDto res = new ResponceDto();

            // Check if email already exists
            User? user = await _userRepository.GetUserByEmail(req.Email);
            if (user != null)
            {
                res.IsSuccessed = false;
                res.Message = $"User with email {req.Email} already exists";
                return BadRequest(res);
            }

            User newUser = new User
            {
                Username = req.UserName,
                Email = req.Email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(req.Password),
                Role = UserRole.ADMIN.ToString()
            };

            bool result = await _userRepository.AddUser(newUser);
            if (!result)
            {
                res.IsSuccessed = false;
                res.Message = "User registration failed";
                return BadRequest(res);
            }

            res.IsSuccessed = true;
            res.Message = "User registered successfully";
            return Ok(res);
        }





        [HttpPost("refresh-token")]
        public async Task<ActionResult<ResponceDto>> RefreshToken([FromBody] TokenRefreshRequest req)
        {
            ResponceDto res = new ResponceDto();

            if (req.RefreshToken == null || req.AccessToken == null)
            {
                res.IsSuccessed = false;
                res.Message = "Invalid Request";
                return BadRequest(res);
            }

            var refreshToken = req.RefreshToken;
            var accessToken = req.AccessToken;

            var principal = _helper.GetPrincipalFromExpiredToken(accessToken);
            var email = principal.Identity.Name;

            User user = await this._userRepository.GetUserByEmail(email);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime < DateTime.Now)
            {
                res.IsSuccessed = false;
                res.Message = "Invalid Request";
                return BadRequest(res);
            }

            // Generate new tokens
            refreshToken = _helper.GenerateRefreshToken();
            accessToken = _helper.GenerateJwtToken(user);

            // Update user refresh token info
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(2);

            await _userRepository.UpdateUser(user);

            // Prepare response DTO
            TokenRefreshRequest data = new TokenRefreshRequest()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            res.Data = data;

            return Ok(res);


        }

        [HttpPost]
        [Authorize]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            ResponceDto res = new ResponceDto();

            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            var user = await this._userRepository.GetUserByEmail(email);

            if (user == null)
            {
                res.IsSuccessed = false;
                res.Message = "Invalid Request";
                return BadRequest(res);
            }

            // Revoke refresh token
            user.RefreshToken = "";
            await this._userRepository.UpdateUser(user);
            res.IsSuccessed = true;
            res.Message = "User successfully logout";
            return Ok(res);
        }



    }
}