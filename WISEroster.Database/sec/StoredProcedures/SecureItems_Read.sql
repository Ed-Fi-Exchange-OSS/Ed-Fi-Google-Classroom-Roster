CREATE PROCEDURE [sec].[SecureItems_Read]
	@ItemKey   NVARCHAR(100)
AS
	Begin
		
		OPEN SYMMETRIC KEY [PII_symkey_sec]
		DECRYPTION BY CERTIFICATE [PII_cert_WISEroster];
	
		SELECT
		  [ItemKey],
		  CONVERT(NVARCHAR(1024), DECRYPTBYKEY(ItemValue)) AS [ItemValue]
		FROM
		  [sec].[SecureItems]
		 WHERE ItemKey = @ItemKey;
		
		CLOSE SYMMETRIC KEY [PII_symkey_sec];
		
	End