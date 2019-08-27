/*unique constraint to league name*/
/*unique constraint cannot be applied it name is too long */
  ALTER TABLE League ALTER COLUMN Name nvarchar(100) NOT NULL;
  ALTER TABLE League ADD CONSTRAINT UC_Name UNIQUE(Name);

 /*unique constraint - (name)*/
 ALTER TABLE Season ALTER COLUMN Name nvarchar(120) NOT NULL;
 ALTER TABLE Season ADD CONSTRAINT UC_SeasonName UNIQUE(Name);

 /*unique constraint - team(name)*/
 ALTER TABLE Team ALTER COLUMN TeamName nvarchar(100) NOT NULL;
 ALTER TABLE Team ADD CONSTRAINT UC_TeamName UNIQUE(TeamName);

