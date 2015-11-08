using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDifference.BusinessClasses
{
    public class TimeZoneEntryDataModel
    {

        public int TimeZoneEntryId { set; get; }

        public string EntryName { set; get; }

        public string City { set; get; }

        public int UserId { set; get; }

        public long Difference { set; get; }

    }
}
