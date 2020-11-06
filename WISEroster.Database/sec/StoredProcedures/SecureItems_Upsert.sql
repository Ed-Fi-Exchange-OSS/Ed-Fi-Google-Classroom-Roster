CREATE PROCEDURE [sec].[SecureItems_Upsert]
	@ItemKey   NVARCHAR(100),
	@ItemValue NVARCHAR(1024) = null
AS
	Begin

		OPEN SYMMETRIC KEY [PII_symkey_sec]
		DECRYPTION BY CERTIFICATE [PII_cert_WISEroster];
		
		If Exists(SELECT 1 from [sec].[SecureItems] WHERE  ItemKey = @ItemKey)
		Begin
			UPDATE [sec].[SecureItems]
			SET [ItemValue] = ENCRYPTBYKEY(KEY_GUID('PII_symkey_sec'), @ItemValue)
			WHERE [ItemKey] = @ItemKey;
		End
		Else
		Begin
			INSERT INTO [sec].[SecureItems] (ItemKey, ItemValue)
			VALUES (@ItemKey, ENCRYPTBYKEY(KEY_GUID('PII_symkey_sec'), @ItemValue));
		End
		
		CLOSE SYMMETRIC KEY [PII_symkey_sec];

	End