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
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Account is deactivated");
            }

            return await GenerateAuthResponse(user);
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // Check if username exists
            if (await _userRepository.UsernameExistsAsync(request.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Check if email exists
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var user = _mapper.Map<User>(request);
            user.PasswordHash = HashPassword(request.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            // Use AddAsync from GenericRepository (changed from CreateAsync)
            var createdUser = await _userRepository.AddAsync(user);

            return await GenerateAuthResponse(createdUser);
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            // Validate refresh token
            var isValid = await _refreshTokenRepository.ValidateTokenAsync(request.RefreshToken);
            if (!isValid)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            // Get user from refresh token
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
            var user = await _userRepository.GetByIdAsync(refreshToken.UserId);

            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("User not found or inactive");
            }

            // Revoke old refresh token
            await _refreshTokenRepository.RevokeTokenAsync(request.RefreshToken);

            // Generate new tokens
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

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        private async Task<AuthResponse> GenerateAuthResponse(User user)
        {
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Save refresh token
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            // Use AddAsync from GenericRepository (changed from CreateAsync)
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            // Update user with refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenEntity.ExpiryDate;
            await _userRepository.UpdateAsync(user);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiry = DateTime.UtcNow.AddMinutes(15),
                User = _mapper.Map<UserDto>(user)
            };
        }
    }
}