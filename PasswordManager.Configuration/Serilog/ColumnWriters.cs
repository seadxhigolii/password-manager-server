using NpgsqlTypes;
using Serilog.Sinks.PostgreSQL;
namespace PasswordManager.Configuration.Serilog
{
    public class ColumnWriters
    {
        public Dictionary<string, ColumnWriterBase> GetColumnWriters()
        {
            var columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                { "message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                { "message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                { "level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                { "timestamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                { "exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                { "properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                { "machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text) }
            };

            return columnWriters;
        }
    }
}
