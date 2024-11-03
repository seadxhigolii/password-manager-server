using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Core.Domain.Shared
{
    public class BaseEntity<TId>
    {
        #region Properties
        public TId Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
        public string? ChangedBy { get; set; }

        #endregion Properties
    }
}
