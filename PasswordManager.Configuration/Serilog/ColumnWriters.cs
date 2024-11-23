using NpgsqlTypes;
using Serilog.Sinks.PostgreSQL;
namespace PasswordManager.Configuration.Serilog
{
    public static class ColumnWriters
    {
        public static Dictionary<string, ColumnWriterBase> GetColumnWriters()
        {
            var columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                { "Timestamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                { "Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                { "Message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                { "Exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                { "Properties", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                { "LogEvent", new RenderedMessageColumnWriter(NpgsqlDbType.Text) }
            };

            return columnWriters;
        }
    }
}
