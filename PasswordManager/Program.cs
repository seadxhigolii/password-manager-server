using PasswordManager.Persistence.Contexts;
using PasswordManager.Configuration.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Configuration.Extensions;
using Serilog.Sinks.PostgreSQL;
using NpgsqlTypes;

namespace PasswordManager
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.AddSerilog();
            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.RegisterIoCServices();
            //builder.Services.AddSwaggerGen(); 
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            builder.Services.AddDbContext<PasswordManagerDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PasswordManagerDatabase")));


            builder.AddJwtBearer();

            builder.Services.AddAuthorization();

            builder.AddCors();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors();


            app.Run();
        }
    }
}
