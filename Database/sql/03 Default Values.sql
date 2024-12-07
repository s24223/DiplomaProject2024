DELETE FROM  [dbo].[UrlType];
DELETE FROM  [dbo].[NotificationStatus];
DELETE FROM  [dbo].[NotificationSender];
DELETE FROM  [dbo].[CommentType];
DELETE FROM  [dbo].[Quality];
DELETE FROM  [dbo].[CharacteristicColocation]
DELETE FROM  [dbo].[Characteristic];
DELETE FROM  [dbo].[CharacteristicType];

INSERT INTO [dbo].[UrlType] 
([Id], [Name], [Description])
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
(1, 'Firma: Dizenny', ''),-- posaiada Ocene
(2, 'Praktykant/Pracownik: Dizenny', ''),-- posaiada Ocene
(7, 'Firma: Tygodniowy', ''),-- posaiada Ocene
(8, 'Praktykant/Pracownik: Tygodniowy', ''),-- posaiada Ocene
(30, 'Firma: Miesieczny', ''),
(31, 'Praktykant/Pracownik: Miesieczny', ''),-- posaiada Ocene
(90, 'Firma: Kwartalny', ''),-- posaiada Ocene
(91, 'Praktykant/Pracownik: Kwartalny', ''),-- posaiada Ocene
(182, 'Firma: Po³roczny', ''),-- posaiada Ocene
(183, 'Praktykant/Pracownik: Po³roczny', ''),-- posaiada Ocene
(365, 'Firma: Roczny', ''),-- posaiada Ocene
(366, 'Praktykant/Pracownik: Roczny', ''),-- posaiada Ocene

(1000, 'Firma: Opinia koñcowa', ''),-- posaiada Ocene
(1001, 'Praktykant/Pracownik: Opinia koñcowa', ''),-- posaiada Ocene

(1002, 'Firma: Opinia dowolna', ''),
(1003, 'Praktykant/Pracownik: Opinia dowolna', ''),

(1004, 'Firma: Pozwolenie na upublicznienie', ''),
(1005, 'Praktykant/Pracownik: Pozwolenie na upublicznienie', '');


INSERT INTO [dbo].[CharacteristicType]
([Id], [Name], [Description])
VALUES 
(1 , 'Poziom zaawansowania', ''),
(2 , 'Typ umowy', ''),
(3 , 'Tryb pracy', ''),
(4 , 'Etat', ''),
(5 , 'Jêzyki programowania', ''),
(6 , 'Jêzyki komunikacji', ''),
(7 , 'Technologie', ''),
(8 , 'Frameworki', ''),
(9 , 'Obszary', '');


INSERT INTO [dbo].[Quality]
([Id], [Name], [Description], [CharacteristicTypeId])
VALUES
--Jêzyki komunikacji
(1,'A1','',6),
(2,'A2','',6),
(3,'B1','',6),
(4,'B2','',6),
(5,'C1','',6),
(6,'C2','',6),
--
(11, 'Wymagane', '', NULL),
(12, 'Mile widziane', '', NULL);


INSERT INTO [dbo].[Characteristic]
([Id], [Name], [Description], [CharacteristicTypeId] )
VALUES

--1 Poziom zaawansowania
(1, 'Junior', '', 1),
(2, 'Mid', '', 1),
(3, 'Senior', '', 1),
(4, 'Lead/Expert', '', 1),

--2 Typ umowy
(21, 'Umowa o pracê', '', 2),
(22, 'Umowa zlecenie', '', 2),
(23, 'B2B', '', 2),
(24, 'Sta¿', '', 2),
(25, 'Praktyki', '', 2),
(26, 'Freelance', '', 2),
(27, 'Umowa o dzie³o', '', 2),
(28, 'Contract-to-Hire', '', 2),

--3 Tryb pracy
(31, 'Zdalna', '', 3),
(32, 'Hybrydowa', '', 3),
(33, 'Stacjonarna', '', 3),

--4 Etat
(41, 'Pe³ny', '', 4),
(42, 'Niepe³ny', '', 4),

