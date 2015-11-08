using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TimeDifference.Business;
using TimeDifference.BusinessClasses;
using TimeDifference.Services.Filters;


namespace TimeDifference.Services.ActionFilters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private const string Token = "Token";
        public UserRole AccessRole { get; set; }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            //  Get API key provider
            //var provider = filterContext.ControllerContext.Configuration
            //    .DependencyResolver.GetService(typeof(ITokenServices)) as ITokenServices;
            var provider = new TokenServices();

            if (filterContext.Request.Headers.Contains(Token))
            {
                var tokenValue = filterContext.Request.Headers.GetValues(Token).First();

                // Validate Token
                if (!provider.ValidateToken(tokenValue))
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        ReasonPhrase = "Invalid Request"
                    };
                    filterContext.Response = responseMessage;
                }
                else
                {
                    var userInformation = new Business.UserMethods().GetUserInformationBasedOnToken(tokenValue);

                    if (Convert.ToInt32(userInformation.Role) < Convert.ToInt32(AccessRole))
                    {
                        filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    }
                    else if (userInformation != null)
                    {
                        var identity = new BasicAuthenticationIdentity(userInformation.Email, "");
                        identity.AddClaims(new List<Claim>
                        {
                            new Claim("UserName",userInformation.UserName),
                            new Claim("Email",userInformation.Email),
                            new Claim("UserId",Convert.ToString(userInformation.UserId)),
                            new Claim("RoleId",Convert.ToString(Convert.ToInt32(userInformation.Role))),
                        });

                        Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
                    }
                }
            }
            else
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(filterContext);

        }
    }
}