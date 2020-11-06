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

    // EducationOrganization
    ///<summary>
    /// This entity represents any public or private institution, organization, or agency that provides instructional or support services to students or staff at any level.
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class EducationOrganization
    {

        ///<summary>
        /// The identifier assigned to an education organization.
        ///</summary>
        public int EducationOrganizationId { get; set; } // EducationOrganizationId (Primary key)

        ///<summary>
        /// The full, legally accepted name of the institution.
        ///</summary>
        public string NameOfInstitution { get; set; } // NameOfInstitution (length: 75)

        ///<summary>
        /// A short name for the institution.
        ///</summary>
        public string ShortNameOfInstitution { get; set; } // ShortNameOfInstitution (length: 75)

        ///<summary>
        /// The public web site address (URL) for the EducationOrganization.
        ///</summary>
        public string WebSite { get; set; } // WebSite (length: 255)

        ///<summary>
        /// The current operational status of the EducationOrganization (e.g., active, inactive).
        ///</summary>
        public int? OperationalStatusDescriptorId { get; set; } // OperationalStatusDescriptorId
        public string Discriminator { get; set; } // Discriminator (length: 128)
        public System.DateTime CreateDate { get; set; } // CreateDate
        public System.DateTime LastModifiedDate { get; set; } // LastModifiedDate
        public System.Guid Id { get; set; } // Id
        public long ChangeVersion { get; set; } // ChangeVersion

        // Reverse navigation

        /// <summary>
        /// Parent (One-to-One) EducationOrganization pointed by [LocalEducationAgency].[LocalEducationAgencyId] (FK_LocalEducationAgency_EducationOrganization)
        /// </summary>
        public LocalEducationAgency LocalEducationAgency { get; set; } // LocalEducationAgency.FK_LocalEducationAgency_EducationOrganization
        /// <summary>
        /// Parent (One-to-One) EducationOrganization pointed by [School].[SchoolId] (FK_School_EducationOrganization)
        /// </summary>
        public School School { get; set; } // School.FK_School_EducationOrganization
        /// <summary>
        /// Child StudentEducationOrganizationAssociations where [StudentEducationOrganizationAssociation].[EducationOrganizationId] point to this entity (FK_StudentEducationOrganizationAssociation_EducationOrganization)
        /// </summary>
        public System.Collections.Generic.ICollection<StudentEducationOrganizationAssociation> StudentEducationOrganizationAssociations { get; set; } // StudentEducationOrganizationAssociation.FK_StudentEducationOrganizationAssociation_EducationOrganization

        public EducationOrganization()
        {
            CreateDate = System.DateTime.Now;
            LastModifiedDate = System.DateTime.Now;
            Id = System.Guid.NewGuid();
            StudentEducationOrganizationAssociations = new System.Collections.Generic.List<StudentEducationOrganizationAssociation>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
