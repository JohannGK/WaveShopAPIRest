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

CREATE PROCEDURE SPPURCHASE_WaveShop_DirectOrder
(
    @idAccount INT,
	@idProduct INT,
	@price FLOAT,
	@quantity INT
)
AS
BEGIN
	DECLARE @total FLOAT;
	DECLARE @currentQuantity INT;
	SET @currentQuantity = ((SELECT stockQuantity FROM [Product] WHERE [Product].[id] = @idProduct) - @quantity);
	SET @total = (@price * @quantity)
	IF @quantity <= (SELECT stockQuantity FROM [Product] WHERE id = @idProduct)
	BEGIN
		BEGIN TRY 
			BEGIN TRANSACTION;

				INSERT INTO [Order] VALUES
				(
					@idAccount,
					-1,
					GETDATE(),
					GETDATE(),
					'Requested',
					@total
				)

				INSERT INTO [ProductSelectedOrder] VALUES
				(
					@total,
					@quantity,
					'Shopped',
					@idProduct,
					SCOPE_IDENTITY()
				)

				IF @currentQuantity = 0 
					BEGIN
						UPDATE [Product] SET
							stockQuantity = @currentQuantity,
							status = 'Unavaliable'
						WHERE id = @idProduct;

						DELETE FROM [ProductSelectedCart] 
						WHERE idProduct = @idProduct
					END
				ELSE
					BEGIN	
						UPDATE [Product] SET
							stockQuantity = @currentQuantity
						WHERE id = @idProduct;

						UPDATE [ProductSelectedCart] SET 
							quantity = @currentQuantity
						WHERE idProduct = @idProduct AND quantity > @currentQuantity
					END
				

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
			RETURN 'Not enough quantity of the product'
		END
END