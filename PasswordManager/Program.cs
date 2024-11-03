using PasswordManager.Persistence.Contexts;
using PasswordManager.Configuration.Middlewares;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using PasswordManager.Core.Domain;
using Serilog;

using SeriLog = Serilog.Log;

namespace PasswordManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            SeriLog.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<PasswordManagerDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PasswordManagerDatabase")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
