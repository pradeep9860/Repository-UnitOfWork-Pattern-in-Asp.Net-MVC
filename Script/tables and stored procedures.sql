USE [HRTest_Timesheet]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 11/27/2017 6:21:54 PM ******/
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
/****** Object:  Table [dbo].[Employees]    Script Date: 11/27/2017 6:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Employees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/27/2017 6:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Timesheets]    Script Date: 11/27/2017 6:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Timesheets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CheckInAt] [datetime] NOT NULL,
	[CheckOutAt] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
	[TimeDuration] [bigint] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_dbo.Timesheets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/27/2017 6:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[RoleId] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Employees_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_dbo.Employees_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[Timesheets]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Timesheets_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Timesheets] CHECK CONSTRAINT [FK_dbo.Timesheets_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.Roles_RoleId]
GO
/****** Object:  StoredProcedure [dbo].[ReportList]    Script Date: 11/27/2017 6:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportList]  
as 
 
SELECT x.ui as UserId, z.Firstname as FirstName, z.Lastname as LastName, y.ChkIn as FirstCheckInAt, y.ChkOut LastCheckoutAt, x.TotalDuration as TotalDuration
	FROM 
		(SELECT SUM(ts.TimeDuration) as TotalDuration, ts.UserId as ui FROM dbo.Timesheets as ts, dbo.Users as us GROUP BY ts.UserId) as x, 
		(SELECT min(tms.CheckInAt) as ChkIn, max(tms.CheckOutAt) as ChkOut, tms.UserId as ui FROM dbo.Timesheets as tms GROUP BY tms.UserId) as y,
		(SELECT emp.FirstName as Firstname, emp.LastName as Lastname, emp.UserId as ui FROM dbo.Employees as emp ) as z
	WHERE x.ui = y.ui and y.ui = z.ui
GO
/****** Object:  StoredProcedure [dbo].[ReportListByDateRange]    Script Date: 11/27/2017 6:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportListByDateRange] 
@FromDate Date='1900-01-01',
@ToDate Date
as 
if @FromDate is null
	set @FromDate='1900-01-01';
if @ToDate is null
	set @ToDate=getdate();


SELECT x.ui as UserId, z.Firstname as FirstName, z.Lastname as LastName, y.ChkIn as FirstCheckInAt, y.ChkOut LastCheckoutAt, x.TotalDuration as TotalDuration
	FROM 
		(SELECT SUM(ts.TimeDuration) as TotalDuration, ts.UserId as ui FROM dbo.Timesheets as ts, dbo.Users as us GROUP BY ts.UserId) as x, 
		(SELECT min(tms.CheckInAt) as ChkIn, max(tms.CheckOutAt) as ChkOut, tms.UserId as ui FROM dbo.Timesheets as tms GROUP BY tms.UserId) as y,
		(SELECT emp.FirstName as Firstname, emp.LastName as Lastname, emp.UserId as ui FROM dbo.Employees as emp ) as z
	WHERE x.ui = y.ui and y.ui = z.ui and y.ChkIn BETWEEN @FromDate and @ToDate
GO
/****** Object:  StoredProcedure [dbo].[ReportListForSpecificDate]    Script Date: 11/27/2017 6:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportListForSpecificDate]
@DateAt Date
as 
if @DateAt is null
	set @DateAt=GETDATE();
  

SELECT x.ui as UserId, z.Firstname as FirstName, z.Lastname as LastName, y.ChkIn as FirstCheckInAt, y.ChkOut LastCheckoutAt, x.TotalDuration as TotalDuration
	FROM 
		(SELECT SUM(ts.TimeDuration) as TotalDuration, ts.UserId as ui FROM dbo.Timesheets as ts, dbo.Users as us GROUP BY ts.UserId) as x, 
		(SELECT min(tms.CheckInAt) as ChkIn, max(tms.CheckOutAt) as ChkOut, tms.UserId as ui FROM dbo.Timesheets as tms GROUP BY tms.UserId) as y,
		(SELECT emp.FirstName as Firstname, emp.LastName as Lastname, emp.UserId as ui FROM dbo.Employees as emp ) as z
	WHERE x.ui = y.ui and y.ui = z.ui and YEAR(y.ChkIn) = YEAR(@DateAt) and MONTH(y.ChkIn) = MONTH(@DateAt) and DAY(y.ChkIn) = DAY(@DateAt)

 
GO
