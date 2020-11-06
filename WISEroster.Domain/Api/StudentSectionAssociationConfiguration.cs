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
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class StudentSectionAssociationConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<StudentSectionAssociation>
    {
        public StudentSectionAssociationConfiguration()
            : this("edfi")
        {
        }

        public StudentSectionAssociationConfiguration(string schema)
        {
            ToTable("StudentSectionAssociation", schema);
            HasKey(x => new { x.BeginDate, x.LocalCourseCode, x.SchoolId, x.SchoolYear, x.SectionIdentifier, x.SessionName, x.StudentUsi });

            Property(x => x.BeginDate).HasColumnName(@"BeginDate").HasColumnType("date").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.LocalCourseCode).HasColumnName(@"LocalCourseCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(60).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SchoolId).HasColumnName(@"SchoolId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SchoolYear).HasColumnName(@"SchoolYear").HasColumnType("smallint").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SectionIdentifier).HasColumnName(@"SectionIdentifier").HasColumnType("nvarchar").IsRequired().HasMaxLength(255).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SessionName).HasColumnName(@"SessionName").HasColumnType("nvarchar").IsRequired().HasMaxLength(60).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.StudentUsi).HasColumnName(@"StudentUSI").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.EndDate).HasColumnName(@"EndDate").HasColumnType("date").IsOptional();
            Property(x => x.HomeroomIndicator).HasColumnName(@"HomeroomIndicator").HasColumnType("bit").IsOptional();
            Property(x => x.RepeatIdentifierDescriptorId).HasColumnName(@"RepeatIdentifierDescriptorId").HasColumnType("int").IsOptional();
            Property(x => x.TeacherStudentDataLinkExclusion).HasColumnName(@"TeacherStudentDataLinkExclusion").HasColumnType("bit").IsOptional();
            Property(x => x.AttemptStatusDescriptorId).HasColumnName(@"AttemptStatusDescriptorId").HasColumnType("int").IsOptional();
            Property(x => x.Discriminator).HasColumnName(@"Discriminator").HasColumnType("nvarchar").IsOptional().HasMaxLength(128);
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").HasColumnType("datetime2").IsRequired();
            Property(x => x.LastModifiedDate).HasColumnName(@"LastModifiedDate").HasColumnType("datetime2").IsRequired();
            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.ChangeVersion).HasColumnName(@"ChangeVersion").HasColumnType("bigint").IsRequired();

            // Foreign keys
            HasRequired(a => a.Section).WithMany(b => b.StudentSectionAssociations).HasForeignKey(c => new { c.LocalCourseCode, c.SchoolId, c.SchoolYear, c.SectionIdentifier, c.SessionName }).WillCascadeOnDelete(false); // FK_StudentSectionAssociation_Section
            HasRequired(a => a.Student).WithMany(b => b.StudentSectionAssociations).HasForeignKey(c => c.StudentUsi).WillCascadeOnDelete(false); // FK_StudentSectionAssociation_Student
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
