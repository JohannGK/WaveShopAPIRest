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

CREATE PROCEDURE SPU_WaveShop_Opinion
(
	@id INT,
	@content NVARCHAR (500),
    @photoAddress NVARCHAR (100) = '',
	@visible NVARCHAR (1)
)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [Opinion] WHERE id=@id)
		BEGIN 
			BEGIN
				BEGIN TRY 
					BEGIN TRANSACTION;

					UPDATE [Opinion] SET
						content = @content,
						photoAddress = @photoAddress,
						visible = @visible
					WHERE id = @id

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
		END
	ELSE
		BEGIN 
			RETURN 'the opinion does not exist';
		END
END