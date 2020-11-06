using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEroster.Business.Models
{
    public class ProvisioningRulesInsertModel
    {
        public short SchoolYear { get; set; }
        public int? SchoolCategoryDescriptorId { get; set; }
        public int[] SelectedSchools { get; set; }
        public string SelectedSession { get; set; }
        public string[] SelectedCourses { get; set; }
        public bool? IncludeExclude { get; set; }
        public bool StaffOnly { get; set; }
        public int EducationOrganizationId { get; set; }
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int[] SelectedGrades { get; set; }
        public bool? GroupByTitle { get; set; }
        public int? SelectedTeacher { get; set; }


    }
}
