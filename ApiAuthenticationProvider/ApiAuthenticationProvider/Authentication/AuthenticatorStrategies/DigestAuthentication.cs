using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;

namespace ApiAuthenticationProvider.Authentication
{
    [AuthenticationType(ImplementedAuthentication = AuthenticationType.Digest)]
    public class DigestAuthentication : IAuthenticator
    {
        private const string SCHEME = "Digest";

        IAuthenticatorRepository repository;


        public DigestAuthentication(IAuthenticatorRepository repository)
        {
            this.repository = repository;
        }

        public AuthenticationType ImplementedAuthentication
        {
            get
            {
                return AuthenticationType.Digest;
            }
        }

        public void Authenticate(HttpRequestMessage request, CancellationToken cancellationToken, List<Claim> claims)
        {
            throw new NotImplementedException();
        }

    }
}