CREATE TABLE [dbo].[RosterLocalCourse]
(
	[RuleId] INT NOT NULL,
	[LocalCourseCode] NVARCHAR(60) NOT NULL, 
	CONSTRAINT PK_RosterLocalCourse PRIMARY KEY (RuleId, LocalCourseCode),	
	CONSTRAINT [FK_RosterLocalCourse_ProvisioningRules] FOREIGN KEY ([RuleId]) REFERENCES [ProvisioningRules]([RuleId]) on delete cascade
)

