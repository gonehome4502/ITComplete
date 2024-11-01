﻿/*
Deployment script for InterviewTestDB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "InterviewTestDB"
:setvar DefaultFilePrefix "InterviewTestDB"
:setvar DefaultDataPath "C:\Users\travi\Documents\Meyer Distributing Exam - Mid\Database\MSSQL16.SQLEXPRESS\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Users\travi\Documents\Meyer Distributing Exam - Mid\Database\MSSQL16.SQLEXPRESS\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
/*
The column [dbo].[Orders].[OrderCartId] is being dropped, data loss could occur.
*/

IF EXISTS (select top 1 1 from [dbo].[Orders])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
PRINT N'Altering Table [dbo].[Orders]...';


GO
ALTER TABLE [dbo].[Orders] DROP COLUMN [OrderCartId];


GO
PRINT N'Update complete.';


GO
