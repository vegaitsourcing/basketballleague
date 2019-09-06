use [VegaIT.BasketballLeague.Umbraco.Development];
DELETE LeagueSeason;
DELETE League;
DELETE Season;
DELETE Stats;
DELETE Game;
DELETE Round;
DELETE PlayerPerTeam;
DELETE Player;
DELETE Team;
--necessary instances for site build
/*
0328e63c-f50c-413b-b19e-11510ec52d25
924407bc-9cbf-406b-9dd1-f5de44807cb5
f16c3522-a856-4513-9f59-844ed3293b87
*/
INSERT INTO Season(Id, Name, SeasonStartYear) VALUES ('0328e63c-f50c-413b-b19e-11510ec52d25', '2016', 2016); 
INSERT INTO League(Id, Name) VALUES ('924407bc-9cbf-406b-9dd1-f5de44807cb5', 'Liga A');
INSERT INTO LeagueSeason(Id, SeasonId, LeagueId) VALUES('f16c3522-a856-4513-9f59-844ed3293b87', '0328e63c-f50c-413b-b19e-11510ec52d25', '924407bc-9cbf-406b-9dd1-f5de44807cb5');
