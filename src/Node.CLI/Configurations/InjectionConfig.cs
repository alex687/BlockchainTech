using Microsoft.Extensions.DependencyInjection;
using Node.CLI.Services;
using Node.Core.Factory;
using Node.Core.Repositories;
using Node.Core.Repositories.Blockchain;
using Node.Core.Validators.Block;
using Node.Core.Validators.Transactions;

namespace Node.CLI.Configurations
{
    public static class InjectionConfig
    {
        public static IServiceCollection AddInjectionConfig(this IServiceCollection services)
        {
            services.AddSingleton<MiningJobsRepository>();
            services.AddSingleton<PeerRepository>();

            services.AddScoped<BlockService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<MiningService>();
            services.AddScoped<PeerService>();
            services.AddScoped<CommunicationService>();
            
            services.AddScoped<BlockchainFactory>();
            services.AddSingleton<BlockchainInstanceHolder>();
            
            return services;
        }
    }
}