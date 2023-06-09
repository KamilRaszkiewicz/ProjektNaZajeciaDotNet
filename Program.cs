using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Projekt.Extensions;
using Projekt.Interfaces;
using Projekt.Models;
using Projekt.Models.Entities;
using Projekt.Options;
using Projekt.Services;
using System.Net.Mail;

namespace Projekt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddRazorPages(opts =>
            {
                opts.Conventions.AddFolderRouteModelConvention("/Comments", opts =>
                {
                    foreach (var x in opts.Selectors)
                    {
                        x.AttributeRouteModel.Order = -1;
                    }
                });
                opts.Conventions.AddPageRouteModelConvention("/Comments/Create", opts =>
                {
                    foreach (var x in opts.Selectors)
                    {
                        x.AttributeRouteModel.Order = -1;
                    }
                });
                opts.Conventions.AddPageRouteModelConvention("/Comments/Delete", opts =>
                {
                    foreach (var x in opts.Selectors)
                    {
                        x.AttributeRouteModel.Order = -1;
                    }
                });
            });

            builder.Services.AddDbContext<ApplicationDbContext>(opts => 
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                )); 

            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole<int>>()
                .AddRoleManager<RoleManager<IdentityRole<int>>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
  
            builder.Services.AddScoped<IPostsService, PostsService>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddSingleton<IImagesService, ImagesService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            Task.Run(async () => await app.Services.CreateScope().SeedDatabase()).Wait();
            app.Run();
        }
    }
}