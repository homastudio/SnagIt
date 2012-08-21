using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Mvc.Routes;
using System.Web.Routing;
using System.Web.Mvc;

namespace CyberStride.Contacts
{
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach(var descriptor in GetRoutes())
                routes.Add(descriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            yield return new RouteDescriptor
            {
                Priority=5,
                Route = new Route("contact/create", 
                    new RouteValueDictionary{
                    {"area", "CyberStride.Contacts"},
                    {"controller","contact"},
                    {"action","create"}
                    },new RouteValueDictionary(),
                    new RouteValueDictionary{
                    {"area", "CyberStride.Contacts"}
                    },
                    new MvcRouteHandler())
            };
        }
    }
}