CREATE TABLE [dbo].[GcLog]
(	[Id] INT IDENTITY(1,1) NOT NULL,
	[GcId] varchar(200) NOT NULL,
	[Error] BIT NOT NULL CONSTRAINT [GcLog_DF_Error] DEFAULT (0),
	[Message] varchar(1000) NOT NULL,
	[DateLogged] datetime not null CONSTRAINT [GcLog_DF_DateLogged]  DEFAULT (getdate()), 
    CONSTRAINT [PK_GcLog] PRIMARY KEY CLUSTERED (Id)
)
