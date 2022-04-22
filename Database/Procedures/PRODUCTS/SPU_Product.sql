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

CREATE PROCEDURE SPU_WaveShop_Product
(
	@id INT,
	@name NVARCHAR (500),
    @description  NVARCHAR (100),
	@stockQuantity INT,
	@unitPrice FLOAT,
    @status  NVARCHAR (100), -- Available | Unavailable
	@idCategory INT,
	@photoAddress  NVARCHAR (100) = NULL
)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [Product] WHERE id=@id)
		BEGIN 
			BEGIN
				BEGIN TRY 
					BEGIN TRANSACTION;

					UPDATE [Product] SET
						name = @name,
						description = @description,
						photoAddress = @photoAddress,
						stockQuantity = @stockQuantity,
						unitPrice = @unitPrice,
						status = @status,
						idCategory = @idCategory
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
			RETURN 'the user does not exist';
		END
END