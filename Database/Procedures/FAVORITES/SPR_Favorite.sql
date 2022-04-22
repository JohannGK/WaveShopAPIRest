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

CREATE PROCEDURE SPR_WaveShop_Favorite
(
	@idAccount INT
)
AS
BEGIN
	SELECT * FROM [Product] INNER JOIN (SELECT idProduct FROM [Favorite] WHERE idAccount = @idAccount) Subquery ON [Subquery].[idProduct] = [Product].[id]
END