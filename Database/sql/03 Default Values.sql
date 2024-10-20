INSERT INTO UrlType ([Id], [Name], [Description])
VALUES 
(1, 'Any', '' ), 
(2, 'GitHubRepository', '' ), 
(3, 'GitHubProject', '' ), 
(4, 'LinkedIn', '' ), 
(5, 'Portfolio', '' ), 
(6, 'StackOverflow', '' ), 
(7, 'CodePen', '' ), 
(8, 'Blog', '' ), 
(9, 'Medium', '' ), 
(10, 'Dev.to', '' ),
(11, 'YouTube', '' ),
(12, 'Twitter', '' );


INSERT INTO [dbo].[NotificationStatus] ([Id] , [Name])
VALUES
(1, 'Utworzone'),
(2, 'Werefikowane'),
(3, 'Rozpatrzone'),
(4, 'Anulowane');


INSERT INTO [dbo].[NotificationSender] ([Id] , [Name], [Description])
VALUES
(1, 'U¿ytkownik', '');


INSERT INTO [dbo].[CommentType] ([Id], [Name], [Description])
VALUES 
(1, 'Name', 'Description');