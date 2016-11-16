using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiAuthenticationProvider.Authentication
{
    [Flags]
    public enum AuthenticationType
    {
        None = 0,
        Basic = 1,
        Digest = 2,
        PresharedKey = 4,
        ClientCertificate = 8
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AuthenticationTypeAttribute : Attribute
    {
        public AuthenticationType ImplementedAuthentication { get; set; }
    }

}