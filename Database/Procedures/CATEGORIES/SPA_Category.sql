USE WaveShop;
Go

-- procedure definition

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author: Josu? Alarc?n
-- Create date: 21-02-2022
-- Description:

CREATE PROCEDURE SPA_WaveShop_Category
(
	@id INT
)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [Category] WHERE id=@id)
		BEGIN 
			BEGIN TRY 
				UPDATE [Category] SET
					status = 'Visible'
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
			RETURN ' ID ' + @id + ' does not exist';
		END
END