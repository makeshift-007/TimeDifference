using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using TimeDifference.BusinessClasses;
using TimeDifference.Entity;
using TimeDifference.Services.Filters;
using UserRole = TimeDifference.BusinessClasses.UserRole;

namespace TimeDifference.Services.UserManager
{
    public class UserInformation
    {

        private UserModel _userInfo;
        private static UserModel _testUserData;
        public UserInformation()
        {
            try
            {
                var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;

                if (basicAuthenticationIdentity == null)
                    _userInfo= null;

                var claimsInformation = basicAuthenticationIdentity.Claims;
                var information = claimsInformation as Claim[] ?? claimsInformation.ToArray();
                _userInfo= new UserModel
                {
                    UserId = Convert.ToInt32(information.FirstOrDefault(m => m.Type == "UserId").Value),
                    Email = information.FirstOrDefault(m => m.Type == "Email").Value,
                    UserName = information.FirstOrDefault(m => m.Type == "UserName").Value,
                    Role = (UserRole)Convert.ToInt32(information.FirstOrDefault(m => m.Type == "RoleId").Value),

                };
            }
            catch (Exception ex)
            {
                _userInfo= null;
            }
        }

        public UserInformation(UserModel userInfo)
        {
            _testUserData = userInfo;
        }

        /// <summary>
        /// Used to get current user Information
        /// </summary>
        /// <returns></returns>
        public UserModel GetCurrentUserInformation()
        {
            return _testUserData??_userInfo;
        }
    }
}