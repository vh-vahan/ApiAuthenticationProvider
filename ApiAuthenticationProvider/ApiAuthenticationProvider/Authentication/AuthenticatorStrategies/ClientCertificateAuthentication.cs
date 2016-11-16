using BasicLibrary.EnterpriseLibraryLogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web;
using System.Web.Http.Hosting;

namespace ApiAuthenticationProvider.Authentication
{
    [AuthenticationType(ImplementedAuthentication = AuthenticationType.ClientCertificate)]
    public class ClientCertificateAuthentication : IAuthenticator
    {
        IAuthenticatorRepository repository;
        string issuer = Constants.ClientCertificateIssuerName;

        public ClientCertificateAuthentication(IAuthenticatorRepository repository)
        {
            this.repository = repository;
        }


        public AuthenticationType ImplementedAuthentication
        {
            get
            {
                return AuthenticationType.ClientCertificate;
            }
        }

        public void Authenticate(HttpRequestMessage request, CancellationToken cancellationToken, List<Claim> claims)
        {
            var cert = request.GetClientCertificate();
            if (cert == null)
            {
                throw new InvalidCredentialException();
            }

            X509Chain chain = new X509Chain();
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

            if (chain.Build(cert) && cert.Issuer.Equals(issuer))
            {
                int userId;
                if (repository.ValidateClientCertificate(cert.Thumbprint, out userId))
                {
                    claims.Add(new Claim(ClaimTypes.Name, userId.ToString()));
                    claims.Add(new Claim(ClaimTypes.X500DistinguishedName, cert.Subject.Substring(3)));
                    claims.Add(new Claim(ClaimTypes.AuthenticationMethod, System.IdentityModel.Tokens.AuthenticationMethods.X509));
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

    }
}