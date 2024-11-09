using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Core.Shared
{
    public class IncludeDelegateDomain
    {
        public delegate IQueryable<TEntity> IncludeDelegate<TEntity>(IQueryable<TEntity> query);
    }
}
