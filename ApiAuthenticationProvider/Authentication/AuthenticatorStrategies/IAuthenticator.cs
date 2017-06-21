using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;

namespace ApiAuthenticationProvider.Authentication
{

    public interface IAuthenticator
    {
        AuthenticationType ImplementedAuthentication { get; }
        void Authenticate(HttpRequestMessage request, CancellationToken cancellationToken, List<Claim> claims);
    }
}