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

            var _columnOptions = new Dictionary<string, ColumnWriterBase>
            {
                { "Timestamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                { "Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                { "Message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                { "Exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                { "Properties", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                { "LogEvent", new RenderedMessageColumnWriter(NpgsqlDbType.Text) }
            };

            Serilog.Log.Logger = new LoggerConfiguration()
            .WriteTo.PostgreSQL(
                connectionString: builder.Configuration.GetConnectionString("PasswordManagerDatabase"),
                tableName: "Serilog",
                needAutoCreateTable: true,
                columnOptions: _columnOptions)
            .CreateLogger();

            builder.Host.UseSerilog();

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

            #region JWT Configuration

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            #endregion JWT Configuration

            builder.Services.AddAuthorization();

            #region Cors

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            #endregion Cors

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
