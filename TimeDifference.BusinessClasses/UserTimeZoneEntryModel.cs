using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDifference.BusinessClasses
{
    public class UserTimeZoneEntryModel
    {
        public int UserId { set; get; }
        public string UserName { set; get; }
        public string EmailId { set; get; }
        public List<TimeZoneEntryModel> TimeZoneEntries { set; get; }
    }
}
