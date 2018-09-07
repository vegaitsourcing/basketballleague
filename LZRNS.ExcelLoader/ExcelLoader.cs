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

        private MapperModel mapper;
        private int currentRow;
        private int maxPlayerCount = 12;

        private String defaultConfigPath = "TableMapper.config";

        private bool indexerInitialized;
        private bool checkMappingValidation = true;

        #endregion Private Fields

        #region Constructors
        public ExcelLoader() {
            this.indexerInitialized = false;
            this.exApp = new Excel.Application();

        }
        #endregion Constructors

        #region Public Methods
        public void LoadFile (String filePath)
        {
            try
            {
                xlWorkbook = exApp.Workbooks.Open(@"filePath");

                foreach(Excel.Worksheet sheet in xlWorkbook.Sheets)
                {
                    if (checkMappingValidation)
                    {
                        CheckMappingValidation(mapper, sheet);
                        checkMappingValidation = false;
                    }

                    ProcessSheet(sheet);
                }
                

            }
            catch (Exception)
            {

            }
        }
        #endregion Public Methods

        #region Private Methods

        private void CheckMappingValidation (MapperModel mapper, Excel.Worksheet sheet)
        {
            Excel.Range exlRange = sheet.UsedRange;
            foreach (FieldItem item in mapper.Fields)
            {
                string value = exlRange.Cells[item.ColumnIndex, item.RowIndex];

                if(value == null || !value.Equals(item.CellName))
                {
                    throw new Exception();
                }
            }
        }

        private void ProcessSheet(Excel.Worksheet sheet)
        {

           



        }

       

        #endregion Private Methods



    }
}
