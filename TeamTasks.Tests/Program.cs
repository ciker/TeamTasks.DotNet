using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using TeamTasks.EntityFramework;

namespace TeamTasks.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            string connectionString = "Data Source=files.digital-edge.biz;Initial Catalog=TeamTasks;User ID=devolved;Password=camp2nick";
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
            ProjectStatusTests.UpdateProjectStatus(db);

            var dummy = 3;
            Console.WriteLine("Hello World!");
            Console.ReadLine();

        }
    }
}