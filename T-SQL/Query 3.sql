USE MovieDB
GO 

SELECT 
      Actor.[Name]
FROM Actor
      JOIN MovieActor ON Actor.id = MovieActor.ActorId 
WHERE 
      MovieActor.MovieId = 1 AND
	  Actor.id NOT IN (SELECT 
	                          Actor.id
                           FROM Actor
			          JOIN MovieActor ON Actor.id = MovieActor.ActorId 
                           WHERE 
                                  MovieActor.MovieId = 2)
ORDER BY Actor.[Name]	