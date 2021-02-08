CREATE TABLE [dbo].[GcCourse]
(
	[EducationOrganizationId] INT NOT NULL,
    [SchoolYear] SMALLINT NOT NULL, 
    [SchoolId] INT NOT NULL, 
    [LocalCourseCode] nvarchar(60) NOT NULL,
    [SessionName] nvarchar(60) NOT NULL,
    [SectionIdentifier] nvarchar(255) NOT NULL,
    [LocalCourseTitle] nvarchar(60) NULL,
    [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(), 
    [GcName]  AS ((((((((CONVERT([varchar],[SchoolId]))+'.')+CONVERT([varchar],[SchoolYear]))+'.')+CONVERT([varchar],[LocalCourseCode]))+'.')+CONVERT([varchar],[SectionIdentifier]))+'.')+CONVERT([varchar],[SessionName]) PERSISTED NOT NULL,
    [Owner] nvarchar(200) NULL,
    [AliasId] nvarchar(200) NULL,
    [Saved] BIT NULL, 
    [Activated] BIT NULL, 
    [GcMessage] VARCHAR(200) NULL, 
    [CourseId] VARCHAR(200) NULL,
    CONSTRAINT [PK_GcCourse] PRIMARY KEY CLUSTERED 
(
	[EducationOrganizationId] ASC, [SchoolYear] ASC, [SchoolId] ASC, [LocalCourseCode] ASC, [SectionIdentifier] ASC, [SessionName] ASC
) 
    

)
