﻿using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton<BlockRepository>();
            services.AddSingleton<JobRepository>();
            services.AddSingleton<PeerRepository>();
            services.AddSingleton<PendingTransactionRepository>();
            services.AddSingleton<TransactionCache>();

            services.AddScoped<BlockService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<MiningService>();
            services.AddScoped<PeerService>();
            
            services.AddScoped<IBlockValidator, PassingBlockValidator>();
            services.AddScoped<ITransactionValidator, PassingTranValidator>();
            //services.AddScoped<IBlockValidator, BlockValidator>();
            //services.AddScoped<ITransactionValidator, TransactionValidator>();
            
            return services;
        }
    }
}