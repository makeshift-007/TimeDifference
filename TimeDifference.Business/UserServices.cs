//using DataModel.UnitOfWork;

using TimeDifference.BusinessClasses;
using TimeDifference.Common;
using TimeDifference.Entity;

namespace TimeDifference.Business
{
    /// <summary>
    /// Offers services for user specific operations
    /// </summary>
    public class UserServices : IUserServices
    {
        private readonly Data.UserMethods _userMethod;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public UserServices()
        {
            _userMethod = new Data.UserMethods();
        }

        /// <summary>
        /// Public method to authenticate user by user name and password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int Authenticate(string userName, string password)
        {
            password = new EncryptionHelper().Encrypt(password);
            var user = _userMethod.CheckLoginInfo(new LoginModel{Email = userName,Password = password});
            if (user != null && user.UserId > 0)
            {
                return user.UserId;
            }
            return 0;
        }
    }
}
