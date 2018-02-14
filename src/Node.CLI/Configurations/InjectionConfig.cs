using Microsoft.Extensions.DependencyInjection;
using Node.CLI.Repositories;
using Node.CLI.Services;
using Node.Core.Validators.Block;
using Node.Core.Validators.Transactions;

namespace Node.CLI.Configurations
{
    public static class InjectionConfig
    {
        public static IServiceCollection AddInjectionConfig(this IServiceCollection services)
        {
            services.AddScoped<IBlockValidator, BlockValidator>();
            services.AddScoped<ITransactionValidator, TransactionValidator>();
            services.AddScoped<BlockService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<MiningService>();
            services.AddScoped<PeerService>();

            services.AddSingleton<BlockRepository>();
            services.AddSingleton<PendingTransactionRepository>();
            services.AddSingleton<TransactionCache>();

            return services;
        }
    }
}