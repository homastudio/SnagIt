using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security.Permissions;

namespace CyberStride.Contacts
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission RequestContact = new Permission { Description = "Send contact request", Name = "RequestContact" };
        public static readonly Permission ManageContacts = new Permission { Description = "Manage contact requests", Name = "ManageContacts" };

        public Orchard.Environment.Extensions.Models.Feature Feature { get; set; }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {ManageContacts, RequestContact}
                },
                new PermissionStereotype {
                    Name = "Anonymous",
                    Permissions = new[] {RequestContact}
                },
                new PermissionStereotype {
                    Name = "Authenticated",
                    Permissions = new[] {RequestContact}
                },
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] {RequestContact}
                },
                new PermissionStereotype {
                    Name = "Moderator",
                    Permissions = new[] {ManageContacts, RequestContact}
                },
                new PermissionStereotype {
                    Name = "Author",
                    Permissions = new[] {RequestContact}
                },
                new PermissionStereotype {
                    Name = "Contributor",
                    Permissions = new[] {RequestContact}
                },
            };
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { RequestContact, ManageContacts };
        }
    }
}