using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace TimeDifference.BusinessClasses
{
    public class LoginModel
    {
        [Required]
        public string Email { set; get; }
        [Required]
        public string Password { set; get; }
    }
}
