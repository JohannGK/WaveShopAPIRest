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

CREATE PROCEDURE SPR_WaveShop_Product
(
	@id INT = NULL,
	@name VARCHAR(500) = NULL
)
AS
BEGIN
	SELECT * FROM [Product] WHERE [id] = ISNULL(@id, id) AND
	name = ISNULL(@name, name);
END