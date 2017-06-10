﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamTasks.EntityFramework;

namespace TeamTasks.DevMigration
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = "Data Source=files.digital-edge.biz;Initial Catalog=TeamTasks;User ID=devolved;Password=camp2nick";
            services.AddDbContext<TeamTasksDbContext>(options =>
            {
                options.UseSqlServer(connectionString, opts => {
                    opts.MigrationsAssembly("TeamTasks.DevMigration");
                    opts.UseRowNumberForPaging();
                });
            });

            services.AddIdentity<TeamTasksUser, TeamTasksRole>()
                .AddEntityFrameworkStores<TeamTasksDbContext, int>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            TeamTasksDbContext db, UserManager<TeamTasksUser> userManager, RoleManager<TeamTasksRole> roleManager)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Seeder seeder = new Seeder(db, userManager, roleManager);
            await seeder.InitializeDataAsync();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
