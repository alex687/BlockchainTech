using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Node.CLI.Configurations;
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

            //services.AddSingleton<IMediator, Mediator>();
            services.AddScoped<BlockService>();
            services.AddSingleton<TransactionService>();
            services.AddSingleton<MiningService>();
            services.AddSingleton<PeerService>();

            services.AddSingleton<IBlockchain, Blockchain>();
            services.AddSingleton<IBlockValidator, BlockValidator>();
            services.AddSingleton<ITransactionValidator, TransactionValidator>();

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