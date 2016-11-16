using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ApiAuthenticationProvider.Authentication
{
    public class DefaultApiAreaSelector : IApiAreaSelector
    {
        public string GetArea(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            string[] area = (string[])routeData.Route.DataTokens[Constants.ApiAreaDataToken];
            return area.Single();
        }
    }
}