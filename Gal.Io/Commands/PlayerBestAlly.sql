-- Get a Player's best ally. Params: NAME

SELECT p.Name, COUNT(DISTINCT m.MatchId) as Wins FROM [Matches] m 
JOIN Participants par ON m.MatchID = par.Match_Id
JOIN Players p ON par.Player_Id = p.PlayerId
JOIN (
SELECT Match_Id, Side FROM Participants p WHERE Player_ID IN (
SELECT PlayerID FROM Players WHERE Name = 'Kyle'
)
AND p.Win = true
) mpar ON m.MatchID = mpar.Match_Id AND par.Side = mpar.Side
WHERE p.Name <> 'Kyle'
GROUP BY p.Name
ORDER BY COUNT(DISTINCT m.MatchId) DESC 
LIMIT 1;