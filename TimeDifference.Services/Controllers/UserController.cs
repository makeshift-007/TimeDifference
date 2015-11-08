using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;
using TimeDifference.Business;
using TimeDifference.BusinessClasses;
using TimeDifference.Entity;
using TimeDifference.Services.ActionFilters;
using TimeDifference.Services.Filters;
using TimeDifference.Services.UserManager;
using UserRole = TimeDifference.BusinessClasses.UserRole;

namespace TimeDifference.Services.Controllers
{

    /// <summary>
    /// Used to performing various user related operation
    /// </summary>

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        #region User Methods
        /// <summary>
        /// Used to check if the login information is valid
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationRequired(AccessRole = UserRole.User)]
        public UserModel GetUserInformation()
        {
            try
            {
                var userInfo = new UserInformation().GetCurrentUserInformation();
                var userData = new Business.UserMethods().GetUserInformationBasedOnId(userInfo.UserId);
                if (userData == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                return userData;
            }
            catch (EntryNotFound ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Entry not Found")),
                    ReasonPhrase = "Entry Not Found"
                });

            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.ServiceUnavailable);
            }
        }


        /// <summary>
        /// Used to check if the Email Id already Exist
        ///  </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        [HttpPost]
        public bool IsEmailExist(string emailId)
        {
            try { return new Business.UserMethods().IsEmailExist(emailId); }
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
        /// Used to save a timeZone Entry
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationRequired(AccessRole = UserRole.User)]
        public int SaveTimeZone(TimeZoneEntryModel entry)
        {
            try
            {
                if (Math.Abs(entry.HourDifference) > 14 || Math.Abs(entry.MinuteDifference) > 60)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);


                TimeZoneEntryModel entryData = null;

                if (entry.TimeZoneEntryId != 0)
                    entryData = new TimeZoneEntryMethods().GetTimeZoneInformation(entry.TimeZoneEntryId);

                if (entry.TimeZoneEntryId != 0 && entryData == null)
                    throw new EntryNotFound();


                entry.UserId = new UserInformation().GetCurrentUserInformation().UserId;

                if (entry.TimeZoneEntryId != 0 && entryData.UserId != entry.UserId)
                    throw new UnAuthorize("User Not Authorize");

                return new Business.TimeZoneEntryMethods().SaveTimeZone(entry);
            }
            catch (EntryNotFound ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No Entry with ID = {0}", entry.TimeZoneEntryId)),
                    ReasonPhrase = "Entry Not Found"
                });

            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("HourDifference or Minute Difference not valid.")),
                    ReasonPhrase = "Bad Request"
                });
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

        /// <summary>
        /// Used to delete timezone entry
        /// </summary>
        /// <param name="timeZoneEntryId"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationRequired(AccessRole = UserRole.User)]
        public bool DeleteTimeZoneEntry(int timeZoneEntryId)
        {
            try
            {
                var userInfo = new UserInformation().GetCurrentUserInformation();

                var entryData = new TimeZoneEntryMethods().GetTimeZoneInformation(timeZoneEntryId);
                if (entryData == null)
                    throw new EntryNotFound();
                if (entryData.UserId != userInfo.UserId)
                    throw new UnAuthorize("User Not Authorize");

                return new Business.TimeZoneEntryMethods().DeleteTimeZoneEntry(timeZoneEntryId);
            }
            catch (EntryNotFound ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No Entry with ID = {0}", timeZoneEntryId)),
                    ReasonPhrase = "Entry Not Found"
                });

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

        /// <summary>
        /// Used to delete timezone entry
        /// </summary>
        /// <param name="timeZoneEntryId"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationRequired(AccessRole = UserRole.User)]
        public TimeZoneEntryModel GetTimeZoneInformation(int timeZoneEntryId)
        {
            try
            {
                var userInfo = new UserInformation().GetCurrentUserInformation();
                var entryData = new Business.TimeZoneEntryMethods().GetTimeZoneInformation(timeZoneEntryId);
                if (entryData.UserId != userInfo.UserId)
                    throw new UnAuthorize("User Not Authorize");
                return entryData;

            }
            catch (EntryNotFound ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No Entry with ID = {0}", timeZoneEntryId)),
                    ReasonPhrase = "Entry Not Found"
                });
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

        /// <summary>
        /// Used to get all the time zone record created by User
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationRequired(AccessRole = UserRole.User)]
        public List<TimeZoneEntryModel> GetAllTimeZone(int pageNo, int records)
        {
            try
            {

                var userInfo = new UserInformation().GetCurrentUserInformation();
                return new Business.TimeZoneEntryMethods().GetAllTimeZone(pageNo, records, userInfo.UserId);
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
        /// Used to modify user information
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationRequired(AccessRole = UserRole.User)]
        public bool ModifyUserInformation(UserModel userInfo)
        {
            try
            {
                var userInfoSession = new UserInformation().GetCurrentUserInformation();
                if (IsEmailExist(userInfo.Email) && userInfoSession.Email != userInfo.Email)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);


                userInfo.UserId = userInfoSession.UserId;
                return new Business.UserMethods().ModifyUserInformation(userInfo);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("Email ID already exist.")),
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
        /// Used to register User
        /// </summary>
        /// <param name="registrationModel"></param>
        /// <returns></returns>
        [HttpPost]
        public int RegisterUser(RegistrationModel registrationModel)
        {
            try
            {
                if (IsEmailExist(registrationModel.Email))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                return new Business.UserMethods().RegisterUser(registrationModel);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("Email ID already exist.")),
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
