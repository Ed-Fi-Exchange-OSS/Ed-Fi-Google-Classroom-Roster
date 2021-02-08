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

    using System.Linq;

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class WISErosterDbContext : System.Data.Entity.DbContext, IWISErosterDbContext
    {
        public System.Data.Entity.DbSet<GcCourse> GcCourses { get; set; } // GcCourse
        public System.Data.Entity.DbSet<GcCourseUser> GcCourseUsers { get; set; } // GcCourseUser
        public System.Data.Entity.DbSet<GcLog> GcLogs { get; set; } // GcLog
        public System.Data.Entity.DbSet<OrgGcPreference> OrgGcPreferences { get; set; } // OrgGcPreferences
        public System.Data.Entity.DbSet<ProvisioningRule> ProvisioningRules { get; set; } // ProvisioningRules
        public System.Data.Entity.DbSet<ProvisioningRuleType> ProvisioningRuleTypes { get; set; } // ProvisioningRuleType
        public System.Data.Entity.DbSet<RosterGradeLevel> RosterGradeLevels { get; set; } // RosterGradeLevel
        public System.Data.Entity.DbSet<RosterLocalCourse> RosterLocalCourses { get; set; } // RosterLocalCourse
        public System.Data.Entity.DbSet<RosterSchool> RosterSchools { get; set; } // RosterSchool
        public System.Data.Entity.DbSet<SecureItem> SecureItems { get; set; } // SecureItems

        static WISErosterDbContext()
        {
            System.Data.Entity.Database.SetInitializer<WISErosterDbContext>(null);
        }

        public WISErosterDbContext()
            : base("Name=WISErosterDbContext")
        {
            InitializePartial();
        }

        public WISErosterDbContext(string connectionString)
            : base(connectionString)
        {
            InitializePartial();
        }

        public WISErosterDbContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model)
            : base(connectionString, model)
        {
            InitializePartial();
        }

        public WISErosterDbContext(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            InitializePartial();
        }

        public WISErosterDbContext(System.Data.Common.DbConnection existingConnection, System.Data.Entity.Infrastructure.DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            InitializePartial();
        }

        public WISErosterDbContext(System.Data.Entity.Core.Objects.ObjectContext objectContext, bool dbContextOwnsObjectContext)
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

            modelBuilder.Configurations.Add(new GcCourseConfiguration());
            modelBuilder.Configurations.Add(new GcCourseUserConfiguration());
            modelBuilder.Configurations.Add(new GcLogConfiguration());
            modelBuilder.Configurations.Add(new OrgGcPreferenceConfiguration());
            modelBuilder.Configurations.Add(new ProvisioningRuleConfiguration());
            modelBuilder.Configurations.Add(new ProvisioningRuleTypeConfiguration());
            modelBuilder.Configurations.Add(new RosterGradeLevelConfiguration());
            modelBuilder.Configurations.Add(new RosterLocalCourseConfiguration());
            modelBuilder.Configurations.Add(new RosterSchoolConfiguration());
            modelBuilder.Configurations.Add(new SecureItemConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        public static System.Data.Entity.DbModelBuilder CreateModel(System.Data.Entity.DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new GcCourseConfiguration(schema));
            modelBuilder.Configurations.Add(new GcCourseUserConfiguration(schema));
            modelBuilder.Configurations.Add(new GcLogConfiguration(schema));
            modelBuilder.Configurations.Add(new OrgGcPreferenceConfiguration(schema));
            modelBuilder.Configurations.Add(new ProvisioningRuleConfiguration(schema));
            modelBuilder.Configurations.Add(new ProvisioningRuleTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new RosterGradeLevelConfiguration(schema));
            modelBuilder.Configurations.Add(new RosterLocalCourseConfiguration(schema));
            modelBuilder.Configurations.Add(new RosterSchoolConfiguration(schema));
            modelBuilder.Configurations.Add(new SecureItemConfiguration(schema));
            OnCreateModelPartial(modelBuilder, schema);
            return modelBuilder;
        }

        partial void InitializePartial();
        partial void DisposePartial(bool disposing);
        partial void OnModelCreatingPartial(System.Data.Entity.DbModelBuilder modelBuilder);
        static partial void OnCreateModelPartial(System.Data.Entity.DbModelBuilder modelBuilder, string schema);

        // Stored Procedures
        public int SecureItems_Delete(string itemKey)
        {
            var itemKeyParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@ItemKey", SqlDbType = System.Data.SqlDbType.NVarChar, Direction = System.Data.ParameterDirection.Input, Value = itemKey, Size = 100 };
            if (itemKeyParam.Value == null)
                itemKeyParam.Value = System.DBNull.Value;

            var procResultParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@procResult", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };

            Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, "EXEC @procResult = [sec].[SecureItems_Delete] @ItemKey", itemKeyParam, procResultParam);

            return (int) procResultParam.Value;
        }

        public System.Collections.Generic.List<SecureItems_ReadReturnModel> SecureItems_Read(string itemKey)
        {
            int procResult;
            return SecureItems_Read(itemKey, out procResult);
        }

        public System.Collections.Generic.List<SecureItems_ReadReturnModel> SecureItems_Read(string itemKey, out int procResult)
        {
            var itemKeyParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@ItemKey", SqlDbType = System.Data.SqlDbType.NVarChar, Direction = System.Data.ParameterDirection.Input, Value = itemKey, Size = 100 };
            if (itemKeyParam.Value == null)
                itemKeyParam.Value = System.DBNull.Value;

            var procResultParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@procResult", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
            var procResultData = Database.SqlQuery<SecureItems_ReadReturnModel>("EXEC @procResult = [sec].[SecureItems_Read] @ItemKey", itemKeyParam, procResultParam).ToList();

            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<SecureItems_ReadReturnModel>> SecureItems_ReadAsync(string itemKey)
        {
            var itemKeyParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@ItemKey", SqlDbType = System.Data.SqlDbType.NVarChar, Direction = System.Data.ParameterDirection.Input, Value = itemKey, Size = 100 };
            if (itemKeyParam.Value == null)
                itemKeyParam.Value = System.DBNull.Value;

            var procResultData = await Database.SqlQuery<SecureItems_ReadReturnModel>("EXEC [sec].[SecureItems_Read] @ItemKey", itemKeyParam).ToListAsync();

            return procResultData;
        }

        public int SecureItems_Upsert(string itemKey, string itemValue)
        {
            var itemKeyParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@ItemKey", SqlDbType = System.Data.SqlDbType.NVarChar, Direction = System.Data.ParameterDirection.Input, Value = itemKey, Size = 100 };
            if (itemKeyParam.Value == null)
                itemKeyParam.Value = System.DBNull.Value;

            var itemValueParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@ItemValue", SqlDbType = System.Data.SqlDbType.NVarChar, Direction = System.Data.ParameterDirection.Input, Value = itemValue, Size = 1024 };
            if (itemValueParam.Value == null)
                itemValueParam.Value = System.DBNull.Value;

            var procResultParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@procResult", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };

            Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, "EXEC @procResult = [sec].[SecureItems_Upsert] @ItemKey, @ItemValue", itemKeyParam, itemValueParam, procResultParam);

            return (int) procResultParam.Value;
        }

    }
}
// </auto-generated>
