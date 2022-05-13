USE [WaveShop]
GO
/****** Object:  StoredProcedure [dbo].[SPG_SAP_Insumo]    Script Date: 4/4/2022 22:59:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SPGUsers] 
	@Valor NVARCHAR (100) = NULL,
	@Status NVARCHAR (100)
AS
BEGIN
	SELECT * FROM [User] WHERE UserName = ISNULL(@Valor, UserName) AND Status = ISNULL(@Status, Status);
END
