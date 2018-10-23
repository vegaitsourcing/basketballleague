using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace LZRNS.ExcelLoader
{
    class MapperModel
    {
        #region Private fields
        // key represent PropertyName
        private List<FieldItem> fields;
        #endregion Private fields

        public MapperModel()
        {
            this.fields = new List<FieldItem>();
        }
        
        public MapperModel(String path)
        {
            this.fields = new List<FieldItem>();
            InitializeFields(path);
        }

        #region Private Methods
        private void InitializeFields(string path)
        {
            XElement root;
            root = XElement.Load(new XmlTextReader(path));

            var items = root.Descendants("Item").ToList();
            
            if (items.Any())
            {
                //iterate through all children of root
                foreach (var item in items)
                {
                    FieldItem fieldItem = PopulateField(item);
                    fields.Add(fieldItem);
                }
            }
        }

        // this can be done dynamic !!!
        private FieldItem PopulateField(XElement item)
        {
            
            String cellName = item.Attribute("CellName").Value;
            Type propertyType = Type.GetType(item.Attribute("Type").Value);
            String propertyName = item.Attribute("PropertyName").Value;
            int columnIndex =  int.Parse(item.Attribute("ColumnId").Value);
            int rowIndex = int.Parse(item.Attribute("RowId").Value);
            String format = (item.Attribute("Format") != null) ? item.Attribute("Format").Value: String.Empty;
            bool global = (item.Attribute("GlobalField") != null) ? bool.Parse(item.Attribute("GlobalField").Value) : false;
            bool directCellData = (item.Attribute("DirectCellData") != null) ? bool.Parse(item.Attribute("DirectCellData").Value) : false;
            

            FieldItem fieldItem = new FieldItem(cellName, propertyType, propertyName, columnIndex, rowIndex, format, global, directCellData);

            return fieldItem;
        }
        #endregion Private Methods

        #region Properties
        public List<FieldItem> Fields
        {
            get { return fields; }
        }
        #endregion Properties
    }
}
