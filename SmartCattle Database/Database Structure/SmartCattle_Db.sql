USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[ActionControllerListTbl]    Script Date: 9/12/2018 2:19:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[ActionControllerListTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Controller] [nvarchar](max) NULL,
	[Action] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[UniqueId] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[ActionControllerListTbl]    Script Date: 9/12/2018 2:19:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[ActionControllerListTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Controller] [nvarchar](max) NULL,
	[Action] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[UniqueId] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CattleFertilityStateTbl]    Script Date: 9/12/2018 2:21:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CattleFertilityStateTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[description] [nvarchar](max) NULL,
	[cattleId] [int] NULL,
	[date] [datetime2](7) NULL,
	[FarmID] [int] NULL,
	[UserName] [nvarchar](max) NULL,
	[UserIdentity] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CattleGroupTbl]    Script Date: 9/12/2018 2:22:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CattleGroupTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[order] [nvarchar](max) NULL,
	[code] [nvarchar](max) NULL,
	[date] [datetime2](7) NULL,
	[FarmID] [int] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[UserIdentity] [int] NULL,
 CONSTRAINT [PK_SmartCattle.CattleGroupTbl] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CattleHealthStateTbl]    Script Date: 9/12/2018 2:22:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CattleHealthStateTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[description] [nvarchar](max) NULL,
	[cattleId] [int] NOT NULL,
	[date] [datetime2](7) NOT NULL,
	[FarmID] [int] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[UserIdentity] [int] NULL,
 CONSTRAINT [PK_SmartCattle.CattleHealthStateTbl] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [SmartCattle].[CattleHealthStateTbl]  WITH CHECK ADD  CONSTRAINT [FK_SmartCattle.CattleHealthStateTbl_SmartCattle.CattleTbl_cattleId] FOREIGN KEY([cattleId])
REFERENCES [SmartCattle].[CattleTbl] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [SmartCattle].[CattleHealthStateTbl] CHECK CONSTRAINT [FK_SmartCattle.CattleHealthStateTbl_SmartCattle.CattleTbl_cattleId]
GO

ALTER TABLE [SmartCattle].[CattleHealthStateTbl]  WITH CHECK ADD  CONSTRAINT [FK_SmartCattle.CattleHealthStateTbl_SmartCattle.FarmTbl_FarmID] FOREIGN KEY([FarmID])
REFERENCES [SmartCattle].[FarmTbl] ([ID])
GO

ALTER TABLE [SmartCattle].[CattleHealthStateTbl] CHECK CONSTRAINT [FK_SmartCattle.CattleHealthStateTbl_SmartCattle.FarmTbl_FarmID]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CattleHeatStateTbl]    Script Date: 9/12/2018 2:24:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CattleHeatStateTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[description] [nvarchar](max) NULL,
	[cattleId] [int] NOT NULL,
	[date] [datetime2](7) NOT NULL,
	[FarmID] [int] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[UserIdentity] [int] NULL,
 CONSTRAINT [PK_SmartCattle.CattleHeatStateTbl] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [SmartCattle].[CattleHeatStateTbl]  WITH CHECK ADD  CONSTRAINT [FK_SmartCattle.CattleHeatStateTbl_SmartCattle.CattleTbl_cattleId] FOREIGN KEY([cattleId])
REFERENCES [SmartCattle].[CattleTbl] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [SmartCattle].[CattleHeatStateTbl] CHECK CONSTRAINT [FK_SmartCattle.CattleHeatStateTbl_SmartCattle.CattleTbl_cattleId]
GO

ALTER TABLE [SmartCattle].[CattleHeatStateTbl]  WITH CHECK ADD  CONSTRAINT [FK_SmartCattle.CattleHeatStateTbl_SmartCattle.FarmTbl_FarmID] FOREIGN KEY([FarmID])
REFERENCES [SmartCattle].[FarmTbl] ([ID])
GO

