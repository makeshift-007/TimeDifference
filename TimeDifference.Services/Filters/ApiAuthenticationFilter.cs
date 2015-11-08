using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using TimeDifference.Business;
using TimeDifference.BusinessClasses;


namespace TimeDifference.Services.Filters
{
    /// <summary>
    /// Custom Authentication Filter Extending basic Authentication
    /// </summary>
    public class ApiAuthenticationFilter : GenericAuthenticationFilter
    {
        /// <summary>
        /// Default Authentication Constructor
        /// </summary>
        public ApiAuthenticationFilter()
        {
        }

        /// <summary>
        /// AuthenticationFilter constructor with isActive parameter
        /// </summary>
        /// <param name="isActive"></param>
        public ApiAuthenticationFilter(bool isActive)
            : base(isActive)
        {
        }

        /// <summary>
        /// Protected overriden method for authorizing user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            //var provider = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(IUserServices)) as IUserServices;
            var provider = new UserServices();

            if (provider != null)
            {
                var userId = provider.Authenticate(username, password);
                if (userId > 0)
                {
                    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                    if (basicAuthenticationIdentity != null)
                    {
                        basicAuthenticationIdentity.UserId = userId;

                        //Below code to store the userInfo
                        var getUserInformation = new UserMethods().GetUserInformationBasedOnId(userId);
                        basicAuthenticationIdentity.AddClaims(new List<Claim>
                        {
                            new Claim("UserName",getUserInformation.UserName),
                            new Claim("Email",getUserInformation.Email),
                            new Claim("UserId",Convert.ToString(getUserInformation.UserId)),
                            new Claim("RoleId",Convert.ToString(Convert.ToInt32(getUserInformation.Role))),
                        });
                    }


                    return true;
                }
            }
            return false;
        }
    }
}