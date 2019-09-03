using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using LZRNS.ExcelLoader.Model;

namespace LZRNS.ExcelLoader
{
    public class ExcelWriter
    {
        private Excel.Application _xlApp;
        private Excel.Workbook _xlWorkBook;
        private Excel.Worksheet _xlWorkSheet;
        private int _lastPopulatedRow = 0;

        public ExcelWriter()
        {
            _xlApp = new Microsoft.Office.Interop.Excel.Application();
            object misValue = System.Reflection.Missing.Value;

            _xlWorkBook = _xlApp.Workbooks.Add(misValue);
            _xlWorkSheet = (Excel.Worksheet)_xlWorkBook.Worksheets.get_Item(1);

        }


        public void CreateHeader()
        {
            _xlWorkSheet.Cells[1, 1] = "ID";
            _xlWorkSheet.Cells[1, 2] = "Ime i prezime";
            _xlWorkSheet.Cells[1, 3] = "Stari tim";
            _xlWorkSheet.Cells[1, 4] = "Stara sezona - liga";
            _xlWorkSheet.Cells[1, 5] = "Novi tim";
            _xlWorkSheet.Cells[1, 6] = "Nova sezona - liga";
            _lastPopulatedRow = 1;

        }

        public void WritePlayerInfoList(List<PlayerInfo> playersData)
        {
            foreach (PlayerInfo pi in playersData)
            {
                AppendPlayerInfo(pi);
            }
        }
        public void AppendPlayerInfo(PlayerInfo playerInfo)
        {

            _lastPopulatedRow++;

            _xlWorkSheet.Cells[_lastPopulatedRow, 1] = playerInfo.UId;
            _xlWorkSheet.Cells[_lastPopulatedRow, 2] = playerInfo.NameAndLastName;
            _xlWorkSheet.Cells[_lastPopulatedRow, 3] = playerInfo.PreviousTeamName;
            _xlWorkSheet.Cells[_lastPopulatedRow, 4] = playerInfo.PreviousLeagueSeasonName;
            _xlWorkSheet.Cells[_lastPopulatedRow, 5] = playerInfo.NewTeamName;
            _xlWorkSheet.Cells[_lastPopulatedRow, 6] = playerInfo.NewLeagueSeasonName;

        }

        public void SaveAndRelease(string leagueSeason)
        {
            _xlWorkBook.SaveAs(leagueSeason);
            _xlWorkBook.Close();
            _xlApp.Quit();

            Marshal.ReleaseComObject(_xlWorkSheet);
            Marshal.ReleaseComObject(_xlWorkBook);
            Marshal.ReleaseComObject(_xlApp);
        }
    }
}
