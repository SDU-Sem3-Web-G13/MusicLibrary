using Microsoft.Extensions.DependencyInjection;
using Backend.Services;
using Backend.DataAccess;
using Backend.DataAccess.Interfaces;
using Npgsql;

namespace Backend
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBackendServices(this IServiceCollection services)
        {
            // Register DataSrouce
            services.AddScoped<NpgsqlConnection>(provider => PostgresDataSource.CreateConnection());
            services.AddScoped<IDataSource, PostgresDataSource>();
            services.AddScoped<SqlAlbumRepository>();
            services.AddScoped<SqlUserRepository>();

            // Register repositories
            services.AddScoped<IUserRepository, SqlUserRepository>();
            services.AddScoped<IAlbumRepository, SqlAlbumRepository>();

            // Register services
            services.AddScoped<IAdministrationService, AdministrationService>();
            services.AddScoped<IAlbumsService, AlbumsService>();
            services.AddScoped<ILoginRegisterService, LoginRegisterService>();

            return services;
        }
    }
}