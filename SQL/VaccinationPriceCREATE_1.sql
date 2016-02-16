--USE [VaccinationManager]
--GO

--/****** Object:  Table [dbo].[VaccincationPrice]    Script Date: 2/12/2016 8:55:25 AM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--SET ANSI_PADDING ON
--GO

--CREATE TABLE [dbo].[VaccincationPrice](
--	[VaccinationPriceId] [int] IDENTITY(1,1) NOT NULL,
--	[VaccinationDefId] [varchar](max) NOT NULL,
--	[PriceAmount] [money] NOT NULL,
-- CONSTRAINT [PK_VaccincationPrice] PRIMARY KEY CLUSTERED 
--(
--	[VaccinationPriceId] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

--GO

--SET ANSI_PADDING OFF
--GO