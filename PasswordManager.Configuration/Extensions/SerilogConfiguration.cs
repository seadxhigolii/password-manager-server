using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using srl = Serilog;

namespace PasswordManager.Configuration.Extensions
{
    public static class SerilogConfiguration
    {
        public static void AddSerilog(this WebApplicationBuilder builder)
        {
            var _columnOptions = Serilog.ColumnWriters.GetColumnWriters();

            srl.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.PostgreSQL(
                    connectionString: builder.Configuration.GetConnectionString("PasswordManagerDatabase"),
                    tableName: "Serilog",
                    needAutoCreateTable: true,
                    columnOptions: _columnOptions)
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
