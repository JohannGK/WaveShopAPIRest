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

CREATE PROCEDURE SPI_WaveShop_Opinion
(
	@userName NVARCHAR (100),
	@content NVARCHAR (500),
    @photoAddress NVARCHAR (100) = '',
	@idProduct INT
)
AS
BEGIN
	BEGIN TRY 
		BEGIN TRANSACTION;

			INSERT INTO [Opinion] VALUES 
			(
				@userName,
				@content,
				'Y',
				@photoAddress,
				0,
				0,
				GETDATE(),
				@idProduct
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