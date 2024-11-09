using PasswordManager.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Core.Shared
{
    public class PaginationFilterRequest
    {
        #region Properties

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; } = "";
        public List<int?> Filter { get; set; } = new List<int?>();
        public SortByValueEnum OrderBy { get; set; } = SortByValueEnum.None;

        #endregion Properties

        #region Ctor

        public PaginationFilterRequest()
        {
            this.PageNumber = 1;
            this.PageSize = 12;
        }

        public PaginationFilterRequest(int pageNumber, int pageSize, string search)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 1000 ? 1000 : pageSize;
            Search = search;
        }

        public PaginationFilterRequest(int pageNumber, int pageSize, string search, SortByValueEnum orderBy, List<int?> filter = null)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 1000 ? 1000 : pageSize;
            Search = search;
            Filter = filter;
            OrderBy = orderBy;
        }

        #endregion Ctor
    }
}
