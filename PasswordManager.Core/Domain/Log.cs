using PasswordManager.Core.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Core.Domain
{
    public class Log : BaseEntity<Guid>
    {
        public string Level { get; set; } 
        public string Message { get; set; }
        public string? Exception { get; set; }
        public string? Properties { get; set; }
        public string? LogEvent { get; set; }
    }
}
