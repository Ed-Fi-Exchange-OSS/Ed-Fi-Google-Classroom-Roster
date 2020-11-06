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

    // StudentEducationOrganizationAssociation
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class StudentEducationOrganizationAssociationConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<StudentEducationOrganizationAssociation>
    {
        public StudentEducationOrganizationAssociationConfiguration()
            : this("edfi")
        {
        }

        public StudentEducationOrganizationAssociationConfiguration(string schema)
        {
            ToTable("StudentEducationOrganizationAssociation", schema);
            HasKey(x => new { x.EducationOrganizationId, x.StudentUsi });

            Property(x => x.EducationOrganizationId).HasColumnName(@"EducationOrganizationId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.StudentUsi).HasColumnName(@"StudentUSI").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SexDescriptorId).HasColumnName(@"SexDescriptorId").HasColumnType("int").IsRequired();
            Property(x => x.ProfileThumbnail).HasColumnName(@"ProfileThumbnail").HasColumnType("nvarchar").IsOptional().HasMaxLength(255);
            Property(x => x.HispanicLatinoEthnicity).HasColumnName(@"HispanicLatinoEthnicity").HasColumnType("bit").IsOptional();
            Property(x => x.OldEthnicityDescriptorId).HasColumnName(@"OldEthnicityDescriptorId").HasColumnType("int").IsOptional();
            Property(x => x.LimitedEnglishProficiencyDescriptorId).HasColumnName(@"LimitedEnglishProficiencyDescriptorId").HasColumnType("int").IsOptional();
            Property(x => x.LoginId).HasColumnName(@"LoginId").HasColumnType("nvarchar").IsOptional().HasMaxLength(60);
            Property(x => x.Discriminator).HasColumnName(@"Discriminator").HasColumnType("nvarchar").IsOptional().HasMaxLength(128);
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").HasColumnType("datetime2").IsRequired();
            Property(x => x.LastModifiedDate).HasColumnName(@"LastModifiedDate").HasColumnType("datetime2").IsRequired();
            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.ChangeVersion).HasColumnName(@"ChangeVersion").HasColumnType("bigint").IsRequired();

            // Foreign keys
            HasRequired(a => a.EducationOrganization).WithMany(b => b.StudentEducationOrganizationAssociations).HasForeignKey(c => c.EducationOrganizationId).WillCascadeOnDelete(false); // FK_StudentEducationOrganizationAssociation_EducationOrganization
            HasRequired(a => a.Student).WithMany(b => b.StudentEducationOrganizationAssociations).HasForeignKey(c => c.StudentUsi).WillCascadeOnDelete(false); // FK_StudentEducationOrganizationAssociation_Student
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>