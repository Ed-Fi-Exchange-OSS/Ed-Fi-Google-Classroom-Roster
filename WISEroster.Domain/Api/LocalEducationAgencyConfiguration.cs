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

    // LocalEducationAgency
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class LocalEducationAgencyConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<LocalEducationAgency>
    {
        public LocalEducationAgencyConfiguration()
            : this("edfi")
        {
        }

        public LocalEducationAgencyConfiguration(string schema)
        {
            ToTable("LocalEducationAgency", schema);
            HasKey(x => x.LocalEducationAgencyId);

            Property(x => x.LocalEducationAgencyId).HasColumnName(@"LocalEducationAgencyId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.LocalEducationAgencyCategoryDescriptorId).HasColumnName(@"LocalEducationAgencyCategoryDescriptorId").HasColumnType("int").IsRequired();
            Property(x => x.CharterStatusDescriptorId).HasColumnName(@"CharterStatusDescriptorId").HasColumnType("int").IsOptional();
            Property(x => x.ParentLocalEducationAgencyId).HasColumnName(@"ParentLocalEducationAgencyId").HasColumnType("int").IsOptional();
            Property(x => x.EducationServiceCenterId).HasColumnName(@"EducationServiceCenterId").HasColumnType("int").IsOptional();
            Property(x => x.StateEducationAgencyId).HasColumnName(@"StateEducationAgencyId").HasColumnType("int").IsOptional();

            // Foreign keys
            HasOptional(a => a.ParentLocalEducationAgency).WithMany(b => b.LocalEducationAgencies).HasForeignKey(c => c.ParentLocalEducationAgencyId).WillCascadeOnDelete(false); // FK_LocalEducationAgency_LocalEducationAgency
            HasRequired(a => a.EducationOrganization).WithOptional(b => b.LocalEducationAgency); // FK_LocalEducationAgency_EducationOrganization
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
