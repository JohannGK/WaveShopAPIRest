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

CREATE PROCEDURE SPU_WaveShop_CustomerAccount
(
	@id INT,
	@email NVARCHAR (500),
    @name NVARCHAR (500),
    @phone NVARCHAR (100),
	@userName NVARCHAR (100),
	@userType NVARCHAR (100),
    @password NVARCHAR (100)
)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [Account] WHERE id=@id)
		BEGIN 
			BEGIN
				BEGIN TRY 
					BEGIN TRANSACTION;

					UPDATE [Customer] SET
						email = @email,
						name = @name,
						phone = @phone
					WHERE id = (SELECT Account.idCustomer FROM [Account] WHERE id=@id);

					UPDATE [Account] SET
						userName = @userName,
						userType = @userType,
						password = @password
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