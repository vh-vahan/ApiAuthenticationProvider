using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;

namespace ApiAuthenticationProvider.Authentication
{
    [AuthenticationType(ImplementedAuthentication = AuthenticationType.PresharedKey)]
    public class PresharedKeyAuthentication : IAuthenticator
    {
        private IAuthenticatorRepository repository;

        public PresharedKeyAuthentication(IAuthenticatorRepository repository)
        {
            this.repository = repository;
        }

        public AuthenticationType ImplementedAuthentication
        {
            get
            {
                return AuthenticationType.PresharedKey;
            }
        }

        public void Authenticate(HttpRequestMessage request, CancellationToken cancellationToken, List<Claim> claims)
        {
            throw new NotImplementedException();
        }
    }
}