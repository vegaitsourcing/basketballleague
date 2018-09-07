using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace LZRNS.ExcelLoader
{
    class ExcelLoader
    {
        #region Private Fields
        private Excel.Application exApp;
        private Excel.Workbook xlWorkbook;
        #endregion Private Fields

        #region Constructors
        public ExcelLoader() {
            this.exApp = new Excel.Application();
        }
        #endregion Constructors

        #region Public Methods
        public void LoadFile (String filePath)
        {
            try
            {
                xlWorkbook = exApp.Workbooks.Open(@"filePath");

            }
            catch (Exception ex)
            {

            }
        }
        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods



    }
}
