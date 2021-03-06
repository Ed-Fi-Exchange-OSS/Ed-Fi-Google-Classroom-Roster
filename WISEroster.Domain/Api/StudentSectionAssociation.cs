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

    // StudentSectionAssociation
    ///<summary>
    /// This association indicates the course sections to which a student is assigned.
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class StudentSectionAssociation
    {

        ///<summary>
        /// Month, day, and year of the Student&apos;s entry or assignment to the Section.
        ///</summary>
        public System.DateTime BeginDate { get; set; } // BeginDate (Primary key)

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
        /// The local identifier assigned to a section.
        ///</summary>
        public string SectionIdentifier { get; set; } // SectionIdentifier (Primary key) (length: 255)

        ///<summary>
        /// The identifier for the calendar for the academic session (e.g., 2010/11, 2011 Summer).
        ///</summary>
        public string SessionName { get; set; } // SessionName (Primary key) (length: 60)

        ///<summary>
        /// A unique alphanumeric code assigned to a student.
        ///</summary>
        public int StudentUsi { get; set; } // StudentUSI (Primary key)

        ///<summary>
        /// Month, day, and year of the withdrawal or exit of the Student from the Section.
        ///</summary>
        public System.DateTime? EndDate { get; set; } // EndDate

        ///<summary>
        /// Indicates the Section is the student&apos;s homeroom. Homeroom period may the convention for taking daily attendance.
        ///</summary>
        public bool? HomeroomIndicator { get; set; } // HomeroomIndicator

        ///<summary>
        /// An indication as to whether a student has previously taken a given course.
        ///         Repeated, counted in grade point average
        ///         Repeated, not counted in grade point average
        ///         Not repeated
        ///         Other.
        ///</summary>
        public int? RepeatIdentifierDescriptorId { get; set; } // RepeatIdentifierDescriptorId

        ///<summary>
        /// Indicates that the student-section combination is excluded from calculation of value-added or growth attribution calculations used for a particular teacher evaluation.
        ///</summary>
        public bool? TeacherStudentDataLinkExclusion { get; set; } // TeacherStudentDataLinkExclusion

        ///<summary>
        /// An indication of the student&apos;s completion status for the section.
        ///</summary>
        public int? AttemptStatusDescriptorId { get; set; } // AttemptStatusDescriptorId
        public string Discriminator { get; set; } // Discriminator (length: 128)
        public System.DateTime CreateDate { get; set; } // CreateDate
        public System.DateTime LastModifiedDate { get; set; } // LastModifiedDate
        public System.Guid Id { get; set; } // Id
        public long ChangeVersion { get; set; } // ChangeVersion

        // Foreign keys

        /// <summary>
        /// Parent Section pointed by [StudentSectionAssociation].([LocalCourseCode], [SchoolId], [SchoolYear], [SectionIdentifier], [SessionName]) (FK_StudentSectionAssociation_Section)
        /// </summary>
        public Section Section { get; set; } // FK_StudentSectionAssociation_Section

        /// <summary>
        /// Parent Student pointed by [StudentSectionAssociation].([StudentUsi]) (FK_StudentSectionAssociation_Student)
        /// </summary>
        public Student Student { get; set; } // FK_StudentSectionAssociation_Student

        public StudentSectionAssociation()
        {
            CreateDate = System.DateTime.Now;
            LastModifiedDate = System.DateTime.Now;
            Id = System.Guid.NewGuid();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
