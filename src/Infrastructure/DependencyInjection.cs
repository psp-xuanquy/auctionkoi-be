using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KoiAuction.Domain.Common.Interfaces;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Domain.Repositories;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Repositories;

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
                });
            options.UseLazyLoadingProxies();
        });

        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<IAuctionRepository, AuctionRepository>();
        services.AddTransient<IAutoBidRepository, AutoBidRepository>();
        services.AddTransient<IBidRepository, BidRepository>();            
        services.AddTransient<IBlogRepository, BlogRepository>();
        services.AddTransient<IBillRepository, BillRepository>();
        services.AddTransient<IKoiRepository, KoiRepository>();
        services.AddTransient<IKoiMediaRepository, KoiMediaRepository>();
        services.AddTransient<IPaymentRepository, PaymentRepository>();
        services.AddTransient<ITransactionRepository, TransactionRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        return services;
    }
}