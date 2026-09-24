using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TechStore.Common.CommonFunction;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Extensions;
using TechStore.Common.Helpers;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Authentication;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Snapshot;
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

        public async Task<ServiceResult<LoginResponseModel>> LoginCustomer(LoginRequestModel loginModel)
        {
            var user = await _uow.Users.
                FindOneAsync(u => u.Email == loginModel.LoginIdentifier || u.PhoneNumber == loginModel.LoginIdentifier);

            if (user == null)
            {
                return ServiceResult<LoginResponseModel>.Failure(EErrorType.NotFound, Messenger.LoginError);
            }

            bool isValid = _passwordService.VerifyPassword(user, loginModel.Password, user.PasswordHash);

            if (!isValid)
            {
                return ServiceResult<LoginResponseModel>.Failure(EErrorType.NotFound, Messenger.LoginError);
            }

            var loginResponse = new LoginResponseModel
            {
                RefreshTokenRotationResult = new RefreshTokenRotationResult
                {
                    AccessToken = GenerateJwtTokenForUser(user.PublicId, AppRoles.Customer),
                    AccessTokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpireMinutes),
                    RefreshToken = GenerateRefreshToken(),
                    RefreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(_jwtConfig.RefreshTokenExpireDays),
                },
                User = user.ToUserResponseModel(AppRoles.Customer)
            };

            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = HashRefreshToken(loginResponse.RefreshTokenRotationResult.RefreshToken),
                ExpiresAt = loginResponse.RefreshTokenRotationResult.RefreshTokenExpiresAt,
                CreatedAt = TimeZoneHelper.GetUtcNow()
            };

            await _uow.RefreshTokens.AddAsync(newRefreshTokenEntity);

            var result = await _uow.CommitAsync();

            if (result < 0)
            {
                return ServiceResult<LoginResponseModel>.Failure(EErrorType.SystemError, Messenger.SystemError);
            }

            return ServiceResult<LoginResponseModel>.Success(loginResponse, Messenger.LoginSuccessfull);
        }

        public async Task<ServiceResult<LoginResponseModel>> LoginAdmin(LoginRequestModel loginModel)
        {
            
            var user = await _uow.Users.
                FindOneAsync(u => u.Email == loginModel.LoginIdentifier || u.PhoneNumber == loginModel.LoginIdentifier);

            if (user == null)
            {
                return ServiceResult<LoginResponseModel>.Failure(EErrorType.NotFound, Messenger.LoginError);
            }

            if (user.RoleId != ERole.Admin && user.RoleId != ERole.Staff)
            {

                return ServiceResult<LoginResponseModel>.Failure(EErrorType.Unauthorized, Messenger.NoPermission);
            }

            bool isValid = _passwordService.VerifyPassword(user, loginModel.Password, user.PasswordHash);

            if (!isValid)
            {
                return ServiceResult<LoginResponseModel>.Failure(EErrorType.NotFound, Messenger.LoginError);
            }

            var loginResponse = new LoginResponseModel
            {
                User = user.ToUserResponseModel(AppRoles.Admin),
                RefreshTokenRotationResult = new RefreshTokenRotationResult
                {
                    AccessToken = GenerateJwtTokenForUser(user.PublicId, AppRoles.Admin),
                    AccessTokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpireMinutes),
                    RefreshToken = GenerateRefreshToken(),
                    RefreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(_jwtConfig.RefreshTokenExpireDays),
                }
            };

            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = HashRefreshToken(loginResponse.RefreshTokenRotationResult.RefreshToken),
                ExpiresAt = TimeZoneHelper.GetUtcNow().AddDays(_jwtConfig.RefreshTokenExpireDays),
                CreatedAt = TimeZoneHelper.GetUtcNow()
            };

            await _uow.RefreshTokens.AddAsync(newRefreshTokenEntity);

            var result = await _uow.CommitAsync();

            if (result < 0)
            {
                return ServiceResult<LoginResponseModel>.Failure(EErrorType.SystemError, Messenger.SystemError);
            }

            return ServiceResult<LoginResponseModel>.Success(loginResponse, Messenger.LoginSuccessfull);
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

        public async Task<ServiceResult<string>> RegisterAdminByEmail(RegisterModel registerModel)
        {
            ServiceResult<string> serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.SystemError
            };

            var existUser = await _uow.Users.
                FindOneAsync(u => u.Email == registerModel.Phonenumber);

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
                City = "",
                District = "",
                Email = registerModel.Phonenumber,
                PasswordHash = registerModel.Password,
                Status = EUserStatus.Active,
                RoleId = ERole.Admin,
                Birthday = TimeZoneHelper.ConvertGmt7ToUtc(registerModel.UserInformation.Birthday),

                CreatedAt = TimeZoneHelper.GetUtcNow(),
            };

            user.PasswordHash = _passwordService.HashPassword(user, registerModel.Password);

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

        private string GenerateJwtTokenForUser(string userPublicId, string userRole)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userPublicId),
                new Claim(AppClaims.UserId, userPublicId), // Ví dụ thêm UserId
                new Claim(AppClaims.Role, userRole),   // Ví dụ thêm Role
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpireMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }

        private string HashRefreshToken(string refreshToken)
        {
            var hash = SHA256.HashData(
                Encoding.UTF8.GetBytes(refreshToken));

            return Convert.ToHexString(hash);
        }


        public async Task<ServiceResult<bool>> LogoutAsync(string accesstoken, string refreshToken)
        {
            var refreshtokenHash = HashRefreshToken(refreshToken);

            var existRefreshtoken = await _uow.RefreshTokens.FindOneAsync(x => x.TokenHash == refreshtokenHash);

            if (existRefreshtoken == null)
            {
                return ServiceResult<bool>.Failure(EErrorType.SystemError, Messenger.SystemError);
            }

            var principal = ValidateJwtToken(accesstoken);
            if (principal == null)
            {
                return ServiceResult<bool>.Failure(EErrorType.SystemError, AuthMessenger.InvalidAccessToken);
            }

            var jti = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrEmpty(jti))
            {
                return ServiceResult<bool>.Failure(EErrorType.SystemError, AuthMessenger.InvalidAccessToken);
            }

            var expiryDate = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            if (!long.TryParse(expiryDate, out var expUnix))
            {
                return ServiceResult<bool>.Failure(EErrorType.SystemError, AuthMessenger.InvalidAccessToken);
            }
            var expiryDateTime = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;

            var invalidToken = new InvalidToken
            {
                Jti = jti,
                Token = accesstoken,
                ExpiryDate = expiryDateTime,
                InvalidatedAt = TimeZoneHelper.GetUtcNow()
            };

            existRefreshtoken.RevokedAt = TimeZoneHelper.GetUtcNow();

            await _uow.InvalidTokens.AddAsync(invalidToken);

            _uow.RefreshTokens.Update(existRefreshtoken);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return ServiceResult<bool>.Failure(EErrorType.SystemError, Messenger.SystemError);
            }

            return ServiceResult<bool>.Success(true);
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

        public async Task<ServiceResult<bool>> RegisterCustomer(CustomerRegisterModel model)
        {
            if (!Validator.IsValidPassword(model.Password))
            {
                return ServiceResult<bool>.Failure(EErrorType.BadRequest, AuthMessenger.InvalidPasswordFormat);
            }

            if (!Validator.IsValidVietnamPhone(model.PhoneNumber))
            {
                return ServiceResult<bool>.Failure(EErrorType.BadRequest, AuthMessenger.InvalidPhoneFormat);
            }

            if (!String.IsNullOrEmpty(model.Email))
            {
                if (!Validator.IsValidEmail(model.Email))
                {
                    return ServiceResult<bool>.Failure(EErrorType.BadRequest, AuthMessenger.InvalidEmailFormat);
                }

                var isExistEmail = await _uow.Users.FindOneAsync(u => u.Email == model.Email);

                if (isExistEmail != null)
                {
                    return ServiceResult<bool>.Failure(EErrorType.ConfictData, AuthMessenger.EmailAlreadyExist);
                }
            }

            if (String.IsNullOrEmpty(model.Address))
            {
                return ServiceResult<bool>.Failure(EErrorType.BadRequest, AuthMessenger.AddressRequired);
            }

            var isExistPhoneNumber = await _uow.Users.FindOneAsync(u => u.PhoneNumber == model.PhoneNumber);

            if (isExistPhoneNumber != null)
            {
                return ServiceResult<bool>.Failure(EErrorType.ConfictData, AuthMessenger.PhonenNumberAlreadyExist);
            }

            var userId = await _sequenceService.GetNextUserIdAsync();

            User user = new User
            {
                PublicId = userId,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Address = model.Address,
                City = model.City,
                District = "",
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                PasswordHash = model.Password,
                Status = EUserStatus.Active,
                RoleId = ERole.Customer,

                CreatedAt = TimeZoneHelper.GetUtcNow(),
            };

            user.PasswordHash = _passwordService.HashPassword(user, model.Password);

            await _uow.Users.AddAsync(user);
            var result = await _uow.CommitAsync();

            if (result < 0)
            {
                return ServiceResult<bool>.Failure(EErrorType.SystemError, Messenger.SystemError);
            }

            return ServiceResult<bool>.Success(true, AuthMessenger.RegisterSuccess);
        }

        public async Task<ServiceResult<bool>> ChangePasswordAsync(string userId, ChangePasswordModel changePasswordModel)
        {
            var user = await _uow.Users.GetByIdAsync(userId);

            if (user == null)
            {
                return ServiceResult<bool>.Failure(EErrorType.NotFound, AuthMessenger.NotFoundUser);
            }

            if (!Validator.IsValidPassword(changePasswordModel.NewPassword))
            {
                return ServiceResult<bool>.Failure(EErrorType.BadRequest, AuthMessenger.InvalidPasswordFormat);
            }

            bool isValid = _passwordService.VerifyPassword(user, changePasswordModel.OldPassword, user.PasswordHash);

            if (!isValid)
            {
                return ServiceResult<bool>.Failure(EErrorType.BadRequest, AuthMessenger.InvalidPasswordFormat);
            }

            user.PasswordHash = _passwordService.HashPassword(user, changePasswordModel.NewPassword);

            _uow.Users.Update(user);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return ServiceResult<bool>.Failure(EErrorType.SystemError, Messenger.SystemError);
            }

            return ServiceResult<bool>.Success(true, AuthMessenger.UpdateSuccessFull);
        }

        /// <summary>
        /// Xử lý việc tạo Access Token mới dựa trên Refresh Token cũ.
        /// Việc cấp lại Access Token và Refresh Token mới cần phải xử lý Concurrency để tránh việc user gọi 1 lượt nhiều request cùng dùng 
        /// chung một Refresh Token cũ (trong khi hệ thống đã xử lý rằng nếu phát hiện 1 request mới dùng 1 Refresh Token cũ thì sẽ xác định
        /// đó là hacker và hệ thống sẽ có hành động phù hợp)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ServiceResult<RefreshTokenRotationResult>> GenerateNewAccessTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return ServiceResult<RefreshTokenRotationResult>.Failure(
                    EErrorType.Unauthorized,
                    AuthMessenger.InvalidRefreshToken
                    );
            }

            var oldTokenHash = HashRefreshToken(request.RefreshToken);

            await using var transaction = await _uow.BeginTransactionAsync(cancellationToken);

            try
            {
                /*
                 * Phải khóa record RefreshToken cũ.
                 *
                 * Nếu hai request đồng thời dùng cùng một token,
                 * request thứ hai phải chờ request thứ nhất xử lý xong.
                 */
                var oldRefreshToken = await _uow.RefreshTokens.GetForUpdateAsync_PostgreSQL(oldTokenHash);

                if (oldRefreshToken is null)
                {
                    await transaction.RollbackAsync(cancellationToken);

                    return ServiceResult<RefreshTokenRotationResult>.Failure(
                        EErrorType.Unauthorized,
                        AuthMessenger.NotFoundRefreshToken);
                }


                #region validate idempotency key
                var existingIdempotencyKey = await _uow.IdempotencyKeys.TableNoTracking.FirstOrDefaultAsync(x =>
                                                        x.UserId == oldRefreshToken.Id &&
                                                        x.RequestKey == request.IdempotencyKey);

                var requestHash = ShareFunctions.ComputeHash(request);

                if (existingIdempotencyKey != null)
                {
                    // Check body request hash to ensure the same request is being made
                    if (existingIdempotencyKey.RequestHash == requestHash)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<RefreshTokenRotationResult>.Success(JsonSerializer.Deserialize<RefreshTokenRotationResult>(existingIdempotencyKey.ResponseBody));
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<RefreshTokenRotationResult>.Failure(EErrorType.IdempotencyKeyConflict, Messenger.IdempotencyKeyConflict);
                    }
                }
                #endregion

                if (oldRefreshToken.RevokedAt.HasValue)
                {
                    //if(oldRefreshToken.ReplacedByTokenHash != null)
                    //{
                    //    // nếu RefreshToken này đã được dùng để cấp 1 Accesstoken mới
                    //    // thì rất có thể RefreshToken này đã bị hacker đánh cắp
                    //    // lúc này nên thu hồi accesstoken để mọi thiết bị bị logout 
                    //    // và để user đăng nhập lại cho an toàn 
                    //}

                    await transaction.RollbackAsync(cancellationToken);

                    return ServiceResult<RefreshTokenRotationResult>.Failure(
                        EErrorType.Unauthorized,
                        AuthMessenger.RevokedRefreshToken);
                }

                if (oldRefreshToken.ExpiresAt <= DateTime.UtcNow)
                {
                    await transaction.RollbackAsync(cancellationToken);

                    return ServiceResult<RefreshTokenRotationResult>.Failure(
                        EErrorType.Unauthorized,
                        AuthMessenger.ExpiredRefreshToken);
                }

                var user = await _uow.Users.TableNoTracking
                    .FirstOrDefaultAsync(u => u.Id == oldRefreshToken.UserId, cancellationToken);

                if (user is null)
                {
                    await transaction.RollbackAsync(cancellationToken);

                    return ServiceResult<RefreshTokenRotationResult>.Failure(
                        EErrorType.Unauthorized,
                        AuthMessenger.NotFoundUser);
                }

                /*
                 * 1. Tạo Access Token mới
                 */
                var newAccessToken = GenerateJwtTokenForUser(
                    user.PublicId,
                    user.RoleId == ERole.Customer ? AppRoles.Customer : AppRoles.Admin);

                /*
                 * 2. Tạo Refresh Token mới
                 */
                var newRefreshToken = GenerateRefreshToken();

                var newRefreshTokenHash = HashRefreshToken(newRefreshToken);

                /*
                 * 3. Revoke Refresh Token cũ
                 */
                oldRefreshToken.RevokedAt = TimeZoneHelper.GetUtcNow();
                oldRefreshToken.ReplacedByTokenHash = newRefreshTokenHash;

                /*
                 * 4. Lưu Refresh Token mới
                 */
                var newRefreshTokenEntity = new RefreshToken
                {
                    UserId = oldRefreshToken.UserId,
                    TokenHash = newRefreshTokenHash,
                    ExpiresAt = TimeZoneHelper.GetUtcNow().AddDays(_jwtConfig.RefreshTokenExpireDays),
                    CreatedAt = TimeZoneHelper.GetUtcNow()
                };

                await _uow.RefreshTokens.AddAsync(newRefreshTokenEntity, cancellationToken);

                var response = new RefreshTokenRotationResult
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    AccessTokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpireMinutes),
                    RefreshTokenExpiresAt = newRefreshTokenEntity.ExpiresAt,
                };

                if (existingIdempotencyKey == null)
                {
                    var idempotencyKeyId = Guid.NewGuid();

                    var idempotencyKeyEntity = new IdempotencyKey
                    {
                        Id = idempotencyKeyId,
                        PublicId = ShareFunctions.GenerateRandomStringId(),
                        UserId = oldRefreshToken.UserId,
                        Endpoint = "/api/authentication/refresh",
                        StatusCode = 200,
                        ExpiredAt = TimeZoneHelper.GetUtcNow().AddMinutes(1),
                        RequestKey = request.IdempotencyKey, // thực tế là lưu token vào db không an toàn. nhưng ở đây vẫn chọn lưu
                                                             // để dễ dàng trả lại cho user nếu trùng idempotencykey và chọn cách khắc 
                                                             // phục là sẽ cho idempotencykey có tuổi thọ sống rất ngắn bằng cách tạo
                                                             // background job chạy liên tục mỗi phút để xóa idempotencykey
                        RequestHash = requestHash,
                        ResponseBody = JsonSerializer.Serialize(response),
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                    };

                    await _uow.IdempotencyKeys.AddAsync(idempotencyKeyEntity);
                }

                /*
                 * 5. Lưu cả revoke token cũ và token mới
                 * trong cùng một transaction.
                 */
                var result =  await _uow.CommitAsync(cancellationToken);

                if(result < 1)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return ServiceResult<RefreshTokenRotationResult>.Failure(
                        EErrorType.SystemError,
                        Messenger.SystemError);
                }

                /*
                 * 6. Commit DB trước khi set Cookie
                 */
                await transaction.CommitAsync(cancellationToken);

                return ServiceResult<RefreshTokenRotationResult>.Success(response);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<bool> IsInvalidAsync(string jti)
        {
            var invalidToken = await _uow.InvalidTokens.TableNoTracking.Where(x => x.Jti == jti).FirstOrDefaultAsync();

            if (invalidToken == null)
            {
                return false;
            }

            return true;
        }
    }
}
