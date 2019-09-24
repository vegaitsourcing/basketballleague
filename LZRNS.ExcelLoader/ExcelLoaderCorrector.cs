using LZRNS.Common.Comparers;
using LZRNS.DomainModels.ExcelLoaderModels;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.ExcelLoader
{
    public class ExcelLoaderCorrector : IExcelLoaderCorrector
    {
        private readonly ITextComparer _comparer;

        public ExcelLoaderCorrector(ITextComparer comparer)
        {
            _comparer = comparer;
        }

        /// <summary>
        /// Tries to auto-correct "AgainstTeam" properties in PlayerScores and TeamStatistics.
        /// Assumes that excel loader has already loaded data.</summary>
        public void CorrectInvalidTeamNames(ExcelReader.ExcelLoader loader)
        {
            var validTeamNames = GetValidTeamNames(loader);
            CorrectAgainstTeamInPlayerScores(loader.PlayerScores, validTeamNames);

            var teamScores = GetTeamScores(loader);
            CorrectAgainstTeamInTeamScores(teamScores, validTeamNames);
        }

        private static List<string> GetValidTeamNames(ExcelReader.ExcelLoader loader)
        {
            return loader.TeamStatisticByTeamName.Keys.ToList();
        }

        private static IEnumerable<TeamScore> GetTeamScores(ExcelReader.ExcelLoader loader)
        {
            return loader.TeamStatisticByTeamName.Values.SelectMany(ts => ts.TeamScores).ToList();
        }

        private void CorrectAgainstTeamInTeamScores(IEnumerable<TeamScore> teamScores, IReadOnlyList<string> validTeamNames)
        {
            foreach (var teamScore in teamScores)
            {
                teamScore.AgainstTeam = _comparer.GetMostSimilar(validTeamNames, teamScore.AgainstTeam);
            }
        }

        private void CorrectAgainstTeamInPlayerScores(IEnumerable<PlayerScore> playerScores, IReadOnlyList<string> validTeamNames)
        {
            foreach (var playerScore in playerScores)
            {
                playerScore.AgainstTeam = _comparer.GetMostSimilar(validTeamNames, playerScore.AgainstTeam);
            }
        }
    }
}