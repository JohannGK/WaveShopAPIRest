USE WaveShop;
Go

-- procedure definition

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author: Josu? Alarc?n
-- Create date: 21-02-2022
-- Description: 

CREATE PROCEDURE SPR_WaveShop_Opinion
(
	@id INT = NULL
)
AS
BEGIN
	SELECT * FROM [Opinion] WHERE [id] = ISNULL(@id, id)
END