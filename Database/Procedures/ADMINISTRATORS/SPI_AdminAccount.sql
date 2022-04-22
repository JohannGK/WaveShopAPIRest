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

CREATE PROCEDURE SPI_WaveShop_AdminAccount
(
	@email NVARCHAR (500),
	@userName NVARCHAR (100),
    @password NVARCHAR (100), 
    @name NVARCHAR (100),
	@phone NVARCHAR (100)
)
AS
BEGIN
	BEGIN TRY 
		BEGIN TRANSACTION;

			INSERT INTO [AdminAccount] VALUES 
			(
				@email,
				@userName,
				@password,
				@name,
				@phone,
				'Offline',
				GETDATE()
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