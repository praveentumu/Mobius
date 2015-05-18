USE [MobiusServer]
GO
/****** Object:  Table [dbo].[FacilityAssociation]    Script Date: 10/16/2008 10:03:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FacilityAssociation](
	[AssociationId] [bigint] IDENTITY(1,1) NOT NULL,
	[SourceFacilityId] [bigint] NOT NULL,
	[TargetFacilityId] [bigint] NOT NULL,
	[PermissionID] [int] NOT NULL,
	[ExpireOn] [datetime] NULL,
	[Notes] [varchar](5000) NULL,
	[TargetFacilityPubicKey] [varchar](1024) NULL,
 CONSTRAINT [PK_FaciltyAssociation] PRIMARY KEY CLUSTERED 
(
	[AssociationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetConstraints]    Script Date: 10/16/2008 10:02:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetConstraints]
(
@PermissionType int
)
AS
declare @PermissionId int,@ConstraintId int

select @PermissionId = PermId from Permission where Permission.PermissionType= @PermissionType

BEGIN
	SELECT * from Constraints where Constraints.ConsrtID in (select ConsrtId from FacAssoPerm where FacAssoPerm.PermId =  @PermissionId)
END
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 10/16/2008 10:03:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Permission](
	[PermID] [int] NOT NULL,
	[PermName] [varchar](256) NULL,
	[PermDesc] [varchar](500) NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[PermID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServerInfo]    Script Date: 10/16/2008 10:03:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ServerInfo](
	[ServerID] [varchar](50) NOT NULL,
	[ServerURI] [varchar](1000) NULL,
	[ServerPubKey] [varchar](1024) NULL,
	[ServerPrivKey] [varchar](1024) NULL,
	[ServerPortNo] [int] NULL,
	[AdapterSrcPath] [varchar](50) NULL,
	[ServerName] [varchar](50) NULL,
	[AdapterDestPath] [varchar](50) NULL,
 CONSTRAINT [PK_MobiusServerInfo] PRIMARY KEY CLUSTERED 
(
	[ServerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Added By Ranjeet 4August 2008' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerInfo', @level2type=N'COLUMN',@level2name=N'ServerPubKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Added By Ranjeet 4August 2008' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerInfo', @level2type=N'COLUMN',@level2name=N'ServerPrivKey'
GO
/****** Object:  StoredProcedure [dbo].[CheckLogin_bak]    Script Date: 10/16/2008 10:02:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CheckLogin_bak]
(	
	@UserID varchar(50),
	@PasswordHash varchar(50),
	@UserTypeID int,
	@FacilityID int
)
AS
BEGIN
	SELECT 
	UserGUID 
	FROM UserInfo 
	WHERE UserID = @UserID and PassHash = @PasswordHash and UserTypeID = @UserTypeID and FacilityID = @FacilityID
END
GO
/****** Object:  Table [dbo].[DefaultPassword]    Script Date: 10/16/2008 10:02:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DefaultPassword](
	[Password] [varchar](1024) NOT NULL,
	[UserTypeId] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CategoryInfo]    Script Date: 10/16/2008 10:02:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CategoryInfo](
	[CategoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](200) NOT NULL,
	[Value] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_CategoryInfo] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Constraints]    Script Date: 10/16/2008 10:02:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Constraints](
	[ConsrtID] [int] NOT NULL,
	[ConsrtName] [varchar](256) NULL,
	[Description] [varchar](1024) NULL,
 CONSTRAINT [PK_Constraints] PRIMARY KEY CLUSTERED 
(
	[ConsrtID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetUsersData]    Script Date: 10/16/2008 10:02:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUsersData]
(
@UserGUID varchar(200),
@userTypeID int
)

AS
BEGIN
	
	SELECT
			UserGUID, 
			UserID,
			FirstName,
			LastName,
			Email,
			SSN,
			'0' as Role,
			IsActive,
			facilityID,
			'' as EICName
		FROM 
			UserInfo 
		WHERE UserGUID = @UserGUID and UserTypeId = @UserTypeId
		

END
GO
/****** Object:  StoredProcedure [dbo].[GetOMFLicensePermissions]    Script Date: 10/16/2008 10:02:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetOMFLicensePermissions] 

@CategoryID int	


AS
BEGIN
	
SELECT Permission.PermName, Permission.PermDesc

FROM  Permission INNER JOIN
               RolePerm ON Permission.PermID = RolePerm.PermID
WHERE (RolePerm.RoleID = @CategoryID)

END
GO
/****** Object:  Table [dbo].[FacilityType]    Script Date: 10/16/2008 10:03:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FacilityType](
	[FacilityTypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](256) NOT NULL,
	[TypeDesc] [varchar](1024) NULL,
 CONSTRAINT [PK_FacilityType] PRIMARY KEY CLUSTERED 
(
	[FacilityTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FacilityLicense]    Script Date: 10/16/2008 10:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FacilityLicense](
	[FacilityId] [int] NOT NULL,
	[License] [xml] NULL,
	[LicenseDate] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
 CONSTRAINT [PK_FacilityLicense] PRIMARY KEY CLUSTERED 
(
	[FacilityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_SyncSettingsData]    Script Date: 10/16/2008 10:02:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Manoj
-- Create date: 
-- Description:	Inserts Synchronization Configuration Data into SyncConfiguration Table
-- =============================================
CREATE PROCEDURE [dbo].[SP_GET_SyncSettingsData] 
	-- Add the parameters for the stored procedure here
	@SourceFolder nvarchar(MAX) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
    -- Insert statements for procedure here
	set @SourceFolder = (select SourceFolder from SyncConfiguration);
END
GO
/****** Object:  StoredProcedure [dbo].[SP_SET_SyncConfigData]    Script Date: 10/16/2008 10:02:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Manoj
-- Create date: 
-- Description:	Inserts Synchronization Configuration Data into SyncConfiguration Table
-- =============================================
CREATE PROCEDURE [dbo].[SP_SET_SyncConfigData] 
	-- Add the parameters for the stored procedure here
	@UserName nvarchar(MAX), 
	@Password nvarchar(MAX),
	@RemUserName bit,
	@RemPassword bit,
	@SourceFolder nvarchar(MAX),
	@AutoEIC bit,
	@AutoMobius bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into SyncConfiguration (UserName, Password, RemUserName, RemPassword, SourceFolder, AutoEIC, AutoMobius) 
	values(@UserName, @Password, @RemUserName,@RemPassword, @SourceFolder, @AutoEIC, @AutoMobius)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_VerifyPatientRegistration]    Script Date: 10/16/2008 10:02:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_VerifyPatientRegistration] 
	-- Add the parameters for the stored procedure here
	@OMFPatientID nchar(10) output,
	@MobiusPatientID nchar(10)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.


    -- Insert statements for procedure here
	set @OMFPatientID = (select OMFPatientID from OMFPatientRegistration 
	where MobiusPatientID = @MobiusPatientID);
	
END
GO
/****** Object:  StoredProcedure [dbo].[GetRoles]    Script Date: 10/16/2008 10:02:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRoles]
AS
BEGIN
	SELECT
		RoleID, 
		RoleName,
		Description		
	FROM 
		Roles 
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 10/16/2008 10:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUser]
(
	@UserGUID	varchar(200)
)
AS
BEGIN    
	Delete from 
		UserInfo 
	WHERE 
		UserGUID = @UserGUID
END
GO
/****** Object:  StoredProcedure [dbo].[SetKeys]    Script Date: 10/16/2008 10:02:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- Template generated from Template Explorer using:


CREATE PROCEDURE [dbo].[SetKeys] 

@PrivKey varchar(1000),
@PubKey varchar(1000)
As

BEGIN
Update UserInfo set PubKey = @Pubkey , PrivKey = @PrivKey where UserGUID = 'e100fba8-113e-4865-a6ac-96020d1da16a'
END
GO
/****** Object:  Table [dbo].[Group]    Script Date: 10/16/2008 10:03:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Group](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[Publickey] [varchar](1024) NULL,
	[PrivateKey] [varchar](1024) NULL,
	[FacilityId] [int] NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[SampleProcedure]    Script Date: 10/16/2008 10:02:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SampleProcedure] 
  @MaxTotal INT OUTPUT
AS


SELECT *
FROM Tokens
SELECT @MaxTotal = MAX(UserGUID)
FROM Tokens;
GO
/****** Object:  Table [dbo].[UserType]    Script Date: 10/16/2008 10:04:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserType](
	[UserTypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](50) NOT NULL,
	[TypeDesc] [varchar](1024) NULL,
	[TableName] [varchar](50) NULL,
 CONSTRAINT [PK_UserType] PRIMARY KEY CLUSTERED 
(
	[UserTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetFacilityId]    Script Date: 10/16/2008 10:02:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[GetFacilityId] 
@PatientID	varchar(250)
AS
BEGIN


	select facilityId from  UserInfo where UserId=@PatientID

END
GO
/****** Object:  StoredProcedure [dbo].[FillListBox]    Script Date: 10/16/2008 10:02:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FillListBox]
(
	@facilityid int
)
AS
BEGIN	    
	SELECT UserGuid,userid from userinfo where userinfo.facilityid =@facilityid
END
GO
/****** Object:  Table [dbo].[Tokens]    Script Date: 10/16/2008 10:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tokens](
	[Token] [varchar](500) NOT NULL,
	[UserGUID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Tokens_UserGUID]  DEFAULT (newid()),
	[CreateTime] [timestamp] NOT NULL,
 CONSTRAINT [PK_Tokens] PRIMARY KEY CLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetRolePermissions]    Script Date: 10/16/2008 10:02:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRolePermissions]
	-- Add the parameters for the stored procedure here
	@RoleId int
AS
BEGIN
	
Select * from RolePerm where RoleID = @RoleId

END
GO
/****** Object:  StoredProcedure [dbo].[GetOMFGUID]    Script Date: 10/16/2008 10:02:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[GetOMFGUID]
	-- Add the parameters for the stored procedure here
@UserId varchar(50),
@Password	varchar(50)
 
AS
BEGIN
	SELECT UserGUID from UserInfo where UserID = @UserId and PassHash = @Password
END
GO
/****** Object:  Table [dbo].[RootCAkey]    Script Date: 10/16/2008 10:03:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RootCAkey](
	[id] [int] NULL,
	[PublicKey] [varchar](1024) NULL,
	[PrivateKey] [varchar](1024) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[Parse]    Script Date: 10/16/2008 10:04:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Parse] ( @Array VARCHAR(2000), @separator VARCHAR(10))
	RETURNS @resultTable TABLE 	(parseValue VARCHAR(100))
AS
BEGIN	
	DECLARE @separator_position INT 	
	DECLARE @array_value VARCHAR(2000) 		
	SET @array = @array + @separator		
	WHILE patindex('%' + @separator + '%' , @array) <> 0 	
		BEGIN		 
			SELECT @separator_position =  patindex('%' + @separator + '%', @array)	  
			SELECT @array_value = left(@array, @separator_position - 1)			
			INSERT @resultTable		VALUES (Cast(@array_value AS varchar))	 
			SELECT @array = stuff(@array, 1, @separator_position, '')	
		END	
	RETURN
END
GO
/****** Object:  StoredProcedure [dbo].[GetUnSyncEicEnc]    Script Date: 10/16/2008 10:02:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Ranjeet Kumar>
-- Create date: <Date,12/June/2008>
-- Description:	<Description,Push Encounter information to Table EncounterSyncInfo>
-- =============================================

CREATE PROCEDURE [dbo].[GetUnSyncEicEnc]
	-- Add the parameters for the stored procedure here

--@EncounterID	varchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
	--Declare @IsExist int
	select DataSyncID,EncounterId,IsSyncEIC from  EncounterInfo where (IsSyncEIC is  NULL or IsSyncEIC=0)--//uncomment this code
	--select DataSynID,EncounterId,IsSyncEIC from  EncounterInfo where (IsSyncEIC is  NULL or IsSyncEIC=0)

	


	
END
GO
/****** Object:  StoredProcedure [dbo].[GetUnSyncAhaltaEnc]    Script Date: 10/16/2008 10:02:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Ranjeet Kumar>
-- Create date: <Date,12/June/2008>
-- Description:	<Description,Push Encounter information to Table EncounterSyncInfo>
-- =============================================

CREATE PROCEDURE [dbo].[GetUnSyncAhaltaEnc]
	-- Add the parameters for the stored procedure here
--@EncounterID	varchar(50)
AS
BEGIN		
	select DataSyncID,EncounterId,EncTitle as Title,PatientID,EncDesc as Description,OwnerID,Category,CreateDate,DateModified,SyncDate,Data,Signature,FacilityID
	from  EncounterInfo where (IsSyncAHALTA is  NULL or IsSyncAHALTA=0)	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_SET_SyncSettingsData]    Script Date: 10/16/2008 10:02:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Manoj
-- Create date: 
-- Description:	Inserts Synchronization Configuration Data into SyncConfiguration Table
-- =============================================
CREATE PROCEDURE [dbo].[SP_SET_SyncSettingsData] 
	-- Add the parameters for the stored procedure here
	@SourceFolder nvarchar(MAX),
	@AutoEIC bit,
	@AutoMobius bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
    -- Insert statements for procedure here
	insert into SyncConfiguration (SourceFolder, AutoEIC, AutoMobius) 
	values(@SourceFolder, @AutoEIC, @AutoMobius)
END
GO
/****** Object:  StoredProcedure [dbo].[GetPatientId]    Script Date: 10/16/2008 10:02:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Ranjeet Kumar>
-- Create date: <Date,5/June/2008>
-- Description:	<Description,Verify PatientID in Table EncounterSyncInfo>
-- =============================================
CREATE PROCEDURE [dbo].[GetPatientId] 
	-- Add the parameters for the stored procedure here

@PatientID	varchar(250),
@IsExist int = NULL OUTPUT


AS
BEGIN
	
	--select @IsExist=count(*) from  EncounterInfo where PatientID=@PatientID --//uncomment this line
	--select @IsExist=count(*) from  EncounterSyncInfo where PatientID=@PatientID
	--if(@IsExist=0)
	select @IsExist=count(*) from  UserInfo where UserID=@PatientID
	




	
END
GO
/****** Object:  StoredProcedure [dbo].[GetPatientInfo]    Script Date: 10/16/2008 10:02:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Ranjeet Kumar>
-- Create date: <Date,24/July/2008>
-- Modified Date: <Date,20/Sep/2008>
-- Description:	<Description,Verify PatientID in Table EncounterInfo>
-- =============================================
--[GetPatientInfo] 'mohan'
CREATE PROCEDURE [dbo].[GetPatientInfo]
	-- Add the parameters for the stored procedure here

@PatientID	varchar(250)



AS
BEGIN
	
	
	SELECT     UserInfo.UserGUID,UserInfo.UserId, UserInfo.LastName, UserInfo.FirstName, UserInfo.MidName,UserInfo.DOB, UserInfo.FMP, 
            UserInfo.SSN
		FROM  UserInfo where --UserGUID=@PatientID
					UserId=@PatientID 
	
	

	
END
GO
/****** Object:  StoredProcedure [dbo].[GetServerKeys]    Script Date: 10/16/2008 10:02:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetServerKeys]
(	
	@ServerURI varchar(100)
)
AS
BEGIN	
	SELECT		
		ServerPrivKey,
		ServerPubKey
	FROM   ServerInfo
	WHERE  LTrim(RTrim(ServerURI)) = LTrim(RTrim(@ServerURI))
END
GO
/****** Object:  StoredProcedure [dbo].[GetConfigurationUser]    Script Date: 10/16/2008 10:02:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetConfigurationUser]
(
	@UserId varchar(40),
	@FacilityId int
)
AS
BEGIN
	SELECT UserID as UserName,
		PassHash as Password,
		FirstName,
		MidName,
		LastName,
		FacilityId,
		PrivKey,
		PubKey
		 
	FROM UserInfo WHERE UserInfo.UserId = @UserId and UserInfo.FacilityId = @FacilityId
END
GO
/****** Object:  StoredProcedure [dbo].[GetMedicInfo]    Script Date: 10/16/2008 10:02:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Ranjeet Kumar>
-- Create date: <Date,20/sep/2008>
-- Description:	<Description,get Medic Info from Table>
-- =============================================
CREATE PROCEDURE [dbo].[GetMedicInfo] 
	-- Add the parameters for the stored procedure here
@UserID	varchar(250)

AS
BEGIN

	SELECT     UserInfo.UserGUID,UserInfo.UserId, UserInfo.LastName, UserInfo.FirstName, UserInfo.MidName
		FROM  UserInfo where --UserGUID=@UserID
			UserId=@UserID 
		
End
GO
/****** Object:  StoredProcedure [dbo].[GetCategoryInfo]    Script Date: 10/16/2008 10:02:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCategoryInfo]

AS
BEGIN
		select * from CategoryInfo
END
GO
/****** Object:  StoredProcedure [dbo].[CheckAdminLogin]    Script Date: 10/16/2008 10:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CheckAdminLogin]
(	
	@UserID varchar(50),
	@FacilityID int
)
AS
BEGIN
	SELECT 
		UserGUID,
		PassHash
	FROM 
		UserInfo 
	WHERE 
		UserID = @UserID and FacilityID = @FacilityID and (UserTypeId = 1 or UserTypeId =5)
END
GO
/****** Object:  StoredProcedure [dbo].[chkUserID]    Script Date: 10/16/2008 10:02:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[chkUserID]
	@userid varchar(50),
	@CheckUserId int output

AS
BEGIN
	Declare	
	@Count int
	
select @Count = count(*) from userinfo where UserID = @UserID

IF(@Count > 0)
	BEGIN
		set @CheckUserId = 1
	END
ELSE IF(@Count <= 0)
	BEGIN
		set @CheckUserId = 0
	END
--print @CheckUserId
END
GO
/****** Object:  StoredProcedure [dbo].[CheckLogin]    Script Date: 10/16/2008 10:02:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CheckLogin]
(	
	@UserID varchar(50),
	@FacilityID int
)
AS
BEGIN
	SELECT 
		UserGUID,
		PassHash
	FROM 
		UserInfo 
	WHERE 
		UserID = @UserID and FacilityID = @FacilityID
END
GO
/****** Object:  Table [dbo].[EICInfo]    Script Date: 10/16/2008 10:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EICInfo](
	[EICSerialID] [varchar](50) NOT NULL,
	[EICName] [varchar](50) NOT NULL,
	[PubKey] [varchar](50) NOT NULL,
	[PrivKey] [varchar](50) NOT NULL,
	[Description] [varchar](50) NULL,
	[EICGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_EICInfo_EICGUID]  DEFAULT (newid()),
	[IsAssigned] [bit] NOT NULL,
 CONSTRAINT [PK_EICInfo_1] PRIMARY KEY CLUSTERED 
(
	[EICGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CategoryAssociation]    Script Date: 10/16/2008 10:02:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryAssociation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryId] [bigint] NOT NULL,
	[AssociationId] [bigint] NOT NULL,
 CONSTRAINT [PK_CategoryAssociation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FacilityPerm]    Script Date: 10/16/2008 10:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FacilityPerm](
	[FacAssoPermID] [int] IDENTITY(1,1) NOT NULL,
	[FacAssoID] [int] NULL,
	[PermID] [int] NULL,
	[ConsrtID] [int] NULL,
	[Value] [nvarchar](100) NULL,
 CONSTRAINT [PK_RolePerm] PRIMARY KEY CLUSTERED 
(
	[FacAssoPermID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FacilityInfo]    Script Date: 10/16/2008 10:03:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FacilityInfo](
	[FacilityID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](256) NOT NULL,
	[City] [varchar](50) NULL,
	[Address1] [varchar](1024) NULL,
	[Address2] [varchar](1024) NULL,
	[State] [varchar](50) NULL,
	[FacilityTypeID] [int] NULL,
	[ZIP] [varchar](50) NULL,
	[Description] [varchar](1024) NOT NULL,
	[DefaultPassword] [varchar](1024) NOT NULL,
	[Email] [varchar](1024) NULL,
	[IsActive] [bit] NULL,
	[FacilityPublicKey] [varchar](1024) NULL,
	[FacilityPrivateKey] [varchar](1024) NULL,
	[FacilityLogo] [varchar](100) NULL,
 CONSTRAINT [PK_Facility] PRIMARY KEY CLUSTERED 
(
	[FacilityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AHLTAPatientUser]    Script Date: 10/16/2008 10:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AHLTAPatientUser](
	[UserGUID] [uniqueidentifier] NOT NULL,
	[EICGUID] [uniqueidentifier] NULL,
	[Unit] [nchar](50) NULL,
 CONSTRAINT [PK_AHLTAPatientUser_1] PRIMARY KEY CLUSTERED 
(
	[UserGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AHLTAMedicUser]    Script Date: 10/16/2008 10:02:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AHLTAMedicUser](
	[UserGUID] [uniqueidentifier] NOT NULL,
	[Unit] [varchar](50) NULL,
 CONSTRAINT [PK_AHLTAMedicUser] PRIMARY KEY CLUSTERED 
(
	[UserGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OMFUser]    Script Date: 10/16/2008 10:03:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OMFUser](
	[UserGUID] [uniqueidentifier] NOT NULL,
	[Initials] [varchar](50) NULL,
	[Category] [varchar](50) NULL,
 CONSTRAINT [PK_OMFUser_1] PRIMARY KEY CLUSTERED 
(
	[UserGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AdminUser]    Script Date: 10/16/2008 10:02:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminUser](
	[UserGUID] [uniqueidentifier] NOT NULL,
	[Unit] [nvarchar](50) NULL,
 CONSTRAINT [PK_AdminUser] PRIMARY KEY CLUSTERED 
(
	[UserGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 10/16/2008 10:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserInfo](
	[UserGUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_UserInfo_UserGUID]  DEFAULT (newid()),
	[UserID] [varchar](40) NOT NULL,
	[PassHash] [varchar](1024) NULL,
	[FirstName] [varchar](256) NOT NULL,
	[MidName] [varchar](256) NULL,
	[LastName] [varchar](256) NOT NULL,
	[DOB] [datetime] NULL,
	[SSN] [varchar](50) NULL,
	[Email] [varchar](1024) NULL,
	[IsActive] [bit] NOT NULL,
	[PubKey] [varchar](1024) NULL,
	[PrivKey] [varchar](1024) NULL,
	[UserTypeID] [int] NOT NULL,
	[FacilityID] [int] NOT NULL,
	[Nationality] [varchar](100) NULL,
	[Force] [varchar](100) NULL,
	[Sex] [varchar](20) NULL,
	[UIC] [varchar](100) NULL,
	[Religion] [varchar](100) NULL,
	[FMP] [varchar](100) NULL,
	[Race] [varchar](100) NULL,
	[MOS] [varchar](100) NULL,
	[Grade] [varchar](100) NULL,
	[CanWorkOffline] [bit] NULL CONSTRAINT [DF_UserInfo_CanWorkOffline]  DEFAULT ((0)),
 CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[UserGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FacilityPermissionInfo]    Script Date: 10/16/2008 10:03:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FacilityPermissionInfo](
	[FacilityId] [int] NOT NULL,
	[UserTypeId] [int] NOT NULL,
	[PubKey] [text] NULL,
	[PrivKey] [text] NULL,
	[FacAssoID] [int] IDENTITY(1,1) NOT NULL,
	[IsShare] [bit] NULL,
 CONSTRAINT [PK_FacilityAssociationInfo] PRIMARY KEY CLUSTERED 
(
	[FacAssoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EncounterInfo]    Script Date: 10/16/2008 10:03:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EncounterInfo](
	[DataSyncID] [int] IDENTITY(1,1) NOT NULL,
	[EncounterID] [uniqueidentifier] NOT NULL,
	[EncTitle] [nvarchar](50) NULL,
	[EncDesc] [nvarchar](1024) NULL,
	[OwnerID] [nvarchar](50) NOT NULL,
	[PatientID] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NULL,
	[Category] [varchar](50) NULL,
	[SyncDate] [datetime] NULL,
	[Data] [varchar](max) NOT NULL,
	[DateModified] [datetime] NULL,
	[Signature] [varchar](1024) NULL,
	[IsSyncEIC] [bit] NULL,
	[IsSyncAHALTA] [bit] NULL,
	[FacilityID] [int] NOT NULL,
 CONSTRAINT [PK_EncounterInfo] PRIMARY KEY CLUSTERED 
(
	[DataSyncID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[InsertFacilityAssociation]    Script Date: 10/16/2008 10:02:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertFacilityAssociation]
(
@sourceFacilityId int,
@targetFacilityTypeId int,
@permissionTypeId int,
@categoryId varchar(256),
@ExpiryDate varchar(50)
)
AS

DECLARE @TargetFacilityId int, @TargetFacilityPublicKey varchar(1024)
SELECT @TargetFacilityId = FacilityId FROM FacilityInfo WHERE FacilityInfo.FacilityTypeId = @targetFacilityTypeId
SELECT @TargetFacilityPublicKey = FacilityPublicKey FROM FacilityInfo WHERE FacilityInfo.FacilityTypeId = @targetFacilityTypeId

BEGIN
	
	INSERT INTO FacilityAssociation(SourceFacilityId,TargetFacilityId,PermissionID,ExpireOn,FacilityAssociation.TargetFacilityPubicKey)
	VALUES (@sourceFacilityId,@TargetFacilityId,@permissionTypeId,@ExpiryDate,@TargetFacilityPublicKey)

END
GO
/****** Object:  StoredProcedure [dbo].[CheckFacilityAssociation]    Script Date: 10/16/2008 10:02:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CheckFacilityAssociation]
(	
	@SourceFacilityId int,
	@TargetFacilityId int
)
AS
BEGIN
	SELECT 
		count(AssociationId)
	FROM 
		FacilityAssociation 
	WHERE 
		SourceFacilityId = @SourceFacilityId and TargetFacilityId = @TargetFacilityId
END
GO
/****** Object:  StoredProcedure [dbo].[GetOtherFacilityKey]    Script Date: 10/16/2008 10:02:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetOtherFacilityKey]
(	
	@FacilityId int
)
AS
BEGIN	
	SELECT top 1	TargetFacilityPubicKey
	FROM   FacilityAssociation
	WHERE  TargetFacilityId = @FacilityId
END
GO
/****** Object:  StoredProcedure [dbo].[GetAssociatedFacility]    Script Date: 10/16/2008 10:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAssociatedFacility]
(
@FacilityId int
)
AS

BEGIN
				
SELECT		
			FacilityInfo.FacilityId,
			FacilityInfo.FacilityLogo as Facility,
			FacilityInfo.Description,
			FacilityInfo.Name + ' ' + FacilityInfo.Address1 + ' ' + FacilityInfo.Address2 + ' ' + FacilityInfo.State 	AS Location,
			FacilityType.TypeName,
			FacilityInfo.FacilityTypeId
					

FROM        FacilityInfo INNER JOIN
            FacilityType
ON			FacilityInfo.FacilityTypeId = FacilityType.FacilityTypeId 

WHERE       FacilityInfo.FacilityId in (SELECT TargetFacilityId FROM FacilityAssociation WHERE FacilityAssociation.SourceFacilityId = @FacilityId)

END
GO
/****** Object:  StoredProcedure [dbo].[InsertCategoryAssociation]    Script Date: 10/16/2008 10:02:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertCategoryAssociation]
(
@sourceFacilityId int,
@targetFacilityTypeId int,
@permissionTypeId int,
@categoryId int
)
AS
DECLARE @TargetFacilityId int,@AssociationId int
SELECT @TargetFacilityId = FacilityId FROM FacilityInfo WHERE FacilityInfo.FacilityTypeId = @targetFacilityTypeId
	SELECT	@AssociationId = AssociationId FROM FacilityAssociation 
	WHERE	FacilityAssociation.sourceFacilityId = @sourceFacilityId 
			and FacilityAssociation.TargetFacilityId = @TargetFacilityId 
			and FacilityAssociation.permissionId = @permissionTypeId 
BEGIN	
	INSERT INTO CategoryAssociation(categoryId , AssociationId) VALUES (@categoryId, @AssociationId)
END
GO
/****** Object:  StoredProcedure [dbo].[DissociateFacility]    Script Date: 10/16/2008 10:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DissociateFacility]
(
@SourceFacilityId int,
@TargetFacilityTypeId varchar(256)
)
AS

DECLARE @TargetFacilityId varchar(256), @AssociationId varchar(256)

SELECT @TargetFacilityId = FacilityId FROM FacilityInfo WHERE FacilityInfo.FacilityTypeId in (SELECT * FROM dbo.Parse (@TargetFacilityTypeId,','))

SELECT @AssociationId = AssociationId FROM FacilityAssociation WHERE FacilityAssociation.SourceFacilityId = @SourceFacilityId and FacilityAssociation.TargetFacilityId in (SELECT * FROM dbo.Parse (@TargetFacilityId,','))

BEGIN

	DELETE FROM CategoryAssociation 
	WHERE CategoryAssociation.AssociationId in (SELECT * FROM dbo.Parse (@AssociationId,','))

	DELETE FROM FacilityAssociation		
	WHERE FacilityAssociation.SourceFacilityId = @SourceFacilityId 
			and FacilityAssociation.TargetFacilityId in (SELECT * FROM dbo.Parse (@TargetFacilityId,','))

	
END
GO
/****** Object:  StoredProcedure [dbo].[GetFacilityInfo]    Script Date: 10/16/2008 10:02:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFacilityInfo]
(	
@FacilityID	int
)
AS

DECLARE @TargetFacilityId varchar(256),@count int 

SELECT @count = count(AssociationId) FROM FacilityAssociation
	IF(@count>0)
	BEGIN
		SELECT @TargetFacilityId = TargetFacilityId FROM FacilityAssociation WHERE FacilityAssociation.SourceFacilityId = @FacilityId
	
	END
	ELSE
	BEGIN
		SET @TargetFacilityId = ''
	END

BEGIN
	IF(@TargetFacilityId != '')
		BEGIN	

		SELECT		
			FacilityInfo.FacilityId,
			FacilityInfo.FacilityLogo as Facility,
			FacilityInfo.Description,								
			FacilityInfo.Name + ', ' +FacilityInfo.Address1 + ' ' + FacilityInfo.Address2 + ' ' + FacilityInfo.State 	AS Location,
			FacilityType.TypeName,
			FacilityInfo.FacilityTypeId,
			(Select serverURI from ServerInfo) as ServerURI,
			FacilityInfo.FacilityPublicKey as serverPublicKey,
			FacilityInfo.FacilityPrivateKey as serverPrivateKey			

		FROM    FacilityInfo INNER JOIN
				FacilityType
		ON		FacilityInfo.FacilityTypeId = FacilityType.FacilityTypeId 

		WHERE   FacilityInfo.FacilityId != @FacilityID and FacilityInfo.FacilityId not in (SELECT TargetFacilityId FROM FacilityAssociation WHERE FacilityAssociation.SourceFacilityId = @FacilityID)
	END

	ELSE 
		BEGIN

		SELECT
			FacilityInfo.FacilityId,
			FacilityInfo.FacilityLogo as Facility,
			--FacilityInfo.Name,		
			FacilityInfo.Description,
			FacilityInfo.Name + ', ' +FacilityInfo.Address1 + ' ' + FacilityInfo.Address2 + ' ' + FacilityInfo.State 	AS Location,
			FacilityType.TypeName,
			FacilityInfo.FacilityTypeId,
			(Select serverURI from ServerInfo) as ServerURI,
			FacilityInfo.FacilityPublicKey as serverPublicKey,
			FacilityInfo.FacilityPrivateKey as serverPrivateKey	

					

		FROM        FacilityInfo INNER JOIN
					FacilityType
		ON			FacilityInfo.FacilityTypeId = FacilityType.FacilityTypeId 

		WHERE       FacilityInfo.FacilityId != @FacilityID 
		END
	
END
GO
/****** Object:  StoredProcedure [dbo].[GetLicensePermission]    Script Date: 10/16/2008 10:02:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetLicensePermission]
(
@SourceFacilityId int,
@TargetFacilityId int

)
AS
declare @Count int

BEGIN
	
	SELECT @Count =  FacilityAssociation.PermissionId

FROM  FacilityAssociation

WHERE FacilityAssociation.Sourcefacilityid =@SourceFacilityId and TargetFacilityId=@TargetFacilityId

if(@Count>0)
BEGIN
	select * from Permission where Permission.PermID <= @Count

END
else
Begin
return 0
END


END
GO
/****** Object:  StoredProcedure [dbo].[GetFaciltyPermission]    Script Date: 10/16/2008 10:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFaciltyPermission]
(
@SourceFacilityID int,
@TargetFacilityID int
)
AS
Declare		
		@AssociationId int,
		@PermissionId int,
		@ExpireOn datetime,
		@Notes varchar(1000)
 Begin
	SELECT 
		@PermissionId = PermissionId, @AssociationId = AssociationId ,@ExpireOn = isnull(ExpireOn,getdate()),@Notes = Notes FROM  FacilityAssociation	WHERE SourceFacilityID = @SourceFacilityID and TargetFacilityID = @TargetFacilityID
	SELECT  
		@PermissionId as PermissionId,
		@ExpireOn as ExpireOn,
		@Notes as notes,
		CategoryName 
	FROM CategoryInfo 
	WHERE CategoryId in
	(SELECT CategoryID FROM CategoryAssociation WHERE  AssociationId = @AssociationId ) 
END
GO
/****** Object:  StoredProcedure [dbo].[GetPermissions]    Script Date: 10/16/2008 10:02:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPermissions]
(
	@UserTypeID int,
	@FacilityID int 
)
AS


BEGIN
	Select 
		PermId,
		PermName,
		PermDesc
	FROM
		Permission
	WHERE
		PermId in (Select PermId from FacilityPerm where facAssoId in (SELECT FacAssoID from FacilityPermissionInfo where UserTypeId = @UserTypeID and facilityId = @FacilityID))
END
GO
/****** Object:  StoredProcedure [dbo].[GetPermissionInfo]    Script Date: 10/16/2008 10:02:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPermissionInfo]
AS
BEGIN
	
	SELECT 
	*
 from Permission
END
GO
/****** Object:  StoredProcedure [dbo].[GetPatientIdnFile]    Script Date: 10/16/2008 10:02:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPatientIdnFile] 
	-- Add the parameters for the stored procedure here
@PatientId	varchar(100)
AS
Declare 
	@PublicKey varchar(1024) ,	 
	@CanWorkOffline bit,
	@FacilityId int,
	@UserTypeID int,
	@ServerUri varchar(100),	
	@Name varchar(100),@Address1 varchar(100),@Address2 varchar(100),@FacilityPublicKey varchar(max)
	
BEGIN	
	Select @CanWorkOffline = CanWorkOffline,@FacilityId = FacilityId,@UserTypeID = UserTypeID FROM UserInfo WHERE UserInfo.UserId = @PatientId
	
	if(@CanWorkOffline = 1 )
	BEGIN
		Select @PublicKey=PublicKey  from [Group] where FacilityId = @FacilityId
	END
	Else
BEGIN
		--Select @PublicKey=PubKey  from FacilityPermissionInfo where FacilityId = @FacilityId and UserTypeId = @UserTypeID
		Select @PublicKey=PubKey  from UserInfo where UserInfo.UserID=@PatientId;
	END
	
	select @FacilityId=FacilityID,@Name=Name,@Address1=Address1,@Address2=Address2,@FacilityPublicKey=FacilityPublicKey from FacilityInfo where IsActive=1
			
			Select		UserInfo.UserGUID,
						UserInfo.UserID,
						UserInfo.FirstName,
						UserInfo.MidName,
						UserInfo.LastName,
						UserInfo.DOB,
						UserInfo.SSN,
						UserInfo.Email,						
						@PublicKey as pubKey,--user Public Key
						UserInfo.UserTypeID,
						--UserInfo.FacilityID, 
						UserInfo.Nationality,
						UserInfo.Force, 
						UserInfo.UIC, 
						UserInfo.Sex,
						UserInfo.Religion,
						UserInfo.FMP ,
						UserInfo.Race, 
						UserInfo.MOS, 
						UserInfo.Grade,
						(select ServerUri from ServerInfo) as ServerUri,
						(select ServerPrivKey from ServerInfo) as ServerPrivKey,
						@FacilityID as FacilityID,
						@Name as Name,						
						@Address1 as Address1,
						@Address2 as Address2,
						@FacilityPublicKey as PublicKey	--Facility Public key
						
						from UserInfo
								
				where UserInfo.UserID=@PatientId;
	
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserInfo]    Script Date: 10/16/2008 10:02:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserInfo]
(  
 @Token varchar(200)  
)  
AS  
Declare 
	@PublicKey varchar(1024) ,
	@PrivateKey varchar(1024) ,
	@ServerPublicKey varchar(1024) ,
	@ServerPrivateKey varchar(1024) ,
	@CanWorkOffline bit,
	@FacilityId int,
	@UserTypeID int

BEGIN
	
	Select @CanWorkOffline = CanWorkOffline,@FacilityId = FacilityId,@UserTypeID = UserTypeID FROM UserInfo WHERE UserInfo.UserGUID = (SELECT UserGUID FROM Tokens where Token = @Token) 
	
	if(@CanWorkOffline = 1 )
	BEGIN
		Select 
			@PublicKey=PublicKey,
			@PrivateKey = PrivateKey,
			@ServerPublicKey = (Select FacilityPublicKey from FacilityInfo where FacilityId = @FacilityId ),
			@ServerPrivateKey = (Select FacilityPrivateKey from FacilityInfo where FacilityId = @FacilityId )
		from 
			[Group] 
		where 
			FacilityId = @FacilityId
	END
	Else
	BEGIN
		Select 
			@PublicKey=PubKey,
			@PrivateKey = PrivKey ,
			@ServerPublicKey = (Select FacilityPublicKey from FacilityInfo where FacilityId = @FacilityId ),
			@ServerPrivateKey = (Select FacilityPrivateKey from FacilityInfo where FacilityId = @FacilityId )
		from 
			UserInfo 
		where 
			FacilityId = @FacilityId and UserGUID = (SELECT UserGUID FROM Tokens where Token = @Token)
	END

	SELECT    
    UserInfo.UserGUID,   
    UserInfo.UserID,   
    UserInfo.PassHash,   
    UserInfo.FirstName,   
    UserInfo.MidName,   
    UserInfo.LastName,   
    UserType.TypeName as RoleName,   
    UserType.TypeDesc as RoleDesc,
	@PublicKey as pubKey,
	@PrivateKey as PrivKey,
	--UserInfo.PubKey as PubKey,
	--UserInfo.PrivKey  as PrivKey, 
	--(Select pubkey from FacilityPermissionInfo where FacilityId = FacilityInfo.facilityId and UserTypeId = UserType.UserTypeID ) as PubKey,
	--(Select PrivKey from FacilityPermissionInfo where FacilityId = FacilityInfo.facilityId and UserTypeId = UserType.UserTypeID ) as PrivKey,      
    UserInfo.UserTypeID,  
    FacilityInfo.facilityId as facilityId,  
    FacilityInfo.name as FacilityName,  
    FacilityInfo.city,  
    FacilityInfo.address1,  
    FacilityInfo.address2,  
    FacilityInfo.state,  
    FacilityInfo.facilityTypeId,  
    FacilityInfo.facilityId,  
    FacilityInfo.zip,  
    FacilityInfo.description as FaciltyDesc,
	(Select TypeName from FacilityType  where FacilityTypeId = FacilityInfo.facilityTypeId) as FacilityType,
	UserInfo.CanWorkOffline,
	(Select ServerURI from ServerInfo) as serverURI,
	@ServerPublicKey as serverPubKey,
	@ServerPrivateKey as serverPrivKey
  
  FROM    
	UserInfo 
	INNER JOIN 
		UserType on UserInfo.UserTypeId = UserType.UserTypeID 
	INNER JOIN 
		FacilityInfo on FacilityInfo.FacilityId = UserInfo.FacilityId  
	WHERE 
		UserInfo.UserGUID = (SELECT UserGUID FROM Tokens where Token = @Token) 

END


--DECLARE   
-- @UserTypeId int, 
-- @FacilityId int,  
-- @RoleId int,   
-- @Unit nvarchar(50),   
-- @Initial varchar(50),   
-- @Category varchar(50)  
--  
-- SELECT @UserTypeId = UserTypeId,@FacilityId = FacilityId FROM UserInfo WHERE UserGUID = (SELECT UserGUID FROM Tokens where Token = @Token)  
--   
-- IF(@UserTypeId=1)  
-- BEGIN  
--  SELECT    
--    UserInfo.UserGUID,   
--    UserInfo.UserID,   
--    UserInfo.PassHash,   
--    UserInfo.FirstName,   
--    UserInfo.MidName,   
--    UserInfo.LastName,   
--    UserType.TypeName as RoleName,   
--    UserType.TypeDesc as RoleDesc,
--	(Select pubkey from FacilityAssociationInfo where FacilityId = FacilityInfo.facilityId and UserTypeId = UserType.UserTypeID ) as PubKey,
--	(Select PrivKey from FacilityAssociationInfo where FacilityId = FacilityInfo.facilityId and UserTypeId = UserType.UserTypeID ) as PubKey,      
--    UserInfo.UserTypeID,  
--    FacilityInfo.facilityId as facilityId,  
--    FacilityInfo.name as FacilityName,  
--    FacilityInfo.city,  
--    FacilityInfo.address1,  
--    FacilityInfo.address2,  
--    FacilityInfo.state,  
--    FacilityInfo.facilityTypeId,  
--    FacilityInfo.facilityId,  
--    FacilityInfo.zip,  
--    FacilityInfo.description as FaciltyDesc  
--  
--  FROM    
--	UserInfo 
--	INNER JOIN 
--		UserType on UserInfo.UserTypeId = UserType.UserTypeID 
--	INNER JOIN 
--		FacilityInfo on FacilityInfo.FacilityId = UserInfo.FacilityId  
--	WHERE 
--		UserInfo.UserGUID = (SELECT UserGUID FROM Tokens where Token = '@HSV0G9Z8-WWL9GXS&@$C!6A20ZJST90O*FZKR$0I67D!CG0ZR')  
-- END  
--  
-- IF(@UserTypeId=2)  
-- BEGIN  
--  SELECT    
--    UserInfo.UserGUID,   
--    UserInfo.UserID,   
--    UserInfo.PassHash,   
--    UserInfo.FirstName,   
--    UserInfo.MidName,   
--    UserInfo.LastName,   
--    Roles.RoleName,   
--    Roles.RoleDesc,  
--    Roles.PubKey,   
--    Roles.PrivKey,  
--    UserInfo.UserTypeID,  
--    FacilityInfo.name as FacilityName,  
--    FacilityInfo.city,  
--    FacilityInfo.address1,  
--    FacilityInfo.address2,  
--    FacilityInfo.state,  
--    FacilityInfo.facilityTypeId,  
--    FacilityInfo.facilityId,  
--    FacilityInfo.zip,  
--    FacilityInfo.description as FaciltyDesc  
--  
--  FROM    Roles INNER JOIN  
--    AHLTAMedicUser ON Roles.RoleID = AHLTAMedicUser.RoleID INNER JOIN  
--    UserInfo ON AHLTAMedicUser.UserGUID = UserInfo.UserGUID INNER JOIN  
--    FacilityInfo on FacilityInfo.FacilityId = UserInfo.FacilityId  
--  WHERE UserInfo.UserGUID = (SELECT UserGUID FROM Tokens where Token = @Token)  
-- END  
--   
-- IF(@UserTypeId=3)  
-- BEGIN  
--  SELECT    
--    UserInfo.UserGUID,   
--    UserInfo.UserID,   
--    UserInfo.PassHash,   
--    UserInfo.FirstName,   
--    UserInfo.MidName,   
--    UserInfo.LastName,   
--    Roles.RoleName,   
--    Roles.RoleDesc,  
--    UserInfo.PubKey,   
--    UserInfo.PrivKey,  
--    UserInfo.UserTypeID,  
--    FacilityInfo.name as FacilityName,  
--    FacilityInfo.city,  
--    FacilityInfo.address1,  
--    FacilityInfo.address2,  
--    FacilityInfo.state,  
--    FacilityInfo.facilityTypeId,  
--    FacilityInfo.facilityId,  
--    FacilityInfo.zip,  
--    FacilityInfo.description as FaciltyDesc  
--  
--  FROM    Roles INNER JOIN   
--    UserInfo ON Roles.UserTypeID = UserInfo.UserTypeID INNER JOIN  
--    FacilityInfo on FacilityInfo.FacilityId = UserInfo.FacilityId  
--  WHERE UserInfo.UserGUID = (SELECT UserGUID FROM Tokens where Token = @Token)  
-- END  
--  
-- IF(@UserTypeId=4)  
--   
-- BEGIN  
--  SELECT    
--    UserInfo.UserGUID,   
--    UserInfo.UserID,   
--    UserInfo.PassHash,   
--    UserInfo.FirstName,   
--    UserInfo.MidName,   
--    UserInfo.LastName,   
--    Roles.RoleName,   
--    Roles.RoleDesc,  
--    UserInfo.PubKey,   
--    UserInfo.PrivKey,  
--    UserInfo.UserTypeID,  
--    FacilityInfo.name as FacilityName,  
--    FacilityInfo.city,  
--    FacilityInfo.address1,  
--    FacilityInfo.address2,  
--    FacilityInfo.state,  
--    FacilityInfo.facilityTypeId,  
--    FacilityInfo.facilityId,  
--    FacilityInfo.zip,  
--    FacilityInfo.description as FaciltyDesc  
--  
--  FROM    Roles INNER JOIN  
--    OMFUser ON Roles.RoleID = OMFUser.RoleID INNER JOIN  
--    UserInfo ON OMFUser.UserGUID = UserInfo.UserGUID INNER JOIN  
--    FacilityInfo on FacilityInfo.FacilityId = UserInfo.FacilityId  
--  WHERE UserInfo.UserGUID = (SELECT UserGUID FROM Tokens where Token = @Token)  
-- END  
--IF(@UserTypeId=5)  
--   
-- BEGIN  
--  SELECT    
--    UserInfo.UserGUID,   
--    UserInfo.UserID,   
--    UserInfo.PassHash,   
--    UserInfo.FirstName,   
--    UserInfo.MidName,   
--    UserInfo.LastName,   
--    Roles.RoleName,   
--    Roles.RoleDesc,  
--    UserInfo.PubKey,   
--    UserInfo.PrivKey,  
--    UserInfo.UserTypeID,  
--    FacilityInfo.name as FacilityName,  
--    FacilityInfo.city,  
--    FacilityInfo.address1,  
--    FacilityInfo.address2,  
--    FacilityInfo.state,  
--    FacilityInfo.facilityTypeId,  
--    FacilityInfo.facilityId,  
--    FacilityInfo.zip,  
--    FacilityInfo.description as FaciltyDesc  
--  
--  FROM    Roles INNER JOIN  
--    OMFUser ON Roles.RoleID = OMFUser.RoleID INNER JOIN  
--    UserInfo ON OMFUser.UserGUID = UserInfo.UserGUID INNER JOIN  
--    FacilityInfo on FacilityInfo.FacilityId = UserInfo.FacilityId  
--  WHERE UserInfo.UserGUID = (SELECT UserGUID FROM Tokens where Token = @Token)  
-- END  
--  
--   
--   
--
GO
/****** Object:  StoredProcedure [dbo].[GetFacilityDetails]    Script Date: 10/16/2008 10:02:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFacilityDetails]
(	
	@FacilityID	int
)
AS
BEGIN
	
	SELECT		
		FacilityInfo.FacilityId,
		FacilityInfo.Name,		
		FacilityInfo.Description,
		FacilityInfo.Address1 + ' ' + FacilityInfo.Address2 + ' ' + FacilityInfo.State 	AS Address ,
		FacilityInfo.FacilityTypeId,
		(Select serverURI from ServerInfo) as ServerURI,
		FacilityInfo.FacilityPublicKey as serverPublicKey,
		FacilityInfo.FacilityPrivateKey as serverPrivateKey			

	FROM    FacilityInfo   
	WHERE   FacilityInfo.FacilityId = @FacilityID

END
GO
/****** Object:  StoredProcedure [dbo].[GetCategories]    Script Date: 10/16/2008 10:02:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCategories]
(
	@Value int
)
AS
BEGIN
	SELECT 
		CategoryName 
	FROM 
		CATEGORYINFO
	WHERE 
		VALUE & @value = VALUE
END
GO
/****** Object:  StoredProcedure [dbo].[PushAhltaSyncStatus]    Script Date: 10/16/2008 10:02:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Ranjeet Kumar>
-- Create date: <Date,5/Jaune/2008>
-- Description:	<Description,Push Encounter information to Table EncounterSyncInfo>
-- =============================================
CREATE PROCEDURE [dbo].[PushAhltaSyncStatus] 
	-- Add the parameters for the stored procedure here
@EncounterId	varchar(250),
@IsSyncAhalta	bit

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	--update EncounterInfo set IsSyncAHALTA=@IsSyncAhalta where EncounterId=@EncounterId --Need to change this Query
	update EncounterInfo set IsSyncAHALTA=@IsSyncAhalta where EncounterId=@EncounterId

	
END
GO
/****** Object:  StoredProcedure [dbo].[GetTotalEncounter]    Script Date: 10/16/2008 10:02:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Ranjeet Kumar>
-- Create date: <Date,5/Jaune/2008>
-- Description:	<Description,Get Total number of Encounter from Table EncounterSyncInfo>
-- =============================================
CREATE PROCEDURE [dbo].[GetTotalEncounter] 
	-- Add the parameters for the stored procedure here

@PatientID	varchar(50)


AS
BEGIN
	
	select count(*) from EncounterInfo where PatientID=(select UserGuid from UserInfo where Userid=@PatientID) 
						   and (IsSyncEIC is NULL or IsSyncEIC=0 )	

--	select count(*) from EncounterInfo where PatientID=@PatientID 
--						   and (IsSyncEIC is NULL or IsSyncEIC=0 ) --//uncomment this code 
	
	


	
END
GO
/****** Object:  StoredProcedure [dbo].[PushEncounterInfo]    Script Date: 10/16/2008 10:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Ranjeet Kumar>
-- Create date: <Date,22/July/2008>
-- Modified date: <Date,13/Oct/2008>
-- Description:	<Description,Push Encounter information to Table EncounterSyncInfo>
-- =============================================
 
CREATE PROCEDURE [dbo].[PushEncounterInfo] 
	-- Add the parameters for the stored procedure here
@EncounterID	nvarchar(50),--uniqueidentifier
@EncTitle	nvarchar(50),
@EncDesc	nvarchar(1024),
@OwnerID	nvarchar(50),
@PatientID	nvarchar(50),--uniqueidentifier
@CreateDate	datetime,
@Category	varchar(50),
@SyncDate	datetime,
@Data		varchar(MAX),
@DateModified	datetime,
@Signature	varchar(1024),
@FacilityID int
--@FacilityName	varchar(50)

AS
BEGIN
	
	--SET NOCOUNT ON;	
	--Declare @FacilityID int
	--select @FacilityID=FacilityTypeId from FacilityType where TypeName=@FacilityName--'FG'
--Declare @UserGUID uniqueidentifier --varchar(250)
	Declare @PatientGUID uniqueidentifier--varchar(250)
--	select @UserGUID=UserGUID from UserInfo where UserID=@OwnerID --MedicGuid
	select @PatientGUID=UserGUID from UserInfo where UserID=@PatientID--PatientGuid
	
	insert into EncounterInfo(
		EncounterID,
		EncTitle,
		EncDesc,
		OwnerID,
		PatientID,
		CreateDate,
		Category,
		SyncDate,
		Data,
		DateModified,
		Signature,
		FacilityID) 
	values (
		@EncounterID,
		@EncTitle,
		@EncDesc,
		@OwnerID,		
		--@UserGUID,
		@PatientGUID,
		--@OwnerID,		
		--@PatientID,
		@CreateDate,
		@Category,
		@SyncDate,
		@Data,
		@DateModified,
		@Signature,
		@FacilityID)
END
GO
/****** Object:  StoredProcedure [dbo].[PullEncounterInfo]    Script Date: 10/16/2008 10:02:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Ranjeet Kumar>
-- Create date: <Date,5/Jaune/2008>
-- Description:	<Description,Pull Encounter information from Table EncounterSyncInfo>
-- =============================================
CREATE PROCEDURE [dbo].[PullEncounterInfo] 
	-- Add the parameters for the stored procedure here
(
@PatientID	varchar(50)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	Declare @PatientGUID uniqueidentifier--varchar(250)
	select @PatientGUID=UserGUID from UserInfo where UserID=@PatientID

	select 
	EncounterID,EncTitle,EncDesc,OwnerID,
		CreateDate,Category,SyncDate,Data,DateModified,Signature,

		(select TypeName  from FacilityType where FacilityTypeId=EncounterInfo.FacilityID) as FacilityName 
	from EncounterInfo where PatientID=@PatientGUID --and (IsSyncEIC is null or IsSyncEIC=0)
	   
	
END
GO
/****** Object:  StoredProcedure [dbo].[GetCurrentFacilityInfo]    Script Date: 10/16/2008 10:02:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[GetCurrentFacilityInfo] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select FacilityID,[Name],City,Address1,Address2,State,ZIP,Description,Email,FacilityPublicKey,FacilityPrivateKey,FacilityInfo.FacilityTypeID,TypeName from FacilityInfo,FacilityType where FacilityInfo.FacilityTypeID=FacilityType.FacilityTypeID and IsActive='True' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetFacilityName]    Script Date: 10/16/2008 10:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFacilityName]
(
	@FacilityId int
)
AS
declare @FacilityTypeid int, @FacilitTypeName varchar(256)
select @FacilityTypeid =  FacilityTypeid from facilityinfo where facilityinfo.FacilityId =@FacilityId
select @FacilitTypeName =  TypeName from facilityType where facilityType.FacilityTypeid =@FacilityTypeid
BEGIN
	SELECT Name as FacilityName
		 --@FacilitTypeName as TypeName
		
from FacilityInfo where FacilityInfo.FacilityId = @FacilityId
END
GO
/****** Object:  StoredProcedure [dbo].[SetFacilityLicense]    Script Date: 10/16/2008 10:02:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetFacilityLicense]
(
@FacilityId int,
@License xml,
@LicenseDate datetime,
@ExpiryDate datetime
)

AS
BEGIN
	insert into FacilityLicense(FacilityId ,License ,LicenseDate ,ExpiryDate )
	values (@FacilityId,@License,@LicenseDate,@ExpiryDate)
END
GO
/****** Object:  StoredProcedure [dbo].[GetFacilityLicense]    Script Date: 10/16/2008 10:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFacilityLicense]
(
@FacilityId int
)
	

AS
BEGIN
	select * from FacilityLicense where FacilityLicense.FacilityId = @FacilityId
END
GO
/****** Object:  StoredProcedure [dbo].[GetOfflineTemplate]    Script Date: 10/16/2008 10:02:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetOfflineTemplate]
@FacilityId int
AS
BEGIN	
	SELECT		
		Publickey as PubKey,
		PrivateKey as PrivKey
	FROM   [Group]
	WHERE FacilityId = @FacilityId
END
GO
/****** Object:  StoredProcedure [dbo].[GetLicenseDetails]    Script Date: 10/16/2008 10:02:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetLicenseDetails]
(  
	@userID varchar(50)  
)  
AS  
Declare 
	@PublicKey varchar(1024) ,
	@PrivateKey varchar(1024) ,
	@CanWorkOffline bit,
	@FacilityId int,
	@UserTypeID int

BEGIN
	
	Select @CanWorkOffline = CanWorkOffline,@FacilityId = FacilityId,@UserTypeID = UserTypeID FROM UserInfo WHERE UserInfo.UserId = @userID
	
	if(@CanWorkOffline = 1 )
	BEGIN
		Select @PublicKey=PublicKey,@PrivateKey = PrivateKey from [Group] where FacilityId = @FacilityId
	END
	Else
	BEGIN
		--Select @PublicKey=PubKey,@PrivateKey = PrivKey from FacilityPermissionInfo where FacilityId = @FacilityId and UserTypeId = @UserTypeID
		Select @PublicKey=PubKey,@PrivateKey = PrivKey from UserInfo where FacilityId = @FacilityId and UserId = @userID
	END

	SELECT  
		UserInfo.UserID,   
		UserInfo.PassHash, 
		@PrivateKey as PrivKey,
		@PublicKey as pubKey
  
	FROM    
		UserInfo 
		INNER JOIN 
			UserType on UserInfo.UserTypeId = UserType.UserTypeID 
		INNER JOIN 
			FacilityInfo on FacilityInfo.FacilityId = UserInfo.FacilityId  
	WHERE 
		UserInfo.UserId = @userID
END
GO
/****** Object:  StoredProcedure [dbo].[FillDropDown]    Script Date: 10/16/2008 10:02:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FillDropDown]
@Enum int,
@UserTypeID int = null,
@FacilityId int =null,
@EicGUID varchar(100) = null

AS
BEGIN
if(@UserTypeID = 0)
BEGIN
		if(@Enum = 1)
		BEGIN
			SELECT 
				'0' as RoleID,
				'--- All ---' as RoleName
			UNION
			SELECT
				RoleID as RoleID, 
				RoleName		
			FROM 
				Roles 
		END
		if(@Enum = 2)
		BEGIN
			SELECT 
				'0' as RoleID,
				'--- Select ---' as RoleName
			UNION
			SELECT
				RoleID as RoleID, 
				RoleName		
			FROM 
				Roles 
		END
		if(@Enum = 3)
		BEGIN
			SELECT 
				'0' as FacilityID,
				'--- All ---' as FacilityName
			FROM 
				FacilityInfo
			UNION	
			SELECT 
				FacilityID,
				Name as FacilityName
			FROM 
				FacilityInfo
			WHERE
				FacilityId = @FacilityId 
		END
		if(@Enum = 4)
		BEGIN
			SELECT 
				'0' as FacilityID,
				'--- Select ---' as FacilityName
			FROM 
				FacilityInfo
			UNION	
			SELECT 
				FacilityID,
				Name as FacilityName
			FROM 
				FacilityInfo
			WHERE
				FacilityId = @FacilityId
		END

		if(@Enum = 14)
		BEGIN
			SELECT 
				'0' as FacilityID,
				'--- Select ---' as FacilityName
			FROM 
				FacilityInfo
			UNION	
			SELECT 
				FacilityID,
				Name as FacilityName
			FROM 
				FacilityInfo where FacilityInfo.IsActive = 1
		END		

		IF(@Enum = 5)
		BEGIN
			SELECT 
				'0' as UserTypeId,
				'--- All ---' as UserType
			UNION
			SELECT
				UserTypeID as UserTypeID, 
				TypeDesc as UserType		
			FROM 
				UserType 
			WHERE
				UserTypeId in (SELECT UserTypeId FROM FacilityPermissionInfo WHERE FacilityId = @FacilityId)
				--FacilityTypeId = (SELECT FacilityTypeId FROM FacilityInfo WHERE  FacilityId = @FacilityId)
		END
		if(@Enum = 6)
		BEGIN
			SELECT 
				'0' as UserTypeId,
				'--- Select ---' as UserType
			UNION
			SELECT
				UserTypeID as UserTypeID, 
				TypeDesc as UserType		
			FROM 
				UserType 
			WHERE
				UserTypeId in (SELECT UserTypeId FROM FacilityPermissionInfo WHERE FacilityId = @FacilityId)
				--FacilityTypeId = (SELECT FacilityTypeId FROM FacilityInfo WHERE  FacilityId = @FacilityId)
		END
		if(@Enum = 8)
		BEGIN
--			SELECT
--				CONVERT(nvarchar(100), '') as EICGUID,
--				'--- Select ---' as  EICName
--			--FROM 
--			--EICInfo 	
--			UNION
			SELECT 
			    EICGUID,
				EICName
			FROM 
			EICInfo 
			Where IsAssigned = 0
			Union
			SELECT
			    EICGUID,
				EicNAME
			from 
				EICInfo
			where
				EicGUID = @EicGUID
			
			
		END	
		if(@Enum = 7)
		BEGIN
			SELECT
				CONVERT(nvarchar(100), '') as EICGUID,
				'--- Select ---' as  EICName
			--FROM 
			--EICInfo 	
			UNION
			SELECT 
				CONVERT(nvarchar(100), EICGUID) as EICGUID,
				EICName
			FROM 
			EICInfo 
			Where IsAssigned = 0
		END		
		if(@Enum = 9)
		BEGIN
			SELECT 
				'0' as RoleID,
				'--- Select ---' as RoleName
			
			UNION
			SELECT
				RoleID as RoleID, 
				RoleName		
			FROM 
				Roles
			WHERE
				UserTypeId = @UserTypeID  
		END	
		if(@Enum = 10)
		BEGIN
			SELECT 
				EICGUID,
				EICName
			FROM 
			EICInfo 
		END	
	
	    IF(@Enum = 11)
	    BEGIN
		
			Select
				CONVERT(nvarchar(100), '') as UserGUID,
				'--- Select ---' as  UserID			
			 
			UNION
			SELECT 
				CONVERT(nvarchar(100), UserGUID) as UserGUID,
				UserID				
			FROM 
			UserInfo USI where (USI.UserTypeID = 3) and USI.IsActive = 1 and USI.UserGUID in (select UserGUID from AHLTAPatientUser where AHLTAPatientUser.EICGUID is null)
		END	
		
		 IF(@Enum = 13)
	    BEGIN
		
			SELECT
				CONVERT(nvarchar(100), '') as UserGUID,
				'--- Not Assigned ---' as  UserID			
			UNION
--		SELECT 
--				CONVERT(nvarchar(100), UserGUID) as UserGUID,
--				UserID				
--			FROM 
--			UserInfo USI where (USI.UserTypeID = 3) and USI.IsActive = 1 and USI.UserGUID not in (select UserGuid from AHLTAPatientUser) 
--		UNION
		SELECT
				CONVERT(nvarchar(100), UserGUID) as UserGUID,
				UserID
			FROM 
				UserInfo USI
			WHERE
				 USI.IsActive = 1 and USI.UserGUID in (select UserGUID from AHLTAPatientUser where AHLTAPatientUser.EICGUID is null /*CONVERT(nvarchar(100), AHLTAPatientUser.EICGUID) = CONVERT(nvarchar(100), @EicGUID)*/)
		
		END	
		
		 IF(@Enum = 15)
	    BEGIN
		
			SELECT
				CONVERT(nvarchar(100), '') as UserGUID,
				'--- Not Assigned ---' as  UserID			
			UNION
		SELECT 
				CONVERT(nvarchar(100), UserGUID) as UserGUID,
				UserID				
			FROM 
			UserInfo USI where (USI.UserTypeID = 3) and USI.IsActive = 1 and USI.UserGUID in (select UserGUID from AHLTAPatientUser where AHLTAPatientUser.EICGUID is null)
		UNION
		SELECT
				CONVERT(nvarchar(100), UserGUID) as UserGUID,
				UserID
			FROM 
				UserInfo USI
			WHERE
				USI.IsActive = 1 and USI.UserGUID = (select UserGUID from AHLTAPatientUser where CONVERT(nvarchar(100), AHLTAPatientUser.EICGUID) = CONVERT(nvarchar(100), @EicGUID))
		
		END	

		IF(@Enum = 12)
	    BEGIN
		SELECT
				CONVERT(nvarchar(100), '') as UserGUID,
				'--- Select ---' as  UserID			
			 
			UNION
		SELECT 
				CONVERT(nvarchar(100), UserGUID) as UserGUID,
				UserID				
			FROM 
			UserInfo USI where (USI.UserTypeID = 3) and USI.IsActive = 1 and USI.UserGUID not in (select UserGuid from AHLTAPatientUser) 
		UNION
		SELECT
				CONVERT(nvarchar(100), UserGUID) as UserGUID,
				UserID
			FROM 
				UserInfo USI
			WHERE
				USI.IsActive = 1 and USI.UserGUID = (select UserGUID from AHLTAPatientUser where CONVERT(nvarchar(100), AHLTAPatientUser.EICGUID) = CONVERT(nvarchar(100), @EicGUID))
		END

		if(@Enum = 16)
		BEGIN
			SELECT 
				'0' as FacilityID,
				'--- Select ---' as FacilityName
			FROM 
				FacilityInfo
			UNION	
			SELECT 
				FacilityID,
				Name as FacilityName
			FROM 
				FacilityInfo	
			WHERE FacilityID != @FacilityId	and IsActive = 1
		END	

		IF(@Enum = 17)
		BEGIN
			SELECT 
				'0' as UserTypeId,
				'--- All ---' as UserType
			UNION
			SELECT
				UserTypeID as UserTypeID, 
				TypeDesc as UserType		
			FROM 
				UserType 
			WHERE
				(UserTypeId = 2 or UserTypeId = 6 or UserTypeId = 7) and
				(UserTypeId in (SELECT UserTypeId FROM FacilityPermissionInfo WHERE FacilityId = @FacilityId))
				--FacilityTypeId = (SELECT FacilityTypeId FROM FacilityInfo WHERE  FacilityId = @FacilityId)
		END	

		
