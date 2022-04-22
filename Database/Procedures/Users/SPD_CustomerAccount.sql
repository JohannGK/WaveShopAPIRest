USE WaveShop;
Go

-- procedure definition

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author: Josué Alarcón
-- Create date: 21-02-2022
-- Description: Reactive account of the user account

CREATE PROCEDURE SPD_WaveShop_CustomerAccount
(
	@id INT
)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [Account] WHERE id=@id)
		BEGIN 
			BEGIN TRY 
				UPDATE [Account] SET
					status = 'Banned'
				WHERE id = @id
			END TRY
			BEGIN CATCH
				RETURN 'Error Number: ' + CAST(ERROR_NUMBER() AS VARCHAR(10)) + '; ' + Char(10) +
				'Error Severity: ' + CAST(ERROR_SEVERITY() AS VARCHAR(10)) + '; ' + Char(10) +
				'Error State: ' + CAST(ERROR_STATE() AS VARCHAR(10)) + '; ' + Char(10) +
				'Error Line: ' + CAST(ERROR_LINE() AS VARCHAR(10)) + '; ' + Char(10) +
				'Error Message: ' + ERROR_MESSAGE()
			END CATCH
		END
	ELSE
		BEGIN 
			RETURN ' Username ' + @id + ' does not exist';
		END
END