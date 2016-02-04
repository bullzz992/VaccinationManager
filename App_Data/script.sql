USE [master]
GO
/****** Object:  Database [VaccinationManager]    Script Date: 2016/01/29 12:37:22 PM ******/
CREATE DATABASE [VaccinationManager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'VaccinationManager', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\VaccinationManager.mdf' , SIZE = 3136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'VaccinationManager_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\VaccinationManager_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [VaccinationManager] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VaccinationManager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [VaccinationManager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [VaccinationManager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [VaccinationManager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [VaccinationManager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [VaccinationManager] SET ARITHABORT OFF 
GO
ALTER DATABASE [VaccinationManager] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [VaccinationManager] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [VaccinationManager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [VaccinationManager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [VaccinationManager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [VaccinationManager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [VaccinationManager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [VaccinationManager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [VaccinationManager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [VaccinationManager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [VaccinationManager] SET  ENABLE_BROKER 
GO
ALTER DATABASE [VaccinationManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [VaccinationManager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [VaccinationManager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [VaccinationManager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [VaccinationManager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [VaccinationManager] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [VaccinationManager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [VaccinationManager] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [VaccinationManager] SET  MULTI_USER 
GO
ALTER DATABASE [VaccinationManager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [VaccinationManager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [VaccinationManager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [VaccinationManager] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [VaccinationManager]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 2016/01/29 12:37:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Addresses]    Script Date: 2016/01/29 12:37:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addresses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AddressLine1] [nvarchar](max) NULL,
	[AddressLine2] [nvarchar](max) NULL,
	[Suburb] [nvarchar](max) NULL,
	[Town] [nvarchar](max) NULL,
	[PostalCode] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Addresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ages]    Script Date: 2016/01/29 12:37:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ages](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Ages] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Allergies]    Script Date: 2016/01/29 12:37:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Allergies](
	[Code] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Child_IdNumber] [nvarchar](13) NULL,
	[Parent_IdNumber] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.Allergies] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ChildMeasurements]    Script Date: 2016/01/29 12:37:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChildMeasurements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Weight] [float] NOT NULL,
	[Height] [float] NOT NULL,
	[HeadCircumference] [float] NOT NULL,
	[Created] [datetime] NOT NULL,
	[ChildID] [nvarchar](13) NULL,
 CONSTRAINT [PK_dbo.ChildMeasurements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Children]    Script Date: 2016/01/29 12:37:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Children](
	[IdNumber] [nvarchar](13) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Weight] [real] NOT NULL,
	[Height] [real] NOT NULL,
	[HeadCircumference] [real] NOT NULL,
	[BloodType] [int] NOT NULL,
	[MotherId] [nvarchar](max) NULL,
	[FatherId] [nvarchar](max) NULL,
	[ParentViewModel_IdNumber] [nvarchar](128) NULL,
	[Father_IdNumber] [nvarchar](128) NULL,
	[Mother_IdNumber] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.Children] PRIMARY KEY CLUSTERED 
(
	[IdNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Parents]    Script Date: 2016/01/29 12:37:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parents](
	[IdNumber] [nvarchar](128) NOT NULL,
	[Surname] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Telephone] [nvarchar](max) NULL,
	[Cellphone] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[BloodType] [int] NOT NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
	[Address_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Parents] PRIMARY KEY CLUSTERED 
(
	[IdNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserBranches]    Script Date: 2016/01/29 12:37:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserBranches](
	[UserName] [nvarchar](128) NOT NULL,
	[Branch] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.UserBranches] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VaccinationDefinitions]    Script Date: 2016/01/29 12:37:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VaccinationDefinitions](
	[Id] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
	[Age_Code] [int] NULL,
 CONSTRAINT [PK_dbo.VaccinationDefinitions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vaccinations]    Script Date: 2016/01/29 12:37:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vaccinations](
	[Id] [uniqueidentifier] NOT NULL,
	[VaccinationDefinitionId] [uniqueidentifier] NOT NULL,
	[IdNumber] [nvarchar](13) NULL,
	[Administrator] [nvarchar](max) NULL,
	[SerialNumber] [nvarchar](max) NULL,
	[Signature] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Vaccinations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Child_IdNumber]    Script Date: 2016/01/29 12:37:22 PM ******/
CREATE NONCLUSTERED INDEX [IX_Child_IdNumber] ON [dbo].[Allergies]
(
	[Child_IdNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Parent_IdNumber]    Script Date: 2016/01/29 12:37:22 PM ******/
CREATE NONCLUSTERED INDEX [IX_Parent_IdNumber] ON [dbo].[Allergies]
(
	[Parent_IdNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ChildID]    Script Date: 2016/01/29 12:37:22 PM ******/
CREATE NONCLUSTERED INDEX [IX_ChildID] ON [dbo].[ChildMeasurements]
(
	[ChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Father_IdNumber]    Script Date: 2016/01/29 12:37:22 PM ******/
CREATE NONCLUSTERED INDEX [IX_Father_IdNumber] ON [dbo].[Children]
(
	[Father_IdNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Mother_IdNumber]    Script Date: 2016/01/29 12:37:22 PM ******/
CREATE NONCLUSTERED INDEX [IX_Mother_IdNumber] ON [dbo].[Children]
(
	[Mother_IdNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ParentViewModel_IdNumber]    Script Date: 2016/01/29 12:37:22 PM ******/
CREATE NONCLUSTERED INDEX [IX_ParentViewModel_IdNumber] ON [dbo].[Children]
(
	[ParentViewModel_IdNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Address_Id]    Script Date: 2016/01/29 12:37:22 PM ******/
CREATE NONCLUSTERED INDEX [IX_Address_Id] ON [dbo].[Parents]
(
	[Address_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Age_Code]    Script Date: 2016/01/29 12:37:22 PM ******/
CREATE NONCLUSTERED INDEX [IX_Age_Code] ON [dbo].[VaccinationDefinitions]
(
	[Age_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_IdNumber]    Script Date: 2016/01/29 12:37:22 PM ******/
CREATE NONCLUSTERED INDEX [IX_IdNumber] ON [dbo].[Vaccinations]
(
	[IdNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Allergies]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Allergies_dbo.Children_Child_IdNumber] FOREIGN KEY([Child_IdNumber])
REFERENCES [dbo].[Children] ([IdNumber])
GO
ALTER TABLE [dbo].[Allergies] CHECK CONSTRAINT [FK_dbo.Allergies_dbo.Children_Child_IdNumber]
GO
ALTER TABLE [dbo].[Allergies]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Allergies_dbo.Parents_Parent_IdNumber] FOREIGN KEY([Parent_IdNumber])
REFERENCES [dbo].[Parents] ([IdNumber])
GO
ALTER TABLE [dbo].[Allergies] CHECK CONSTRAINT [FK_dbo.Allergies_dbo.Parents_Parent_IdNumber]
GO
ALTER TABLE [dbo].[ChildMeasurements]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ChildMeasurements_dbo.Children_ChildID] FOREIGN KEY([ChildID])
REFERENCES [dbo].[Children] ([IdNumber])
GO
ALTER TABLE [dbo].[ChildMeasurements] CHECK CONSTRAINT [FK_dbo.ChildMeasurements_dbo.Children_ChildID]
GO
ALTER TABLE [dbo].[Children]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Children_dbo.Parents_Father_IdNumber] FOREIGN KEY([Father_IdNumber])
REFERENCES [dbo].[Parents] ([IdNumber])
GO
ALTER TABLE [dbo].[Children] CHECK CONSTRAINT [FK_dbo.Children_dbo.Parents_Father_IdNumber]
GO
ALTER TABLE [dbo].[Children]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Children_dbo.Parents_Mother_IdNumber] FOREIGN KEY([Mother_IdNumber])
REFERENCES [dbo].[Parents] ([IdNumber])
GO
ALTER TABLE [dbo].[Children] CHECK CONSTRAINT [FK_dbo.Children_dbo.Parents_Mother_IdNumber]
GO
ALTER TABLE [dbo].[Children]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Children_dbo.Parents_ParentViewModel_IdNumber] FOREIGN KEY([ParentViewModel_IdNumber])
REFERENCES [dbo].[Parents] ([IdNumber])
GO
ALTER TABLE [dbo].[Children] CHECK CONSTRAINT [FK_dbo.Children_dbo.Parents_ParentViewModel_IdNumber]
GO
ALTER TABLE [dbo].[Parents]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Parents_dbo.Addresses_Address_Id] FOREIGN KEY([Address_Id])
REFERENCES [dbo].[Addresses] ([Id])
GO
ALTER TABLE [dbo].[Parents] CHECK CONSTRAINT [FK_dbo.Parents_dbo.Addresses_Address_Id]
GO
ALTER TABLE [dbo].[VaccinationDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.VaccinationDefinitions_dbo.Ages_Age_Code] FOREIGN KEY([Age_Code])
REFERENCES [dbo].[Ages] ([Code])
GO
ALTER TABLE [dbo].[VaccinationDefinitions] CHECK CONSTRAINT [FK_dbo.VaccinationDefinitions_dbo.Ages_Age_Code]
GO
ALTER TABLE [dbo].[Vaccinations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Vaccinations_dbo.Children_IdNumber] FOREIGN KEY([IdNumber])
REFERENCES [dbo].[Children] ([IdNumber])
GO
ALTER TABLE [dbo].[Vaccinations] CHECK CONSTRAINT [FK_dbo.Vaccinations_dbo.Children_IdNumber]
GO
USE [master]
GO
ALTER DATABASE [VaccinationManager] SET  READ_WRITE 
GO
