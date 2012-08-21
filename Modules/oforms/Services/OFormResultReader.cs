using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace oforms.Services
{
    public class OFormResultReader
    {
        private XDocument xdoc;

        public OFormResultReader(string xml)
        {
            this.xdoc = XDocument.Parse(xml);
        }

        public string[] Columns
        {
            get 
            {
                return xdoc.Root.Elements().Select(x => x.Name.ToString()).ToArray();
            }
        }

        public string GetColumnValue(string columnName)
        {
            var xelement = xdoc.Root.Elements().FirstOrDefault(x => x.Name == columnName);
            if (xelement == null) 
            {
                return null;
            }

            return xelement.Value;
        }
    }
}