using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Paynext.Application.Authentication;
using Paynext.Application.Business;
using Paynext.Application.Interfaces;
using Paynext.Application.Interfaces.UseCases;
using Paynext.Application.Profiles;
using Paynext.Application.UseCases;
using Paynext.Domain.Interfaces.Repositories;
using Paynext.Infra.Context;
using Paynext.Infra.Repositories;

namespace Paynext.IOC
{
    public static class Ioc
    {
        public static void ConfigureServicesIoc(IServiceCollection services, IConfiguration configuration, ref string tokenSecret)
        {
            services.ConfigureEnvironmentVariables(configuration, ref tokenSecret, out string connectString);
            services.ConfigureDBContext(configuration, connectString);
            services.ConfigureRepositories();
            services.ConfigureValidators();
            services.ConfigureBusiness();
            services.ConfigureProfiles();
            services.ConfigureUseCases();
            services.ConfigureServices(tokenSecret);
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

        }

        public static void ConfigureBusiness(this IServiceCollection services)
        {

            services.AddScoped<IUserBusiness, UserBusiness>();

        }

        public static void ConfigureProfiles(this IServiceCollection services)
        {
            MapsterConfiguration.Configure();
        }

        public static void ConfigureServices(this IServiceCollection services, string tokenSecret)
        {
            services.AddScoped<ITokenService, TokenService>();
        }

        public static void ConfigureValidators(this IServiceCollection services)
        {
            //services.AddSingleton<IValidatorProvider, ValidatorFactory>();
        }

        public static void ConfigureUseCases(this IServiceCollection services)
        {
            services.AddScoped<IUserAuthentication, UserAuthentication>();

        }

        public static void ConfigureEnvironmentVariables(this IServiceCollection services, IConfiguration configuration, ref string tokenSecret, out string connectString)
        {
            Env.TraversePath().Load();

            connectString = Environment.GetEnvironmentVariable("DATABASE_URL") ??  configuration.GetConnectionString("DefaultConnection");

            tokenSecret = Environment.GetEnvironmentVariable("TOKEN_JWT_SECRET") ?? configuration["Jwt:Key"];


        }

        public static void ConfigureDBContext(this IServiceCollection services, IConfiguration configuration, string connectString)
        {
            services.AddDbContextPool<AppDbContext>(options =>
                  options.UseNpgsql(connectString, x => x.MigrationsAssembly("SalesOrderManagement.API.Infra")));
        }
    }
}