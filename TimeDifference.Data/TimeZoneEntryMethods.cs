using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDifference.BusinessClasses;
using TimeDifference.Entity;

namespace TimeDifference.Data
{
    public class TimeZoneEntryMethods
    {

   



        /// <summary>
        /// Used to delete timeZone entry
        /// </summary>
        /// <param name="timeZoneEntryId"></param>
        /// <returns></returns>
        public bool DeleteTimeZoneEntry(int timeZoneEntryId)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    var entry = tde.TimeZoneEntries.FirstOrDefault(m => m.Id == timeZoneEntryId);
                    if (entry == null)
                        return false;

                    entry.IsActive = false;
                    tde.SaveChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }
        }

        /// <summary>
        /// Used to get timeZone entry
        /// </summary>
        /// <param name="timeZoneEntryId"></param>
        /// <returns></returns>
        public TimeZoneEntry GetTimeZoneEntry(int timeZoneEntryId)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    var entry = tde.TimeZoneEntries.FirstOrDefault(m => m.Id == timeZoneEntryId && m.IsActive);
                    return entry;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }
        }

        /// <summary>
        /// Used to save a timeZone Entry
        /// </summary>
        /// <returns></returns>
        public int SaveNewTimeZoneEntry(TimeZoneEntry entry)
        {

            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    tde.TimeZoneEntries.Add(entry);
                    tde.SaveChanges();
                    return entry.Id;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }
        }

        /// <summary>
        /// Used to get all the timezone entry made by user
        /// </summary>
        /// <returns></returns>
        public List<TimeZoneEntry> GetAllTimeZoneBasedOnId(int pageNo, int records, int userId)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {

                    return tde.TimeZoneEntries.Where(m => m.IsActive && m.UserId == userId).OrderBy(m => m.Id)
                        .Skip((pageNo * records) - records).Take(records).ToList();

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }
        }


        /// <summary>
        /// Used to edit a timeZone Entry
        /// </summary>
        /// <returns></returns>
        public int ModifyTimeZoneEntry(TimeZoneEntry entry)
        {

            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    var timeZoneEntry =
                        tde.TimeZoneEntries.FirstOrDefault(m => m.IsActive && m.Id == entry.Id);

                    if (timeZoneEntry == null)
                        return -1;
                    timeZoneEntry.City = entry.City;
                    timeZoneEntry.Difference = entry.Difference;
                    timeZoneEntry.EntryName = entry.EntryName;
                    tde.SaveChanges();
                    return timeZoneEntry.Id;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }
        }




        ///// <summary>
        ///// Used to check if the current user is authorize for editing
        ///// </summary>
        ///// <returns></returns>
        //public bool UserAuthorizeForTimeZone(int entryId, int userId)
        //{
        //    try
        //    {
        //        using (var tde = new TimeDifferenceEntities())
        //        {
        //            return tde.TimeZoneEntries.FirstOrDefault(m => m.IsActive && m.Id == entryId && m.UserId == userId) != null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Exception Occured", ex.InnerException);
        //    }
        //}
    }
}
