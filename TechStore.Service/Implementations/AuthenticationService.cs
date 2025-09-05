using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Helpers;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Authentication;
using TechStore.Model.DTOs.User;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JWTConfig _jwtConfig;
        private readonly IPasswordService _passwordService;

        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;

        public AuthenticationService(IOptions<JWTConfig> jwtOptions,
            IUnitOfWork unitOfWork,
            SequenceGeneratorService sequenceService,
            IPasswordService passwordService
            )
        {
            _jwtConfig = jwtOptions.Value;
            _uow = unitOfWork;
            _sequenceService = sequenceService;
            _passwordService = passwordService;
        }

        public async Task<ServiceResult<LoginResponseModel>> CustomerLogin(LoginRequestModel loginModel)
        {
            ServiceResult<LoginResponseModel> serviceResult = new ServiceResult<LoginResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.LoginError
            };

            var user = await _uow.Users.
                FindOneAsync(u => u.Email == loginModel.LoginIdentifier || u.PhoneNumber == loginModel.LoginIdentifier);

            if (user == null)
            {
                return serviceResult;
            }

            //bool isValid = _passwordService.VerifyPassword(user, loginModel.Password, user.PasswordHash);

            if (user.PasswordHash != loginModel.Password)
            {
                serviceResult.Message = Messenger.LoginError;
                return serviceResult;
            }

            serviceResult.Data = new LoginResponseModel
            {
                Token = GenerateJwtTokenForUser(user.PublicId, AppRoles.Customer),
                User = user.ToUserResponseModel(AppRoles.Customer)
            };

            serviceResult.Message = Messenger.LoginSuccessfull;
            serviceResult.IsSuccess = true;

            return serviceResult;
        }

        public async Task<ServiceResult<LoginResponseModel>> AdminLogin(LoginRequestModel loginModel)
        {
            ServiceResult<LoginResponseModel> serviceResult = new ServiceResult<LoginResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.LoginError
            };

            var user = await _uow.Users.
                FindOneAsync(u => u.Email == loginModel.LoginIdentifier || u.PhoneNumber == loginModel.LoginIdentifier);

            if (user == null)
            {
                return serviceResult;
            }

            if (user.RoleId != ERole.Admin && user.RoleId != ERole.Staff)
            {

                serviceResult.ErrorCode = "NoPermission";
                serviceResult.Message = Messenger.NoPermission;
                return serviceResult;
            }

            //bool isValid = _passwordService.VerifyPassword(user, loginModel.Password, user.PasswordHash);

            if (user.PasswordHash != loginModel.Password)
            {
                serviceResult.Message = Messenger.LoginError;
                return serviceResult;
            }

            serviceResult.Data = new LoginResponseModel
            {
                Token = GenerateJwtTokenForUser(user.PublicId, AppRoles.Admin),
                User = user.ToUserResponseModel(AppRoles.Admin)
            };

            serviceResult.Message = Messenger.LoginSuccessfull;
            serviceResult.IsSuccess = true;

            return serviceResult;
        }


        public async Task<ServiceResult<bool>> IsUserExist(string identifier)
        {
            ServiceResult<bool> serviceResult = new ServiceResult<bool>
            {
                IsSuccess = true,
                Data = false,
                Message = string.Empty
            };

            var user = await _uow.Users.
                FindOneAsync(u => u.Email == identifier || u.PhoneNumber == identifier);

            if (user != null)
            {
                serviceResult.Data = true;
            }

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateUserRole(UserRoleUpdateModel model)
        {
            ServiceResult<bool> serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var user = await _uow.Users.
                FindOneAsync(u => u.PublicId == model.UserId);

            if (user == null)
            {
                return serviceResult;
            }

            user.RoleId = model.Role;

            _uow.Users.Update(user);
            var result = await _uow.CommitAsync();

            if (result < 0)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.Data = true;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<string>> AdminRegisterWithEmail(RegisterModel registerModel)
        {
            ServiceResult<string> serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.SystemError
            };

            var existUser = await _uow.Users.
                FindOneAsync(u => u.Email == registerModel.RegisterIdentifier);

            if (existUser != null)
            {
                serviceResult.Message = Messenger.EmailAlreadyExist;
                return serviceResult;
            }

            var userId = await _sequenceService.GetNextUserIdAsync();

            User user = new User
            {
                PublicId = userId,
                LastName = registerModel.UserInformation.LastName,
                FirstName = registerModel.UserInformation.FirstName,
                Address = registerModel.UserInformation.Address,
                PhoneNumber = registerModel.UserInformation.PhoneNumber,
                Email = registerModel.RegisterIdentifier,
                PasswordHash = registerModel.Password,
                Status = EUserStatus.Active,
                RoleId = ERole.Admin,
                Birthday = registerModel.UserInformation.Birthday,

                EntityStatus = EEntityStatus.Active,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
            };

            //user.PasswordHash = _passwordService.HashPassword(user, registerModel.Password);

            await _uow.Users.AddAsync(user);
            var result = await _uow.CommitAsync();

            if (result < 0)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.Data = userId;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<string>> UserRegisterWithEmail(RegisterModel registerModel)
        {
            ServiceResult<string> serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.BadRequest
            };

            var existUser = await _uow.Users.
                FindOneAsync(u => u.Email == registerModel.RegisterIdentifier || u.PhoneNumber == registerModel.RegisterIdentifier);

            if (existUser != null)
            {
                serviceResult.Message = Messenger.EmailAlreadyExist;
                return serviceResult;
            }

            var userId = await _sequenceService.GetNextUserIdAsync();

            User user = new User
            {
                PublicId = userId,
                LastName = registerModel.UserInformation.LastName,
                FirstName = registerModel.UserInformation.FirstName,
                Address = registerModel.UserInformation.Address,
                PhoneNumber = registerModel.UserInformation.PhoneNumber,
                Email = registerModel.RegisterIdentifier,
                PasswordHash = registerModel.Password,
                Status = EUserStatus.Active,
                RoleId = ERole.Customer,
                Birthday = registerModel.UserInformation.Birthday,

                EntityStatus = EEntityStatus.Active,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
            };

            //user.PasswordHash = _passwordService.HashPassword(user, registerModel.Password);

            await _uow.Users.AddAsync(user);
            var result = await _uow.CommitAsync();

            if (result < 0)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.Data = userId;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;

            return serviceResult;
        }


        public async Task<ServiceResult<string>> Register(RegisterModel registerModel)
        {
            ServiceResult<string> serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.SystemError
            };

            var existUser = await _uow.Users.
                FindOneAsync(u => u.Email == registerModel.RegisterIdentifier || u.PhoneNumber == registerModel.RegisterIdentifier);

            if (existUser != null)
            {
                serviceResult.Message = "Phone number or email already exist!";
                return serviceResult;
            }

            var userId = await _sequenceService.GetNextUserIdAsync();

            User user = new User
            {
                PublicId = userId,
                LastName = registerModel.UserInformation.LastName,
                FirstName = registerModel.UserInformation.FirstName,
                Address = registerModel.UserInformation.Address,
                PhoneNumber = registerModel.UserInformation.PhoneNumber,
                Email = registerModel.RegisterIdentifier,
                PasswordHash = registerModel.Password,
                Status = EUserStatus.Active,
                RoleId = ERole.Customer,
                Birthday = registerModel.UserInformation.Birthday,

                EntityStatus = EEntityStatus.Active,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
            };

            //user.PasswordHash = _passwordService.HashPassword(user, registerModel.Password);

            await _uow.Users.AddAsync(user);
            var result = await _uow.CommitAsync();

            if (result < 0)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.Data = userId;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;

            return serviceResult;
        }

        private string GenerateJwtTokenForUser(string userId, string appRoles)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(AppClaims.UserId, userId), // Ví dụ thêm UserId
                new Claim(AppClaims.Role, appRoles),   // Ví dụ thêm Role
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<ServiceResult<bool>> LogoutAsync(string token)
        {
            var serviceResult = new ServiceResult<bool>()
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.SystemError
            };

            var principal = ValidateJwtToken(token);
            if (principal == null)
            {
                serviceResult.Message = "Invalid token.";
                return serviceResult;
            }

            var jti = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrEmpty(jti))
            {
                return serviceResult;
            }

            var expiryDate = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            if (!long.TryParse(expiryDate, out var expUnix))
            {
                return serviceResult;
            }
            var expiryDateTime = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;

            var invalidToken = new InvalidToken
            {
                Jti = jti,
                Token = token,
                ExpiryDate = expiryDateTime,
                InvalidatedAt = DateTime.UtcNow
            };

            await _uow.InvalidTokens.AddAsync(invalidToken);
            var result = await _uow.CommitAsync();

            if (result < 0)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.Data = true;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;

            return serviceResult;
        }

        // Hàm validate token
        private ClaimsPrincipal? ValidateJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtConfig.SigningKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtConfig.Issuer,
                    ValidAudience = _jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero // Không cho phép sai lệch thời gian
                };

                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }
    }
}
