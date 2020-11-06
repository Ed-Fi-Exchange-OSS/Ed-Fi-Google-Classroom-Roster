CREATE TABLE [sec].[SecureItems]
(
  [ItemKey]   NVARCHAR(100)   NOT NULL,
  [ItemValue] VARBINARY(2048)  NULL,
  CONSTRAINT [PK_SecureItems] PRIMARY KEY CLUSTERED ([ItemKey] ASC)
);