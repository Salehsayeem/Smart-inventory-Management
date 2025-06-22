using Sims.Api.IRepositories;
using Sims.Api.Repositories;
using Sims.Api.StoredProcedure;

namespace Sims.Api.Helper
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddSingleton<CallStoredProcedure>();
            return services;
        }
    }
}
