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


namespace WISEroster.Domain.Api
{

    // CourseOffering
    ///<summary>
    /// This entity represents an entry in the course catalog of available courses offered by the school during a session.
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class CourseOffering
    {

        ///<summary>
        /// The local code assigned by the School that identifies the course offering provided for the instruction of students.
        ///</summary>
        public string LocalCourseCode { get; set; } // LocalCourseCode (Primary key) (length: 60)

        ///<summary>
        /// The identifier assigned to a school.
        ///</summary>
        public int SchoolId { get; set; } // SchoolId (Primary key)

        ///<summary>
        /// The identifier for the school year.
        ///</summary>
        public short SchoolYear { get; set; } // SchoolYear (Primary key)

        ///<summary>
        /// The identifier for the calendar for the academic session (e.g., 2010/11, 2011 Summer).
        ///</summary>
        public string SessionName { get; set; } // SessionName (Primary key) (length: 60)

        ///<summary>
        /// The descriptive name given to a course of study offered in the school, if different from the CourseTitle.
        ///</summary>
        public string LocalCourseTitle { get; set; } // LocalCourseTitle (length: 60)

        ///<summary>
        /// The planned total number of clock minutes of instruction for this course offering. Generally, this should be at least as many minutes as is required for completion by the related state- or district-defined course.
        ///</summary>
        public int? InstructionalTimePlanned { get; set; } // InstructionalTimePlanned

        ///<summary>
        /// A unique alphanumeric code assigned to a course.
        ///</summary>
        public string CourseCode { get; set; } // CourseCode (length: 60)

        ///<summary>
        /// The identifier assigned to an education organization.
        ///</summary>
        public int EducationOrganizationId { get; set; } // EducationOrganizationId
        public string Discriminator { get; set; } // Discriminator (length: 128)
        public System.DateTime CreateDate { get; set; } // CreateDate
        public System.DateTime LastModifiedDate { get; set; } // LastModifiedDate
        public System.Guid Id { get; set; } // Id
        public long ChangeVersion { get; set; } // ChangeVersion

        // Reverse navigation

        /// <summary>
        /// Child Sections where [Section].([LocalCourseCode], [SchoolId], [SchoolYear], [SessionName]) point to this entity (FK_Section_CourseOffering)
        /// </summary>
        public System.Collections.Generic.ICollection<Section> Sections { get; set; } // Section.FK_Section_CourseOffering

        // Foreign keys

        /// <summary>
        /// Parent School pointed by [CourseOffering].([SchoolId]) (FK_CourseOffering_School)
        /// </summary>
        public School School { get; set; } // FK_CourseOffering_School

        /// <summary>
        /// Parent Session pointed by [CourseOffering].([SchoolId], [SchoolYear], [SessionName]) (FK_CourseOffering_Session)
        /// </summary>
        public Session Session { get; set; } // FK_CourseOffering_Session

        public CourseOffering()
        {
            CreateDate = System.DateTime.Now;
            LastModifiedDate = System.DateTime.Now;
            Id = System.Guid.NewGuid();
            Sections = new System.Collections.Generic.List<Section>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>