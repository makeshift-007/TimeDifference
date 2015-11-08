using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDifference.BusinessClasses;
using TimeDifference.Entity;

namespace TimeDifference.Data
{
    public class UserMethods
    {


        /// <summary>
        /// Used to filter data from database for DataTable
        /// </summary>
        /// <returns></returns>
        public List<UserModelDataTable> GetUsersDataTable(BusinessClasses.UserRole role)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    var roleId = Convert.ToInt32(role);
                    return tde.Users.Where(m => m.IsActive && m.RoleId <= roleId).Select(m => new UserModelDataTable
                      {
                          Email = m.Email,
                          Role = (BusinessClasses.UserRole)m.RoleId,
                          UserId = m.Id,
                          UserName = m.UserName,
                          TimeZoneRecords = m.TimeZoneEntries.Count(c => c.IsActive)
                      }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }
        }

        /// <summary>
        /// Used to get user information based on user ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserModel GetUserInformationBasedOnId(int userId)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    var userData =
                        tde.Users.FirstOrDefault(m => m.IsActive && m.Id == userId);

                    if (userData == null)
                        return null;

                    return new UserModel
                    {
                        Email = userData.Email,
                        Role = (BusinessClasses.UserRole)userData.RoleId,
                        UserId = userData.Id,
                        UserName = userData.UserName
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }
        }

        /// <summary>
        /// Used to check if the email Id already Exist
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public bool IsEmailExist(string emailId)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    var userData =
                        tde.Users.FirstOrDefault(m => m.IsActive && m.Email == emailId);

                    return userData != null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }
        }

        /// <summary>
        /// Used to get all the users based on pageNo and Records
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUsers(int pageNo, int records,BusinessClasses.UserRole role)
        {
            try
            {

                using (var tde = new TimeDifferenceEntities())
                {
                    var roleId = Convert.ToInt32(role);
                    return tde.Users.Where(m => m.IsActive && m.RoleId <= roleId).OrderBy(m => m.Id)
                        .Skip((pageNo * records) - records).Take(records).Select(
                        m => new UserModel
                        {
                            Email = m.Email,
                            UserId = m.Id,
                            UserName = m.UserName,
                            Role = (BusinessClasses.UserRole)m.RoleId
                        }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }
        }

        /// <summary>
        /// Used to register User
        /// </summary>
        /// <param name="registrationModel"></param>
        /// <returns></returns>
        public int RegisterUser(RegistrationModel registrationModel)
        {
            try
            {

                using (var tde = new TimeDifferenceEntities())
                {
                    var userData = new User
                    {
                        Email = registrationModel.Email,
                        UserName = registrationModel.UserName,
                        Password = registrationModel.Password,
                        RoleId = Convert.ToInt32(registrationModel.RoleId),
                        IsActive = true

                    };
                    tde.Users.Add(userData);
                    tde.SaveChanges();
                    return userData.Id;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }
        }

        /// <summary>
        /// Used to modify user Information
        /// </summary>
        /// <returns></returns>
        public bool ModifyUserInformation(UserModel userInformation)
        {
            try
            {

                using (var tde = new TimeDifferenceEntities())
                {

                    var userInfo = tde.Users.FirstOrDefault(m => m.IsActive && m.Id == userInformation.UserId);
                    if (userInfo == null)
                        return false;
                    userInfo.Email = userInformation.Email;
                    userInfo.UserName = userInformation.UserName;
                    if (!string.IsNullOrEmpty(userInformation.Password))
                    {
                        userInfo.Password = userInformation.Password;
                    }
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
        /// Used to modify user Information
        /// </summary>
        /// <returns></returns>
        public bool ModifyUserInformationAdmin(UserModel userInformation)
        {
            try
            {

                using (var tde = new TimeDifferenceEntities())
                {

                    var userInfo = tde.Users.FirstOrDefault(m => m.IsActive && m.Id == userInformation.UserId);
                    if (userInfo == null)
                        return false;
                    userInfo.Email = userInformation.Email;
                    userInfo.UserName = userInformation.UserName;
                    userInfo.RoleId = Convert.ToInt32(userInformation.Role);
                    if (!string.IsNullOrEmpty(userInformation.Password))
                    {
                        userInfo.Password = userInformation.Password;
                    }
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
        /// Used to check if the login exist for any user
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public UserModel CheckLoginInfo(LoginModel loginInfo)
        {
            try
            {

                using (var tde = new TimeDifferenceEntities())
                {
                    var userData =
                        tde.Users.FirstOrDefault(
                            m => m.IsActive && m.Email == loginInfo.Email && m.Password == loginInfo.Password);
                    if (userData == null)
                        return null;
                    return new UserModel
                    {
                        Email = userData.Email,
                        Role = (BusinessClasses.UserRole)userData.RoleId,
                        UserId = userData.Id,
                        UserName = userData.UserName
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }

        }

        /// <summary>
        /// Used to remove user from the database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool RemoveUser(int userId)
        {
            try
            {

                using (var tde = new TimeDifferenceEntities())
                {

                    var userInfo = tde.Users.FirstOrDefault(m => m.IsActive && m.Id == userId);
                    if (userInfo == null)
                        return false;
                    userInfo.IsActive = false;
                    tde.SaveChanges();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured", ex.InnerException);
            }
        }

    }
}
