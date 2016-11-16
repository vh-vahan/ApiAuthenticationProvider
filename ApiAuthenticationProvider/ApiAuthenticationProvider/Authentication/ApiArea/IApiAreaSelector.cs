using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ApiAuthenticationProvider.Authentication
{
    public interface IApiAreaSelector
    {
        string GetArea(HttpRequestMessage request);
    }
}