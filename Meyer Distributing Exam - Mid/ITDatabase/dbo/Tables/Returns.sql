CREATE TABLE [dbo].[Returns] (
    [ReturnId]            INT           IDENTITY (1, 1) NOT NULL,
    [OriginalOrderNumber] NVARCHAR (50) NULL,
    [ReturnedProducts]    NVARCHAR (50) NULL, 
    [ReturnNumber] NVARCHAR(50) NULL
);

