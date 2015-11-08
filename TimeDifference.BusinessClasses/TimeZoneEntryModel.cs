using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDifference.BusinessClasses
{
    public class TimeZoneEntryModel
    {
        [Required]
        public int TimeZoneEntryId { set; get; }
        [Required]
        public string EntryName { set; get; }
        [Required]
        public string City { set; get; }
        [Required]
        public int UserId { set; get; }
        [Required]
        public int HourDifference { set; get; }
        [Required]
        public int MinuteDifference { set; get; }

        public DateTime CurrentTimeCity { set; get; }

    }
}
