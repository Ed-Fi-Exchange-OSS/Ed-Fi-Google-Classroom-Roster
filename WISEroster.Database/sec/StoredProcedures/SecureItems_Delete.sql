CREATE PROCEDURE [sec].[SecureItems_Delete]
	@ItemKey   NVARCHAR(100)
AS
	Begin

		DELETE FROM [sec].[SecureItems]
		WHERE ItemKey = @ItemKey;
		
	End