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

    // Descriptor
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class DescriptorConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Descriptor>
    {
        public DescriptorConfiguration()
            : this("edfi")
        {
        }

        public DescriptorConfiguration(string schema)
        {
            ToTable("Descriptor", schema);
            HasKey(x => x.DescriptorId);

            Property(x => x.DescriptorId).HasColumnName(@"DescriptorId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Namespace).HasColumnName(@"Namespace").HasColumnType("nvarchar").IsRequired().HasMaxLength(255);
            Property(x => x.CodeValue).HasColumnName(@"CodeValue").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.ShortDescription).HasColumnName(@"ShortDescription").HasColumnType("nvarchar").IsRequired().HasMaxLength(75);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar").IsOptional().HasMaxLength(1024);
            Property(x => x.PriorDescriptorId).HasColumnName(@"PriorDescriptorId").HasColumnType("int").IsOptional();
            Property(x => x.EffectiveBeginDate).HasColumnName(@"EffectiveBeginDate").HasColumnType("date").IsOptional();
            Property(x => x.EffectiveEndDate).HasColumnName(@"EffectiveEndDate").HasColumnType("date").IsOptional();
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").HasColumnType("datetime2").IsRequired();
            Property(x => x.LastModifiedDate).HasColumnName(@"LastModifiedDate").HasColumnType("datetime2").IsRequired();
            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.ChangeVersion).HasColumnName(@"ChangeVersion").HasColumnType("bigint").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
