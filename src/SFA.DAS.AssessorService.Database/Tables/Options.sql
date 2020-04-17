﻿CREATE TABLE [dbo].[Options]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [StdCode] INT NOT NULL, 
    [OptionName] NVARCHAR(500) NOT NULL, 
    [DateAdded] DATETIME NOT NULL DEFAULT GETUTCDATE(),
	[DateUpdated] DATETIME NULL,
	[DateRemoved] DATETIME NULL,
	[IsLive] BIT NOT NULL DEFAULT 1
)

GO

CREATE INDEX [IX_Options_LarsCode] ON [dbo].[Options] ([StdCode])
