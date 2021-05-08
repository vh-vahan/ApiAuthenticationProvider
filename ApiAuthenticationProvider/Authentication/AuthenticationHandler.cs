using BasicLibrary.EnterpriseLibraryLogging;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ApiAuthenticationProvider.Authentication
{

    public class AuthenticationHandler : DelegatingHandler
    {
        IAuthenticatorRepository repository;
        IApiAreaSelector apiAreaSelector;

        public AuthenticationHandler()
            : this(ApiAuthenticationProviderBootstrapper.Instance.Resolve<IAuthenticatorRepository>(), ApiAuthenticationProviderBootstrapper.Instance.Resolve<IApiAreaSelector>())
        {

        }
        public AuthenticationHandler(IAuthenticatorRepository repository)
            : this(repository, ApiAuthenticationProviderBootstrapper.Instance.Resolve<IApiAreaSelector>())
        {

        }
        public AuthenticationHandler(IAuthenticatorRepository repository, IApiAreaSelector apiAreaSelector)
        {
            this.repository = repository;
            this.apiAreaSelector = apiAreaSelector;
        }

        protected virtual string GetAreaFromRequest(HttpRequestMessage request)
        {
            return apiAreaSelector.GetArea(request);
        }
        protected virtual List<AuthenticationType> GetAreaAuthenticationTypes(string area)
        {
            HashSet<AuthenticationType> res = new HashSet<AuthenticationType>();
            foreach (var item in repository.GetAreaAuthenticationTypes(area))
            {
                foreach (var val in Enum.GetValues(typeof(AuthenticationType)))
                {
                    if (item.HasFlag((AuthenticationType)val))
                        res.Add((AuthenticationType)val);
                } 
            }
            res.Remove(AuthenticationType.None);
            return res.ToList();
        }
        private AuthenticationType ConcatenateAuthenticationTypes(List<AuthenticationType> authTypes)
        {
            AuthenticationType auth = AuthenticationType.None;
            foreach (var item in authTypes)
            {
                auth &= item;
            }
            return auth;
        }
        protected virtual IEnumerable<IAuthenticator> GetAuthenticators(List<AuthenticationType> types)
        {
            foreach (var item in types)
            {
                yield return ApiAuthenticationProviderBootstrapper.Instance.Resolve<IAuthenticator>(item.ToString(), new ParameterOverride("repository", repository));
            }
        }
        protected virtual IPrincipal CreatePrincipal(List<Claim> claims, List<AuthenticationType> authTypes)
        {
            var userIds = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).ToList();
            if (userIds.Count != authTypes.Count)
                throw new SecurityException();
            if (userIds.Distinct().Count() != 1)
                throw new SecurityException();

            int userId = int.Parse(userIds[0]);
            dynamic user = repository.LoadUser<dynamic>(userId);
            List<string> roles = user.GetRoles();
            List<string> areas = user.GetAreas();

            roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r.ToString())));
            areas.ForEach(a => claims.Add(new Claim(CustomClaimTypes.ApiArea, a.ToString())));

            return new ClaimsPrincipal(new[] { new ClaimsIdentity(claims, ConcatenateAuthenticationTypes(authTypes).ToString()) });
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                List<Claim> claims = new List<Claim>();
                List<AuthenticationType> authTypes = GetAreaAuthenticationTypes(GetAreaFromRequest(request));
                foreach (var item in GetAuthenticators(authTypes))
                {
                    item.Authenticate(request, cancellationToken, claims);
                }
                if (claims.Count > 0)
                {
                    var principal = CreatePrincipal(claims, authTypes);
                    Thread.CurrentPrincipal = principal;
                    if (HttpContext.Current != null)
                        HttpContext.Current.User = principal;
                }
                var response = await base.SendAsync(request, cancellationToken);
                //if (response.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic"));
                //}
                return response;
            }
            catch (Exception ex)
            {
                LoggingManager.Write(ex, Category.General, Severity.Error);
                var response = request.CreateResponse(HttpStatusCode.Unauthorized);
                //response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(Basic));
                return response;
            }
        }


    }
}