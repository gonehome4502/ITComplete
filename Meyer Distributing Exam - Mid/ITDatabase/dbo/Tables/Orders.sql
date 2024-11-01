CREATE TABLE [dbo].[Orders] (
    [OrderId]       INT           IDENTITY (1, 1) NOT NULL,
    [OrderNumber]   NVARCHAR (50) NULL,
    [Customer]      NVARCHAR (50) NULL,
    [ProcessedDate] DATETIME      NOT NULL
);

