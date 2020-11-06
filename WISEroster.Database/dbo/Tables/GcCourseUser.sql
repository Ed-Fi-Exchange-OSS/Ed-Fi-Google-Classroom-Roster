CREATE TABLE [dbo].[GcCourseUser]
(
	[EducationOrganizationId] INT NOT NULL,
    [SchoolYear] SMALLINT NOT NULL, 
    [SchoolId] INT NOT NULL, 
    [LocalCourseCode] nvarchar(60) NOT NULL,
    [SessionName] nvarchar(60) NOT NULL,
    [SectionIdentifier] nvarchar(255) NOT NULL,
    [EmailAddress] nvarchar(128) NOT NULL,
    [IsTeacher] bit not null, 
    CONSTRAINT [PK_GcCourseUser] PRIMARY KEY CLUSTERED ([EducationOrganizationId] ASC, [SchoolYear] ASC, [SchoolId] ASC, [LocalCourseCode] ASC, [SectionIdentifier] ASC, [SessionName] ASC, [EmailAddress] ASC) ,
	CONSTRAINT [FK_GcCourse_GcCourseUser] FOREIGN KEY ([EducationOrganizationId],[SchoolYear],[SchoolId],[LocalCourseCode],[SectionIdentifier],[SessionName]) REFERENCES [GcCourse]([EducationOrganizationId],[SchoolYear],[SchoolId],[LocalCourseCode],[SectionIdentifier],[SessionName]) on delete cascade
)
