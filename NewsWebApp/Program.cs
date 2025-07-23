using AutoMapper;
using DotNetEd.CoreAdmin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using NewsWebsite.DAL;
using NewsWebsite.DAL.Context;
using NewsWebsite.DAL.Contracts;
using NewsWebsite.DAL.Etities.Identity;
using NewsWebsite.DAL.IdentityData;
using NewsWebsite.Services.Abstraction;
using NewsWebsite.Services.Services;
using persistence.Repositories;
using System;
using System.Text;

namespace NewsWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddIdentityCore<ApplicationUser>(config =>
            {
                config.Password.RequiredLength = 3;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireDigit = false;
            }).AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<NewsIdentityDbContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(NewsWebsite.Services.AssemblyReference).Assembly);
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            //builder.Services.AddAuto
            builder.Services.AddDbContext<NewsIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

            //Add JWT
            var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<JwtSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key)
                        ),
                        ValidateLifetime = true
                    };

                    // Add event handlers for debugging
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"[JWT] Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            Console.WriteLine($"[JWT] OnChallenge: {context.Error}, {context.ErrorDescription}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("[JWT] Token validated successfully");
                            return Task.CompletedTask;
                        }
                    };
                });


            //add IStringLocalizer
            builder.Services.AddSingleton<IStringLocalizer<JsonLocalizer>, JsonLocalizer>();

            //add the default connection to the Database 
            builder.Services.AddDbContext<NewsDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // Required for CoreAdmin's JsonLocalizer
            builder.Services.AddHttpContextAccessor();

            //AddCoreAdmin
            builder.Services.AddCoreAdmin();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();

            //must come with this order and after useRouting and berfore MapControllers
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            //Initializing Database Async
            await InitializeDatabaseAsync(app);

            //list my endpoints
            foreach (var endpoint in app.Services.GetRequiredService<EndpointDataSource>().Endpoints)
            {
                Console.WriteLine(endpoint.DisplayName);
            }

            app.Run();

            static async Task InitializeDatabaseAsync(WebApplication app)
            {
                using var scope = app.Services.CreateScope();
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await dbInitializer.InitailizeIdentityAsync();
                await dbInitializer.InitailizeAsync();
            }
        }
    }
}
