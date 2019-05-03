SELECT COUNT(DISTINCT p.Match_Id) as LastPicks FROM Participants p
WHERE p.Player_Id IN (
	SELECT DISTINCT PlayerID FROM Players WHERE Name = 'Kyle'
)
AND p.[Order] = 4