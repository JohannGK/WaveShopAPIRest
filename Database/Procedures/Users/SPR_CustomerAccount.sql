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

CREATE PROCEDURE SPR_WaveShop_CustomerAccount
(
	@id INT = NULL,
	@userName VARCHAR(100) = NULL
)
AS
BEGIN
	SELECT * FROM [Account] WHERE [id] = ISNULL(@id, id) AND
	userName = ISNULL(@userName, userName);
END