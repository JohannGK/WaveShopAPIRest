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

CREATE PROCEDURE SPR_WaveShop_Category
(
	@id INT = NULL,
	@name VARCHAR(100) = NULL
)
AS
BEGIN
	SELECT * FROM [Category] WHERE [id] = ISNULL(@id, id) AND
	name = ISNULL(@name , name);
END