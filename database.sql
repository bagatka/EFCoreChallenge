CREATE DATABASE Cinemas;
USE Cinemas;

CREATE TABLE Movie (
    Id INT NOT NULL PRIMARY KEY CLUSTERED,
    Name NVARCHAR(50) NOT NULL
);

CREATE TABLE Cinema (
    Id INT NOT NULL PRIMARY KEY CLUSTERED,
    Name NVARCHAR(50) NOT NULL,
);

CREATE TABLE Hall (
    Id INT NOT NULL PRIMARY KEY CLUSTERED,
    Name NVARCHAR(50) NOT NULL,
    CinemaId INT NOT NULL,
    FOREIGN KEY (CinemaId) REFERENCES Cinema(Id)
);

CREATE TABLE Showtime (
    Id INT NOT NULL PRIMARY KEY CLUSTERED,
    Name NVARCHAR(50) NOT NULL,
    HallId INT NOT NULL,
    MovieId INT NOT NULL,
    FOREIGN KEY (HallId) REFERENCES Hall(Id),
    FOREIGN KEY (MovieId) REFERENCES Movie(Id)
);

INSERT INTO Movie (Id, Name)
VALUES
       (1, 'James Bond 007'),
       (2, 'Dune'),
       (3, 'Green Mile'),
       (4, 'Tenet');

INSERT INTO Cinema (Id, Name)
VALUES
       (1, 'Silver Screen by VOKA'),
       (2, 'Falcon Club'),
       (3, 'Belarus'),
       (4, 'Aurora');


INSERT INTO Hall (Id, Name, CinemaId)
VALUES
       (1, 'Hall 1', 1),
       (2, 'Hall 2', 1),
       (3, 'Hall 1', 2),
       (4, 'Hall 2', 2),
       (5, 'Hall 1', 3),
       (6, 'Hall 2', 3),
       (7, 'Hall 1', 4),
       (8, 'Hall 2', 4);

INSERT INTO Showtime (Id, Name, HallId, MovieId)
VALUES
       (1, 'Showtime of Dune', 1, 2),
       (2, 'Showtime of Dune', 2, 2),
       (3, 'Showtime of Tenet', 3, 4),
       (4, 'Showtime of Tenet', 4, 4),
       (5, 'Showtime of Dune', 5, 2),
       (6, 'Showtime of Green Mile', 6, 3),
       (7, 'Showtime of Green Mile', 7, 3),
       (8, 'Showtime of James Bond 007', 8, 1);


-- Expected result.

SELECT *
FROM dbo.Cinema
LEFT JOIN Hall H on Cinema.Id = H.CinemaId
LEFT JOIN Showtime S on H.Id = S.HallId
LEFT JOIN Movie M on S.MovieId = M.Id
WHERE M.Id = 2;