using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System.Collections.Generic;

namespace LZRNS.Core
{
    public interface IRoundGenerator
    {
        IEnumerable<Round> GenerateRoundsWithGames(IReadOnlyList<Team> teams, LeagueSeason leagueSeason);
    }
}