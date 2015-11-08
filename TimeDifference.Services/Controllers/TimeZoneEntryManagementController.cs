using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TimeDifference.Business;
using TimeDifference.BusinessClasses;
using TimeDifference.Services.ActionFilters;
using TimeDifference.Services.UserManager;

namespace TimeDifference.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AuthorizationRequired(AccessRole = UserRole.Admin)]
    public class TimeZoneEntryManagementController : ApiController
    {
        /// <summary>
        /// Used to get all the Records of the user, If userId==0 then al records to be returned
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageNo"></param>
        /// <param name="noOfUserRecords"></param>
        /// <param name="timeZoneRecordPageNo"></param>
        /// <param name="timeZoneRecordToGet"></param>
        /// <returns></returns>
        [HttpPost]
        public List<UserTimeZoneEntryModel> GetUserTimeZoneInformation(int userId, int pageNo, int noOfUserRecords, int timeZoneRecordPageNo, int timeZoneRecordToGet)
        {
            try
            {
                var userInfo = new UserInformation().GetCurrentUserInformation();
                var data = new Business.TimeZoneEntryMethods().GetUserTimeZoneInformation(userId, pageNo, noOfUserRecords, timeZoneRecordPageNo, timeZoneRecordToGet, userInfo.Role);
                if (data == null)
                    throw new EntryNotFound();
                return data;
            }
            catch (EntryNotFound ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No Entry Found.")),
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
        /// Used to get the Time Zone entry Information
        /// </summary>
        /// <param name="timeZoneEntryId"></param>
        /// <returns></returns>
        [HttpPost]
        public TimeZoneEntryModel GetTimeZoneInformation(int timeZoneEntryId)
        {
            try
            {
                var entryData = new Business.TimeZoneEntryMethods().GetTimeZoneInformation(timeZoneEntryId);
                if (entryData == null)
                    throw new EntryNotFound();
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
                    Content = new StringContent(string.Format("Hour Difference or Minute Difference not valid.")),
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
        /// Used to delete timezone entry
        /// </summary>
        /// <param name="timeZoneEntryId"></param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteTimeZoneEntry(int timeZoneEntryId)
        {
            try
            {
                var entryData = new TimeZoneEntryMethods().GetTimeZoneInformation(timeZoneEntryId);
                if (entryData == null)
                    throw new EntryNotFound();

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
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(string.Format("Service is currently unavailable.")),
                    ReasonPhrase = "Service Unavailable "
                });
            }
        }

    }
}
