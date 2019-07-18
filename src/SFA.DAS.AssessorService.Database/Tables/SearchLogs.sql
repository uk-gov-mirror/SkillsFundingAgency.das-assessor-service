﻿CREATE TABLE [dbo].[SearchLogs]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [Surname] NVARCHAR(50) NOT NULL, 
    [Uln] BIGINT NOT NULL, 
    [SearchTime] DATETIME2 NOT NULL, 
	[SearchData] NVARCHAR(MAX) NULL,
    [NumberOfResults] INT NOT NULL, 
    [Username] NVARCHAR(256) NOT NULL
)
