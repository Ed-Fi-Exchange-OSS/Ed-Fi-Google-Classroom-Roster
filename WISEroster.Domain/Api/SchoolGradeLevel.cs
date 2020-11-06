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

    // SchoolGradeLevel
    ///<summary>
    /// The grade levels served at the school.
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class SchoolGradeLevel
    {

        ///<summary>
        /// The grade levels served at the school.
        ///</summary>
        public int GradeLevelDescriptorId { get; set; } // GradeLevelDescriptorId (Primary key)

        ///<summary>
        /// The identifier assigned to a school.
        ///</summary>
        public int SchoolId { get; set; } // SchoolId (Primary key)
        public System.DateTime CreateDate { get; set; } // CreateDate

        // Foreign keys

        /// <summary>
        /// Parent GradeLevelDescriptor pointed by [SchoolGradeLevel].([GradeLevelDescriptorId]) (FK_SchoolGradeLevel_GradeLevelDescriptor)
        /// </summary>
        public GradeLevelDescriptor GradeLevelDescriptor { get; set; } // FK_SchoolGradeLevel_GradeLevelDescriptor

        /// <summary>
        /// Parent School pointed by [SchoolGradeLevel].([SchoolId]) (FK_SchoolGradeLevel_School)
        /// </summary>
        public School School { get; set; } // FK_SchoolGradeLevel_School

        public SchoolGradeLevel()
        {
            CreateDate = System.DateTime.Now;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>