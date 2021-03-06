USE [master]
GO
/****** Object:  Database [Web_Api_Company_Employees]    Script Date: 2018-10-31 21:18:45 ******/
CREATE DATABASE [Web_Api_Company_Employees]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Web_Api_Company_Employees', FILENAME = N'E:\SQL_DB\MSSQL12.SQLEXPRESS\MSSQL\DATA\Web_Api_Company_Employees.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Web_Api_Company_Employees_log', FILENAME = N'E:\SQL_DB\MSSQL12.SQLEXPRESS\MSSQL\DATA\Web_Api_Company_Employees_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Web_Api_Company_Employees] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Web_Api_Company_Employees].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Web_Api_Company_Employees] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET ARITHABORT OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET  MULTI_USER 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Web_Api_Company_Employees] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Web_Api_Company_Employees] SET DELAYED_DURABILITY = DISABLED 
GO
USE [Web_Api_Company_Employees]
GO
/****** Object:  User [Web_Api]    Script Date: 2018-10-31 21:18:45 ******/
CREATE USER [Web_Api] FOR LOGIN [Web_Api] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [Web_Api]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [Web_Api]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 2018-10-31 21:18:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Companies](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[EstablishmentYear] [int] NOT NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 2018-10-31 21:18:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Employees](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[JobTitle] [varchar](50) NOT NULL,
	[CompanyToken] [bigint] NOT NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_WorksIn] FOREIGN KEY([CompanyToken])
REFERENCES [dbo].[Companies] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_WorksIn]
GO
USE [master]
GO
ALTER DATABASE [Web_Api_Company_Employees] SET  READ_WRITE 
GO