ALTER TABLE [SmartCattle].[CattleHeatStateTbl] CHECK CONSTRAINT [FK_SmartCattle.CattleHeatStateTbl_SmartCattle.FarmTbl_FarmID]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CattleHerds]    Script Date: 9/12/2018 2:25:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CattleHerds](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[FarmID] [int] NULL,
	[UserName] [nvarchar](max) NULL,
	[UserIdentity] [int] NULL,
	[date] [datetime2](7) NULL,
 CONSTRAINT [PK_SmartCattle.CattleHerds] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CattleNotificationsSetting]    Script Date: 9/12/2018 2:26:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CattleNotificationsSetting](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FarmId] [int] NULL,
	[GroupName] [nvarchar](max) NULL,
	[Topic] [nvarchar](max) NULL,
	[Roles] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[PeroidTime] [float] NULL,
	[WindowTime] [float] NULL,
	[CattleTempMin] [float] NULL,
	[CattleTempMax] [float] NULL,
	[SittingMin] [float] NULL,
	[SittingMax] [float] NULL,
	[WalkingMin] [float] NULL,
	[WalkingMax] [float] NULL,
	[RuminationMin] [float] NULL,
	[RuminationMax] [float] NULL,
	[DrinkingMin] [float] NULL,
	[DrinkingMax] [float] NULL,
	[EatingMin] [float] NULL,
	[EatingMax] [float] NULL,
	[StandingMin] [float] NULL,
	[StandingMax] [float] NULL,
	[CreateDate] [datetime2](7) NULL,
	[ActivationState] [nvarchar](max) NULL,
	[SnoozeTime] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CattlePositionTbl]    Script Date: 9/12/2018 2:26:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CattlePositionTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[cattleId] [int] NOT NULL,
	[date] [datetime2](7) NOT NULL,
	[LastRecievedId] [bigint] NOT NULL,
	[FarmID] [int] NOT NULL,
	[FreeStall] [int] NOT NULL,
 CONSTRAINT [PK_SmartCattle.CattlePositionTbl] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [SmartCattle].[CattlePositionTbl]  WITH CHECK ADD  CONSTRAINT [FK_SmartCattle.CattlePositionTbl_SmartCattle.CattleTbl_cattleId] FOREIGN KEY([cattleId])
REFERENCES [SmartCattle].[CattleTbl] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [SmartCattle].[CattlePositionTbl] CHECK CONSTRAINT [FK_SmartCattle.CattlePositionTbl_SmartCattle.CattleTbl_cattleId]
GO

ALTER TABLE [SmartCattle].[CattlePositionTbl]  WITH CHECK ADD  CONSTRAINT [FK_SmartCattle.CattlePositionTbl_SmartCattle.FarmTbl_FarmID] FOREIGN KEY([FarmID])
REFERENCES [SmartCattle].[FarmTbl] ([ID])
GO

