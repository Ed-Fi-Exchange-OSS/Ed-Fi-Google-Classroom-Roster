using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WISEroster.Mvc.Models
{
    public class ProvisioningRulesViewModel
    {
        [DisplayName("School Year")]
        public short SchoolYear { get; set; }
        public List<SelectListItem> Years { get; set; }

        [DisplayName("School Type")]
        public int? SchoolCategoryDescriptorId { get; set; }
        public List<SelectListItem> Categories { get; set; }

        [DisplayName("Schools")]
        public int[] SelectedSchools { get; set; }
        public List<SelectListItem> Schools { get; set; }

        [DisplayName("Sessions")]
        public string SelectedSession { get; set; }
        public List<SelectListItem> Sessions { get; set; }

        [DisplayName("Courses")]
        public string[] SelectedCourses { get; set; }
        public List<SelectListItem> Courses { get; set; }

        public bool? IncludeExclude { get; set; }
        public bool GroupByTitle { get; set; }


        [DisplayName("Staff Only")]
        public bool StaffOnly { get; set; }
        public int EducationOrganizationId { get; set; }
        public int Id { get; set; }
        public int TypeId { get; set; }


        [DisplayName("Grades")]
        public int[] SelectedGrades { get; set; }
        public List<SelectListItem> Grades { get; set; }

        [DisplayName("Teachers")]
        public int? SelectedTeacher { get; set; }
        public List<SelectListItem> Teachers { get; set; }
        [DisplayName("Sessions")]
        public string SelectedSessionByTeacher { get; set; }
        [DisplayName("Courses")]
        public string[] SelectedCoursesByTeacher { get; set; }
        public bool? IncludeExcludeByTeacher { get; set; }
        public bool GroupByTitleByTeacher { get; set; }

        [DisplayName("Staff Only")]
        public bool StaffOnlyByTeacher { get; set; }
        public List<SelectListItem> CoursesByTeacher { get; set; }
    }
}