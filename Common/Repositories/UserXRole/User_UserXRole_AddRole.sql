INSERT INTO [user].[UserXRole]
           ([CreateDate]
           ,[CreateUser_Id]
           ,[ModifyDate]
           ,[ModifyUser_Id]
           ,[IsActive]
           ,[User_Id]
           ,[Role_Id])
VALUES
           (GETDATE(),       -- CreateDate (obecny czas)
           1,               -- CreateUser_Id (przyk³adowy ID u¿ytkownika tworz¹cego)
           NULL,            -- ModifyDate (brak modyfikacji przy tworzeniu)
           NULL,            -- ModifyUser_Id (brak modyfikacji przy tworzeniu)
           1,               -- IsActive (1 = aktywny)
           @UserId,             -- User_Id (przyk³adowy ID u¿ytkownika)
           @RoleId);              -- Role_Id (przyk³adowy ID roli)