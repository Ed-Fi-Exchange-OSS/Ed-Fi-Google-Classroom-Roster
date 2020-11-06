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

    // Session
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class SessionConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Session>
    {
        public SessionConfiguration()
            : this("edfi")
        {
        }

        public SessionConfiguration(string schema)
        {
            ToTable("Session", schema);
            HasKey(x => new { x.SchoolId, x.SchoolYear, x.SessionName });

            Property(x => x.SchoolId).HasColumnName(@"SchoolId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SchoolYear).HasColumnName(@"SchoolYear").HasColumnType("smallint").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SessionName).HasColumnName(@"SessionName").HasColumnType("nvarchar").IsRequired().HasMaxLength(60).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.BeginDate).HasColumnName(@"BeginDate").HasColumnType("date").IsRequired();
            Property(x => x.EndDate).HasColumnName(@"EndDate").HasColumnType("date").IsRequired();
            Property(x => x.TermDescriptorId).HasColumnName(@"TermDescriptorId").HasColumnType("int").IsRequired();
            Property(x => x.TotalInstructionalDays).HasColumnName(@"TotalInstructionalDays").HasColumnType("int").IsRequired();
            Property(x => x.Discriminator).HasColumnName(@"Discriminator").HasColumnType("nvarchar").IsOptional().HasMaxLength(128);
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").HasColumnType("datetime2").IsRequired();
            Property(x => x.LastModifiedDate).HasColumnName(@"LastModifiedDate").HasColumnType("datetime2").IsRequired();
            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.ChangeVersion).HasColumnName(@"ChangeVersion").HasColumnType("bigint").IsRequired();

            // Foreign keys
            HasRequired(a => a.School).WithMany(b => b.Sessions).HasForeignKey(c => c.SchoolId).WillCascadeOnDelete(false); // FK_Session_School
            HasRequired(a => a.TermDescriptor).WithMany(b => b.Sessions).HasForeignKey(c => c.TermDescriptorId).WillCascadeOnDelete(false); // FK_Session_TermDescriptor
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
