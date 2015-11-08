using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDifference.BusinessClasses
{
    public class RegistrationModel
    {
        public int UserId { set; get; }

        [Required]
        [StringLength(50, ErrorMessage = "Email is too long max supported length is 50")]
        public string Email { set; get; }

        [Required]
        [StringLength(50, ErrorMessage = "Password is too long max supported length is 50")]
        public string Password { set; get; }

        [Required]
        [StringLength(50, ErrorMessage = "UserName is too long max supported length is 50")]
        public string UserName { set; get; }

        public UserRole RoleId { set; get; }
    }
}
