USE [smartCattle]
GO
/****** Object:  StoredProcedure [dbo].[AssignPartialView]    Script Date: 9/23/2018 3:08:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	
ALTER PROCEDURE [dbo].[AssignPartialView]

as
begin

	IF NOT EXISTS (
		  SELECT * 
		  FROM   sys.columns 
		  WHERE  object_id = OBJECT_ID(N'[SmartCattle].[ActionControllerListTbl]') 
				 AND name = 'PartialView'
	)
	begin 
		ALTER TABLE ActionControllerListTbl ADD PartialView BIT		
	end

	IF OBJECT_ID('tempdb..#asc') IS NOT NULL
		DROP TABLE #asc

	IF OBJECT_ID('tempdb..#tempList') IS NOT NULL
		DROP TABLE #tempList


	update smartCattle.ActionControllerListTbl set PartialView=0

	declare @StrList varchar(max) = 'Cattle_Detail,Cattle_CattleEvent,Cattle_CattleScore,Cattle_getSpecTemperature,Cattle_getSpecActivity,Cattle_CattlePosition,FreeStall_getEncryptedValue
		  ,Cattle_CattleNotification,Cattle_Advancefilter,Notification_DeactiveAction,Notification_SaveAction,Notification_ActiveAgainAction,Cattle_RefreshDetail
	,Sensor_paging,Cattle_CattleLocation,Cattle_CattlePosition,Cattle_setCattle,Setting_LoadAllRoleOfFarm,Setting_LoadRoleOfStaff,Setting_SaveAccount
	,Setting_EditDefineFarmAcc,Setting_RemoveDefineFarmAcc,Setting_UpdateFarmAccount,CattleGroup_DeleteGroup,CattleHerd_DeleteHerd,FreeStall_EditFreeStallName,
	Cattle_getSpecTimeBudget,Sensor_Advancefilter,Home_SitePath,Sensor_UnassignSensor,Home_Weather';


	select Substring(value, 1,Charindex('_', value)-1) as [Controller],
	Substring(value, Charindex('_', value)+1, LEN(value)) as  [Action] into #asc from [dbo].[SplitStringList] (@StrList) 

	update smartCattle.ActionControllerListTbl	set PartialView=1 from smartCattle.ActionControllerListTbl ac
	 right join #asc  on ac.Action = #asc.Action  COLLATE SQL_Latin1_General_CP1_CI_AS and ac.Controller = #asc.Controller COLLATE SQL_Latin1_General_CP1_CI_AS
 
 end

