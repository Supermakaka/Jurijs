using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataTables.AspNet.AspNet5.Extensions.Linq
{
    using Core;

    public class FilteringResult<T>
    {
        public int TotalRecords { get; set; }

        public int TotalRecordsFiltered { get; set; }

        public IQueryable<T> QueryFiltered { get; set; }
    }
}
