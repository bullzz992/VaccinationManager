USE [VaccinationManager]
GO

/****** Object:  Table [dbo].[UserStatus]    Script Date: 2016/03/02 10:20:13 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserStatus](
	[UserStatusID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [varchar](200) NOT NULL,
	[UserFullname] [varchar](200) NULL,
	[AccessLevelID] [int] NOT NULL,
 CONSTRAINT [PK_UserStatus] PRIMARY KEY CLUSTERED 
(
	[UserStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


