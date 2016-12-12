USE [ATTSAssignment]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Account](
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CurrencyCode] [varchar](50) NOT NULL,
	[Value] [numeric](18, 0) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


