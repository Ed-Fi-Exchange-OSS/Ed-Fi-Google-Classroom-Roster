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

    // TermDescriptor
    ///<summary>
    /// This descriptor defines the term of a session during the school year (e.g., Semester).
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.5.0")]
    public partial class TermDescriptor
    {

        ///<summary>
        /// A unique identifier used as Primary Key, not derived from business logic, when acting as Foreign Key, references the parent table.
        ///</summary>
        public int TermDescriptorId { get; set; } // TermDescriptorId (Primary key)

        // Reverse navigation

        /// <summary>
        /// Child Sessions where [Session].[TermDescriptorId] point to this entity (FK_Session_TermDescriptor)
        /// </summary>
        public System.Collections.Generic.ICollection<Session> Sessions { get; set; } // Session.FK_Session_TermDescriptor

        // Foreign keys

        /// <summary>
        /// Parent Descriptor pointed by [TermDescriptor].([TermDescriptorId]) (FK_TermDescriptor_Descriptor)
        /// </summary>
        public Descriptor Descriptor { get; set; } // FK_TermDescriptor_Descriptor

        public TermDescriptor()
        {
            Sessions = new System.Collections.Generic.List<Session>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
