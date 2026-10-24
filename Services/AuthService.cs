// Services/AuthService.cs
using AutoMapper;
using server.Dto;
using server.Enity;
using server.Interfaces.Repository;
using server.Interfaces.Services;

namespace server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtService jwtService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated");

            return await GenerateAuthResponse(user);
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepository.UsernameExistsAsync(request.Username))
                throw new InvalidOperationException("Username already exists");

            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new InvalidOperationException("Email already exists");

            var user = _mapper.Map<User>(request);
            user.PasswordHash = HashPassword(request.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            var createdUser = await _userRepository.AddAsync(user);
            return await GenerateAuthResponse(createdUser);
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var isValid = await _refreshTokenRepository.ValidateTokenAsync(request.RefreshToken);
            if (!isValid)
                throw new UnauthorizedAccessException("Invalid refresh token");

            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
            var user = await _userRepository.GetByIdAsync(refreshToken.UserId);

            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException("User not found or inactive");

            await _refreshTokenRepository.RevokeTokenAsync(request.RefreshToken);
            return await GenerateAuthResponse(user);
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            await _refreshTokenRepository.RevokeAllUserTokensAsync(userId);
            return true;
        }

        public async Task<bool> RevokeAllTokensAsync(int userId)
        {
            await _refreshTokenRepository.RevokeAllUserTokensAsync(userId);
            return true;
        }

        public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
        {
            return await _refreshTokenRepository.ValidateTokenAsync(refreshToken);
        }

        public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);

        private async Task<AuthResponse> GenerateAuthResponse(User user)
        {
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenEntity.ExpiryDate;
            await _userRepository.UpdateAsync(user);

            // ✅ Create a manual UserDto to ensure ALL fields (including department) are mapped
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role ?? "User",
                CompanyId = user.CompanyId,
                BranchId = user.BranchId,
                DepartmentId = user.DepartmentId
            };

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiry = DateTime.UtcNow.AddMinutes(15),
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role ?? "User",
                CompanyId = user.CompanyId,
                BranchId = user.BranchId,
                DepartmentId = user.DepartmentId,
                User = userDto
            };
        }
    }
}