ALTER TABLE [SmartCattle].[CattlePositionTbl] CHECK CONSTRAINT [FK_SmartCattle.CattlePositionTbl_SmartCattle.FarmTbl_FarmID]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CattleScoreTbl]    Script Date: 9/12/2018 2:26:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CattleScoreTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[item] [nvarchar](max) NOT NULL,
	[value] [float] NOT NULL,
	[CattleId] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[UserId] [int] NULL,
	[UserName] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CattleTbl]    Script Date: 9/12/2018 2:27:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CattleTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[age] [int] NULL,
	[preg] [int] NULL,
	[milkAvg] [float] NULL,
	[milkAvgDate] [datetime2](7) NULL,
	[healthStatus] [nvarchar](max) NULL,
	[healthStatusDate] [datetime2](7) NULL,
	[animalNumber] [int] NULL,
	[heatStatus] [nvarchar](max) NULL,
	[heatStatusDate] [datetime2](7) NULL,
	[birthDate] [datetime2](7) NULL,
	[Dim] [int] NULL,
	[fertilityStatus] [nvarchar](max) NULL,
	[fertilityStatusDate] [datetime2](7) NULL,
	[lactationNumber] [int] NULL,
	[InseminationCount] [int] NULL,
	[lastInseminationDate] [datetime2](7) NULL,
	[lastCalvingDate] [datetime2](7) NULL,
	[calvingCount] [int] NULL,
	[CattleGroupId] [int] NULL,
	[FreeStallId] [int] NULL,
	[FarmID] [int] NULL,
	[UserId] [nvarchar](128) NULL,
	[CattleHerd_ID] [int] NULL,
	[Sex] [nvarchar](max) NULL,
	[MotherID] [int] NULL,
	[Genetics_type_num] [nvarchar](max) NULL,
	[Body_Condition_Score] [float] NULL,
	[Body_Condition_ScoreDate] [datetime2](7) NULL,
	[Cleanliness] [float] NULL,
	[CleanlinessDate] [datetime2](7) NULL,
	[Hock] [float] NULL,
	[HockDate] [datetime2](7) NULL,
	[Mobility] [float] NULL,
	[MobilityDate] [datetime2](7) NULL,
	[Manure] [float] NULL,
	[ManureDate] [datetime2](7) NULL,
	[Rumen] [float] NULL,
	[RumenDate] [datetime2](7) NULL,
	[Teat] [float] NULL,
	[TeatDate] [datetime2](7) NULL,
	[Name] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_SmartCattle.CattleTbl] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CattleTransfer]    Script Date: 9/12/2018 2:27:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CattleTransfer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CattleID] [int] NULL,
	[Topic] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[NewTopicID] [int] NULL,
	[OldTopicID] [int] NULL,
	[NewTopicName] [nvarchar](max) NULL,
	[OldTopicName] [nvarchar](max) NULL,
	[FarmID] [int] NULL,
	[date] [datetime2](7) NULL,
	[UserName] [nvarchar](max) NULL,
	[UserIdentity] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CattleVetTbl]    Script Date: 9/12/2018 2:28:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CattleVetTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](max) NULL,
	[description] [nvarchar](max) NULL,
	[cattleId] [int] NULL,
	[date] [datetime2](7) NULL,
	[FarmID] [int] NULL,
	[UserName] [nvarchar](max) NULL,
	[UserIdentity] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CitiesTbl]    Script Date: 9/12/2018 2:28:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CitiesTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[region_id] [int] NULL,
	[country_id] [int] NULL,
	[latitude] [nvarchar](max) NULL,
	[longitude] [nvarchar](max) NULL,
	[name] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[CountryAndCity]    Script Date: 9/12/2018 2:28:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[CountryAndCity](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CountryFa] [nvarchar](max) NULL,
	[CityFa] [nvarchar](max) NULL,
	[CountryEn] [nvarchar](max) NULL,
	[CityEn] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[EnvSensors]    Script Date: 9/12/2018 2:29:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[EnvSensors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[FreeStallId] [int] NULL,
	[FarmId] [int] NULL,
	[Lat] [float] NULL,
	[Lng] [float] NULL,
	[MAC] [nvarchar](50) NULL
) ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[EnvTHITbl]    Script Date: 9/12/2018 2:29:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[EnvTHITbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FarmID] [int] NOT NULL,
	[FreeStallId] [int] NULL,
	[TdbValue] [float] NOT NULL,
	[RHValue] [float] NOT NULL,
	[THIValue] [float] NOT NULL,
	[SensorLat] [float] NULL,
	[SensorLng] [float] NULL,
	[LastId] [int] NULL,
	[MAC] [nchar](50) NULL,
	[date] [datetime2](7) NOT NULL
) ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[EquipmentTbl]    Script Date: 9/12/2018 2:29:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[EquipmentTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceCategory] [nvarchar](max) NULL,
	[projectName] [nvarchar](max) NULL,
	[subId] [int] NULL,
	[FarmId] [int] NULL,
	[DeviceType] [nvarchar](max) NULL,
	[DeviceSubtype] [nvarchar](max) NULL,
	[PacketType] [nvarchar](max) NULL,
	[Version] [nvarchar](max) NULL,
	[PowerType] [nvarchar](max) NULL,
	[Equipmentid] [int] NULL,
	[Mac] [nvarchar](max) NULL,
	[Projectid] [nvarchar](max) NULL,
	[Subprojectid] [nvarchar](max) NULL,
	[Zoneid] [nvarchar](max) NULL,
	[Locationx] [nvarchar](max) NULL,
	[Locationy] [nvarchar](max) NULL,
	[Locationz] [nvarchar](max) NULL,
	[Date1] [nvarchar](max) NULL,
	[Date2] [nvarchar](max) NULL,
	[Reserved1] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[ExceptionTbl]    Script Date: 9/12/2018 2:29:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[ExceptionTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[Date] [datetime2](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[FarmTbl]    Script Date: 9/12/2018 2:30:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[FarmTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SubprojectID] [int] NULL,
	[FarmName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Latitude] [nvarchar](max) NULL,
	[Longitude] [nvarchar](max) NULL,
	[Country] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[Province] [nvarchar](max) NULL,
	[No] [nvarchar](max) NULL,
	[Street1] [nvarchar](max) NULL,
	[Street2] [nvarchar](max) NULL,
	[PostalCode] [nvarchar](max) NULL,
	[Phone1] [nvarchar](max) NULL,
	[Phone2] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NULL,
	[FarmTypeUId] [nvarchar](max) NULL,
	[FarmTypeId] [int] NULL,
 CONSTRAINT [PK_SmartCattle.FarmTbl] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[FreeStallNotificationsSetting]    Script Date: 9/12/2018 2:30:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[FreeStallNotificationsSetting](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FarmId] [int] NULL,
	[GroupName] [nvarchar](max) NULL,
	[Topic] [nvarchar](max) NULL,
	[Roles] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[PeroidTime] [int] NULL,
	[WindowTime] [int] NULL,
	[TempMin] [float] NULL,
	[TempMax] [float] NULL,
	[HumMin] [float] NULL,
	[HumMax] [float] NULL,
	[THIMin] [float] NULL,
	[THIMax] [float] NULL,
	[CreateDate] [datetime2](7) NULL,
	[ActivationState] [nvarchar](max) NULL,
	[SnoozeTime] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[FreeStallTbl]    Script Date: 9/12/2018 2:30:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[FreeStallTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[ServerName] [nvarchar](max) NULL,
	[FarmID] [int] NULL,
	[GroupID] [int] NULL,
	[UserId] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[NotificationsTable]    Script Date: 9/12/2018 2:31:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[NotificationsTable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[uId] [nvarchar](max) NULL,
	[Topic] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[FarmID] [int] NULL,
	[RoleName] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[Status] [nvarchar](max) NULL,
	[NotificationType] [nvarchar](max) NULL,
	[Snooze] [int] NULL,
	[TagName] [nvarchar](max) NULL,
	[SnoozeMsg] [nvarchar](max) NULL,
	[Username] [nvarchar](max) NULL,
	[Cattle_Freestall_Id] [int] NULL,
	[NotificationGroup] [nvarchar](max) NULL,
	[Act] [nvarchar](max) NULL,
	[DeactiveAt] [datetime2](7) NULL,
	[ActDate] [datetime2](7) NULL,
	[ActionComment] [nvarchar](max) NULL,
	[Deactive] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[RolesList_FarmTbl]    Script Date: 9/12/2018 2:31:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[RolesList_FarmTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[jName] [nvarchar](max) NULL,
	[Permissions] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[uId] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[RolesList_StaffTbl]    Script Date: 9/12/2018 2:31:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[RolesList_StaffTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[jName] [nvarchar](max) NULL,
	[Permissions] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[uId] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[RolesList_UserTbl]    Script Date: 9/12/2018 2:32:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[RolesList_UserTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[jName] [nvarchar](max) NULL,
	[Permissions] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[FarmId] [int] NULL,
	[uId] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[SensorTbl]    Script Date: 9/12/2018 2:32:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[SensorTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MacAddress] [nvarchar](max) NULL,
	[cattleId] [int] NULL,
	[FarmID] [int] NULL,
 CONSTRAINT [PK_SmartCattle.SensorTbl] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [SmartCattle].[SensorTbl]  WITH CHECK ADD  CONSTRAINT [FK_SmartCattle.SensorTbl_SmartCattle.FarmTbl_FarmID] FOREIGN KEY([FarmID])
REFERENCES [SmartCattle].[FarmTbl] ([ID])
GO

ALTER TABLE [SmartCattle].[SensorTbl] CHECK CONSTRAINT [FK_SmartCattle.SensorTbl_SmartCattle.FarmTbl_FarmID]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[TempretureTbl]    Script Date: 9/12/2018 2:32:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[TempretureTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[value] [float] NOT NULL,
	[point] [nvarchar](max) NULL,
	[cattleId] [int] NOT NULL,
	[date] [datetime2](7) NOT NULL,
	[LastRecievedId] [bigint] NOT NULL,
	[FarmID] [int] NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[FreeStall] [int] NULL,
	[dateStr] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[UserInfo]    Script Date: 9/12/2018 2:32:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[UserInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Family] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[RoleId] [nvarchar](max) NULL,
	[FarmId] [int] NULL,
	[FarmIdList] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NULL,
	[RoleName] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[ActivityStateTbl]    Script Date: 9/15/2018 10:51:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[ActivityStateTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[jsonedActivities] [nvarchar](max) NULL,
	[Sitting] [decimal](18, 2) NOT NULL,
	[Standing] [decimal](18, 2) NOT NULL,
	[Walking] [decimal](18, 2) NOT NULL,
	[Eating] [decimal](18, 2) NOT NULL,
	[Rumination] [decimal](18, 2) NOT NULL,
	[Drinking] [decimal](18, 2) NOT NULL,
	[cattleId] [int] NOT NULL,
	[date] [datetime2](7) NOT NULL,
	[FarmID] [int] NOT NULL,
	[LastRecievedId] [bigint] NULL,
	[UserId] [nvarchar](max) NULL,
 CONSTRAINT [PK_SmartCattle.ActivityStateTbl] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [SmartCattle].[ActivityStateTbl]  WITH CHECK ADD  CONSTRAINT [FK_SmartCattle.ActivityStateTbl_SmartCattle.FarmTbl_FarmID] FOREIGN KEY([FarmID])
REFERENCES [SmartCattle].[FarmTbl] ([ID])
GO

ALTER TABLE [SmartCattle].[ActivityStateTbl] CHECK CONSTRAINT [FK_SmartCattle.ActivityStateTbl_SmartCattle.FarmTbl_FarmID]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[NotificationsTable]    Script Date: 9/16/2018 1:28:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[NotificationsTable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[uId] [nvarchar](max) NULL,
	[Topic] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[FarmID] [int] NULL,
	[RoleName] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[Status] [nvarchar](max) NULL,
	[NotificationType] [nvarchar](max) NULL,
	[Snooze] [int] NULL,
	[TagName] [nvarchar](max) NULL,
	[SnoozeMsg] [nvarchar](max) NULL,
	[Username] [nvarchar](max) NULL,
	[Cattle_Freestall_Id] [int] NULL,
	[NotificationGroup] [nvarchar](max) NULL,
	[Act] [nvarchar](max) NULL,
	[DeactiveAt] [datetime2](7) NULL,
	[ActDate] [datetime2](7) NULL,
	[ActionComment] [nvarchar](max) NULL,
	[Deactive] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[AspNetUsers]    Script Date: 9/17/2018 11:58:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[firstName] [nvarchar](max) NULL,
	[lastName] [nvarchar](max) NULL,
	[lastLogin] [datetime2](7) NULL,
	[avatarUrl] [nvarchar](max) NULL,
	[phone] [nvarchar](max) NULL,
	[FarmID] [int] NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime2](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[NameFamily] [nvarchar](max) NULL,
 CONSTRAINT [PK_SmartCattle.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [SmartCattle].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_SmartCattle.AspNetUsers_SmartCattle.FarmTbl_FarmID] FOREIGN KEY([FarmID])
REFERENCES [SmartCattle].[FarmTbl] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [SmartCattle].[AspNetUsers] CHECK CONSTRAINT [FK_SmartCattle.AspNetUsers_SmartCattle.FarmTbl_FarmID]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[ActivityStateTbl]    Script Date: 9/17/2018 4:43:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[ActivityStateTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[jsonedActivities] [nvarchar](max) NULL,
	[Sitting] [decimal](18, 2) NOT NULL,
	[Standing] [decimal](18, 2) NOT NULL,
	[Walking] [decimal](18, 2) NOT NULL,
	[Eating] [decimal](18, 2) NOT NULL,
	[Rumination] [decimal](18, 2) NOT NULL,
	[Drinking] [decimal](18, 2) NOT NULL,
	[cattleId] [int] NOT NULL,
	[date] [datetime2](7) NOT NULL,
	[FarmID] [int] NOT NULL,
	[LastRecievedId] [bigint] NULL,
	[UserId] [nvarchar](max) NULL,
 CONSTRAINT [PK_SmartCattle.ActivityStateTbl] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [SmartCattle].[ActivityStateTbl]  WITH CHECK ADD  CONSTRAINT [FK_SmartCattle.ActivityStateTbl_SmartCattle.FarmTbl_FarmID] FOREIGN KEY([FarmID])
REFERENCES [SmartCattle].[FarmTbl] ([ID])
GO

ALTER TABLE [SmartCattle].[ActivityStateTbl] CHECK CONSTRAINT [FK_SmartCattle.ActivityStateTbl_SmartCattle.FarmTbl_FarmID]
GO

USE [smartCattle]
GO

/****** Object:  Table [SmartCattle].[BatteryLevelTbl]    Script Date: 10/9/2018 1:42:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SmartCattle].[BatteryLevelTbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MacAddress] [nvarchar](max) NULL,
	[BatteryLevel] [int] NULL,
	[Date] [datetime2](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

