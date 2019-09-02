using System;

namespace LZRNS.ExcelLoader
{
    public class FieldItem
    {
        private String cellName;
        private Type propertyType;
        private String propertyName;
        private int columnIndex;
        private int rowIndex;
        private String format;
        private bool globalField;
        private bool directCellData;

        public FieldItem(String cellName, Type propType, String propName, int colIndex, int rowIndex, String format = "", bool globalField = false, bool directCellData = false)
        {
            this.cellName = cellName;
            this.propertyType = propType;
            this.propertyName = propName;
            this.columnIndex = colIndex;
            this.rowIndex = rowIndex;
            this.format = format;
            this.globalField = globalField;
            this.directCellData = directCellData;
    }


        public String CellName { get { return this.cellName; } }
        public Type PropertyType { get { return this.propertyType; } }
        public String PropertyName { get { return this.propertyName; } }
        public int ColumnIndex { get { return this.columnIndex; } }
        public int RowIndex { get { return this.rowIndex; } }
        public String Format { get { return this.format; } }
        public bool GlobalField { get { return this.globalField; } }
        public bool DirectCellData { get { return this.directCellData; } }

        public dynamic GetValueConverted(Object rawValue)
        {
            try
            {
                //I am realy sorry for this hack.
                if (propertyType == typeof(DateTime))
                {
                    if (rawValue is DateTime)
                    {
                        return (DateTime)rawValue;
                    }

                    long lv;
                    if (long.TryParse(rawValue.ToString(), out lv)) {
                        Loger.log.Warn("FieldItem - GetValueConverted: datetime rawValue:" + rawValue.ToString());
                        return new DateTime(lv);
                    }

                    Loger.log.Error("FieldItem - GetValueConverted: datetime in not formated rawValue:" + rawValue.ToString());
                   return DateTime.MinValue;
                }
                    return Convert.ChangeType(rawValue, propertyType);
                }
            catch (Exception ex)
            {
                Loger.log.Error("FieldItem - GetValueConverted: Field: " + propertyName + ", RawValue: " + rawValue + " exceptionMessage: " + ex.Message);
                
            }
            return null;
        }
    }
}
