using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiAuthenticationProvider.Authentication
{
    [AuthenticationType(ImplementedAuthentication = AuthenticationType.None)]
    public class NoneAuthentication : IAuthenticator
    {

        private IAuthenticatorRepository repository;

        public NoneAuthentication(IAuthenticatorRepository repository)
        {
            this.repository = repository;
        }

        public AuthenticationType ImplementedAuthentication
        {
            get
            {
                return AuthenticationType.None;
            }
        }

        public void Authenticate(HttpRequestMessage request, CancellationToken cancellationToken, List<Claim> claims)
        {
            return;
        }
    }
}
