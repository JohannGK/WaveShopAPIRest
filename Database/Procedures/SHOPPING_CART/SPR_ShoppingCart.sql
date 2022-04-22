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

CREATE PROCEDURE SPR_WaveShop_ShoppingCart
(
	@id INT 
)
AS
BEGIN
	SELECT [Product].* FROM (SELECT [idProduct] FROM [ProductSelectedCart] 
		WHERE [idShoppingCart] = @id) Subquery INNER JOIN 
			[Product] ON [Subquery].[idProduct] = [Product].[id];
END