using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using TimeDifference.Business;
using TimeDifference.BusinessClasses;
using TimeDifference.Services.ActionFilters;
using TimeDifference.Services.UserManager;

namespace TimeDifference.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AuthorizationRequired(AccessRole = UserRole.Manager)]
    public class UserManagementController : ApiController
    {
        #region ManagerMethods
        /// <summary>
        /// Used to get user information based on userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public UserModel GetUserInformationById(int userId)
        {
            try
            {

                var userData = new Business.UserMethods().GetUserInformationBasedOnId(userId);
                if (userData == null)
                    throw new EntryNotFound();

                return userData;
            }
            catch (EntryNotFound ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No Entry with ID = {0}", userId)),
                    ReasonPhrase = "Entry Not Found"
                });
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(string.Format("Service is currently unavailable.")),
                    ReasonPhrase = "Service Unavailable "
                });
            }
        }

        /// <summary>
        /// Used to save/edit the user information
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool ModifyUserInformation(UserModel userInfo)
        {
            try
            {
                var loggedUserInfo = new UserInformation().GetCurrentUserInformation();
                var userInformation = new UserMethods().GetUserInformationBasedOnId(userInfo.UserId);
               
                if ((new Business.UserMethods().IsEmailExist(userInfo.Email) && userInformation.Email != userInfo.Email) )
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                //Checking Role Privilege
                if(Convert.ToInt32(userInformation.Role) > Convert.ToInt32(loggedUserInfo.Role))
                    throw new UnAuthorize();

                return new Business.UserMethods().ModifyUserInformation(userInfo);
            }
            catch (UnAuthorize ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(string.Format("You are not authorize.")),
                    ReasonPhrase = "UnAuthorize"
                });
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("Email ID already exist")),
                    ReasonPhrase = "Bad Request"
                });
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(string.Format("Service is currently unavailable.")),
                    ReasonPhrase = "Service Unavailable "
                });
            }
        }

       




        /// <summary>
        /// Used to get all the records of users
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        [HttpPost]
        public List<UserModel> GetAllUsers(int pageNo, int records)
        {
            try
            {
                var loggedUserInfo = new UserInformation().GetCurrentUserInformation();
                return new Business.UserMethods().GetAllUsers(pageNo, records, loggedUserInfo.Role);
            }   
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(string.Format("Service is currently unavailable.")),
                    ReasonPhrase = "Service Unavailable "
                });
            }
        }

        /// <summary>
        /// Used to get details of users for DataTable
        /// </summary>
        /// <param name="filterData"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTableFilteredData GetUsersDataTable(UserManagementDataTable filterData)
        {
            try
            {
                var loggedUserInfo = new UserInformation().GetCurrentUserInformation();
                return new Business.UserMethods().GetUsersDataTable(filterData, loggedUserInfo.Role);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(string.Format("Service is currently unavailable.")),
                    ReasonPhrase = "Service Unavailable "
                });
            }
        }

        /// <summary>
        /// Used to remove user from application
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public bool RemoveUser(int userId)
        {
            try
            {
                var loggedUserInfo = new UserInformation().GetCurrentUserInformation(); 
                var userInformation = new UserMethods().GetUserInformationBasedOnId(userId);

                if (Convert.ToInt32(userInformation.Role) > Convert.ToInt32(loggedUserInfo.Role))
                    throw new UnAuthorize();
                return new UserMethods().RemoveUser(userId);
            }
            catch (UnAuthorize ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(string.Format("You are not authorize.")),
                    ReasonPhrase = "UnAuthorize"
                });
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(string.Format("Service is currently unavailable.")),
                    ReasonPhrase = "Service Unavailable "
                });
            }

        }

        #endregion
        #region Admin Methods
        /// <summary>
        /// Used by Admin to change the user Information
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationRequired(AccessRole = UserRole.Admin)]
        public bool ModifyUserInformationAdmin(UserModel userInfo)
        {
            try
            {
                var loggedUserInfo = new UserInformation().GetCurrentUserInformation();
                var userInformation = new UserMethods().GetUserInformationBasedOnId(userInfo.UserId);
                //Checking Role Privilege
                if ((new Business.UserMethods().IsEmailExist(userInfo.Email) && userInformation.Email != userInfo.Email))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                return new Business.UserMethods().ModifyUserInformationAdmin(userInfo);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("Email ID already exist")),
                    ReasonPhrase = "Bad Request"
                });
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(string.Format("Service is currently unavailable.")),
                    ReasonPhrase = "Service Unavailable "
                });
            }
        }
          #endregion
    }
}