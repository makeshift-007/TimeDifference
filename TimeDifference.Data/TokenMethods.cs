using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDifference.Entity;

namespace TimeDifference.Data
{
    public class TokenMethods
    {
        /// <summary>
        /// Used to add token in the database
        /// </summary>
        /// <returns></returns>
        public bool InsertToken(Token token)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    tde.Tokens.Add(new Token
                    {
                        AuthToken = token.AuthToken,
                        ExpiresOn = token.ExpiresOn,
                        IssuedOn = token.IssuedOn,
                        UserId = token.UserId,
                    });
                    tde.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Used to delete Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool DeleteToken(string token)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    var tokenInfo = tde.Tokens.FirstOrDefault(m => m.AuthToken == token);
                    if (tokenInfo == null)
                        return false;
                    tde.Tokens.Remove(tokenInfo);
                    tde.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Used to update the token information
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool UpdateToken(Token token)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    var tokenInfo = tde.Tokens.FirstOrDefault(m => m.AuthToken == token.AuthToken);
                    if (tokenInfo == null)
                        return false;

                    tokenInfo.AuthToken = token.AuthToken;
                    tokenInfo.ExpiresOn = token.ExpiresOn;
                    tokenInfo.IssuedOn = token.IssuedOn;
                    tokenInfo.UserId = token.UserId;
                    tde.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Used to get the token information
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Token GetTokenInformation(string token)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    return tde.Tokens.FirstOrDefault(m => m.AuthToken == token);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Used to get tokens based on user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Token> GetTokensBasedOnUserId(int userId)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    return tde.Tokens.Where(m => m.UserId == userId).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Used to delete token based on user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteTokensByUserId(int userId)
        {
            try
            {
                using (var tde = new TimeDifferenceEntities())
                {
                    var tokens = tde.Tokens.Where(m => m.UserId == userId);
                    tde.Tokens.RemoveRange(tokens);
                    tde.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
