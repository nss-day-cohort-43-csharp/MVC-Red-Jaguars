USE [TabloidMVC]
GO

SET IDENTITY_INSERT [UserType] ON
INSERT INTO [UserType] ([ID], [Name]) VALUES (1, 'Admin'), (2, 'Author');
SET IDENTITY_INSERT [UserType] OFF


SET IDENTITY_INSERT [Category] ON
INSERT INTO [Category] ([Id], [Name]) 
VALUES (1, 'Technology'), (2, 'Close Magic'), (3, 'Politics'), (4, 'Science'), (5, 'Improv'), 
	   (6, 'Cthulhu Sightings'), (7, 'History'), (8, 'Home and Garden'), (9, 'Entertainment'), 
	   (10, 'Cooking'), (11, 'Music'), (12, 'Movies'), (13, 'Regrets');
SET IDENTITY_INSERT [Category] OFF


SET IDENTITY_INSERT [Tag] ON
INSERT INTO [Tag] ([Id], [Name])
VALUES (1, 'C#'), (2, 'JavaScript'), (3, 'Cyclopean Terrors'), (4, 'Family');
SET IDENTITY_INSERT [Tag] OFF

SET IDENTITY_INSERT [UserProfile] ON
INSERT INTO [UserProfile] (
	[Id], [FirstName], [LastName], [DisplayName], [Email], [CreateDateTime], [ImageLocation], [UserTypeId], [Active])
VALUES (1, 'Admina', 'Strator', 'admin', 'admin@example.com', SYSDATETIME(), NULL, 1, 1);
SET IDENTITY_INSERT [UserProfile] OFF

SET IDENTITY_INSERT [Post] ON
INSERT INTO [Post] (
	[Id], [Title], [Content], [ImageLocation], [CreateDateTime], [PublishDateTime], [IsApproved], [CategoryId], [UserProfileId])
VALUES (
	1, 'C# is the Best Language', 
'There are those' + char(10) + 'who do not believe' + char(10) + 'C# is the best.' + char(10) + 'They are wrong.',
    'https://gizmodiva.com/wp-content/uploads/2017/10/SCOTT-A-WOODWARD_1SW1943-1170x689.jpg',SYSDATETIME(), SYSDATETIME(), 1, 1, 1);
SET IDENTITY_INSERT [Post] OFF

SET IDENTITY_INSERT [Reaction] ON
INSERT INTO [Reaction] ( [Id], [Name], [ImageLocation])
VALUES (1, 'Like', 'https://www.pngkey.com/png/detail/45-451702_thumbs-up-emoji-png-transparent-thumbs-up-sticker.png');

INSERT INTO [Reaction] ( [Id], [Name], [ImageLocation])
VALUES (2, 'Love', 'https://toppng.com/uploads/preview/heart-emoji-11549911583t6kulc2slx.png');

INSERT INTO [Reaction] ( [Id], [Name], [ImageLocation])
VALUES (3, 'Laughing', 'https://www.clipartmax.com/png/middle/426-4265976_free-png-download-emoji-transparent-laughing-emoji-laugh-emoji-png-transparent.png');

INSERT INTO [Reaction] ( [Id], [Name], [ImageLocation])
VALUES (4, 'Wow', 'https://cdn.shopify.com/s/files/1/1061/1924/products/12_grande.png');

INSERT INTO [Reaction] ( [Id], [Name], [ImageLocation])
VALUES (5, 'Sad', 'https://toppng.com/uploads/preview/sad-emoji-11549513329eul5223kjq.png');

INSERT INTO [Reaction] ( [Id], [Name], [ImageLocation])
VALUES (6, 'Angry', 'https://cdn.shopify.com/s/files/1/1061/1924/products/Super_Angry_Face_Emoji_ios10_grande.png');
SET IDENTITY_INSERT [Reaction] OFF