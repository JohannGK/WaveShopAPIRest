CREATE DATABASE WaveShop;
Go
USE WaveShop;

CREATE TABLE [User] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
	[Email] NVARCHAR (500) NOT NULL,
    [UserName] NVARCHAR (100) NOT NULL,
    [Password] NVARCHAR (100) NOT NULL, 
	[Phone] NVARCHAR (100) NOT NULL,
	[Description] NVARCHAR (500) NOT NULL,
	[Status] NVARCHAR (100) NOT NULL,
	[BirthDay] DATETIME NOT NULL,
	[Age] INT NOT NULL, --Auto generated
	[UserType] NVARCHAR (500) NOT NULL,   --  Customer | Admin
	[Reputation] NVARCHAR (100) NOT NULL, -- Friendly | Neutral | Unfriendly
	[LastLogin] DATETIME NOT NULL, -- Auto generated
	[LastUpdate] DATETIME NOT NULL, -- Auto generated
    CONSTRAINT [PK_dbo.AdminAccount] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [ShoppingCart] (
	[id] INT IDENTITY (1, 1) NOT NULL,
    [productsQuantity] INT NOT NULL, -- Auto generated
	[subtotal] FLOAT NOT NULL, -- Auto generated
	[LastUpdate] DATETIME NOT NULL, -- Auto generated
	[IdUser] INT, 
	CONSTRAINT [PK_dbo.ShoppingCart] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [Address] (
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [Zip] NVARCHAR (100) NOT NULL,
    [Street] NVARCHAR (500) NOT NULL,
    [State]  NVARCHAR (100) NOT NULL,
    [city]  NVARCHAR (100) NOT NULL,
	[IdUser] INT NOT NULL,
	CONSTRAINT [PK_dbo.Address] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Address.Account] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[User] ([Id]) ON UPDATE CASCADE
);

CREATE TABLE [Category] (
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL,
    [Description]  NVARCHAR (500) NOT NULL,
	[Status]  NVARCHAR (500) NOT NULL, -- Visible | Hidden
	CONSTRAINT [PK_dbo.Category] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [Product] (
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    [Description]  NVARCHAR (500) NOT NULL,
	[VideoAddress]  NVARCHAR (MAX),
	[StockQuantity] INT NOT NULL,
	[UnitPrice] FLOAT NOT NULL,
    [Status]  NVARCHAR (100) NOT NULL, -- Available | Unavailable
    [Published] DATETIME NOT NULL,
	[Country] NVARCHAR (100) NOT NULL,
	[Location] NVARCHAR (500) NOT NULL,
	[IdCategory] INT,
	[IdVendor] INT NOT NULL,
	[LikesNumber] INT NOT NULL,
	[DislikesNumber] INT NOT NULL,
	[ShoppedTimes] INT NOT NULL,
	[CommentsNumber] INT NOT NULL,
	[LastUpdate] DATETIME NOT NULL,
	[VendorUsername] NVARCHAR (500),
	CONSTRAINT [PK_dbo.Product] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [Product.Image] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
	[Url] NVARCHAR (MAX) NOT NULL,
	[LastUpdate] DATETIME NOT NULL,
	[IdProduct] INT,
	CONSTRAINT [PK_dbo.Image] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_dbo.Image.Product] FOREIGN KEY ([IdProduct]) REFERENCES [dbo].[Product] ([Id]) ON UPDATE CASCADE
);

CREATE TABLE [Order] (
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [IdUser] INT NOT NULL,
	[IdShoppingCart] INT,
	[Ordered] DATETIME NOT NULL,
	[Shipped] DATETIME,
	[Status] NVARCHAR(100) NOT NULL, -- Requested | Shopped | Delivered | Canceled
	[Total] FLOAT NOT NULL,
	CONSTRAINT [PK_dbo.Order] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Order.Account] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[User] ([Id]) ON UPDATE CASCADE
);

CREATE TABLE [ProductSelectedCart] (
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [Price] FLOAT NOT NULL,
    [Quantity] INT NOT NULL,
    [Status]  NVARCHAR (100), -- Requested | Shopped | Shipped | Delivered | Closed
	[IdProduct] INT NOT NULL,
	[IdShoppingCart] INT NOT NULL,
	CONSTRAINT [PK_dbo.ProductSelected] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ProductSelected.Product] FOREIGN KEY ([IdProduct]) REFERENCES [dbo].[Product] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_dbo.ProductSelected.ShoppingCart] FOREIGN KEY ([IdShoppingCart]) REFERENCES [dbo].[ShoppingCart] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE [ProductSelectedOrder] (
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [Price] FLOAT NOT NULL,
    [Quantity] INT NOT NULL,
    [Status]  NVARCHAR (100), -- Requested | Shopped | Shipped | Delivered | Closed
	[IdProduct] INT NOT NULL,
	[IdOrder] INT NOT NULL,
	CONSTRAINT [PK_dbo.ProductSelected.order] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ProductSelected.Product.Order] FOREIGN KEY ([IdProduct]) REFERENCES [dbo].[Product] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_dbo.ProductSelected.Order] FOREIGN KEY ([IdOrder]) REFERENCES [dbo].[Order] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE [Comment] (
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[UserName] NVARCHAR (100) NOT NULL,
	[OpinionResume] NVARCHAR (100) NOT NULL,
    [Content] NVARCHAR (500) NOT NULL,
    [Visible]  NVARCHAR (1) NOT NULL, -- N | Y
    [PhotoAddress]  NVARCHAR (100),
	[Likes] INT NOT NULL,
	[Dislikes] INT NOT NULL,
    [Published] DATETIME NOT NULL,
	[IdProduct] INT,
	[IdComment] INT, 
	CONSTRAINT [PK_dbo.Opinion] PRIMARY KEY CLUSTERED ([id] ASC),
	CONSTRAINT [FK_dbo.Comment.Product] FOREIGN KEY ([IdProduct]) REFERENCES [dbo].[Product] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE [Favorite] (
	[IdUser] INT NOT NULL,
	[IdProduct] INT NOT NULL,
	[Creation] DATETIME NOT NULL,
    PRIMARY KEY(IdUser, IdProduct),
	CONSTRAINT [FK_dbo.Favorite.User] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[User] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_dbo.Favorite.Product] FOREIGN KEY ([IdProduct]) REFERENCES [dbo].[Product] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE
);


USE WaveShop;
CREATE UNIQUE INDEX PK_dbo_AdminAccount_userName ON [dbo].[User] ([UserName]);
CREATE UNIQUE INDEX PK_dbo_AdminAccount_email ON [dbo].[User] ([Email]);
CREATE UNIQUE INDEX PK_dbo_Category_Name ON [dbo].[Category] ([Name]);
CREATE UNIQUE INDEX PK_dbo_Product_Name ON [dbo].[Product] ([Name]);

