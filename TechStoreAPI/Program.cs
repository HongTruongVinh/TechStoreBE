
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
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

            #region Add environment variables when deploy
            builder.Configuration
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<InitialDataService, InitialDataService>();
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IQRCodeService, QRCodeService>();
            builder.Services.AddScoped<SequenceGeneratorService, SequenceGeneratorService>();
            builder.Services.AddScoped<IShipperService, ShipperService>();
            builder.Services.AddScoped<IStatisticsService, StatisticsService>();
            builder.Services.AddScoped<IUserService, UserService>();

            #endregion

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
