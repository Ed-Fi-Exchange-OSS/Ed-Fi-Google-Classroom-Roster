using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEroster.Business.Models
{
    public class RulesListModel
    {
        public short SchoolYear { get; set; }
        public int? SchoolCategoryDescriptorId { get; set; }
        public List<int> Schools { get; set; }
        public string SessionName { get; set; }
        public List<string> Courses { get; set; }
        public bool? IncludeExclude { get; set; }
        public bool StaffOnly { get; set; }
        public int EducationOrganizationId { get; set; }
        public int RuleId { get; set; }
        public List<string> SchoolNames { get; set; }
        public List<int> GradeLevels { get; set; }
        public List<string> GradeNames { get; set; }
        public bool? GroupByTitle { get; set; }
        public int TypeId { get; set; }

        public string SelectionDetail
        {
            get
            {
                if (TypeId == 1)
                {
                    return string.Format("{0} Courses{1}",IncludeExclude==true?"Include":"Exclude", GroupByTitle == true ? ", Group Sections" : "");
                }
                else if (TypeId == 2)
                {
                    return "By Grade";
                }
                else
                {
                    return string.Format("{0}{1} Courses{2}","By Teacher", IncludeExclude == true ? ", Include" : ", Exclude", GroupByTitle == true ? ", Group Sections" : "");
                }
            }
        }
    }
}
