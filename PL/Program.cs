using AutoMapper;
using BLL.Interfaces;
using BLL.Repositories;
using DAL.Contexts;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PL.MappingProfile;
using PL.MappingProfiles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace PL
{
    public class Program
    {
        //entry point
        public static void Main(string[] args)
        {
           // 1)create Host Builder
            var Builder = WebApplication.CreateBuilder(args);



            #region Configure Services That Allow Dependency Injection (called by the runtime.  add services to the container.)


            Builder.Services.AddControllersWithViews();//Mvc

            Builder.Services.AddDbContext<MvcAppDbContext>(options =>
            {
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
            });//Allow dependency Injection




            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // options.Password.RequireNonAlphanumeric = true;//@ #
                // options.Password.RequireDigit = true;//123
                // options.Password.RequireLowercase = true;//a
                // options.Password.RequireUppercase = true;//A
            }).

            AddEntityFrameworkStores<MvcAppDbContext>().AddDefaultTokenProviders();

            Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "Account/Login";
                options.AccessDeniedPath = "Home/Error";
            });//Allow dependency injection for Identity Services
               //Cookie is Key for Token

            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();//Allow Dependency Injection from class unitOfWork

            //services.AddAutoMapper(M=>M.AddProfile(new EmployeeProfile()));//Allow Dependency Injection for profile




            Builder.Services.AddAutoMapper(M => M.AddProfiles(new List<Profile>()
            {
                new EmployeeProfile(),new UserProfile(),new RoleProfile(),
            }));//Allow Dependency Injection for all profiles


            #endregion


            var app=Builder.Build();



            #region configure HttpRequest Pipeline (MidleWare)


            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });



            #endregion


            app.Run();

    }
    }
}
