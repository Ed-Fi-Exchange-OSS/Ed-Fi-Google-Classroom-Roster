using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEroster.Business.Models
{
    public class ClassToRosterModel: GcClassName
    {
        public string SchoolName { get; set; }
        public string LocalCourseTitle { get; set; }
        public int? StudentCount { get; set; }
        public List<string> StaffName { get; set; }
        public string StaffNames
        {
            get { return StaffName != null ? string.Join(", ", StaffName) : ""; }
        }

    }
}
