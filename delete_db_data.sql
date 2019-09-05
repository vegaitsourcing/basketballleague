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
INSERT INTO Season(Id, Name, SeasonStartYear) VALUES ('15dc57bd-262a-49e5-8b32-ef8da07ae7fe', '2016', 2016); 
INSERT INTO League(Id, Name) VALUES ('15dc57bd-262a-49e5-8b32-ef8da07ae7fe', 'Liga A');
INSERT INTO LeagueSeason(Id, SeasonId, LeagueId) VALUES('15dc57bd-262a-49e5-8b32-ef8da07ae7fe', '15dc57bd-262a-49e5-8b32-ef8da07ae7fe', '15dc57bd-262a-49e5-8b32-ef8da07ae7fe');
