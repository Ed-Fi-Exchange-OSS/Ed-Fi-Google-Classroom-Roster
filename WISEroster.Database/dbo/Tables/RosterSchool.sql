CREATE TABLE [dbo].[RosterSchool]
(
	[RuleId] INT NOT NULL , 
    [SchoolId] INT NOT NULL,
	CONSTRAINT PK_RosterSchool PRIMARY KEY (RuleId, SchoolId),
	CONSTRAINT [FK_RosterSchool_ProvisioningRules] FOREIGN KEY ([RuleId]) REFERENCES [ProvisioningRules]([RuleId]) on delete cascade
)

