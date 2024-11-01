USE [ITDatabase];
GO
Delete FROM dbo.OrderCart
GO
Delete FROM dbo.Orders
GO
Delete FROM dbo.Products
GO
Delete FROM dbo.[Returns]
GO
Insert into dbo.Products(ProductNumber, SellingPrice)
	Values ('Rugged Linder F55U15', 150),
			('DrawTite 5504', 70),
			('Sherman 036-87-1', 155),
			('Mobil 1 5W-30', 25)