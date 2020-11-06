CREATE TABLE [dbo].[ProvisioningRules]
(
	[RuleId] INT NOT NULL IDENTITY(1,1), 
    [EducationOrganizationId] INT NOT NULL,
    [SchoolYear] SMALLINT NOT NULL, 
    [TypeId] INT NOT NULL CONSTRAINT [ProvisioningRules_DF_TypeId]  DEFAULT (1), 
    [SchoolCategoryTypeId] INT NULL, 
    [SessionName] NVARCHAR(60) NULL,
    [StaffUSI] INT NULL,
    [IncludeExclude] BIT NULL, 
    [StaffOnly] BIT NOT NULL, 
    [GroupByTitle] BIT NULL, 
    [CreateDate] DATETIME NOT NULL CONSTRAINT [ProvisioningRules_DF_CreateDate] DEFAULT GETDATE(), 
 CONSTRAINT [PK_ProvisioningRules] PRIMARY KEY CLUSTERED 
(
	[RuleId] ASC
)
)