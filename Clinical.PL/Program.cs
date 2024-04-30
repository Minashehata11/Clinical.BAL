using Clinical.BAL.Interfaces;
using Clinical.BAL.Repository;
using Clinical.DAL.Context;
using Clinical.DAL.Entities.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NToastNotify;

using Clinical.PL.Mapper;

namespace Clinical.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            builder.Services.AddScoped<IUnitWork, UnitWork>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            //builder.Services.AddScoped<ILogger,Logger<>();
            
            builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                PositionClass = ToastPositions.TopRight,
                PreventDuplicates = true,
                CloseButton = true

            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
                {
                    option.LoginPath = new PathString("/Account/SignIn");
                    option.AccessDeniedPath = new PathString("/Home/error");

                });
           
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
            {
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireDigit = true;
                option.Password.RequiredLength = 8;

                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
         
           
            app.Run();
        }
    }
}