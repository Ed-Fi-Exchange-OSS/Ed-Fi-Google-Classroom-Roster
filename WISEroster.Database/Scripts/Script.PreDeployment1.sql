IF COL_LENGTH('dbo.GcCourse', 'GcId') IS NOT NULL
EXEC sp_rename 'dbo.GcCourse.GcId', 'AliasId', 'COLUMN';