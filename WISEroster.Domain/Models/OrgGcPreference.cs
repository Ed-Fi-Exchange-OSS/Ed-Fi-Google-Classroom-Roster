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

    // OrgGcPreferences
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class OrgGcPreference
    {
        public int EducationOrganizationId { get; set; } // EducationOrganizationId (Primary key)
        public string GcUserEmail { get; set; } // GcUserEmail (length: 200)
        public bool AllowExternalDomains { get; set; } // AllowExternalDomains

        public OrgGcPreference()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>