USE [PolicyTestDB]
GO
/****** Object:  StoredProcedure [dbo].[GetUserPermissions]    Script Date: 24-01-2024 22:45:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
 Purpose: Gets the user roles and permissions

 EX:
  EXEC GetUserPermissions '2'

*/

ALTER PROC [dbo].[GetUserPermissions]
	(
		@UserId NVARCHAR(256)
	)
AS
BEGIN
	SELECT
		pol.id [PolicyId]
		, pol.Name [PolicyName]
		, r.Id [RoleId]
		, r.RoleName
		, per.Id [PermisionId]
		, per.Name [PermissionName]
		, u.Id [UserId]
		, app.Id [AppId]
		, app.Name [AppName]
	FROM Users u 
	INNER JOIN UserRoles ur 
		ON u.Id=ur.UserId AND U.IsDeleted=0
	INNER JOIN Roles r 
		ON ur.RoleId = r.Id AND ur.IsDeleted=0
	LEFT JOIN AppPolicies ap 
		ON ur.RoleId=ap.RoleId AND r.PolicyId=ap.PolicyId AND ap.RoleId = ur.RoleId AND ap.IsDeleted=0
	LEFT JOIN Policies pol 
		ON ap.PolicyId=pol.Id AND pol.IsDeleted=0
	LEFT JOIN Applications app 
		ON app.Id=pol.ApplicationId AND app.IsDeleted=0
	LEFT JOIN [Permissions] per 
		ON per.PolicyId=pol.Id AND per.Id=ap.PermissionId AND per.IsDeleted=0
	WHERE
		U.UserId = @UserId;
END

