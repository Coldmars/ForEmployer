USE MovieDB
GO

CREATE PROCEDURE MaxActorsCountInMovie
        @max INT OUTPUT
AS SELECT 
        @max = MAX(c) 
   FROM Movie, 
	   (SELECT 
	         COUNT(MovieActor.ActorId) AS c
            FROM Movie
	         JOIN MovieActor ON Movie.id = MovieActor.MovieId
            GROUP BY 
		     Movie.Title) res
GO

DECLARE @max INT
EXEC MaxActorsCountInMovie @max OUTPUT

SELECT 
     Movie.Title
FROM Movie
     JOIN MovieActor ON Movie.id = MovieActor.MovieId
GROUP BY 
     Movie.Title
HAVING 
     COUNT(MovieActor.ActorId) = @max
ORDER BY 
     Movie.Title