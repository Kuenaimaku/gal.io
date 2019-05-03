--Get's a player's champion picks, side, and role
--RETURNS PlayerId, ChampionKey, Side, Win, and Role

SELECT cp.Player_Id, cp.Champion_Key, Side, Win, Role  FROM ChampionPicks cp
JOIN (
	SELECT p.Match_Id, p.Player_Id, p.Side, p.Win, ps.Role FROM Participants p 
	JOIN PlayerStats ps ON p.Player_Id = ps.Player_ID AND p.Match_ID = ps.Match_ID
	WHERE p.Player_ID IN (
		SELECT DISTINCT PlayerID FROM Players WHERE Name = 'Kyle'
	)
) pp on cp.Match_Id = pp.Match_ID AND cp.Player_Id = pp.Player_Id