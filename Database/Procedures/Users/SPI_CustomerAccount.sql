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

CREATE PROCEDURE SPI_WaveShop_CustomerAccount
(
	@email NVARCHAR (500),
    @name NVARCHAR (500),
    @phone NVARCHAR (100),
	@userName NVARCHAR (100),
	@userType NVARCHAR (100),
    @password NVARCHAR (100)
)
AS
BEGIN
	BEGIN TRY 
		BEGIN TRANSACTION;

			INSERT INTO [Customer] VALUES
			(
				@email,
				@name,
				@phone
			)

			INSERT INTO [Account] VALUES
			(
				SCOPE_IDENTITY(),
				@userName,
				@userType,
				@password,
				'Offline',
				'Neutral',
				GETDATE()
			)

			INSERT INTO [ShoppingCart] VALUES
			(
				0,
				0,
				GETDATE(),
				SCOPE_IDENTITY()
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