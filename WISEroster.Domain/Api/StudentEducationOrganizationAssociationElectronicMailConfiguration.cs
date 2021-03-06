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

    // StudentEducationOrganizationAssociationElectronicMail
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class StudentEducationOrganizationAssociationElectronicMailConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<StudentEducationOrganizationAssociationElectronicMail>
    {
        public StudentEducationOrganizationAssociationElectronicMailConfiguration()
            : this("edfi")
        {
        }

        public StudentEducationOrganizationAssociationElectronicMailConfiguration(string schema)
        {
            ToTable("StudentEducationOrganizationAssociationElectronicMail", schema);
            HasKey(x => new { x.EducationOrganizationId, x.ElectronicMailAddress, x.ElectronicMailTypeDescriptorId, x.StudentUsi });

            Property(x => x.EducationOrganizationId).HasColumnName(@"EducationOrganizationId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ElectronicMailAddress).HasColumnName(@"ElectronicMailAddress").HasColumnType("nvarchar").IsRequired().HasMaxLength(128).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ElectronicMailTypeDescriptorId).HasColumnName(@"ElectronicMailTypeDescriptorId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.StudentUsi).HasColumnName(@"StudentUSI").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.PrimaryEmailAddressIndicator).HasColumnName(@"PrimaryEmailAddressIndicator").HasColumnType("bit").IsOptional();
            Property(x => x.DoNotPublishIndicator).HasColumnName(@"DoNotPublishIndicator").HasColumnType("bit").IsOptional();
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").HasColumnType("datetime2").IsRequired();

            // Foreign keys
            HasRequired(a => a.ElectronicMailTypeDescriptor).WithMany(b => b.StudentEducationOrganizationAssociationElectronicMails).HasForeignKey(c => c.ElectronicMailTypeDescriptorId).WillCascadeOnDelete(false); // FK_StudentEducationOrganizationAssociationElectronicMail_ElectronicMailTypeDescriptor
            HasRequired(a => a.StudentEducationOrganizationAssociation).WithMany(b => b.StudentEducationOrganizationAssociationElectronicMails).HasForeignKey(c => new { c.EducationOrganizationId, c.StudentUsi }); // FK_StudentEducationOrganizationAssociationElectronicMail_StudentEducationOrganizationAssociation
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
