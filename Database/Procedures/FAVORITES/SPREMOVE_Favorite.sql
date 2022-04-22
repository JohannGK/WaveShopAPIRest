USE WaveShop;
Go

-- procedure definition

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author: Josué Alarcón
-- Create date: 21-02-2022
-- Description: 

CREATE PROCEDURE SPREMOVE_WaveShop_Favorite
(
    @id INT
)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [Favorite] WHERE id = @id)
		BEGIN
			BEGIN TRY 

				BEGIN TRANSACTION;

					DELETE FROM [Favorite] WHERE 
					(
						[Favorite].[id] = @id
					)

				COMMIT TRANSACTION;
	
			END TRY
			BEGIN CATCH
				RETURN 'Error Number: ' + CAST(ERROR_NUMBER() AS VARCHAR(10)) + '; ' + Char(10) +
				'Error Severity: ' + CAST(ERROR_SEVERITY() AS VARCHAR(10)) + '; ' + Char(10) +
				'Error State: ' + CAST(ERROR_STATE() AS VARCHAR(10)) + '; ' + Char(10) +
				'Error Line: ' + CAST(ERROR_LINE() AS VARCHAR(10)) + '; ' + Char(10) +
				'Error Message: ' + ERROR_MESSAGE()
				ROLLBACK TRANSACTION
			END CATCH
		END
	ELSE 
		BEGIN
			RETURN 'The id does not exist'
		END
END