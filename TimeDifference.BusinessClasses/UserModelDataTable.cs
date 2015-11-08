using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDifference.BusinessClasses
{
    public class UserModelDataTable
    {
        public int UserId { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
        public UserRole Role { set; get; }
        public int TimeZoneRecords { set; get; }
    }

}
