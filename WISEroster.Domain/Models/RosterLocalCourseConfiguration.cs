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

    // RosterLocalCourse
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class RosterLocalCourseConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<RosterLocalCourse>
    {
        public RosterLocalCourseConfiguration()
            : this("dbo")
        {
        }

        public RosterLocalCourseConfiguration(string schema)
        {
            ToTable("RosterLocalCourse", schema);
            HasKey(x => new { x.RuleId, x.LocalCourseCode });

            Property(x => x.RuleId).HasColumnName(@"RuleId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.LocalCourseCode).HasColumnName(@"LocalCourseCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(60).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            // Foreign keys
            HasRequired(a => a.ProvisioningRule).WithMany(b => b.RosterLocalCourses).HasForeignKey(c => c.RuleId); // FK_RosterLocalCourse_ProvisioningRules
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>