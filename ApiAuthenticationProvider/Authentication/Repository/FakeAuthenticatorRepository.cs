using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiAuthenticationProvider.Authentication
{
    public class FakeAuthenticatorRepository : IAuthenticatorRepository
    {
        public List<AuthenticationType> GetAreaAuthenticationTypes(string area)
        {
            return new List<AuthenticationType>() { AuthenticationType.Basic, AuthenticationType.ClientCertificate };
        }

        public string GetPassword(int userId)
        {
            return "123456";
        }

        public List<string> GetUserAreas(int userId)
        {
            return new List<string>() { "AreaAdmin", "AreaUser" };
        }

        public int GetUserIdByUserName(string userName)
        {
            return 1;
        }

        public List<string> GetUserRoles(int userId)
        {
            return new List<string>() { "Admin", "User" };
        }

        public object LoadUser(int userId)
        {
            throw new NotImplementedException();
        }

        public T LoadUser<T>(int userId) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public bool ValidateClientCertificate(string Thumbprint, out int userId)
        {
            userId = 1;
            return true;
        }

        public bool ValidatePresharedKey(string publicKey, string privateKey, out int userId)
        {
            userId = 1;
            return true;
        }

        public bool ValidateUserNameAndPassword(string userName, string password, out int userId)
        {
            userId = 1;
            return true;
        }
    }
}