using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.Entity
{
    public class OrganizationalProvider:Provider
    {
        private string _organizatioName;

        public OrganizationalProvider()
        { }
        public string OrganizatioName
        {
            get { return _organizatioName; }
            set { _organizatioName = value; }
        }
    }
}
