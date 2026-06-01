
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Techstore.API.Hubs;
using TechStore.Common.Constants;
using TechStore.Data.Context;
using TechStore.Data.Repositories;
using TechStore.Data.Repositories.Implementations;
using TechStore.Data.Repositories.Interfaces;
using TechStore.Data.UnitOfWork;
using TechStore.Service.Implementations;
using TechStore.Service.Interfaces;

namespace TechStoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region add service config
            builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<CloudinaryConfig>(builder.Configuration.GetSection("CloudinarySettings"));
            //builder.Services.Configure<VietQRConfig>(builder.Configuration.GetSection("VietQrSettings"));

            var jwtConfig = builder.Configuration.GetSection("JWT").Get<JWTConfig>() ?? new JWTConfig();
            #endregion

            #region Add environment variables when deploy
            builder.Configuration
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile(
                        $"appsettings.{builder.Environment.EnvironmentName}.json",
                        optional: true,
                        reloadOnChange: true
                    )
                    .AddEnvironmentVariables();
            #endregion

            #region Get sections in appsettings.json
            builder.Services.Configure<ConnectionStringSetting>(builder.Configuration.GetSection("ConnectionStrings"));
            #endregion

            #region Database Setting
            var connectionSetting = builder.Configuration
                .GetSection("ConnectionStrings")
                .Get<ConnectionStringSetting>();

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionSetting!.DefaultConnection));
            #endregion

            #region Add services to the container
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<TechStore.Service.Interfaces.IAuthenticationService, TechStore.Service.Implementations.AuthenticationService>();
            builder.Services.AddScoped<IPasswordService, PasswordService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<InitialDataService, InitialDataService>();
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IQRCodeService, QRCodeService>();
            builder.Services.AddScoped<SequenceGeneratorService, SequenceGeneratorService>();
            builder.Services.AddScoped<IShipperService, ShipperService>();
            builder.Services.AddScoped<IStatisticsService, StatisticsService>();
            builder.Services.AddScoped<IUploadDataToCloudService, UploadDataToCloudService>();
            builder.Services.AddHttpClient<VietQrService>();
            builder.Services.AddScoped<IVietQrService, VietQrService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IHomeService, HomeService>();

            builder.Services.AddSignalR();
            #endregion

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            #region Thêm nút đăng nhập bằng token trên Swagger

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Nhập 'Bearer {token}' (bao gồm chữ Bearer và khoảng trắng)",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            #endregion

            #region Config JWT Authentication (cấu hình mặc định một authentication scheme)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(jwtConfig.SigningKey)
                    ),

                    // Đây là dòng bạn cần thêm để ASP.NET biết lấy Role từ claim "Role" thay vì "role"
                    RoleClaimType = AppClaims.Role, // hoặc "Role" nếu bạn hardcode
                    NameClaimType = AppClaims.UserId, // Nếu không set NameClaimType và RoleClaimType, thì ASP.NET Core mặc định sẽ đọc sub cho User.Identity.Name và role cho quyền, nên AppClaims.UserId sẽ bị bỏ qua
                };

                // Bắt sự kiện thất bại
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Token authentication failed:");
                        Console.WriteLine(context.Exception.ToString()); // Thêm dòng này để xem lỗi thật sự
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated successfully.");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine("Token challenge triggered:");
                        Console.WriteLine(context.Error);
                        Console.WriteLine(context.ErrorDescription);
                        return Task.CompletedTask;
                    }
                };
            });
            #endregion

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Angular", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            //app.UseCors("AllowAnyOrigin");
            app.UseCors("Angular");

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<PaymentHub>("/payments/hub");

            app.Run();
        }
    }
}
