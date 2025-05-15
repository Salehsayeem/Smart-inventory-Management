using Sims.Api.IRepositories;
using Sims.Api.Repositories;

namespace Sims.Api.Helper
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            services.AddTransient<IAuthRepository, AuthRepository>();
            return services;
        }
    }
}
