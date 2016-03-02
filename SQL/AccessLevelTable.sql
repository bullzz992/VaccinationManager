USE [VaccinationManager]
GO

/****** Object:  Table [dbo].[UserStatus]    Script Date: 2016/03/02 09:56:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AccessLevels](
	[AccessLevelID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](200) NULL,
 CONSTRAINT [PK_AccessLevelI] PRIMARY KEY CLUSTERED 
(
	[AccessLevelID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

