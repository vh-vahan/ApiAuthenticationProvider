using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiAuthenticationProvider.Authentication
{
    public interface IAuthenticatorRepository
    {
        List<AuthenticationType> GetAreaAuthenticationTypes(string area);

        int GetUserIdByUserName(string userName);
        string GetPassword(int userId);

        bool ValidateUserNameAndPassword(string userName, string password, out int userId);
        bool ValidateClientCertificate(string Thumbprint, out int userId);
        bool ValidatePresharedKey(string publicKey, string privateKey, out int userId);

        object LoadUser(int userId);
        T LoadUser<T>(int userId) where T : class, new();

        List<string> GetUserAreas(int userId);
        List<string> GetUserRoles(int userId);
    }


}