using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WISEroster.Mvc.Models
{
    public class UserDebugViewModel
    {
        public string Name { get; set; }
        public int? EducationOrganizationId { get; set; }
        public List<int> Agencies { get; set; }
        public short SchoolYear { get; set; }
        public List<SelectableAgency> SelectableAgencies { get; set; }
        public bool CanImpersonate { get; set; }

        public bool ApiConnect { get; set; }
        public bool DbConnect { get; set; }
        public string Exception { get; set; }
    }
}