using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.ExcelLoader.Model
{
    public class PlayerInfo
    {
        #region Private Fields
        private string nameAndLastName = "-";
        #endregion Private Fields

        public int Number { get; set; }
        public String NameAndLastName
        {
            get
            {
                return nameAndLastName;
            }
            set
            {
                nameAndLastName = value;
            }
        }
        public string FirstName
        {
            get
            {
                if (string.IsNullOrEmpty(NameAndLastName))
                {
                    return String.Empty;
                }
                return NameAndLastName.Split(' ').First();
            }
        }
        public string LastName
        {
            get
            {
                if (string.IsNullOrEmpty(NameAndLastName))
                {
                    return String.Empty;
                }
                return NameAndLastName.Split(' ').Last();
            }
        }
        public string MiddleName
        {
            get
            {
                if (string.IsNullOrEmpty(NameAndLastName))
                {
                    return String.Empty;
                }

                string[] names = NameAndLastName.Split(' ');
                if (names.Count() == 3)
                {
                    return names[1];
                }

                return string.Empty;
            }
        }
        public bool OnLoan { get; set; }
        public string UId { get; set; }

        public string PreviousLeagueSeasonName { get; set; }
        public string PreviousTeamName { get; set; }

        public string NewLeagueSeasonName { get; set; }
        public string NewTeamName { get; set; }


    }

}
