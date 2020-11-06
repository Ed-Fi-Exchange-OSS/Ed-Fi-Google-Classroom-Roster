CREATE TABLE [dbo].[RosterGradeLevel]
(
	[GradeLevelDescriptorId] INT NOT NULL , 
    [RuleId] INT NOT NULL,
	CONSTRAINT PK_RosterGradeLevel PRIMARY KEY (RuleId, GradeLevelDescriptorId),
	CONSTRAINT [FK_RosterGradeLevel_ProvisioningRules] FOREIGN KEY ([RuleId]) REFERENCES [ProvisioningRules]([RuleId]) on delete cascade
)
