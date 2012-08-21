using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberStride.Contacts.Models
{
    public static class Validations
    {
        public const string URL_PATTERN = @"^(http(s)?://)?([a-zA-Z0-9]([a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}[\S]+$";

        public const string EMAIL_ADDRESS_PATTERN = @"^[^@\s]+@[^@\s]+$";

        public const string PHONE_PATTERN = @"^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}$";
    }
}