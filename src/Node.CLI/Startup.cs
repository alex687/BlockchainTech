using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Node.CLI.Configurations;
using Node.CLI.Services;

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
            
            services.AddSingleton<BlockService>();
            services.AddSingleton<TransactionService>();
            services.AddSingleton<MiningService>();
            services.AddSingleton<PeerService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}