using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WISEroster.Domain.Models;

namespace WISEroster.Mvc.Models
{
    public class SyncViewModel
    {
        [DisplayName("School Year")]
        [Required(ErrorMessage = "School Year is required.")]
        public short SchoolYear { get; set; }
        public List<SelectListItem> Years { get; set; }

        [DisplayName("School")]
        [Required(ErrorMessage = "School is required.")]
        public int SelectedSchool { get; set; }
        public List<SelectListItem> Schools { get; set; }


        public List<GcCourse> SyncList { get; set; }
    }
}