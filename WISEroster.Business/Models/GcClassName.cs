namespace WISEroster.Business.Models
{
    public class GcClassName
    {
        public string LocalCourseCode { get; set; }
        public string SectionIdentifier { get; set; }
        public short SchoolYear { get; set; }
        public int SchoolId { get; set; }
        public string SessionName { get; set; }
        public int StaffUSI { get; set; }
        public string ClassName
        {
            get { return string.Format("{0}.{1}.{2}.{3}.{4}.{5}", SchoolId, SchoolYear, LocalCourseCode, StaffUSI, SectionIdentifier??"NA",SessionName??SchoolYear.ToString()); }
        }
    }
}
