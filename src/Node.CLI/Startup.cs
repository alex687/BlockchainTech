using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Node.CLI.Configurations;
using Node.CLI.Repositories;
using Node.CLI.Services;
using Node.Core;
using Node.Core.Validators.Block;
using Node.Core.Validators.Transactions;
using Swashbuckle.AspNetCore.Swagger;

namespace Node.CLI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(c => c.AddProfile(typeof(AutoMapperConfig)));
            services.AddMediatR();
            services.AddMvc();

            // injection config -> to extract in Config folder as extension
            services.AddScoped<IBlockValidator, BlockValidator>();
            services.AddScoped<ITransactionValidator, TransactionValidator>();
            services.AddScoped<BlockService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<MiningService>();
            services.AddScoped<PeerService>();

            services.AddSingleton<BlockRepository>();
            services.AddScoped<TransactionRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "BlockCainTech", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chain API V1");
            });

            app.UseMvc();
        }
    }
}