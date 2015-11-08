using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDifference.BusinessClasses;
using TimeDifference.Common;
using TimeDifference.Data;
using TimeDifference.Entity;
using UserRole = TimeDifference.BusinessClasses.UserRole;


namespace TimeDifference.Business
{
    public class UserMethods
    {

        /// <summary>
        /// Used to get user information based on user ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserModel GetUserInformationBasedOnId(int userId)
        {
            return new Data.UserMethods().GetUserInformationBasedOnId(userId);
        }

        /// <summary>
        /// Used to get user information based on token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserModel GetUserInformationBasedOnToken(string token)
        {
            try
            {
                var tokenInfo = new Data.TokenMethods().GetTokenInformation(token);
                if (tokenInfo == null)
                    return null;
                var userInfo = new Data.UserMethods().GetUserInformationBasedOnId(tokenInfo.UserId);
                if (userInfo == null)
                    return null;
                return new UserModel
                {
                    Email = userInfo.Email,
                    UserId = userInfo.UserId,
                    UserName = userInfo.UserName,
                    Role = userInfo.Role
                };
            }
            catch (Exception ex)
            { return null; }
        }

        /// <summary>
        /// Used to filter data from database for DataTable
        /// </summary>
        /// <param name="filterData"></param>
        /// <param name="loggedInUserRole"></param>
        /// <returns></returns>
        public DataTableFilteredData GetUsersDataTable(UserManagementDataTable filterData, UserRole loggedInUserRole)
        {
            var data = new Data.UserMethods().GetUsersDataTable(loggedInUserRole);

            //Performing filter action
            var userData = (from e in data
                            where (filterData.Search.Value == null ||
                                                 e.Email.Contains(filterData.Search.Value, StringComparison.OrdinalIgnoreCase) ||
                                                 e.UserName.Contains(filterData.Search.Value, StringComparison.OrdinalIgnoreCase)
                                )
                            select e);


            //Sorting
            var sortingData = filterData.Order.FirstOrDefault();


            if (sortingData != null)
                switch (sortingData.Column)
                {
                    case 0:
                        userData = sortingData.Dir == "asc" ? userData.OrderBy(m => m.UserName) : userData.OrderByDescending(m => m.UserName);
                        break;
                    case 1:
                        userData = sortingData.Dir == "asc" ? userData.OrderBy(m => m.Email) : userData.OrderByDescending(m => m.Email);
                        break;
                    case 2:
                        userData = sortingData.Dir == "asc" ? userData.OrderBy(m => m.TimeZoneRecords) : userData.OrderByDescending(m => m.TimeZoneRecords);
                        break;

                }

            var userModelDataTables = userData as UserModelDataTable[] ?? userData.ToArray();
            var userDataFiltering = userModelDataTables.Skip(filterData.Start).Take(filterData.Length);




            //Creating Table Object
            var records = userDataFiltering.Select(dataCurrent => new List<string>
            {
                dataCurrent.UserName, dataCurrent.Email, Convert.ToString(dataCurrent.TimeZoneRecords),Convert.ToString(dataCurrent.UserId)
            }).ToList();

            //

            return new DataTableFilteredData
            {
                draw = filterData.Draw,
                recordsFiltered = userModelDataTables.Count(),
                recordsTotal = data.Count,
                error = "",
                data = records
            };
        }

        /// <summary>
        /// Used to get all the users
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUsers(int pageNo, int records, UserRole role)
        {
            return new Data.UserMethods().GetAllUsers(pageNo, records, role);
        }

        /// <summary>
        /// Used to modify user Information
        /// </summary>
        /// <returns></returns>
        public bool ModifyUserInformation(UserModel userInformation)
        {
            if (!string.IsNullOrEmpty(userInformation.Password))
            {
                userInformation.Password = new EncryptionHelper().Encrypt(userInformation.Password);
            }
            userInformation.Email = userInformation.Email.ToLower();
            return new Data.UserMethods().ModifyUserInformation(userInformation);
        }

        /// <summary>
        /// Used to modify user Information also the role
        /// </summary>
        /// <returns></returns>
        public bool ModifyUserInformationAdmin(UserModel userInformation)
        {
            if (!string.IsNullOrEmpty(userInformation.Password))
            {
                userInformation.Password = new EncryptionHelper().Encrypt(userInformation.Password);
            }
            userInformation.Email = userInformation.Email.ToLower();
            return new Data.UserMethods().ModifyUserInformationAdmin(userInformation);
        }

        /// <summary>
        /// Used to register User
        /// </summary>
        /// <param name="registrationModel"></param>
        /// <returns></returns>
        public int RegisterUser(RegistrationModel registrationModel)
        {
            registrationModel.RoleId = UserRole.User;
            registrationModel.Email = registrationModel.Email.ToLower();
            registrationModel.Password = new EncryptionHelper().Encrypt(registrationModel.Password);

            return new Data.UserMethods().RegisterUser(registrationModel);
        }

        /// <summary>
        /// Used to remove user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool RemoveUser(int userId)
        {
            return new Data.UserMethods().RemoveUser(userId);
        }

        /// <summary>
        /// Used to check if the login exist for any user
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public UserModel CheckLoginInfo(LoginModel loginInfo)
        {
            loginInfo.Email = loginInfo.Email.ToLower();
            loginInfo.Password = new EncryptionHelper().Encrypt(loginInfo.Password);
            return new Data.UserMethods().CheckLoginInfo(loginInfo);
        }

        /// <summary>
        /// Used to check if the email Id already Exist
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public bool IsEmailExist(string emailId)
        {
            return new Data.UserMethods().IsEmailExist(emailId);
        }
    }
}
