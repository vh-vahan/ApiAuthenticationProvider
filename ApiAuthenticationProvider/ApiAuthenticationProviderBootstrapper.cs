using ApiAuthenticationProvider.Authentication;
using BasicLibrary.DependencyInjection;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ApiAuthenticationProvider
{
    public class ApiAuthenticationProviderBootstrapper : Bootstrapper<ApiAuthenticationProviderBootstrapper>
    {


        private ApiAuthenticationProviderBootstrapper()
        {

        }

        protected override IUnityContainer BuildUnityContainer()
        {
            Container.RegisterType<IAuthenticator, NoneAuthentication>(AuthenticationType.None.ToString());
            Container.RegisterType<IAuthenticator, BasicAuthentication>(AuthenticationType.Basic.ToString());
            Container.RegisterType<IAuthenticator, DigestAuthentication>(AuthenticationType.Digest.ToString());
            Container.RegisterType<IAuthenticator, PresharedKeyAuthentication>(AuthenticationType.PresharedKey.ToString());
            Container.RegisterType<IAuthenticator, ClientCertificateAuthentication>(AuthenticationType.ClientCertificate.ToString());

            Container.RegisterType<IApiAreaSelector, DefaultApiAreaSelector>();
            Container.RegisterType<IAuthenticatorRepository, FakeAuthenticatorRepository>();

            RegisterTypes(Container);
            return Container;
        }


    }
}