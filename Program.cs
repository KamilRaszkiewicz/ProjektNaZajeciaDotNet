using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Projekt.Interfaces;
using Projekt.Models;
using Projekt.Models.Entities;
using Projekt.Services;

namespace Projekt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<ApplicationDbContext>(opts => 
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                ));
  
            builder.Services.AddIdentity<User, IdentityRole<int>>()
                .AddRoles<IdentityRole<int>>()
                .AddRoleManager<RoleManager<IdentityRole<int>>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

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

            app.Run();
        }
    }
}