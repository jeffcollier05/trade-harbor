SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Inserts linked account information
-- =============================================
CREATE PROCEDURE [dbo].[InsertLinkedAccountInformation] 
	@UserId nvarchar(450),
	@FirstName varchar(20),
	@LastName varchar(20),
	@ProfilePictureUrl varchar(1000)
AS
BEGIN
	INSERT INTO dbo.Accounts
		(User_id, FirstName, LastName, ProfilePictureUrl)
	VALUES
		(@UserId, @FirstName, @LastName, @ProfilePictureUrl);
END
