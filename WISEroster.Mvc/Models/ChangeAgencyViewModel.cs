using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WISEroster.Mvc.Models
{
    public class ChangeAgencyViewModel
    {
        public bool CanImpersonate { get; set; }
        public List<SelectableAgency> Organizations { get; set; }
        public List<SelectableAgency> AllOrganizations { get; set; }
    }

    public class SelectableAgency
    {
        public int EducationOrganizationId { get; set; }
        public string NameOfInstitution { get; set; }
    }
}