USE [SplitCost]
GO
/****** Object:  Table [dbo].[Addresses]    Script Date: 27/05/2025 18:52:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addresses](
	[Id] [uniqueidentifier] NOT NULL,
	[Street] [nvarchar](200) NOT NULL,
	[Number] [nvarchar](20) NOT NULL,
	[Apartment] [nvarchar](200) NOT NULL,
	[City] [nvarchar](200) NOT NULL,
	[Prefecture] [nvarchar](200) NOT NULL,
	[Country] [nvarchar](200) NOT NULL,
	[PostalCode] [nvarchar](20) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expenses]    Script Date: 27/05/2025 18:52:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenses](
	[Id] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[Category] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[IsSharedAmongMembers] [bit] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[ResidenceId] [uniqueidentifier] NOT NULL,
	[RegisteredByUserId] [uniqueidentifier] NOT NULL,
	[PaidByUserId] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExpenseShares]    Script Date: 27/05/2025 18:52:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpenseShares](
	[Id] [uniqueidentifier] NOT NULL,
	[ResidenceExpenseId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_ExpenseShares] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResidenceMembers]    Script Date: 27/05/2025 18:52:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResidenceMembers](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[ResidenceId] [uniqueidentifier] NOT NULL,
	[JoinedAt] [datetime2](7) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_ResidenceMembers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Residences]    Script Date: 27/05/2025 18:52:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Residences](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[AddressId] [uniqueidentifier] NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Residences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 27/05/2025 18:52:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[AvatarUrl] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_Residences_ResidenceId] FOREIGN KEY([ResidenceId])
REFERENCES [dbo].[Residences] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Expenses] CHECK CONSTRAINT [FK_Expenses_Residences_ResidenceId]
GO
ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_Users_PaidByUserId] FOREIGN KEY([PaidByUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Expenses] CHECK CONSTRAINT [FK_Expenses_Users_PaidByUserId]
GO
ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_Users_RegisteredByUserId] FOREIGN KEY([RegisteredByUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Expenses] CHECK CONSTRAINT [FK_Expenses_Users_RegisteredByUserId]
GO
ALTER TABLE [dbo].[ExpenseShares]  WITH CHECK ADD  CONSTRAINT [FK_ExpenseShares_Expenses_ResidenceExpenseId] FOREIGN KEY([ResidenceExpenseId])
REFERENCES [dbo].[Expenses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExpenseShares] CHECK CONSTRAINT [FK_ExpenseShares_Expenses_ResidenceExpenseId]
GO
ALTER TABLE [dbo].[ExpenseShares]  WITH CHECK ADD  CONSTRAINT [FK_ExpenseShares_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ExpenseShares] CHECK CONSTRAINT [FK_ExpenseShares_Users_UserId]
GO
ALTER TABLE [dbo].[ResidenceMembers]  WITH CHECK ADD  CONSTRAINT [FK_ResidenceMembers_Residences_ResidenceId] FOREIGN KEY([ResidenceId])
REFERENCES [dbo].[Residences] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ResidenceMembers] CHECK CONSTRAINT [FK_ResidenceMembers_Residences_ResidenceId]
GO
ALTER TABLE [dbo].[ResidenceMembers]  WITH CHECK ADD  CONSTRAINT [FK_ResidenceMembers_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ResidenceMembers] CHECK CONSTRAINT [FK_ResidenceMembers_Users_UserId]
GO
ALTER TABLE [dbo].[Residences]  WITH CHECK ADD  CONSTRAINT [FK_Residences_Addresses_AddressId] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Addresses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Residences] CHECK CONSTRAINT [FK_Residences_Addresses_AddressId]
GO
ALTER TABLE [dbo].[Residences]  WITH CHECK ADD  CONSTRAINT [FK_Residences_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Residences] CHECK CONSTRAINT [FK_Residences_Users_CreatedByUserId]
GO
