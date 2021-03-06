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

    // RosterSchool
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class RosterSchoolConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<RosterSchool>
    {
        public RosterSchoolConfiguration()
            : this("dbo")
        {
        }

        public RosterSchoolConfiguration(string schema)
        {
            ToTable("RosterSchool", schema);
            HasKey(x => new { x.RuleId, x.SchoolId });

            Property(x => x.RuleId).HasColumnName(@"RuleId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SchoolId).HasColumnName(@"SchoolId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            // Foreign keys
            HasRequired(a => a.ProvisioningRule).WithMany(b => b.RosterSchools).HasForeignKey(c => c.RuleId); // FK_RosterSchool_ProvisioningRules
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
