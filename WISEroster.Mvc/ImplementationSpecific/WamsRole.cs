using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WISEroster.Mvc.ImplementationSpecific
{
    public class WamsRole
    {
        public int EducationOrganizationId { get; set; }
        public string AgencyName { get; set; }
        public string AgencyTypeId { get; set; }
        public string Role { get; set; }
        public string SchoolName { get; set; }

        public List<WisePrivilege> Privileges { get; set; }
    }
}