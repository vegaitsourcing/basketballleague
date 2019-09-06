

Import of excel files - notes:


1) Filename must be in xlsx format, currently it is 97-2003 
2) Sheet names should be in format GAMEx where x is round name
3) Check date formats - mm/dd/yyyy, if format is bad, default date will be used (01/01/1970)
4) Filename must be exactly the same as team name, so no abbreviations or upper case/lower case changes 
5) Filename must be in the followig format - 
stats-teams-<teamname>.xlsx 
Filename example: stats-teams-Bečej.xlsx
6) When import, you should import all of the files for teams in that league season -> one file must be named in this format>
stats-teams-<teamname>-<seasonname>-<leaguename>.xlsx, other files should follow naming convention defined in 5)
Filename example: stats-teams-Apsolventi-2016-Liga A.xlsx
7) Importing of files has two phases:
I) Analyzing - import all of the xlsx files, result is xlsx file - coding list of player data read from the improted excel files (it will be in response)
Codefile's header:
ID	| Ime i prezime	| Stari tim	 | Stara sezona - liga	| Novi tim	| Nova sezona - liga

II) Importing - import all of the xlsx files and coding list - coding list will be used for player identification, so edit it If there are any duplicates, player transfers to other team,
other league etc.


