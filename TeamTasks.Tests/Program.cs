using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using TeamTasks.EntityFramework;

namespace TeamTasks.Tests
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            var services = new ServiceCollection();

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

            var provider = services.BuildServiceProvider();

            TeamTasksDbContext db = provider.GetRequiredService<TeamTasksDbContext>();

            //ProjectStatusTests.CreateProjectStatus(db);
            //ProjectStatusTests.UpdateProjectStatus(db);
            //CryptopgraphyTests.TestCryptography();
            CryptopgraphyTests.TestWebTokens();

            var dummy = 3;
            Console.WriteLine("Hello World!");
            Console.ReadLine();

        }
    }
}