SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/14/2023
-- Description:	Gets statistics about a user
-- =============================================
CREATE PROCEDURE [dbo].[GetUserStatistics] 
	@UserId nvarchar(450)
AS
BEGIN
	DECLARE @FriendCount int;
	SET @FriendCount = (
		SELECT Count(*)
		FROM [TradeHarbor].[dbo].[FriendPairs]
		where Person1Id = @UserId OR Person2Id = @UserId
	);

	DECLARE @PostCount int;
	SET @PostCount = (
		SELECT count(*) AS PostCount
		FROM [TradeHarbor].[dbo].[Trades]
		where User_id = @UserId
	);

	DECLARE @Downvotes int;
	SET @Downvotes = (
		SELECT count(*)
		FROM [TradeHarbor].[dbo].[PostReactions] p
		join dbo.Trades t on t.Trade_id = p.PostId
		where User_id = @UserId
			and ReactionType = 'DOWNVOTE'
	);

	DECLARE @Upvotes int;
	SET @Upvotes = (
		SELECT count(*)
		FROM [TradeHarbor].[dbo].[PostReactions] p
		join dbo.Trades t on t.Trade_id = p.PostId
		where User_id = @UserId
			and ReactionType = 'UPVOTE'
	);

	SELECT
		@FriendCount as FriendCount,
		@PostCount as PostCount,
		@Downvotes as Downvotes,
		@Upvotes as Upvotes
END
