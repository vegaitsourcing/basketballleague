using System;

namespace LZRNS.ExcelLoader
{
    public class FieldItem
    {
        public FieldItem(string cellName, Type propType, string propName, int colIndex, int rowIndex, string format = "", bool globalField = false, bool directCellData = false)
        {
            CellName = cellName;
            PropertyType = propType;
            PropertyName = propName;
            ColumnIndex = colIndex;
            RowIndex = rowIndex;
            Format = format;
            GlobalField = globalField;
            DirectCellData = directCellData;
        }

        public string CellName { get; }
        public Type PropertyType { get; }
        public string PropertyName { get; }
        public int ColumnIndex { get; }
        public int RowIndex { get; }
        public string Format { get; }
        public bool GlobalField { get; }
        public bool DirectCellData { get; }

        public dynamic GetValueConverted(object rawValue)
        {
            try
            {
                if (PropertyType == typeof(DateTime))
                {
                    if (rawValue is DateTime time)
                    {
                        return time;
                    }

                    if (long.TryParse(rawValue.ToString(), out long lv))
                    {
                        Loger.log.Warn("FieldItem - GetValueConverted: datetime rawValue:" + rawValue);
                        return new DateTime(lv);
                    }

                    Loger.log.Error("FieldItem - GetValueConverted: datetime in not formatted rawValue:" + rawValue);
                    return DateTime.MinValue;
                }

                if (PropertyType != typeof(bool))
                    return Convert.ChangeType(rawValue, PropertyType);

                if (rawValue is bool boolean)
                {
                    return boolean;
                }

                return rawValue.ToString().Equals("1");
            }
            catch (Exception ex)
            {
                Loger.log.Error("FieldItem - GetValueConverted: Field: " + PropertyName + ", RawValue: " + rawValue + " exceptionMessage: " + ex.Message);
            }

            return null;
        }
    }
}