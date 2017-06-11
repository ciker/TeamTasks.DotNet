using CoreLibrary.AuthServer;
using CoreLibrary.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TeamTasks.AuthServer.Providers;
using TeamTasks.EntityFramework;

namespace TeamTasks.AuthServer
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration["TeamTasksConnectionString"];
            services.AddDbContext<TeamTasksDbContext>(options =>
            {
                options.UseSqlServer(connectionString, opts => {
                    opts.UseRowNumberForPaging();
                });
            });

            services.AddIdentity<TeamTasksUser, TeamTasksRole>()
                .AddEntityFrameworkStores<TeamTasksDbContext, int>()
                .AddDefaultTokenProviders();

            services.AddTransient<ICredentialsProvider, SimpleCredentialsProvider>();
            services.AddTransient<IAdditionalClaimsProvider, AdditionalClaimsProvider>();
            services.AddSingleton<ICrypter, Crypt>();

            services.AddCors();

            services.AddMvcCore().AddJsonFormatters(setupAction => {
                setupAction.ContractResolver = new CamelCasePropertyNamesContractResolver();
                setupAction.DefaultValueHandling = DefaultValueHandling.Ignore;
                setupAction.NullValueHandling = NullValueHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            TokenIssuerOptions tokenIssuerOptions = new TokenIssuerOptions();
            tokenIssuerOptions.Issuer = "FlixReseller Issuer";
            tokenIssuerOptions.CryptionKey = Configuration["CryptionKey"];
            
            app.UseTokenIssuerMiddleware<TeamTasksTokenIssuerMiddleware, TeamTasksAuthServerResponse>(tokenIssuerOptions);

            app.UseMvc();
        }
    }
}