--5 Jêzyki programowania
(401, 'Python', '', 5),
(402, 'JavaScript', '', 5),
(403, 'TypeScript', '', 5),
(404, 'Java', '', 5),
(405, 'C#', '', 5),
(406, 'C++', '', 5),
(407, 'PHP', '', 5),
(408, 'Ruby', '', 5),
(409, 'Swift', '', 5),
(410, 'Kotlin', '', 5),
(411, 'Go (Golang)', '', 5),
(412, 'Rust', '', 5),
(413, 'R', '', 5),
(414, 'Perl', '', 5),
(415, 'Scala', '', 5),
(416, 'Objective-C', '', 5),
(417, 'Shell (Bash, Zsh)', '', 5),
(418, 'MATLAB', '', 5),
(419, 'Dart', '', 5),
(420, 'Elixir', '', 5),
(421, 'Haskell', '', 5),
(422, 'Lua', '', 5),
(423, 'Groovy', '', 5),
(424, 'Clojure', '', 5),
(425, 'Erlang', '', 5),
(426, 'Julia', '', 5),
(427, 'Visual Basic .NET', '', 5),
(428, 'F#', '', 5),
(429, 'COBOL', '', 5),
(430, 'Fortran', '', 5),
(431, 'Assembly Language', '', 5),
(432, 'VBScript', '', 5),
(433, 'Tcl', '', 5),
(434, 'Crystal', '', 5),
(435, 'Nim', '', 5),
(436, 'APL', '', 5),
--SQL

--6 Jêzyki komunikacji
(201, 'Polski', '', 6),
(202, 'Angielski', '', 6),
(203, 'Niemiecki', '', 6),
(204, 'Francuski', '', 6),
(205, 'Hiszpañski', '', 6),
(206, 'W³oski', '', 6),
(207, 'Rosyjski ', '', 6),

--7 Technologie
(301, 'Amazon Web Services (AWS)', '', 7),
(302, 'Microsoft Azure', '', 7),
(303, 'Google Cloud Platform (GCP)', '', 7),
(304, 'Docker', '', 7),
(305, 'Kubernetes', '', 7),
(306, 'Apache Hadoop', '', 7),
(307, 'Apache Spark', '', 7),
(308, 'TensorFlow', '', 7),
(309, 'PyTorch', '', 7),
(310, 'Scikit-learn', '', 7),
(311, 'Jenkins', '', 7),
(312, 'GitLab CI', '', 7),
(313, 'Selenium', '', 7),
(314, 'Postman', '', 7),
(315, 'JUnit', '', 7),
(316, 'GraphQL', '', 7),
(317, 'RESTful API', '', 7),
(318, 'WebAssembly', '', 7),
(320, 'Machine Learning', '', 7),
(321, 'Elasticsearch', '', 7),
(322, 'Redis', '', 7),
(323, 'RabbitMQ', '', 7),
(324, 'Terraform', '', 7),
(325, 'Ansible', '', 7),
(326, 'Prometheus', '', 7),
(327, 'Grafana', '', 7),
(328, 'Nagios', '', 7),
(329, 'OpenShift', '', 7),
(330, 'Service Mesh (Istio, Linkerd)', '', 7),
(331, 'Apache Kafka', '', 7),
(332, 'Blockchain', '', 7),

