USE MovieDB
GO

SELECT 
      Director.[Name] 
FROM  Director
WHERE 
      NOT EXISTS (SELECT *
                  FROM Movie 
                  WHERE Movie.DirectorId = Director.id)
ORDER BY 
      Director.[Name]