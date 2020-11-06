using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WISEroster.Domain.Api;
using WISEroster.Domain.Models;

namespace WISEroster.Mvc.Models
{
   
    public class SyncCourseMessage
    {
        public int LeaId { get; set; }
        public string UserEmail { get; set; }
        public List<GcCourse> Courses { get; set; }
        public string Message { get; set; }
        public EducationOrganization School { get; set; }
    }
}