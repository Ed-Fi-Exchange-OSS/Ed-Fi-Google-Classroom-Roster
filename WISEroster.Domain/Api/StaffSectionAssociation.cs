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

    // StaffSectionAssociation
    ///<summary>
    /// This association indicates the class sections to which a staff member is assigned.
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class StaffSectionAssociation
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
        /// The local identifier assigned to a section.
        ///</summary>
        public string SectionIdentifier { get; set; } // SectionIdentifier (Primary key) (length: 255)

        ///<summary>
        /// The identifier for the calendar for the academic session (e.g., 2010/11, 2011 Summer).
        ///</summary>
        public string SessionName { get; set; } // SessionName (Primary key) (length: 60)

        ///<summary>
        /// A unique alphanumeric code assigned to a staff.
        ///</summary>
        public int StaffUSI { get; set; } // StaffUSI (Primary key)

        ///<summary>
        /// The type of position the Staff member holds in the specific class/section; for example:
        ///         Teacher of Record, Assistant Teacher, Support Teacher, Substitute Teacher...
        ///</summary>
        public int ClassroomPositionDescriptorId { get; set; } // ClassroomPositionDescriptorId

        ///<summary>
        /// Month, day, and year of a teacher&apos;s assignment to the Section. If blank, defaults to the first day of the first grading period for the Section.
        ///</summary>
        public System.DateTime? BeginDate { get; set; } // BeginDate

        ///<summary>
        /// Month, day, and year of the last day of a staff member&apos;s assignment to the Section.
        ///</summary>
        public System.DateTime? EndDate { get; set; } // EndDate

        ///<summary>
        /// An indication of whether a teacher is classified as highly qualified for his/her assignment according to state definition. This attribute indicates the teacher is highly qualified for this section being taught.
        ///</summary>
        public bool? HighlyQualifiedTeacher { get; set; } // HighlyQualifiedTeacher

        ///<summary>
        /// Indicates that the entire section is excluded from calculation of value-added or growth attribution calculations used for a particular teacher evaluation.
        ///</summary>
        public bool? TeacherStudentDataLinkExclusion { get; set; } // TeacherStudentDataLinkExclusion

        ///<summary>
        /// Indicates the percentage of the total scheduled course time, academic standards, and/or learning activities delivered in this section by this staff member. A teacher of record designation may be based solely or partially on this contribution percentage.
        ///</summary>
        public decimal? PercentageContribution { get; set; } // PercentageContribution
        public string Discriminator { get; set; } // Discriminator (length: 128)
        public System.DateTime CreateDate { get; set; } // CreateDate
        public System.DateTime LastModifiedDate { get; set; } // LastModifiedDate
        public System.Guid Id { get; set; } // Id
        public long ChangeVersion { get; set; } // ChangeVersion

        // Foreign keys

        /// <summary>
        /// Parent Section pointed by [StaffSectionAssociation].([LocalCourseCode], [SchoolId], [SchoolYear], [SectionIdentifier], [SessionName]) (FK_StaffSectionAssociation_Section)
        /// </summary>
        public Section Section { get; set; } // FK_StaffSectionAssociation_Section

        /// <summary>
        /// Parent Staff pointed by [StaffSectionAssociation].([StaffUSI]) (FK_StaffSectionAssociation_Staff)
        /// </summary>
        public Staff Staff { get; set; } // FK_StaffSectionAssociation_Staff

        public StaffSectionAssociation()
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
