// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.8
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace WISEroster.Domain.Models
{

    // GcCourse
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class GcCourse
    {
        public int EducationOrganizationId { get; set; } // EducationOrganizationId (Primary key)
        public short SchoolYear { get; set; } // SchoolYear (Primary key)
        public int SchoolId { get; set; } // SchoolId (Primary key)
        public string LocalCourseCode { get; set; } // LocalCourseCode (Primary key) (length: 60)
        public string SessionName { get; set; } // SessionName (Primary key) (length: 60)
        public string SectionIdentifier { get; set; } // SectionIdentifier (Primary key) (length: 255)
        public string LocalCourseTitle { get; set; } // LocalCourseTitle (length: 60)
        public System.DateTime CreateDate { get; set; } // CreateDate
        public string GcName { get; private set; } // GcName (length: 154)
        public string Owner { get; set; } // Owner (length: 200)
        public string AliasId { get; set; } // AliasId (length: 200)
        public bool? Saved { get; set; } // Saved
        public bool? Activated { get; set; } // Activated
        public string GcMessage { get; set; } // GcMessage (length: 200)
        public string CourseId { get; set; } // CourseId (length: 200)
  
        // Reverse navigation

        /// <summary>
        /// Child GcCourseUsers where [GcCourseUser].([EducationOrganizationId], [LocalCourseCode], [SchoolId], [SchoolYear], [SectionIdentifier], [SessionName]) point to this entity (FK_GcCourse_GcCourseUser)
        /// </summary>
        public System.Collections.Generic.ICollection<GcCourseUser> GcCourseUsers { get; set; } // GcCourseUser.FK_GcCourse_GcCourseUser

        public GcCourse()
        {
            CreateDate = System.DateTime.Now;
            GcCourseUsers = new System.Collections.Generic.List<GcCourseUser>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