--8 Frameworki
-- Py
(1001, 'Django', '', 8),
(1002, 'Flask', '', 8),
(1003, 'FastAPI', '', 8),
(1004, 'Pyramid', '', 8),
(1005, 'Pandas', '', 8),
(1006, 'NumPy', '', 8),
(1007, 'SciPy', '', 8),
(1008, 'Matplotlib', '', 8),
(1009, 'TensorFlow', '', 8),
(1010, 'PyTorch', '', 8),
(1011, 'Scikit-learn', '', 8),
(1012, 'Keras', '', 8),
(1013, 'TensorFlow', '', 8),
(1014, 'Tkinter', '', 8),
(1015, 'PyQt', '', 8),
(1016, 'Kivy', '', 8),
(1017, 'Selenium', '', 8),
(1018, 'Scrapy', '', 8),
(1147, 'Pygame', '', 8),
--JS
(1019, 'React', '', 8), --TS
(1020, 'Angular', '', 8),--TS
(1021, 'Vue', '', 8),--TS
(1022, 'Express', '', 8),--TS
(1023, 'Next.js', '', 8),
(1024, 'React Native', '', 8),
(1025, 'Ionic', '', 8),
(1026, 'Jest', '', 8),
(1027, 'Mocha', '', 8),
(1028, 'Cypress', '', 8),
(1029, 'Phaser', '', 8),
(1030, 'Babylon.js', '', 8),
(1146, 'Node.js', '', 8),--TS
--Java
(1031, 'Spring', '', 8),
(1032, 'Hibernate', '', 8),
(1033, 'JavaServer Faces (JSF)', '', 8),
(1034, 'Grails', '', 8), --Groovy
(1035, 'Android SDK', '', 8),
(1036, 'Apache Cordova', '', 8),
(1037, 'Java EE', '', 8),
(1038, 'Jakarta EE', '', 8),
(1039, 'Apache Hadoop', '', 8),
--C#
(1041, 'ASP.NET Core', '', 8),
(1042, 'MVC', '', 8),
(1043, 'Blazor', '', 8),
(1044, 'Unity', '', 8),
(1045, 'WPF', '', 8), --Visual Basic .NET
(1046, 'Xamarin', '', 8),
(1047, 'Entity Framework', '', 8),
--C++
(1048, 'Unreal Engine', '', 8),
(1049, 'Cocos2d', '', 8),
(1050, 'Qt', '', 8),
(1051, 'wxWidgets', '', 8),
(1052, 'ROS (Robot Operating System)', '', 8),
(1053, 'OpenCV', '', 8),
(1054, 'Boost', '', 8),
(1055, 'Armadillo', '', 8),
--PHP
(1056, 'Laravel, Symfony', '', 8),
(1057, 'CodeIgniter', '', 8),
(1058, 'CakePHP', '', 8),
(1059, 'WordPress', '', 8),
(1060, 'Drupal', '', 8),
(1061, 'Joomla', '', 8),
--Ruby
(1062, 'Ruby on Rails', '', 8),
(1063, 'Sinatra', '', 8),
(1064, 'RSpec', '', 8),
(1065, 'Capybara', '', 8),
(1066, 'Chef', '', 8),
(1067, 'Puppet', '', 8),
--Swift
(1068, 'iOS SDK', '', 8),
(1069, 'SwiftUI', '', 8),
(1070, 'Vapor', '', 8),
--Kotlin
(1071, 'Android SDK', '', 8),
(1072, 'Ktor', '', 8), --Web Development
(1073, 'Spring', '', 8),
--Go (Golang)
(1074, 'Gin', '', 8),
(1075, 'Echo', '', 8),
(1076, 'Revel', '', 8),
(1077, 'gRPC', '', 8),
(1078, 'Micro', '', 8),
--Rust
(1079, 'Rocket', '', 8),
(1080, 'Actix', '', 8),
(1081, 'Amethyst', '', 8),
(1082, 'Bevy', '', 8),
--SQL Frameworks
(1083, 'NHibernate', '', 8), --C#
(1084, 'SQLAlchemy ', '', 8), --Py
(1085, 'Hibernate ', '', 8),--Java
(1086, 'Dapper ', '', 8), --C#
--R
(1087, 'Shiny', '', 8),
(1088, 'ggplot2', '', 8),
(1089, 'dplyr', '', 8),
(1090, 'caret', '', 8),
(1091, 'R Markdown', '', 8),
(1092, 'Bioconductor', '', 8),
--Perl
(1093, 'Dancer', '', 8),
(1094, 'Mojolicious', '', 8),
(1095, 'Test::More', '', 8),
--Scala
(1096, 'Play Framework', '', 8),
(1097, 'Akka', '', 8),
--Objective-C	
(1098, 'Cocoa Touch (iOS)', '', 8),
(1099, 'AppKit (macOS)', '', 8),
--Shell (Bash, Zsh)
(1100, 'Bash scripts', '', 8),
(1101, 'Zsh scripts', 'do zadañ automatyzacji, zarz¹dzania systemem', 8),
--MATLAB
(1102, 'MATLAB Toolboxes', 'Signal Processing, Image Processing', 8),
(1103, 'MATLAB Statistics and Machine Learning Toolbox', '', 8),
--Dart
(1104, 'Flutter', '', 8), --C, C++, Web Development, Mobile Developmen
--Elixir
(1105, 'Phoenix Framework', '', 8), --Erlang
(1106, 'Nerves ', '', 8),
--Haskell
(1107, 'Yesod', '', 8),
(1108, 'Snap', '', 8),
(1109, 'GHC', '', 8),
(1110, 'Haskell Platform', '', 8),
--Lua
(1111, 'Love2D', '', 8),
(1112, 'Corona SDK', '', 8),
(1113, 'LuaJIT', '', 8),
--Groovy
(1114, 'Spring Boot', '', 8), -- Java
(1115, 'Spock', '', 8),
--Clojure
(1116, 'Ring', '', 8),
(1117, 'Compojure', '', 8),
(1118, 'Core.async', '', 8),
--Erlang
(1119, 'Cowboy', '', 8),
(1120, 'RabbitMQ (AMQP)', '', 8),
--Julia
(1121, 'JuMP', '', 8),
(1122, 'DataFrames.jl', '', 8),
(1123, 'Plots.jl', '', 8),
(1124, 'JuliaDB', '', 8),
--Visual Basic .NET
(1125, 'Windows Forms', '', 8), --C#
(1126, 'ASP.NET Web Forms', '', 8),
--F#
(1127, 'Giraffe', '', 8),
(1128, 'Saturn', '', 8),
(1129, 'FSharp.Data', '', 8),
--COBOL
(1130, 'CICS', '', 8),
(1131, 'DB2', '', 8),
--Fortran
(1132, 'LAPACK', '', 8),
(1133, 'BLAS', '', 8),
--Assembly 
(1134, 'NASM', '', 8),
(1135, 'MASM', '', 8),
--VBScript
(1136, 'ASP', 'Active Server Pages', 8),
(1137, 'WSH', 'Windows Script Host', 8),
--Tcl
(1138, 'Expect', '', 8),
(1139, 'Tk (GUI)', '', 8),
--Crystal
(1140, 'Lucky', '', 8),
(1141, 'Kemal', '', 8),
--Nim
(1142, 'Jester', '', 8),
(1143, 'Nimble', '', 8),
--APL
(1144, 'Dyalog APL', '', 8),
(1145, 'APL2', '', 8),
--1147

