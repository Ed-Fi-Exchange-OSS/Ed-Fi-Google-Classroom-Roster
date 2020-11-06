
MERGE INTO [dbo].[ProvisioningRuleType] AS Target 
USING (VALUES 
 (1, 'Course')
,(2, 'Enrollment')
,(3, 'Teacher')
) 
AS Source (TypeId, Name) 
ON Target.TypeId = Source.TypeId 
WHEN MATCHED THEN 
UPDATE SET
	Name = Source.Name
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (TypeId, Name) 
	VALUES (TypeId, Name);