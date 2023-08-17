using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortsApi.Models;
using PortsApi.Services;
using System;

namespace PortsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PortsOrigin", builder => builder
                    .WithOrigins("https://localhost:3001")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition")
                    .WithExposedHeaders("WWW-Authenticate"));
            });

            builder.Services.AddControllers();

            builder.Services.AddDbContext<TestContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TESTConnectionString")));
            builder.Services.AddDbContext<GeoContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("TESTConnectionString"));
            });
            builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<FilesLogic>();
            builder.Services.AddScoped<FoldersLogic>();
            builder.Services.AddScoped<UsersLogic>();
            builder.Services.AddScoped<PermissionsLogic>();
            builder.Services.AddScoped<GeoContext>();
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy.
                options.FallbackPolicy = options.DefaultPolicy;
            });

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("PortsOrigin");

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

