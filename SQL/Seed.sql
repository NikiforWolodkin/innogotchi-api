USE innogotchidb;
GO

INSERT INTO Users VALUES
('user1@mail.com', 'John', 'Doe');
INSERT INTO Users VALUES
('user2@mail.com', 'Jane', 'Doe');
INSERT INTO Users VALUES
('user3@mail.com', 'Sam', 'Doe');
INSERT INTO Farms VALUES
('farm1', 'user1@mail.com');
INSERT INTO Farms VALUES
('farm2', 'user2@mail.com');
INSERT INTO Farms VALUES
('farm3', 'user3@mail.com');
INSERT INTO Innogotchis VALUES
('inno1', 0, GETDATE(), 'farm1'),
('inno2', 0, GETDATE(), 'farm1');
INSERT INTO FeedingAndQuenchings VALUES
(GETDATE(), GETDATE(), 0, 'inno1'),
(GETDATE(), GETDATE(), 0, 'inno2');
INSERT INTO Collaboration VALUES
('farm2', 'user1@mail.com'),
('farm3', 'user1@mail.com');