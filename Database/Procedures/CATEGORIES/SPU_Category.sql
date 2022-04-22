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

CREATE PROCEDURE SPU_WaveShop_Category
(
	@id INT,
	@name NVARCHAR (100),
    @description  NVARCHAR (500),
	@status  NVARCHAR (500)
)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [Category] WHERE id=@id)
		BEGIN 
			BEGIN
				BEGIN TRY 
					BEGIN TRANSACTION;

					UPDATE [Category] SET
						name = @name,
						description = @description,
						status = @status
					WHERE id = @id;

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
			RETURN 'the user does not exist';
		END
END