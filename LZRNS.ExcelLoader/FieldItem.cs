using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.ExcelLoader
{
    class FieldItem
    {
        public String CellName { get; set; }
        public Type PropertyType { get; set; }
        public String PropertyName { get; set; }
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
        public String Format { get; set; }
        
        public FieldItem() { }

    }
}
