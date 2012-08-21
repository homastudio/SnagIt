using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Navigation;
using Orchard.Localization;

namespace CyberStride.Contacts
{
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }
        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("Contact Requests"), "3",
                menu => menu.Add(T("List"), "0", item => item.Action("Index", "Admin", new { area = "CyberStride.Contacts" })));
        }

        public string MenuName { get { return "admin"; } }
    }
}