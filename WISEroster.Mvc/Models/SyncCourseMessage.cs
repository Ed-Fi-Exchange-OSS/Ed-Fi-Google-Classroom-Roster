using System.Collections.Generic;
using WISEroster.Domain.Api;
using WISEroster.Domain.Models;

namespace WISEroster.Mvc.Models
{
   
    public class SyncCourseMessage
    {
        public SyncCourseMessage()
        {
            Courses= new List<GcCourse>();
            Logs= new List<GcLog>();
        }


        public int LeaId { get; set; }
        public string UserEmail { get; set; }
        public List<GcCourse> Courses { get; set; }
        public string Message { get; set; }
        public EducationOrganization School { get; set; }
        public List<GcLog> Logs { get; set; }

    }
}