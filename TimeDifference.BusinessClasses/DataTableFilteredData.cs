using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDifference.BusinessClasses
{
    public class DataTableFilteredData
    {
        public int draw { set; get; }
        public int recordsTotal { set; get; }
        public int recordsFiltered { set; get; }
        public string error { set; get; }
        public List<List<string>> data { get; set; }

    }
}