END
ELSE
BEGIN
		if(@Enum = 9)
		BEGIN
			SELECT 
				'0' as RoleID,
				'--- Select ---' as RoleName
			UNION
			SELECT
				RoleID as RoleID, 
				RoleName		
			FROM 
				Roles
			WHERE
				UserTypeId = @UserTypeID  
		END		
END
END
GO
/****** Object:  StoredProcedure [dbo].[GetUsers]    Script Date: 10/16/2008 10:02:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUsers]
(
	@UserTypeID int,
	@FacilityID int
)
AS

BEGIN
if(@UserTypeID > 0)
	BEGIN
		if(@UserTypeID = 1)
		BEGIN	
		 SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,	
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName
			
		FROM 
			UserInfo Usr where UserTypeID = 1 and FacilityID = @FacilityID
		END
	
		if(@UserTypeID = 2)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AMU.RoleID From AhltaMedicUser AMU where AMU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 2 and FacilityID = @FacilityID

		END
		if(@UserTypeID = 3)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--'N/A' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 3 and FacilityID = @FacilityID

		END
		if(@UserTypeID = 4)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select OU.RoleID From OmfUser OU where OU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 4 and FacilityID = @FacilityID
		END		
		
		if(@UserTypeID = 5)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 5 and FacilityID = @FacilityID
		END		

		if(@UserTypeID = 6)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 6 and FacilityID = @FacilityID
		END		
		
		if(@UserTypeID = 7)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 7 and FacilityID = @FacilityID
		END		

	END
	ELSE IF(@UserTypeID = 0)
	BEGIN
	
		 SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = 1) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 1 and FacilityId = @FacilityID
			
		UNION

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = 2) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AMU.RoleID From AhltaMedicUser AMU where AMU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 2 and FacilityId = @FacilityID

		UNION

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = 3) as UserType,
			--'N/A' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 3 and FacilityId = @FacilityID

		UNION

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = 4) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select OU.RoleID From OmfUser OU where OU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 4 and FacilityId = @FacilityID
		
		UNION
		
			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = 5) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 5 and FacilityId = @FacilityID
		
		UNION
		
			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = 6) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 6 and FacilityId = @FacilityID

		UNION
		
			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = 7) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 7 and FacilityId = @FacilityID

	END
END
GO
/****** Object:  StoredProcedure [dbo].[GetDistinctUsers]    Script Date: 10/16/2008 10:02:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDistinctUsers]
(
	@UserTypeID int,
	@FacilityID int
)
AS

BEGIN
if(@UserTypeID > 0)
	BEGIN
		if(@UserTypeID = 1)
		BEGIN	
		 SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,	
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName
			
		FROM 
			UserInfo Usr where UserTypeID = 1
		END
	
		if(@UserTypeID = 2)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AMU.RoleID From AhltaMedicUser AMU where AMU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 2

		END
		if(@UserTypeID = 3)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--'N/A' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 3

		END
		if(@UserTypeID = 4)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select OU.RoleID From OmfUser OU where OU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 4
		END		
		
		if(@UserTypeID = 5)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 5
		END		

		if(@UserTypeID = 6)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 6
		END		
		
		if(@UserTypeID = 7)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 7
		END		

	END
	ELSE IF(@UserTypeID = 0)
	BEGIN
	
		 
			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = 2) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AMU.RoleID From AhltaMedicUser AMU where AMU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 2 and FacilityId = @FacilityID

		
		UNION
		
			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = 6) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 6 and FacilityId = @FacilityID

		UNION
		
			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = 7) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr where UserTypeID = 7 and FacilityId = @FacilityID

	END
END
GO
/****** Object:  StoredProcedure [dbo].[ImportUser]    Script Date: 10/16/2008 10:02:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ImportUser]

	@UserGUID		  varchar(200),
	@UserID			  varchar(50),
    @FirstName		  varchar(50),
	@MiddleName		  varchar(50),
    @LastName		  varchar(50),	
    @SSN		      varchar(50),
	@UserType         varchar(50),	
	@FacilityName	  varchar(256),
	@IsActive		  bit,	
	@Nationality	  varchar(100),
	@Force			  varchar(100),
	@Sex			  varchar(20),
	@UIC			  varchar(100),
	@Religion	      varchar(100),   
	@FMP			  varchar(100),
	@Race			  varchar(100),
	@Mos			  varchar(100),
	@Grade			  varchar(100),	
	@CheckUserId	  int output
AS

BEGIN
	
Declare	
	@Count int,
	@Roleid int,
	@FacilityId int,
	@UserTypeId int

BEGIN
	SELECT @UserTypeId = UserTypeId FROM UserType WHERE UserType.TypeName = @UserType
END


SELECT @FacilityId = FacilityId FROM FacilityInfo WHERE FacilityInfo.Name = @FacilityName

SELECT @Count = count(*) from Userinfo where UserID = @UserID
			
		IF(@Count <= 0)
			BEGIN
			SET @CheckUserID = 0
				INSERT INTO UserInfo
					(
						UserID,
						FirstName,
						MidName,
						LastName,
						SSN,
						UserTypeId,	
						IsActive,			
						FacilityID,						
						Nationality,
						Force,
						Sex,
						UIC,
						Religion,
						FMP,
						Race,
						MOS,
						Grade
					)
					VALUES 
					(
						@UserID,						
						@FirstName, 
						@MiddleName,
						@LastName, 
						@SSN, 
						@UserTypeId,
						@IsActive,
						@FacilityId,						
						@Nationality,
						@Force,
						@Sex,
						@UIC,
						@Religion,
						@FMP,
						@Race,
						@MOS,
						@Grade
					)
			END
			ELSE IF (@Count > 0)
				BEGIN
						set @CheckUserID = 1
				END
--print @CheckUserID
END
GO
/****** Object:  StoredProcedure [dbo].[ExportUser]    Script Date: 10/16/2008 10:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ExportUser]
(
	@UserID  varchar(1024)	
)
AS
BEGIN
		SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') as FirstName,
			IsNull(Usr.MidName,'') as MiddleName,
			IsNull(Usr.LastName,'') as LastName,
			Usr.Email as Email,
			Usr.SSN as SSN,
			(select TypeName from UserType where UserType.UserTypeId = Usr.UserTypeId) as UserType,
			--'N/A' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName,
			Nationality,
			Force,
			Sex,
			UIC,
			Religion,
			FMP,
			Race,
			MOS,
			Grade,
			DOB			
		FROM 
			UserInfo Usr where Usr.UserID in (select * from dbo.Parse (@UserID,','))	 --calling the Function Parse
END
GO
/****** Object:  StoredProcedure [dbo].[SearchUser]    Script Date: 10/16/2008 10:02:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchUser]
(	
	@UserID varchar(100),
	@Firstname varchar(50),
	@MiddleName varchar(50),
	@LastName varchar(50),
	@Email varchar(50),
	@DOB varchar(10),
	@SSN varchar(50),	
	@UserTypeID int,
	@FacilityID int,
	@EICGUID varchar(100)
	--@RoleId int 
)
AS
	
	if @UserID <> '' set @UserID=@UserID + '%'
	if @FirstName <> '' set @Firstname=@FirstName + '%'
	if @MiddleName <> '' set @MiddleName=@MiddleName + '%'
	if @LastName <> '' set @LastName=@LastName + '%'
	if @Email <> '' set @Email=@Email + '%'		
	--if @DOB <> '' set @DOB=@DOB + '%'
	if @SSN <> '' set @SSN=@SSN +'%'
 
	if @UserID = '' set @UserID=null
	if @FirstName = '' set @Firstname=null
	if @MiddleName = '' set @MiddleName=null
	if @LastName = '' set @LastName=null
	if @Email = '' set @Email=null
	if @DOB = '' set @DOB=null
	if @SSN = '' set @SSN=null
	--if @FacilityID = 0 set @FacilityID = null

Declare @FacilityName varchar(256)

IF(@UserTypeID > 0)
BEGIN
		IF(@FacilityID > 0) --1st if
		BEGIN

		IF(@UserTypeID = 1)
		BEGIN	
			SELECT 
				Usr.UserGUID as ID, 
				Usr.UserID as UserID,
				IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
				Usr.Email as Email,
				Usr.SSN as SSN,
				(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
				--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,							
				Usr.IsActive,
				(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName			
			FROM 
				UserInfo Usr
			INNER JOIN AdminUser AU ON Usr.UserGUID = AU.UserGUID --and AU.RoleID = @RoleId
			
			WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				and FacilityID = @FacilityID 
				and UserTypeID = @UserTypeID 
				
		END

		IF(@UserTypeID = 2)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AMU.RoleID From AhltaMedicUser AMU where AMU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr
		INNER JOIN AhltaMedicUser AMU ON Usr.UserGUID = AMU.UserGUID --and AMU.RoleID = @RoleId
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				and FacilityID = @FacilityID
				and UserTypeID = @UserTypeID 
		END

		IF(@UserTypeID = 3)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--'' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
		
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				and FacilityID = @FacilityID and UserTypeID = @UserTypeID 
		END

		IF(@UserTypeID = 4)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select OU.RoleID From OmfUser OU where OU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
		INNER JOIN OmfUser OU ON Usr.UserGUID = OU.UserGUID --and OU.RoleID = @RoleId
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				and FacilityID = @FacilityID and UserTypeID = @UserTypeID 
		END
			
		IF(@UserTypeID = 5)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select OU.RoleID From OmfUser OU where OU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
		INNER JOIN OmfUser OU ON Usr.UserGUID = OU.UserGUID --and OU.RoleID = @RoleId
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				and FacilityID = @FacilityID and UserTypeID = @UserTypeID 
		END

		IF(@UserTypeID = 6)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--'' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
		
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				and FacilityID = @FacilityID and UserTypeID = @UserTypeID 
		END

		IF(@UserTypeID = 7)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--'' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
		
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				and FacilityID = @FacilityID and UserTypeID = @UserTypeID 
		END

		END --1st if end
		
		IF(@FacilityID = 0) --1.2st if
		BEGIN

		IF(@UserTypeID = 1)
		BEGIN	
			SELECT 
				Usr.UserGUID as ID, 
				Usr.UserID as UserID,
				IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
				Usr.Email as Email,
				Usr.SSN as SSN,
				(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
				--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
				Usr.IsActive,
				(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
			FROM 
				UserInfo Usr
			INNER JOIN AdminUser AU ON Usr.UserGUID = AU.UserGUID --and AU.RoleID = @RoleId
			WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				--and FacilityID = @FacilityID 
				and UserTypeID = @UserTypeID 
				
		END

		IF(@UserTypeID = 2)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AMU.RoleID From AhltaMedicUser AMU where AMU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr
		INNER JOIN AhltaMedicUser AMU ON Usr.UserGUID = AMU.UserGUID --and AMU.RoleID = @RoleId
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				--and FacilityID = @FacilityID
				and UserTypeID = @UserTypeID 
		END

		IF(@UserTypeID = 3)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--'' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
		
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				--and FacilityID = @FacilityID 
				and UserTypeID = @UserTypeID 
		END

		IF(@UserTypeID = 4)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select OU.RoleID From OmfUser OU where OU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
		INNER JOIN OmfUser OU ON Usr.UserGUID = OU.UserGUID --and OU.RoleID = @RoleId
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				--and FacilityID = @FacilityID 
				and UserTypeID = @UserTypeID 
		END
		
		IF(@UserTypeID = 5)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select OU.RoleID From OmfUser OU where OU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
		INNER JOIN OmfUser OU ON Usr.UserGUID = OU.UserGUID --and OU.RoleID = @RoleId
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				--and FacilityID = @FacilityID 
				and UserTypeID = @UserTypeID 
		END
		
		IF(@UserTypeID = 6)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--'' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
		
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				--and FacilityID = @FacilityID 
				and UserTypeID = @UserTypeID 
		END
		
		IF(@UserTypeID = 7)
		BEGIN	

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = @UserTypeID) as UserType,
			--'' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
		
		WHERE
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				--and FacilityID = @FacilityID 
				and UserTypeID = @UserTypeID 
		END

		END --1.2st if end


END

ElSE IF(@UserTypeID = 0)
BEGIN
		IF(@FacilityID > 0) --3rd if start
		BEGIN
		 SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = Usr.UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
			INNER JOIN AdminUser AU ON Usr.UserGUID = AU.UserGUID --and AU.RoleID = @RoleId
			WHERE 
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB) 
				and FacilityID = @FacilityID
	
		UNION

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = Usr.UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AMU.RoleID From AhltaMedicUser AMU where AMU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
			INNER JOIN AhltaMedicUser AMU ON Usr.UserGUID = AMU.UserGUID --and AMU.RoleID = @RoleId
		WHERE 
			(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
			and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
			and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email)and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)  
			and FacilityID = @FacilityID 

		UNION

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = Usr.UserTypeID) as UserType,
			--'' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
			INNER JOIN AhltaPatientUser APU ON Usr.UserGUID = APU.UserGUID --and AMU.RoleID = @RoleId
		WHERE 
			(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
			and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
			and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB) 
			and FacilityID = @FacilityID 

		UNION

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = Usr.UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select OU.RoleID From OmfUser OU where OU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr 
			INNER JOIN OmfUser OU ON Usr.UserGUID = OU.UserGUID --and OU.RoleID = @RoleId
		WHERE 
			(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
			and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
			and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
			and FacilityID = @FacilityID
		

	END --3rd if end
	

	IF(@FacilityID = 0) --3.2rd if start
		BEGIN
		 SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = Usr.UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AU.RoleID From AdminUser AU where AU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName			
		FROM 
			UserInfo Usr 
			INNER JOIN AdminUser AU ON Usr.UserGUID = AU.UserGUID --and AU.RoleID = @RoleId
			WHERE 
				(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
				and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
				and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
				--and FacilityID = @FacilityID
	
		UNION

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = Usr.UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select AMU.RoleID From AhltaMedicUser AMU where AMU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
			INNER JOIN AhltaMedicUser AMU ON Usr.UserGUID = AMU.UserGUID --and AMU.RoleID = @RoleId
		WHERE 
			(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
			and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
			and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)   
			--and FacilityID = @FacilityID 

		UNION

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = Usr.UserTypeID) as UserType,
			--'' as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName				
		FROM 
			UserInfo Usr 
			INNER JOIN AhltaPatientUser AMU ON Usr.UserGUID = AMU.UserGUID --and AMU.RoleID = @RoleId
		WHERE 
			(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
			and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
			and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)  
			--and FacilityID = @FacilityID 

		UNION

			SELECT 
			Usr.UserGUID as ID, 
			Usr.UserID as UserID,
			IsNull(Usr.FirstName,'') + ' ' + IsNull(Usr.MidName,'') + ' ' + IsNull(Usr.LastName,'') as UserName,		
			Usr.Email as Email,
			Usr.SSN as SSN,
			(SELECT UserType.TypeName FROM UserType WHERE UserType.UserTypeId = Usr.UserTypeID) as UserType,
			--(Select Rls.RoleName from Roles Rls where Rls.RoleID = (Select OU.RoleID From OmfUser OU where OU.UserGUID = Usr.UserGUID )) as Role,			
			Usr.IsActive,
			(Select Name from FacilityInfo where FacilityInfo.FacilityId = Usr.FacilityId) as FacilityName					
		FROM 
			UserInfo Usr 
			INNER JOIN OmfUser OU ON Usr.UserGUID = OU.UserGUID --and OU.RoleID = @RoleId
		WHERE 
			(@UserID is null or UserID like @UserID) and (@FirstName is null or FirstName like @FirstName) 
			and (@LastName is null or LastName like @LastName) and (@MiddleName is null or MidName like @MiddleName) 
			and (@SSN is null or SSN like @SSN) and (@Email is null or Email like @Email) and (@DOB is null or convert(varchar(10),DOB,101) like @DOB)
			--and FacilityID = @FacilityID
		

	END --3.2rd if end


END
GO
/****** Object:  StoredProcedure [dbo].[AddToken]    Script Date: 10/16/2008 10:02:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddToken]
(
	@Token			  varchar(200),
    @UserGUID         varchar(200) 
)

AS
BEGIN
	
	INSERT Tokens(Token, UserGUID)
    VALUES (@Token, @UserGUID)

END
GO
/****** Object:  StoredProcedure [dbo].[RemoveToken]    Script Date: 10/16/2008 10:02:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveToken]
(
	@Token	varchar(100)    
)

AS
BEGIN

	Delete from 
		Tokens 
	WHERE 
		Token = @Token	

END
GO
/****** Object:  StoredProcedure [dbo].[GetTokens]    Script Date: 10/16/2008 10:02:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTokens] 
	@Token  varchar(100)
AS
BEGIN  

	SELECT 
		UserGUID
	FROM 
		Tokens where Token = @Token
END
GO
/****** Object:  StoredProcedure [dbo].[GetRootCAKey]    Script Date: 10/16/2008 10:02:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRootCAKey]
	
AS
BEGIN
	select * from RootCAKey
END
GO
/****** Object:  StoredProcedure [dbo].[GetSingleUserPermission]    Script Date: 10/16/2008 10:02:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetSingleUserPermission]
(
@UserId varchar(256)
)
AS

DECLARE @UserTypeId int,@FacilityId int, @FacAssoId int, @IsShare bit, @PermId int

BEGIN	
	SELECT @UserTypeId = UserTypeId from UserInfo where UserInfo.UserId = @UserId
END

BEGIN	
	SELECT @FacilityId = FacilityId from UserInfo where UserInfo.UserId = @UserId
END

BEGIN
	SELECT @FacAssoId = FacAssoId from FacilityPermissionInfo where FacilityPermissionInfo.FacilityId = @FacilityId and FacilityPermissionInfo.UserTypeId = @UserTypeId
END

BEGIN	
	SELECT @IsShare = IsShare from FacilityPermissionInfo where FacilityPermissionInfo.FacilityId = @FacilityId 
									and FacilityPermissionInfo.UserTypeId = @UserTypeId and FacilityPermissionInfo.FacAssoId = @FacAssoId
END

BEGIN
	IF(@IsShare=1)
		BEGIN
			SELECT PermId+32 as PermId, ConsrtId ,value as ConstraintsValue from FacilityPerm where FacilityPerm.FacAssoId = @FacAssoId 
			order by PermId
		END
	ELSE
		BEGIN
			SELECT PermId, ConsrtId ,value as ConstraintsValue from FacilityPerm where FacilityPerm.FacAssoId = @FacAssoId 
			order by PermId
		END
END
GO
/****** Object:  StoredProcedure [dbo].[ManageUser]    Script Date: 10/16/2008 10:02:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ManageUser]	
	@UserGUID		  varchar(200),
	@UserID			  varchar(50),
    @PasswordHash     varchar(50),
    @FirstName		  varchar(50),
	@MiddleName		  varchar(50),
    @LastName		  varchar(50),
	@Email			  varchar(50),
	@DOB			  varchar(50),
    @SSN		      varchar(50),
	@UserTypeID	      int,
	--@RoleID		      int,
	@FacilityID		  int,
    @EICSerialID      varchar(50) = null,    
    @IsActive		  bit,
	@Unit			  varchar(50) = null,
	@Initials		  varchar(50) = null,
	@Category		  varchar(50) = null,
	@PubKey			  varchar(2000) = null,
	@PrivKey		  varchar(2000) = null,
	@Nationality	  varchar(100) = null,
	@Force			  varchar(100) = null,
	@Sex			  varchar(20),
	@UIC			  varchar(100) = null,
	@Religion	      varchar(100) = null,
	@FMP			  varchar(100) = null,
	@Race			  varchar(100) = null,
	@Mos			  varchar(100) = null,
	@Grade			  varchar(100) = null,
	@canWorkOffline	  bit,
	@CheckUserId	  int output
	
AS

BEGIN
	Declare
	@ID varchar(200),
	@OldUserTypeID int,
	@OldEicGUID varchar(100),
	@Count int
	
	IF(@UserGUID = '')
		BEGIN
			select @Count = count(UserGUID) from Userinfo where UserID = @UserID
			if(@Count <= 0)
			BEGIN
			set @CheckUserID = 0
				INSERT INTO UserInfo
					(
						UserID,
						PassHash,
						FirstName,
						MidName,
						LastName,
						Email,
						DOB,
						SSN,
						UserTypeiD,
						FacilityID,
						IsActive,
						PubKey,
						PrivKey,
						Nationality,
						Force,
						Sex,
						UIC,
						Religion,
						FMP,
						Race,
						MOS,
						Grade,
						canWorkOffline
					)
					VALUES 
					(
						@UserID,
						@PasswordHash,
						@FirstName, 
						@MiddleName,
						@LastName, 
						@Email,
						@DOB,
						@SSN, 
						@UserTypeID,
						@FacilityID,
						@IsActive,
						@PubKey,
						@PrivKey,
						@Nationality,
						@Force,
						@Sex,
						@UIC,
						@Religion,
						@FMP,
						@Race,
						@MOS,
						@Grade,
						@canWorkOffline
					)
				select @ID = UserGUID from UserInfo where UserId = @UserID 

				IF(@UserTypeID = 1)
				BEGIN
					INSERT INTO AdminUser
					(
						UserGUID,
						Unit
						--RoleID				
					)
					VALUES
					(
						@ID,
						@Unit
						--@RoleID
					)
				END
				IF(@UserTypeID = 2 or @UserTypeID = 6 or  @UserTypeID = 7)
				BEGIN
					INSERT INTO AHLTAMedicUser
					(
						UserGUID,
						Unit
						--RoleID				
					)
					VALUES
					(
						@ID,
						@Unit
						--@RoleID
					)
				END
				IF(@UserTypeID = 3)
				BEGIN
					 IF(@EICSerialID = '')
                             BEGIN
                                   INSERT INTO AHLTAPatientUser
                                   (
                                         UserGUID, 
					 EICGUID,                       
                                         Unit                    
                                   )
                                   VALUES
                                   (
                                         @ID, 
					 null,                      
                                         @Unit                         
                                   )
                             End
                             ELSE IF(@EICSerialID != '')
                                   BEGIN
                                         INSERT INTO AHLTAPatientUser
                                   (
                                         UserGUID,
                                         EICGUID,                            
                                         Unit                    
                                   )
                                   VALUES
                                   (
                                         @ID,
                                         @EICSerialID,
                                         @Unit                         
                                   )
                                         UPDATE EicInfo SET IsAssigned = 1 WHERE EICGUID = @EICSerialID
                                   END

				END
				IF(@UserTypeID = 4 or @UserTypeID = 5)
				BEGIN
					INSERT INTO OMFUser
					(
						UserGUID,					
						Initials,
						Category
						--RoleID				
					)
					VALUES
					(
						@ID,					
						@Initials,
						@Category
						--@RoleID					
					)
				END
				
			END
			ELSE
			BEGIN
				set @CheckUserID = 1
			END
			
		END
	ELSE IF(@UserGUID != '')
		BEGIN
		select @Count = count(UserGUID) from Userinfo where UserID = @UserID and UserGUID != @UserGUID
			if(@Count <= 0)
			BEGIN		
				Select @OldUserTypeID = UserTypeID from UserInfo where UserGUID = @UserGUID				

				Update Userinfo SET 				
					UserID = @UserID,				
					FirstName = @FirstName,
					MidName = @MiddleName,
					LastName = @LastName,
					Email = @Email,
					DOB = @DOB,
					SSN = @SSN,
					UserTypeID = @UserTypeID,						
					FacilityID = @FacilityID,
					IsActive = @IsActive,
					Nationality=@Nationality,
					Force=@Force,
					Sex=@Sex,
					UIC=@UIC,
					Religion=@Religion,
					FMP=@FMP,
					Race=@Race,
					MOS=@MOS,
					Grade=@Grade,
					canWorkOffline=@canWorkOffline

				WHERE UserGUID = @UserGUID
				
				IF(@UserTypeID = 1)
				BEGIN
					IF(@OldUserTypeID = 1)
					BEGIN
						UPDATE 
							AdminUser 
						SET
							Unit = @Unit
							--RoleID = @RoleID
						WHERE
							UserGUID = @UserGUID	
						
					END
					ELSE
					BEGIN					
						DELETE FROM AHLTAMedicUser WHERE UserGUID =  @UserGUID
						DELETE FROM AHLTAPatientUser WHERE UserGUID =  @UserGUID
						DELETE FROM OMFUser WHERE UserGUID =  @UserGUID	
						INSERT INTO AdminUser
						(
							UserGUID,
							Unit
							--RoleID				
						)
						VALUES
						(
							@UserGUID,
							@Unit
							--@RoleID
						)
						 
					END
				END
				IF(@UserTypeID = 2 or @UserTypeID = 6 or @UserTypeID = 7)
				BEGIN
					IF(@OldUserTypeID = 2 or @OldUserTypeID = 6 or @OldUserTypeID = 7)
					BEGIN
						UPDATE AHLTAMedicUser SET
						Unit = @Unit
						--RoleID = @RoleID
						WHERE
							UserGUID = @UserGUID
					END
					ELSE
					BEGIN
						DELETE FROM AdminUser WHERE UserGUID =  @UserGUID
						DELETE FROM AHLTAPatientUser WHERE UserGUID =  @UserGUID
						DELETE FROM OMFUser WHERE UserGUID =  @UserGUID
						INSERT INTO AHLTAMedicUser
						(
							UserGUID,
							Unit
							--RoleID				
						)
						VALUES
						(
							@UserGUID,
							@Unit
							--@RoleID
						)
					END
				END
				IF(@UserTypeID = 3)
				BEGIN
					Select @OldEicGuid = EicGuid from  AHLTAPatientUser where UserGuid = @UserGuid
					If(@OldEicGuid='' or @OldEicGuid = null)
					Begin
							if(@EICSerialID !='' or @EICSerialID !=null)
							
							Begin
								UPDATE AHLTAPatientUser SET
								Unit = @Unit,
								EICGUID = @EICSerialID
								where UserGUID = @UserGUID	
								UPDATE EicInfo SET IsAssigned = 1 WHERE EICGUID = @EICSerialID
							End
							
					End --end if
					Else 
					Begin
						if(@EICSerialID='' or @EICSerialID=null)
						Begin
							UPDATE EicInfo SET IsAssigned = 0 WHERE EICGUID = @OldEicGuid
							--DELETE FROM AHLTAMedicUser WHERE UserGUID =  @UserGUID
							UPDATE AHLTAPatientUser SET
								Unit = @Unit,
								EICGUID = null
							where UserGUID = @UserGUID							
						End

						Else
						Begin  --else2 begin
							if(@OldEicGUID != @EICSerialID)
							BEGIN
								UPDATE EicInfo SET IsAssigned = 0 WHERE EICGUID = @OldEicGUID
								UPDATE EicInfo SET IsAssigned = 1 WHERE EICGUID = @EICSerialID
--							END
--							IF(@OldUserTypeID = 3)
--							BEGIN
								UPDATE AHLTAPatientUser SET
								Unit = @Unit,
								EICGUID = @EICSerialID
								where UserGUID = @UserGUID						
							END
							
							ELSE
							BEGIN
								DELETE FROM AdminUser WHERE UserGUID =  @UserGUID
								DELETE FROM AHLTAMedicUser WHERE UserGUID =  @UserGUID
								DELETE FROM OMFUser WHERE UserGUID =  @UserGUID
								INSERT INTO AHLTAPatientUser
								(
									UserGUID,					
									EICGUID,
									Unit				
								)
								VALUES
								(
									@UserGUID,					
									@EICSerialID,
									@Unit					
								)
								UPDATE EicInfo SET IsAssigned = 1 WHERE EICGUID = @EICSerialID
							END
						End	 --else2 end
					End
				END
				IF(@UserTypeID = 4 or @UserTypeID = 5)
				BEGIN
					IF(@OldUserTypeID = 4 or @OldUserTypeID = 5)
					BEGIN
						UPDATE OMFUser SET
						Initials = @Initials,
						Category = @Category
						--RoleID  = @RoleID
					WHERE
							UserGUID = @UserGUID
					END
					ELSE
					BEGIN
						DELETE FROM AdminUser WHERE UserGUID =  @UserGUID
						DELETE FROM AHLTAMedicUser WHERE UserGUID =  @UserGUID
						DELETE FROM AHLTAPatientUser WHERE UserGUID =  @UserGUID
						INSERT INTO OMFUser
						(
							UserGUID,					
							Initials,
							Category
							--RoleID				
						)
						VALUES
						(
							@UserGUID,					
							@Initials,
							@Category
							--@RoleID					
						)
					END
				END
				set @CheckUserID = 0
			END
			ELSE
			BEGIN
				set @CheckUserID = 1
			END
		END
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserDetails]    Script Date: 10/16/2008 10:02:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserDetails]
(
	@UserGUID varchar(200)
)
AS
declare @UserTypeId int, @RoleId int, @Unit nvarchar(50), @Initial varchar(50), @Category varchar(50)

select @UserTypeId = UserTypeId from UserInfo where UserGUID=@UserGUID

--IF(@UserTypeId=1 or @UserTypeId=4)
--	BEGIN
--		select @RoleId = RoleID from Roles where UserTypeId = @UserTypeId
--	END
--ELSE IF(@UserTypeId=3)
--	BEGIN
--		set @RoleId = 2 --In case of AHLTA Patient User 
--	END
--
--ELSE IF(@UserTypeId=2)
--	BEGIN
--		select @RoleId = RoleID from AHLTAMedicUser inner join UserInfo on AHLTAMedicUser.UserGUID = UserInfo.UserGUID
--	END
--ELSE IF(@UserTypeId=5)
--	BEGIN
--		select @RoleId = RoleID from Roles where UserTypeId = @UserTypeId
--	END

--For UNIT, Initial, Category
IF(@UserTypeId=1)
BEGIN
	select @Unit = Unit from AdminUser where AdminUser.UserGuid = @UserGUID
	set @Initial = ''
	set @Category = ''
END

IF(@UserTypeId=2 or @UserTypeId=6 or @UserTypeId=7)
BEGIN
	select @Unit = Unit from AHLTAMedicUser where AHLTAMedicUser.UserGuid = @UserGUID
	set @Initial = ''
	set @Category = ''
END

IF(@UserTypeId=3)
BEGIN
	select @Unit = Unit from AHLTAPatientUser where AHLTAPatientUser.UserGuid = @UserGUID
	IF(@Unit = NULL)
		BEGIN
		set @Unit = ''
		END
	set @Initial = ''
	set @Category = ''
END

IF(@UserTypeId=4)
BEGIN
	select @Initial = Initials, @Category = Category  from OMFUser where OMFUser.UserGuid = @UserGUID
	set @Unit = ''
END

IF(@UserTypeId=5)
BEGIN
	select @Unit = Unit from AdminUser where AdminUser.UserGuid = @UserGUID
	set @Initial = ''
	set @Category = ''
END

BEGIN	
	SELECT
		UserGUID, 
		UserID,
		FirstName,
		LastName,
		MidName,
		Email,
		SSN,
		UserTypeID,	
--		@RoleId as RoleID,
		FacilityID,			 
		(Select EICInfo.EICGUID from EICinfo inner join AHLTAPatientUser on EICinfo.EICGUID = AHLTAPatientUser.EICGUID where AHLTAPatientUser.UserGUID = UserInfo.UserGUID) as EICGUID,
		IsActive,
		DOB,
		@Unit as Unit,
		@Initial as Initials,
		@Category as Category,
		Nationality,
		Force,
		Sex,
		UIC,
		Religion,
		FMP,
		Race,
		MOS,
		Grade,
		CanWorkOffline
	FROM 
		UserInfo 

	
	WHERE UserGUID = @UserGUID	

	

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteEICInfo]    Script Date: 10/16/2008 10:02:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteEICInfo]
	@EICGUID varchar(200)
AS

BEGIN
	DELETE FROM AHLTAPatientUser WHERE AHLTAPatientUser.EICGUID = @EICGUID
	DELETE FROM EICInfo WHERE EICInfo.EICGUID = @EICGUID	
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllEICInfo]    Script Date: 10/16/2008 10:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllEICInfo]
	@EICGUID varchar(200)
AS
DECLARE @Unit nchar(50), @UserId varchar(256), @UserGUID varchar(200), @count int

If(@EICGUID!='')
BEGIN
	SELECT @Count = count(UserGUID) FROM AHLTAPatientUser WHERE AHLTAPatientUser.EICGUID = @EICGUID
	IF(@Count > 0)
	BEGIN
		SELECT @UserGUID = UserGUID FROM AHLTAPatientUser WHERE AHLTAPatientUser.EICGUID = @EICGUID
	END
	ELSE IF(@Count <= 0)
	BEGIN
		SELECT @UserGUID = '' FROM AHLTAPatientUser
	END
END

IF(@UserGUID!='')
BEGIN
	SELECT @UserId = UserId FROM UserInfo WHERE UserInfo.UserGUID = @UserGUID
END
ELSE IF(@UserGUID='')
BEGIN
	SELECT @UserId = '' FROM UserInfo
END

IF(@EICGUID!='')
BEGIN
	SELECT @Unit = Unit FROM AHLTAPatientUser WHERE AHLTAPatientUser.EICGUID = @EICGUID
END

BEGIN
	IF(@EICGUID = '')
	BEGIN
	SELECT 
		EICGUID,
		EICSerialID,
		EICName,
		Description,
		IsAssigned,
		'' as UserId,
		'' as Unit,
		'' as UserGUID

	FROM EICInfo
	END

	ELSE IF(@EICGUID != '')
	BEGIN
	SELECT 
		EICGUID,
		EICSerialID,
		EICName,
		Description,
		IsAssigned,
		@UserId as UserId,
		@Unit as Unit,
		@UserGUID as UserGUID

	FROM EICInfo WHERE EICGUID = @EICGUID
	END

END
GO
/****** Object:  StoredProcedure [dbo].[RegisterEIC]    Script Date: 10/16/2008 10:02:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RegisterEIC]

	@EICGuid varchar(200),
	@EICSerialID varchar(50),
	@EICName varchar(50),
	@PubKey varchar(50),
	@PrivKey varchar(50),	
	@Description varchar(50),
	@IsAssigned bit,
	@UserGUID varchar(200),
	@Unit nchar(50),
	@CheckEICSerialID int output
AS

DECLARE
	--@UserGUID varchar(200),
	@nEICGuid varchar(200),
	@nUserGUID varchar(200),
	@OldEICSerialID varchar(50),
	@Count int,
	@UserGUIDCount int,
	@AHLTAPatientCount int

BEGIN
	
	IF(@EICGuid = '')
		BEGIN

			IF(@IsAssigned = 'true' and @UserGUID != '')
			BEGIN
				SELECT @Count = count(EICGuid) FROM EICInfo WHERE EICSerialID = @EICSerialID
				IF(@Count <= 0)
				BEGIN
					SET @CheckEICSerialID = 0
				INSERT INTO EICInfo
					(
						EICSerialID,
						EICName,
						PubKey,
						PrivKey,
						Description,
						IsAssigned					
					)
					VALUES 
					(
						@EICSerialID,
						@EICName,
						@PubKey, 
						@PrivKey,
						@Description, 
						@IsAssigned					
					)
				
			    SELECT @nEICGuid = EICGUID FROM EICInfo WHERE EICSerialID = @EICSerialID
				INSERT INTO AHLTAPatientUser
					(
						UserGUID,					
						EICGUID
						--Unit				
					)
					VALUES
					(
						@UserGUID,					
						@nEICGuid
						--@Unit					
					)
				END
				ELSE
				BEGIN 
					SET @CheckEICSerialID = 1
				END
			END

			IF(@IsAssigned = 'false' and @UserGUID = '')
			BEGIN
				SELECT @Count = count(EICGUID) FROM EICInfo WHERE EICSerialID = @EICSerialID
				IF(@Count <= 0)
				BEGIN
					SET @CheckEICSerialID = 0
				INSERT INTO EICInfo
					(
						EICSerialID,
						EICName,
						PubKey,
						PrivKey,
						Description,
						IsAssigned					
					)
					VALUES 
					(
						@EICSerialID,
						@EICName,
						@PubKey, 
						@PrivKey,
						@Description, 
						@IsAssigned					
					)
				END
				ELSE
				BEGIN 
					SET @CheckEICSerialID = 1
				END
			
			END  
		END

---------------------------
		IF(@EICGuid != '')
		BEGIN
			SELECT @OldEICSerialID = EICSerialID FROM EICInfo WHERE  EICGuid = @EICGuid	
			IF(@OldEICSerialID = @EICSerialID)
			BEGIN
				IF(@IsAssigned = 'true' and @UserGUID != '')
				BEGIN
					
					Update EICInfo SET 
					EICSerialID = @EICSerialID,
					EICName = @EICName,
					PubKey = @PubKey,
					PrivKey = @PrivKey,
					Description = @Description,
					IsAssigned = @IsAssigned
					WHERE EICGUID = @EICGUID
					
					SELECT @UserGUIDCount = count(UserGUID) FROM AHLTAPatientUser WHERE UserGUID = @UserGUID
					
					IF(@UserGUIDCount > 0)
					BEGIN
						Update AHLTAPatientUser SET 
						UserGUID = @UserGUID,
						EICGUID = @EICGUID
						--Unit = @Unit
						WHERE UserGUID = @UserGUID
					END
					ELSE IF(@UserGUIDCount <= 0)
					BEGIN
						INSERT INTO AHLTAPatientUser
						(
							UserGUID,					
							EICGUID
							--Unit				
						)
						VALUES
						(
							@UserGUID,					
							@EICGuid
							--@Unit					
						)
					END
			    END
				IF(@IsAssigned = 'false' and @UserGUID = '')
				BEGIN

					Update EICInfo SET 
					EICSerialID = @EICSerialID,
					EICName = @EICName,
					PubKey = @PubKey,
					PrivKey = @PrivKey,
					Description = @Description,
					IsAssigned = @IsAssigned
					WHERE EICGUID = @EICGUID

					SELECT @UserGUIDCount = count(UserGUID) FROM AHLTAPatientUser WHERE EICGUID = @EICGUID					
					IF(@UserGUIDCount > 0)
					BEGIN
						UPDATE AHLTAPatientUser SET EICGUID = null WHERE EICGUID = @EICGUID	
						--DELETE FROM AHLTAPatientUser WHERE EICGUID =  @EICGUID
					END					
					
				END
				SET @CheckEICSerialID = 0
			END --old eic end

			ELSE --new eic
				SELECT @Count = count(EICGUID) FROM EICInfo WHERE EICSerialID = @EICSerialID
			BEGIN 
				IF(@Count <= 0)
			BEGIN
				IF(@IsAssigned = 'true' and @UserGUID != '')
				BEGIN
					
					Update EICInfo SET 
					EICSerialID = @EICSerialID,
					EICName = @EICName,
					PubKey = @PubKey,
					PrivKey = @PrivKey,
					Description = @Description,
					IsAssigned = @IsAssigned
					WHERE EICGUID = @EICGUID
					
					SELECT @UserGUIDCount = count(UserGUID) FROM AHLTAPatientUser WHERE UserGUID = @UserGUID
									
					IF(@UserGUIDCount > 0)
					BEGIN
						Update AHLTAPatientUser SET 
						UserGUID = @UserGUID,
						EICGUID = @EICGUID
						--Unit = @Unit
						WHERE EICGUID = @EICGUID
					END
					ELSE IF(@UserGUIDCount <= 0)
					BEGIN
						INSERT INTO AHLTAPatientUser
						(
							UserGUID,					
							EICGUID
							--Unit				
						)
						VALUES
						(
							@UserGUID,					
							@EICGuid
							--@Unit					
						)
					END
					
				END
				
				IF(@IsAssigned = 'false' and @UserGUID = '')
				BEGIN

					Update EICInfo SET 
					EICSerialID = @EICSerialID,
					EICName = @EICName,
					PubKey = @PubKey,
					PrivKey = @PrivKey,
					Description = @Description,
					IsAssigned = @IsAssigned
					WHERE EICGUID = @EICGUID

					SELECT @UserGUIDCount = count(UserGUID) FROM AHLTAPatientUser WHERE EICGUID = @EICGUID					
					IF(@UserGUIDCount > 0)
					BEGIN
						UPDATE AHLTAPatientUser SET EICGUID = null WHERE EICGUID = @EICGUID	
						--DELETE FROM AHLTAPatientUser WHERE EICGUID =  @EICGUID
					END

				END
				SET @CheckEICSerialID = 0
				
			END--count end
			IF(@Count > 0)
			BEGIN
				SET @CheckEICSerialID = 1
			END
			
		END--else end
		
END--guid end

	END -- main end
GO
/****** Object:  StoredProcedure [dbo].[GetConfigurationFacilityInfo]    Script Date: 10/16/2008 10:02:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetConfigurationFacilityInfo]
(
	@FacilityId int
)
AS
BEGIN
	SELECT  FacilityId,
			Name,
			FacilityInfo.Address1 + ' ' + FacilityInfo.Address2 + ' ' + FacilityInfo.State 	AS Address,
			'' as Contactno,
			FacilityTypeId as Type,
			(select serveruri from serverinfo) as ServerUri,
			(select ServerPubKey from serverinfo) as ServerPublicKey,
			(select ServerPrivKey from serverinfo) as ServerPrivateKey
			FROM FacilityInfo WHERE FacilityInfo.FacilityId = @FacilityId

	

END
GO
/****** Object:  StoredProcedure [dbo].[SetDefaultPassword]    Script Date: 10/16/2008 10:02:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetDefaultPassword]
(	
	@Password varchar(50),
	@FacilityId int
)
AS

Declare @OldPassword varchar(1024), @nFacilityId int

select @OldPassword = DefaultPassword from FacilityInfo where FacilityId = @FacilityId
--select @nFacilityId = FacilityId from DefaultPassword where @FacilityId = @FacilityId

BEGIN
	UPDATE FacilityInfo SET DefaultPassword = @Password WHERE FacilityId = @FacilityId
END
GO
/****** Object:  StoredProcedure [dbo].[GetDefaultPassword]    Script Date: 10/16/2008 10:02:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDefaultPassword]
(
	@FacilityId int
)
AS
BEGIN
	SELECT DefaultPassword
	FROM FacilityInfo WHERE FacilityId = @FacilityId
END
GO
/****** Object:  StoredProcedure [dbo].[GetFacilityKeys]    Script Date: 10/16/2008 10:02:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFacilityKeys]
AS
BEGIN	
	SELECT	Top 1 
		FacilityPrivateKey as PrivKey	,
		FacilityPublicKey as PubKey
		
	FROM FacilityInfo 
	WHERE IsActive = 1
END
GO
/****** Object:  ForeignKey [FK_AdminUser_UserInfo]    Script Date: 10/16/2008 10:02:46 ******/
ALTER TABLE [dbo].[AdminUser]  WITH CHECK ADD  CONSTRAINT [FK_AdminUser_UserInfo] FOREIGN KEY([UserGUID])
REFERENCES [dbo].[UserInfo] ([UserGUID])
GO
ALTER TABLE [dbo].[AdminUser] CHECK CONSTRAINT [FK_AdminUser_UserInfo]
GO
/****** Object:  ForeignKey [FK_AHLTAMedicUser_UserInfo]    Script Date: 10/16/2008 10:02:48 ******/
ALTER TABLE [dbo].[AHLTAMedicUser]  WITH CHECK ADD  CONSTRAINT [FK_AHLTAMedicUser_UserInfo] FOREIGN KEY([UserGUID])
REFERENCES [dbo].[UserInfo] ([UserGUID])
GO
ALTER TABLE [dbo].[AHLTAMedicUser] CHECK CONSTRAINT [FK_AHLTAMedicUser_UserInfo]
GO
/****** Object:  ForeignKey [FK_AHLTAPatientUser_UserInfo]    Script Date: 10/16/2008 10:02:50 ******/
ALTER TABLE [dbo].[AHLTAPatientUser]  WITH CHECK ADD  CONSTRAINT [FK_AHLTAPatientUser_UserInfo] FOREIGN KEY([UserGUID])
REFERENCES [dbo].[UserInfo] ([UserGUID])
GO
ALTER TABLE [dbo].[AHLTAPatientUser] CHECK CONSTRAINT [FK_AHLTAPatientUser_UserInfo]
GO
/****** Object:  ForeignKey [FK_CategoryAssociation_FaciltyAssociation]    Script Date: 10/16/2008 10:02:53 ******/
ALTER TABLE [dbo].[CategoryAssociation]  WITH CHECK ADD  CONSTRAINT [FK_CategoryAssociation_FaciltyAssociation] FOREIGN KEY([AssociationId])
REFERENCES [dbo].[FacilityAssociation] ([AssociationId])
GO
ALTER TABLE [dbo].[CategoryAssociation] CHECK CONSTRAINT [FK_CategoryAssociation_FaciltyAssociation]
GO
/****** Object:  ForeignKey [FK_EncounterInfo_AHLTAPatientUser]    Script Date: 10/16/2008 10:03:10 ******/
ALTER TABLE [dbo].[EncounterInfo]  WITH CHECK ADD  CONSTRAINT [FK_EncounterInfo_AHLTAPatientUser] FOREIGN KEY([PatientID])
REFERENCES [dbo].[AHLTAPatientUser] ([UserGUID])
GO
ALTER TABLE [dbo].[EncounterInfo] CHECK CONSTRAINT [FK_EncounterInfo_AHLTAPatientUser]
GO
/****** Object:  ForeignKey [FK_FacilityInfo_FacilityType]    Script Date: 10/16/2008 10:03:22 ******/
ALTER TABLE [dbo].[FacilityInfo]  WITH CHECK ADD  CONSTRAINT [FK_FacilityInfo_FacilityType] FOREIGN KEY([FacilityTypeID])
REFERENCES [dbo].[FacilityType] ([FacilityTypeID])
GO
ALTER TABLE [dbo].[FacilityInfo] CHECK CONSTRAINT [FK_FacilityInfo_FacilityType]
GO
/****** Object:  ForeignKey [FK_FacAssoPerm_Constraints]    Script Date: 10/16/2008 10:03:27 ******/
ALTER TABLE [dbo].[FacilityPerm]  WITH CHECK ADD  CONSTRAINT [FK_FacAssoPerm_Constraints] FOREIGN KEY([ConsrtID])
REFERENCES [dbo].[Constraints] ([ConsrtID])
GO
ALTER TABLE [dbo].[FacilityPerm] CHECK CONSTRAINT [FK_FacAssoPerm_Constraints]
GO
/****** Object:  ForeignKey [FK_FacAssoPerm_FacilityAssociationInfo]    Script Date: 10/16/2008 10:03:28 ******/
ALTER TABLE [dbo].[FacilityPerm]  WITH NOCHECK ADD  CONSTRAINT [FK_FacAssoPerm_FacilityAssociationInfo] FOREIGN KEY([FacAssoID])
REFERENCES [dbo].[FacilityPermissionInfo] ([FacAssoID])
GO
ALTER TABLE [dbo].[FacilityPerm] CHECK CONSTRAINT [FK_FacAssoPerm_FacilityAssociationInfo]
GO
/****** Object:  ForeignKey [FK_FacAssoPerm_Permission]    Script Date: 10/16/2008 10:03:28 ******/
ALTER TABLE [dbo].[FacilityPerm]  WITH CHECK ADD  CONSTRAINT [FK_FacAssoPerm_Permission] FOREIGN KEY([PermID])
REFERENCES [dbo].[Permission] ([PermID])
GO
ALTER TABLE [dbo].[FacilityPerm] CHECK CONSTRAINT [FK_FacAssoPerm_Permission]
GO
/****** Object:  ForeignKey [FK_FacilityAssociationInfo_FacilityInfo]    Script Date: 10/16/2008 10:03:32 ******/
ALTER TABLE [dbo].[FacilityPermissionInfo]  WITH CHECK ADD  CONSTRAINT [FK_FacilityAssociationInfo_FacilityInfo] FOREIGN KEY([FacilityId])
REFERENCES [dbo].[FacilityInfo] ([FacilityID])
GO
ALTER TABLE [dbo].[FacilityPermissionInfo] CHECK CONSTRAINT [FK_FacilityAssociationInfo_FacilityInfo]
GO
/****** Object:  ForeignKey [FK_FacilityAssociationInfo_UserType]    Script Date: 10/16/2008 10:03:32 ******/
ALTER TABLE [dbo].[FacilityPermissionInfo]  WITH CHECK ADD  CONSTRAINT [FK_FacilityAssociationInfo_UserType] FOREIGN KEY([UserTypeId])
REFERENCES [dbo].[UserType] ([UserTypeID])
GO
ALTER TABLE [dbo].[FacilityPermissionInfo] CHECK CONSTRAINT [FK_FacilityAssociationInfo_UserType]
GO
/****** Object:  ForeignKey [FK_OMFUser_UserInfo]    Script Date: 10/16/2008 10:03:38 ******/
ALTER TABLE [dbo].[OMFUser]  WITH CHECK ADD  CONSTRAINT [FK_OMFUser_UserInfo] FOREIGN KEY([UserGUID])
REFERENCES [dbo].[UserInfo] ([UserGUID])
GO
ALTER TABLE [dbo].[OMFUser] CHECK CONSTRAINT [FK_OMFUser_UserInfo]
GO
/****** Object:  ForeignKey [FK_UserInfo_FacilityInfo]    Script Date: 10/16/2008 10:04:01 ******/
ALTER TABLE [dbo].[UserInfo]  WITH CHECK ADD  CONSTRAINT [FK_UserInfo_FacilityInfo] FOREIGN KEY([FacilityID])
REFERENCES [dbo].[FacilityInfo] ([FacilityID])
GO
ALTER TABLE [dbo].[UserInfo] CHECK CONSTRAINT [FK_UserInfo_FacilityInfo]
GO
/****** Object:  ForeignKey [FK_UserInfo_UserType]    Script Date: 10/16/2008 10:04:01 ******/
ALTER TABLE [dbo].[UserInfo]  WITH CHECK ADD  CONSTRAINT [FK_UserInfo_UserType] FOREIGN KEY([UserTypeID])
REFERENCES [dbo].[UserType] ([UserTypeID])
GO
ALTER TABLE [dbo].[UserInfo] CHECK CONSTRAINT [FK_UserInfo_UserType]
GO
