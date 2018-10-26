using LZRNS.DomainModel.Models;

namespace LZRNS.DomainModels.ViewModels
{
	public class PlayerInTeamDetails
	{
		private PlayerInTeamDetails(string name, int height, int weight, int yearOfBirth)
		{
			Name = name;
			Height = height;
			Weight = weight;
			YearOfBirth = yearOfBirth;
		}

		public string Name { get; }
		public int Height { get; }
		public int Weight { get; }
		public int YearOfBirth { get; }

		public static explicit operator PlayerInTeamDetails(Player p) =>
			new PlayerInTeamDetails(p.GetFullName, p.Height, p.Weight, p.YearOfBirth);
	}
}