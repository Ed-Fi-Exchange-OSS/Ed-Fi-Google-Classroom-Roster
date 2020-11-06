CREATE TABLE [dbo].[OrgGcPreferences]
(
	EducationOrganizationId INT NOT NULL,
	[GcUserEmail] VARCHAR(200) NOT NULL, 
    [AllowExternalDomains] BIT NOT NULL, 
    CONSTRAINT PK_OrgPreferences PRIMARY KEY (EducationOrganizationId)
)
