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

ALTER PROCEDURE SPI_WaveShop_Product
(
    @name NVARCHAR (500),
    @description  NVARCHAR (100),
	@stockQuantity INT,
	@unitPrice FLOAT,
    @status  NVARCHAR (100), -- Available | Unavailable
	@country NVARCHAR (100),
	@location NVARCHAR (500),
	@idCategory INT,
	@idAccountVendor INT,
	@photoAddress  NVARCHAR (100) = NULL,
	@videoAddress NVARCHAR (100) = NULL
)
AS
BEGIN
	BEGIN TRY 
		BEGIN TRANSACTION;

			INSERT INTO [Product] VALUES
			(
				@name,
				@description,
				@photoAddress,
				@videoAddress,
				@stockQuantity,
				@unitPrice,
				@status,
				GETDATE(),
				@country,
				@location,
				@idCategory,
				@idAccountVendor
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