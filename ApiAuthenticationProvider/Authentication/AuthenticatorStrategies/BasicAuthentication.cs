using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;

namespace ApiAuthenticationProvider.Authentication
{
    [AuthenticationType(ImplementedAuthentication = AuthenticationType.Basic)]
    public class BasicAuthentication : IAuthenticator
    {
        private IAuthenticatorRepository repository;

        private const string SCHEME = "Basic";



        public BasicAuthentication(IAuthenticatorRepository repository)
        {
            this.repository = repository;
        }

        public AuthenticationType ImplementedAuthentication
        {
            get
            {
                return AuthenticationType.Basic;
            }
        }

        public void Authenticate(HttpRequestMessage request, CancellationToken cancellationToken, List<Claim> claims)
        {
            try
            {
                var headers = request.Headers;
                if (headers.Authorization != null && SCHEME.Equals(headers.Authorization.Scheme))
                {
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");

                    string credentials = encoding.GetString(Convert.FromBase64String(headers.Authorization.Parameter));
                    string[] parts = credentials.Split(':');
                    string userName = parts[0].Trim();
                    string password = parts[1].Trim();

                    int userId;
                    if (repository.ValidateUserNameAndPassword(userName, password, out userId))
                    {
                        claims.Add(new Claim(ClaimTypes.Name, userId.ToString()));
                        claims.Add(new Claim(ClaimTypes.AuthenticationMethod, System.IdentityModel.Tokens.AuthenticationMethods.Password));
                    }
                    else
                    {
                        throw new InvalidCredentialException();
                    }
                }
                else
                {
                    throw new InvalidCredentialException();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}