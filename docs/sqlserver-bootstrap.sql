IF DB_ID(N'DataBase') IS NULL
BEGIN
    CREATE DATABASE [DataBase];
END;
GO

USE [DataBase];
GO

PRINT N'DataBase database is ready. Then run docs/sqlserver-create-database.sql to create tables.';
GO
