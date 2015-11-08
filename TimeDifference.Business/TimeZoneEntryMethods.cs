using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDifference.BusinessClasses;
using TimeDifference.Entity;

namespace TimeDifference.Business
{
    public class TimeZoneEntryMethods
    {
        /// <summary>
        /// Used to get all the timezone information entered by user
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="records"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TimeZoneEntryModel> GetAllTimeZone(int pageNo, int records, int userId)
        {
            return new Data.TimeZoneEntryMethods().GetAllTimeZoneBasedOnId(pageNo, records, userId).Select(m => new TimeZoneEntryModel
            {
                UserId = m.UserId,
                EntryName = m.EntryName,
                City = m.City,
                TimeZoneEntryId = m.Id,
                HourDifference = new TimeSpan(m.Difference).Hours,
                MinuteDifference = new TimeSpan(m.Difference).Minutes,
                CurrentTimeCity = DateTime.UtcNow + new TimeSpan(m.Difference)
            }).ToList();
        }

        /// <summary>
        /// Used to get all the Records of the user, If userId==0 then al records to be returned
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageNo"></param>
        /// <param name="records"></param>
        /// <param name="recordPageNo"></param>
        /// <returns></returns>
        public List<UserTimeZoneEntryModel> GetUserTimeZoneInformation(int userId, int pageNo, int records, int recordPageNo, int recordPageEntries, BusinessClasses.UserRole role)
        {
            var returnData = new List<UserTimeZoneEntryModel>();
            if (userId != 0)
            {
                var userInfo = new Data.UserMethods().GetUserInformationBasedOnId(userId);
                if (userInfo == null)
                    throw new EntryNotFound();
                var userRecords = new Data.TimeZoneEntryMethods().GetAllTimeZoneBasedOnId(recordPageNo, recordPageEntries, userId);

                returnData.Add(new UserTimeZoneEntryModel
                {
                    UserId = userInfo.UserId,
                    UserName = userInfo.UserName,
                    EmailId = userInfo.Email,
                    TimeZoneEntries = userRecords.Select(m => new TimeZoneEntryModel
                    {
                        UserId = m.UserId,
                        EntryName = m.EntryName,
                        City = m.City,
                        TimeZoneEntryId = m.Id,
                        HourDifference = new TimeSpan(m.Difference).Hours,
                        MinuteDifference = new TimeSpan(m.Difference).Minutes,
                        CurrentTimeCity = DateTime.UtcNow + new TimeSpan(m.Difference)
                    }).ToList()
                });

            }
            else
            {
                returnData = new Data.UserMethods().GetAllUsers(pageNo, records, role).Select(c => new UserTimeZoneEntryModel
                    {
                        EmailId = c.Email,
                        UserId = c.UserId,
                        UserName = c.UserName,
                        TimeZoneEntries = new Data.TimeZoneEntryMethods().GetAllTimeZoneBasedOnId(recordPageNo, recordPageEntries, c.UserId).Select(
                        m => new TimeZoneEntryModel
                        {
                            UserId = m.UserId,
                            EntryName = m.EntryName,
                            City = m.City,
                            TimeZoneEntryId = m.Id,
                            HourDifference = new TimeSpan(m.Difference).Hours,
                            MinuteDifference = new TimeSpan(m.Difference).Minutes,
                            CurrentTimeCity = DateTime.UtcNow + new TimeSpan(m.Difference)
                        }).ToList()
                    }).ToList();
            }
            return returnData;

        }

        /// <summary>
        /// Used to delete Time zone entry
        /// </summary>
        /// <param name="timeZoneEntryId"></param>
        /// <returns></returns>
        public bool DeleteTimeZoneEntry(int timeZoneEntryId)
        {
            return new Data.TimeZoneEntryMethods().DeleteTimeZoneEntry(timeZoneEntryId);
        }

        /// <summary>
        /// Used to save a timeZone Entry
        /// </summary>
        /// <returns></returns>
        public int SaveTimeZone(TimeZoneEntryModel entry)
        {

            var timeDifferenceSpan = new TimeSpan(entry.HourDifference, entry.MinuteDifference, 0).Ticks;

            if (entry.TimeZoneEntryId != 0)
                return new Data.TimeZoneEntryMethods().ModifyTimeZoneEntry(new TimeZoneEntry
                {
                    City = entry.City,
                    EntryName = entry.EntryName,
                    UserId = entry.UserId,
                    Id = entry.TimeZoneEntryId,
                    Difference = timeDifferenceSpan
                });

            return new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry
            {
                City = entry.City,
                EntryName = entry.EntryName,
                UserId = entry.UserId,
                Difference = timeDifferenceSpan,
                IsActive = true
            });
        }

        /// <summary>
        /// Used to get timezone inforamtion
        /// </summary>
        /// <param name="timeZoneEntryId"></param>
        public TimeZoneEntryModel GetTimeZoneInformation(int timeZoneEntryId)
        {
            var entryData = new Data.TimeZoneEntryMethods().GetTimeZoneEntry(timeZoneEntryId);

            if (entryData == null)
                throw new EntryNotFound();

            var timeSpan = new TimeSpan(entryData.Difference);
            return new TimeZoneEntryModel
            {
                City = entryData.City,
                UserId = entryData.UserId,
                EntryName = entryData.EntryName,
                TimeZoneEntryId = entryData.Id,
                HourDifference = timeSpan.Hours,
                MinuteDifference = timeSpan.Minutes,
                CurrentTimeCity = DateTime.UtcNow + timeSpan
            };
        }

    }
}