--9 Obszary
(101, 'Web Development', '', 9),
(102, 'Mobile Development', '', 9),
(103, 'Desktop Development', '', 9),
(104, 'Data Science & Machine Learning', '', 9),
(105, 'Game Development', '', 9),
(106, 'Testing', 'Testowanie', 9),
(107, 'Functional Programming', '', 9),
(108, 'Scripting & Automation', '', 9),
(109, 'Systems Programming', 'Systems Programming (programowanie systemowe) to obszar programowania, który koncentruje siê na tworzeniu oprogramowania niskopoziomowego, dzia³aj¹cego bezpoœrednio nad sprzêtem lub bardzo blisko systemu operacyjnego. Ten rodzaj programowania wymaga g³êbokiego zrozumienia architektury komputerów, systemów operacyjnych oraz zarz¹dzania zasobami sprzêtowymi.', 9);

INSERT INTO [dbo].[CharacteristicColocation]
([ParentCharacteristicId], [ChildCharacteristicId])
VALUES
--==========================================================================================================
--Languages and FrameWorks
--Python
(401,1001),
(401,1002),
(401,1003),
(401,1004),
(401,1005),
(401,1006),
(401,1007),
(401,1008),
(401,1009),
(401,1010),
(401,1011),
(401,1012),
(401,1013),
(401,1014),
(401,1015),
(401,1016),
(401,1017),
(401,1018),
(401,1084),
(401,1147),
--JavaScript
(402,1019),
(402,1020),
(402,1021),
(402,1022),
(402,1023),
(402,1024),
(402,1025),
(402,1026),
(402,1027),
(402,1028),
(402,1029),
(402,1030),
(402,1146),
-- TypeScript
(403,1019),
(403,1020),
(403,1021),
(403,1022),
(403,1146),
--Java
(404,1031),
(404,1032),
(404,1033),
(404,1034),
(404,1035),
(404,1036),
(404,1037),
(404,1038),
(404,1039),
(404,1085),
(404,1114),
--C#
(405,1041),
(405,1042),
(405,1043),
(405,1044),
(405,1045),
(405,1046),
(405,1047),
(405,1083),
(405,1086),
(405,1125),
--C++
(406,1048),
(406,1049),
(406,1050),
(406,1051),
(406,1052),
(406,1053),
(406,1054),
(406,1055),
(406,1104),
--PHP
(407,1056),
(407,1057),
(407,1058),
(407,1059),
(407,1060),
(407,1061),
--Ruby
(408,1062),
(408,1063),
(408,1064),
(408,1065),
(408,1066),
(408,1067),
--Swift
(409,1068),
(409,1069),
(409,1070),
--Kotlin
(410,1071),
(410,1072),
(410,1073),
--Go (Golang)
(411,1074),
(411,1075),
(411,1076),
(411,1077),
(411,1078),
--Rust
(412,1079),
(412,1080),
(412,1081),
(412,1082),
--R
(413,1087),
(413,1088),
(413,1089),
(413,1090),
(413,1091),
(413,1092),
--Perl
(414,1093),
(414,1094),
(414,1095),
--Scala
(415,1096),
(415,1097),
--Objective-C
(416,1098),
(416,1099),
--Shell (Bash, Zsh)
(417,1100),
(417,1101),
--MATLAB
(418,1102),
(418,1103),
--Dart
(419,1104),
--Elixir
(420,1105),
(420,1106),
--Haskell
(421,1107),
(421,1108),
(421,1109),
(421,1110),
--Lua
(422,1111),
(422,1112),
(422,1113),
--Groovy
(423,1114),
(423,1115),
(423,1034),
--Clojure
(424,1116),
(424,1117),
(424,1118),
--Erlang
(425,1119),
(425,1120),
(425,1105),
--Julia
(426,1121),
(426,1122),
(426,1123),
(426,1124),
--Visual Basic .NET
(427,1125),
(427,1126),
(427,1045),
--F#
(428,1127),
(428,1128),
(428,1129),
--COBOL
(429,1130),
(429,1131),
--Fortran
(430,1132),
(430,1133),
--Assembly 
(431,1134),
(431,1135),
--VBScript
(432,1136),
(432,1137),
--Tcl
(433,1138),
(433,1139),
--Crystal
(434,1140),
(434,1141),
--Nim
(435,1142),
(435,1143),
--APL
(436,1144),
(436,1145);

