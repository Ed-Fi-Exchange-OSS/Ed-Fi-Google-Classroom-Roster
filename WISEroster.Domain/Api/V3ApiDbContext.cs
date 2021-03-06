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


    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class V3ApiDbContext : System.Data.Entity.DbContext, IEdfiApiV3DbContext
    {
        public System.Data.Entity.DbSet<CourseOffering> CourseOfferings { get; set; } // CourseOffering
        public System.Data.Entity.DbSet<Descriptor> Descriptors { get; set; } // Descriptor
        public System.Data.Entity.DbSet<EducationOrganization> EducationOrganizations { get; set; } // EducationOrganization
        public System.Data.Entity.DbSet<ElectronicMailTypeDescriptor> ElectronicMailTypeDescriptors { get; set; } // ElectronicMailTypeDescriptor
        public System.Data.Entity.DbSet<EntryGradeLevelReasonDescriptor> EntryGradeLevelReasonDescriptors { get; set; } // EntryGradeLevelReasonDescriptor
        public System.Data.Entity.DbSet<GradeLevelDescriptor> GradeLevelDescriptors { get; set; } // GradeLevelDescriptor
        public System.Data.Entity.DbSet<LocalEducationAgency> LocalEducationAgencies { get; set; } // LocalEducationAgency
        public System.Data.Entity.DbSet<Location> Locations { get; set; } // Location
        public System.Data.Entity.DbSet<School> Schools { get; set; } // School
        public System.Data.Entity.DbSet<SchoolCategory> SchoolCategories { get; set; } // SchoolCategory
        public System.Data.Entity.DbSet<SchoolCategoryDescriptor> SchoolCategoryDescriptors { get; set; } // SchoolCategoryDescriptor
        public System.Data.Entity.DbSet<SchoolGradeLevel> SchoolGradeLevels { get; set; } // SchoolGradeLevel
        public System.Data.Entity.DbSet<Section> Sections { get; set; } // Section
        public System.Data.Entity.DbSet<Session> Sessions { get; set; } // Session
        public System.Data.Entity.DbSet<Staff> Staffs { get; set; } // Staff
        public System.Data.Entity.DbSet<StaffElectronicMail> StaffElectronicMails { get; set; } // StaffElectronicMail
        public System.Data.Entity.DbSet<StaffSectionAssociation> StaffSectionAssociations { get; set; } // StaffSectionAssociation
        public System.Data.Entity.DbSet<Student> Students { get; set; } // Student
        public System.Data.Entity.DbSet<StudentEducationOrganizationAssociation> StudentEducationOrganizationAssociations { get; set; } // StudentEducationOrganizationAssociation
        public System.Data.Entity.DbSet<StudentEducationOrganizationAssociationElectronicMail> StudentEducationOrganizationAssociationElectronicMails { get; set; } // StudentEducationOrganizationAssociationElectronicMail
        public System.Data.Entity.DbSet<StudentSchoolAssociation> StudentSchoolAssociations { get; set; } // StudentSchoolAssociation
        public System.Data.Entity.DbSet<StudentSchoolAssociationEnrollmentType> StudentSchoolAssociationEnrollmentTypes { get; set; } // StudentSchoolAssociationEnrollmentType
        public System.Data.Entity.DbSet<StudentSectionAssociation> StudentSectionAssociations { get; set; } // StudentSectionAssociation
        public System.Data.Entity.DbSet<TermDescriptor> TermDescriptors { get; set; } // TermDescriptor

        static V3ApiDbContext()
        {
            System.Data.Entity.Database.SetInitializer<V3ApiDbContext>(null);
        }

        public V3ApiDbContext()
            : base("Name=ApiV3DbContext")
        {
            InitializePartial();
        }

        public V3ApiDbContext(string connectionString)
            : base(connectionString)
        {
            InitializePartial();
        }

        public V3ApiDbContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model)
            : base(connectionString, model)
        {
            InitializePartial();
        }

        public V3ApiDbContext(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            InitializePartial();
        }

        public V3ApiDbContext(System.Data.Common.DbConnection existingConnection, System.Data.Entity.Infrastructure.DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            InitializePartial();
        }

        public V3ApiDbContext(System.Data.Entity.Core.Objects.ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            InitializePartial();
        }

        protected override void Dispose(bool disposing)
        {
            DisposePartial(disposing);
            base.Dispose(disposing);
        }

        public bool IsSqlParameterNull(System.Data.SqlClient.SqlParameter param)
        {
            var sqlValue = param.SqlValue;
            var nullableValue = sqlValue as System.Data.SqlTypes.INullable;
            if (nullableValue != null)
                return nullableValue.IsNull;
            return (sqlValue == null || sqlValue == System.DBNull.Value);
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CourseOfferingConfiguration());
            modelBuilder.Configurations.Add(new DescriptorConfiguration());
            modelBuilder.Configurations.Add(new EducationOrganizationConfiguration());
            modelBuilder.Configurations.Add(new ElectronicMailTypeDescriptorConfiguration());
            modelBuilder.Configurations.Add(new EntryGradeLevelReasonDescriptorConfiguration());
            modelBuilder.Configurations.Add(new GradeLevelDescriptorConfiguration());
            modelBuilder.Configurations.Add(new LocalEducationAgencyConfiguration());
            modelBuilder.Configurations.Add(new LocationConfiguration());
            modelBuilder.Configurations.Add(new SchoolConfiguration());
            modelBuilder.Configurations.Add(new SchoolCategoryConfiguration());
            modelBuilder.Configurations.Add(new SchoolCategoryDescriptorConfiguration());
            modelBuilder.Configurations.Add(new SchoolGradeLevelConfiguration());
            modelBuilder.Configurations.Add(new SectionConfiguration());
            modelBuilder.Configurations.Add(new SessionConfiguration());
            modelBuilder.Configurations.Add(new StaffConfiguration());
            modelBuilder.Configurations.Add(new StaffElectronicMailConfiguration());
            modelBuilder.Configurations.Add(new StaffSectionAssociationConfiguration());
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new StudentEducationOrganizationAssociationConfiguration());
            modelBuilder.Configurations.Add(new StudentEducationOrganizationAssociationElectronicMailConfiguration());
            modelBuilder.Configurations.Add(new StudentSchoolAssociationConfiguration());
            modelBuilder.Configurations.Add(new StudentSchoolAssociationEnrollmentTypeConfiguration());
            modelBuilder.Configurations.Add(new StudentSectionAssociationConfiguration());
            modelBuilder.Configurations.Add(new TermDescriptorConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        public static System.Data.Entity.DbModelBuilder CreateModel(System.Data.Entity.DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new CourseOfferingConfiguration(schema));
            modelBuilder.Configurations.Add(new DescriptorConfiguration(schema));
            modelBuilder.Configurations.Add(new EducationOrganizationConfiguration(schema));
            modelBuilder.Configurations.Add(new ElectronicMailTypeDescriptorConfiguration(schema));
            modelBuilder.Configurations.Add(new EntryGradeLevelReasonDescriptorConfiguration(schema));
            modelBuilder.Configurations.Add(new GradeLevelDescriptorConfiguration(schema));
            modelBuilder.Configurations.Add(new LocalEducationAgencyConfiguration(schema));
            modelBuilder.Configurations.Add(new LocationConfiguration(schema));
            modelBuilder.Configurations.Add(new SchoolConfiguration(schema));
            modelBuilder.Configurations.Add(new SchoolCategoryConfiguration(schema));
            modelBuilder.Configurations.Add(new SchoolCategoryDescriptorConfiguration(schema));
            modelBuilder.Configurations.Add(new SchoolGradeLevelConfiguration(schema));
            modelBuilder.Configurations.Add(new SectionConfiguration(schema));
            modelBuilder.Configurations.Add(new SessionConfiguration(schema));
            modelBuilder.Configurations.Add(new StaffConfiguration(schema));
            modelBuilder.Configurations.Add(new StaffElectronicMailConfiguration(schema));
            modelBuilder.Configurations.Add(new StaffSectionAssociationConfiguration(schema));
            modelBuilder.Configurations.Add(new StudentConfiguration(schema));
            modelBuilder.Configurations.Add(new StudentEducationOrganizationAssociationConfiguration(schema));
            modelBuilder.Configurations.Add(new StudentEducationOrganizationAssociationElectronicMailConfiguration(schema));
            modelBuilder.Configurations.Add(new StudentSchoolAssociationConfiguration(schema));
            modelBuilder.Configurations.Add(new StudentSchoolAssociationEnrollmentTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new StudentSectionAssociationConfiguration(schema));
            modelBuilder.Configurations.Add(new TermDescriptorConfiguration(schema));
            OnCreateModelPartial(modelBuilder, schema);
            return modelBuilder;
        }

        partial void InitializePartial();
        partial void DisposePartial(bool disposing);
        partial void OnModelCreatingPartial(System.Data.Entity.DbModelBuilder modelBuilder);
        static partial void OnCreateModelPartial(System.Data.Entity.DbModelBuilder modelBuilder, string schema);
    }
}
// </auto-generated>
