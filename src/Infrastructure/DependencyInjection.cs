using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KoiAuction.Domain.Common.Interfaces;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Domain.Repositories;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Repositories;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Domain.IRepositories.IBaseRepositories;
using Infrastructure.Repositories.BaseRepositories;

namespace KoiAuction.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("Server"),
                b =>
                {
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    //b.EnableRetryOnFailure();
                });
            options.UseLazyLoadingProxies();
        });

        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IRepositoryFactory, RepositoryFactory>();

        services.AddTransient<IAutoBidRepository, AutoBidRepository>();
        services.AddTransient<IBidRepository, BidRepository>();            
        services.AddTransient<IBlogRepository, BlogRepository>();
        services.AddTransient<IKoiImageRepository, KoiImageRepository>();
        services.AddTransient<IKoiRepository, KoiRepository>();
        services.AddTransient<IKoiBreederRepository, KoiBreederRepository>();
        services.AddTransient<ITransactionRepository, TransactionRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IAuctionMethodRepository, AuctionMethodRepository>();

        return services;
    }
}
