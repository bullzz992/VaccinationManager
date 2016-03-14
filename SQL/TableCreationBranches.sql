USE [VaccinationManager]
GO

/****** Object:  Table [dbo].[Branches]    Script Date: 3/14/2016 6:42:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Branches](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Overseer_Name] [varchar](50) NOT NULL,
	[Overseer_Surname] [varchar](50) NOT NULL,
	[Practice_No] [varchar](50) NOT NULL,
	[Tel_Number] [varchar](20) NULL,
	[Fax_Number] [varchar](20) NULL,
	[Email_Address] [varchar](50) NOT NULL,
	[Bank_Name] [varchar](50) NULL,
	[Branch_Number] [varchar](20) NULL,
	[Account_Number] [varchar](30) NULL,
	[Address] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


