USE MovieDB
GO

CREATE TABLE Astrology
(
id INT IDENTITY, 
[Sign] NVARCHAR(15),
StartDay INT,
EndDay INT
)
GO

INSERT INTO Astrology VALUES ('Aries', DATENAME(DAYOFYEAR, '0001-3-21'), DATENAME(DAYOFYEAR, '0001-4-19'))
INSERT INTO Astrology VALUES ('Taurus', DATENAME(DAYOFYEAR, '0001-4-20'), DATENAME(DAYOFYEAR, '0001-5-20'))
INSERT INTO Astrology VALUES ('Gemini', DATENAME(DAYOFYEAR, '0001-5-21'), DATENAME(DAYOFYEAR, '0001-6-21'))
INSERT INTO Astrology VALUES ('Cancer', DATENAME(DAYOFYEAR, '0001-6-22'), DATENAME(DAYOFYEAR, '0001-7-22'))
INSERT INTO Astrology VALUES ('Leo', DATENAME(DAYOFYEAR, '0001-7-23'), DATENAME(DAYOFYEAR, '0001-8-22'))
INSERT INTO Astrology VALUES ('Virgo', DATENAME(DAYOFYEAR, '0001-8-23'), DATENAME(DAYOFYEAR, '0001-9-22'))
INSERT INTO Astrology VALUES ('Libra', DATENAME(DAYOFYEAR, '0001-9-23'), DATENAME(DAYOFYEAR, '0001-10-23'))
INSERT INTO Astrology VALUES ('Scorpio', DATENAME(DAYOFYEAR, '0001-10-24'), DATENAME(DAYOFYEAR, '0001-11-21'))
INSERT INTO Astrology VALUES ('Sagittarius', DATENAME(DAYOFYEAR, '0001-11-22'), DATENAME(DAYOFYEAR, '0001-12-21'))
-- Capricorn 2 times
INSERT INTO Astrology VALUES ('Capricorn', DATENAME(DAYOFYEAR, '0001-12-22'), DATENAME(DAYOFYEAR, '0001-12-31'))
INSERT INTO Astrology VALUES ('Capricorn', DATENAME(DAYOFYEAR, '0001-1-1'), DATENAME(DAYOFYEAR, '0001-1-19'))
--
INSERT INTO Astrology VALUES ('Aquarius', DATENAME(DAYOFYEAR, '0001-1-20'), DATENAME(DAYOFYEAR, '0001-2-18'))
INSERT INTO Astrology VALUES ('Pisces', DATENAME(DAYOFYEAR, '0001-2-19'), DATENAME(DAYOFYEAR, '0001-3-20'))
GO


-- 4A
DECLARE @MinMovieDurationMinutes INT
SET @MinMovieDurationMinutes = 120

SELECT
      Actor.[Name], 
	  Actor.BirthDate, 
	  DATEDIFF(YEAR, Actor.BirthDate, GETDATE()) AS [Years Old], 
	  COUNT(Actor.[Name]) AS [Directors Count], 
	  Astrology.[Sign] AS Zodiac
FROM Movie 
      JOIN MovieActor ON Movie.id = MovieActor.MovieId
      JOIN Actor ON Actor.id = MovieActor.ActorId
      JOIN Astrology ON DATENAME(DAYOFYEAR, Actor.BirthDate) BETWEEN Astrology.StartDay AND Astrology.EndDay
      JOIN Director ON Movie.DirectorId = Director.id
WHERE 
      Movie.DurationMinutes > @MinMovieDurationMinutes
GROUP BY
      Actor.[Name], 
	  Actor.BirthDate, 
	  Astrology.[Sign]
ORDER BY 
      Actor.BirthDate DESC


-- 4B
DECLARE @MinMovieRating FLOAT
SET @MinMovieRating = 6.5

SELECT
      Actor.[Name], 
	  Actor.BirthDate, 
	  DATEDIFF(YEAR, Actor.BirthDate, GETDATE()) AS [Years Old], 
	  COUNT(Actor.[Name]) AS [Directors Count], 
	  Astrology.[Sign] AS Zodiac
FROM Movie 
      JOIN MovieActor ON Movie.id = MovieActor.MovieId
      JOIN Actor ON Actor.id = MovieActor.ActorId
      JOIN Astrology ON DATENAME(DAYOFYEAR, Actor.BirthDate) BETWEEN Astrology.StartDay AND Astrology.EndDay
      JOIN Director ON Movie.DirectorId = Director.id
WHERE 
      Actor.[Name] NOT IN (SELECT 
	                            Actor.[Name]
                           FROM Movie
							    JOIN MovieActor ON Movie.id = MovieActor.MovieId
							    JOIN Actor ON Actor.id = MovieActor.ActorId
                           WHERE 
						        Movie.Rating < @MinMovieRating)
GROUP BY
      Actor.[Name], 
	  Actor.BirthDate, 
	  Astrology.[Sign]
ORDER BY 
      Actor.BirthDate DESC
