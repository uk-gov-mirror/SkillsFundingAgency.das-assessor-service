﻿CREATE TABLE [dbo].[Certificates](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
	[CertificateData] [nvarchar](max) NOT NULL,
	[ToBePrinted] [datetime2](7) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[DeletedAt] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[CertificateReference] NVARCHAR(50) NOT NULL,
	[OrganisationId] [uniqueidentifier] NOT NULL,
	[BatchNumber] [int] NULL,
	[Status] [nvarchar](20) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](256) NULL, 
    [Uln] BIGINT NOT NULL, 
    [StandardCode] INT NOT NULL, 
    [ProviderUkPrn] INT NOT NULL, 
    [CertificateReferenceId] INT NOT NULL IDENTITY(10001,1), 
	[LearnRefNumber] NVARCHAR(12) NULL,
	[CreateDay] DATE NOT NULL,
	[IsPrivatelyFunded] BIT, 
	[PrivatelyFundedStatus] NVARCHAR(20) NULL, 
    [StandardUId] VARCHAR(20)  NULL ,
    CONSTRAINT [PK_Certificates] PRIMARY KEY ([Id]),
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Certificates]  ADD  CONSTRAINT [FK_Certificates_Organisations_OrganisationId] FOREIGN KEY([OrganisationId])
REFERENCES [dbo].[Organisations] ([Id]);
GO
 ALTER TABLE [dbo].[Certificates] CHECK CONSTRAINT [FK_Certificates_Organisations_OrganisationId]
GO

ALTER TABLE [dbo].[Certificates]  ADD  CONSTRAINT [FK_Certificates_CertificateBatchLogs] FOREIGN KEY([CertificateReference], [BatchNumber])
REFERENCES [dbo].[CertificateBatchLogs] ([CertificateReference], [BatchNumber])
GO

ALTER TABLE [dbo].[Certficates] CHECK CONSTRAINT [FK_Certificates_CertificateBatchLogs]
GO

CREATE UNIQUE INDEX [IXU_Certificates] ON [Certificates] ([Uln], [StandardCode])
GO

CREATE INDEX [IX_Certificates_CreateDay] ON [Certificates] ([CreateDay]) INCLUDE ([Status], [CertificateData])
GO

CREATE INDEX [IX_Certificates_CertificateReference] ON [Certificates] ([CertificateReference]) INCLUDE ([Id])
GO
