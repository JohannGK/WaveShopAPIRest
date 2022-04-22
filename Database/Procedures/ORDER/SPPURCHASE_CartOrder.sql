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

CREATE PROCEDURE SPPURCHASE_WaveShop_CartOrder
(
    @idAccount INT,
	@idShoppinCart INT
)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [ProductSelectedCart] WHERE idShoppingCart = @idShoppinCart)
		BEGIN
					-- Variables scope
			DECLARE @total FLOAT = 0;
			DECLARE @count INT = 0;
			DECLARE @idProduct INT;
			DECLARE @quantity INT;
			DECLARE @price FLOAT;
			DECLARE @stock INT;
			DECLARE @idCart INT;
			DECLARE @currentStock INT;
			DECLARE @orderIdentity INT;


			BEGIN TRY 
				BEGIN TRANSACTION;

					CREATE TABLE #ProductCLONE
					(
						idProduct INT,
						quantity INT,
						price FLOAT,
						stock INT,
						idCart INT
					)

					INSERT INTO #ProductCLONE SELECT Product.id, Subquery.quantity, Product.unitPrice, Product.stockQuantity, Subquery.id  FROM [Product] INNER JOIN (SELECT * FROM [ProductSelectedCart] WHERE idShoppingCart = @idShoppinCart) Subquery ON Subquery.idProduct = Product.id;
					SELECT @count = COUNT(*) FROM #ProductCLONE;

					INSERT INTO [Order] VALUES
					(
						@idAccount,
						@idShoppinCart,
						GETDATE(),
						GETDATE(),
						'Requested',
						@total
					)
					SET @orderIdentity = SCOPE_IDENTITY();

					WHILE @count > 0
					BEGIN
						SELECT TOP(1) @idProduct = idProduct, @quantity = quantity, @price = price, @stock = stock, @idCart = idCart FROM #ProductCLONE;
						INSERT INTO [ProductSelectedOrder] VALUES
						(
							@price * @quantity,
							@quantity,
							'Shopped',
							@idProduct,
							@orderIdentity
						)
						SET @currentStock= @stock - @quantity;

						IF @currentStock = 0 
							BEGIN
								UPDATE [Product] SET
									stockQuantity = @currentStock,
									status = 'Unavaliable'
								WHERE id = @idProduct;

								DELETE FROM [ProductSelectedCart] 
								WHERE idProduct = @idProduct
							END
						ELSE
							BEGIN	
								UPDATE [Product] SET
									stockQuantity = @currentStock
								WHERE id = @idProduct;

								UPDATE [ProductSelectedCart] SET 
									quantity = @currentStock
								WHERE idProduct = @idProduct AND quantity > @currentStock
							END

						DELETE FROM [ProductSelectedCart] WHERE id = @idCart;
						DELETE TOP (1) FROM #ProductCLONE;
						SELECT @count = COUNT(*) FROM #ProductCLONE;

					END

					DROP TABLE #ProductCLONE;

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
			RETURN 'Error, there are not enough products in the shopping cart'
		END	
END