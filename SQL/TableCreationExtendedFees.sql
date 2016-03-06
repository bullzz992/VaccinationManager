USE [VaccinationManager]
GO

/****** Object:  Table [dbo].[ExtendedFees]    Script Date: 3/6/2016 5:06:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ExtendedFees](
	[FeeId] [int] IDENTITY(1,1) NOT NULL,
	[FeeName] [varchar](100) NOT NULL,
	[FeeDescription] [varchar](200) NOT NULL,
	[Amount] [money] NOT NULL,
 CONSTRAINT [PK_ExtendedFees] PRIMARY KEY CLUSTERED 
(
	[FeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


