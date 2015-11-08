using System;
using System.Configuration;
using System.Linq;
//using BusinessEntities;
//using DataModel;
//using DataModel.UnitOfWork;
using TimeDifference.BusinessClasses;
using TimeDifference.Data;
using TimeDifference.Entity;

namespace TimeDifference.Business
{
    public class TokenServices : ITokenServices
    {
        #region Private member variables.

        private TokenMethods _tokenMethod;
        #endregion

        #region Public constructor.
        /// <summary>
        /// Public constructor.
        /// </summary>
        public TokenServices()
        {
            _tokenMethod = new TokenMethods();
        }
        #endregion

        #region Public member methods.

        /// <summary>
        ///  Function to generate unique token with expiry against the provided userId.
        ///  Also add a record in database for generated token.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Token GenerateToken(int userId)
        {
            string token = Guid.NewGuid().ToString();
            DateTime issuedOn = DateTime.Now;
            DateTime expiredOn = DateTime.Now.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
            var tokendomain = new Token
                                  {
                                      UserId = userId,
                                      AuthToken = token,
                                      IssuedOn = issuedOn,
                                      ExpiresOn = expiredOn
                                  };

            _tokenMethod.InsertToken(tokendomain);
            var tokenModel = new Token()
                                 {
                                     UserId = userId,
                                     IssuedOn = issuedOn,
                                     ExpiresOn = expiredOn,
                                     AuthToken = token
                                 };

            return tokenModel;
        }

        /// <summary>
        /// Method to validate token against expiry and existence in database.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public bool ValidateToken(string tokenId)
        {
            var token = _tokenMethod.GetTokenInformation(tokenId);
            if (token != null && !(DateTime.Now > token.ExpiresOn))
            {
                token.ExpiresOn = token.ExpiresOn.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
                _tokenMethod.UpdateToken(token);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to kill the provided token id.
        /// </summary>
        /// <param name="tokenId">true for successful delete</param>
        public bool Kill(string tokenId)
        {
            _tokenMethod.DeleteToken(tokenId);

            return _tokenMethod.GetTokenInformation(tokenId) == null;
            //var isNotDeleted = 
            //if (isNotDeleted) { return false; }
            //return true;
        }

        /// <summary>
        /// Delete tokens for the specific deleted user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>true for successful delete</returns>
        public bool DeleteByUserId(int userId)
        {
            _tokenMethod.DeleteTokensByUserId(userId);
            return _tokenMethod.GetTokensBasedOnUserId(userId) == null;
        }

        #endregion
    }
}