INSERT INTO [dbo].[CharacteristicColocation]
([ParentCharacteristicId], [ChildCharacteristicId])
VALUES
--(101, 'Web Development', '', 9),
--Py
(1001, 101),
(1002, 101),
(1003, 101),
(1004, 101),
(1018, 101),
(1017, 101),
--JS
(1019, 101),
(1020, 101),
(1021, 101),
(1022, 101),
(1023, 101),
(1024, 101),
(1025, 101),
(1026, 101),
(1027, 101),
(1028, 101),
(1029, 101),
(1030, 101),
(1146, 101),
--Java
(1031, 101),
(1032, 101),
(1033, 101),
(1034, 101),
(1037, 101),
(1038, 101),
(1039, 101),
--C#
(1041, 101),
(1042, 101),
(1043, 101),
--PHP
(1056, 101),
(1057, 101),
(1058, 101),
(1059, 101),
(1060, 101),
(1061, 101),
--Ruby
(1062, 101),
(1063, 101),
--Go (Golang)
(1074, 101),
(1075, 101),
(1076, 101),
(1077, 101),
--Elixir
(1105, 101),
(1106, 101),
--Scala
(1096, 101),
(1097, 101),
--Rust
(1079, 101),
(1080, 101),

--(104, 'Data Science & Machine Learning', '', 9),
--Py
(1005, 104),
(1006, 104),
(1007, 104),
(1008, 104),
(1009, 104),
(1010, 104),
(1011, 104),
(1012, 104),
--R
(1087, 104),
(1088, 104),
(1089, 104),
(1090, 104),
(1091, 104),
(1092, 104),


--(102, 'Mobile Development', '', 9),
(1035, 102),
(1046, 102),
(1104, 102),

--(105, 'Game Development', '', 9),
(1048, 105),
(1044, 105),
(1049, 105),
(1147, 105),
(1029, 105),

--(103, 'Desktop Development', '', 9),
(1045, 103),
(1125, 103),
(1126, 103),
(1014, 103),
(1015, 103),
(1016, 103),

--(106, 'Testing', 'Testowanie', 9),
--JS
(1026, 106),
(1027, 106),
(1028, 106),
--Ruby
(1064, 106),
(1065, 106),

--(107, 'Functional Programming', '', 9),
--Haskell
(1107, 107),
(1108, 107),
(1109, 107),
(1110, 107),
--Elixir
(1105, 107),
--F#
(1127, 107),
(1128, 107),
(1129, 107),

--(109, 'Systems Programming'
--Rust
(1079, 109),
(1080, 109),

--(108, 'Scripting & Automation', '', 9),
--Shell (Bash, Zsh)
(1100, 108),
(1101, 108),
--Perl
(1093, 108),
(1094, 108);
