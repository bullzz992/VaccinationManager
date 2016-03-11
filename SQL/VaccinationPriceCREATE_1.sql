USE [VaccinationManager]
GO

/****** Object:  Table [dbo].[VaccinationPrices]    Script Date: 3/11/2016 12:19:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VaccinationPrices](
	[VaccinationPriceId] [int] IDENTITY(1,1) NOT NULL,
	[VaccinationDefId] [nvarchar](max) NULL,
	[PriceAmount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_dbo.VaccinationPrices] PRIMARY KEY CLUSTERED 
(
	[VaccinationPriceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


