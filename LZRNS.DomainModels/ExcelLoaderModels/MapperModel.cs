using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace LZRNS.DomainModels.ExcelLoaderModels
{
    public class MapperModel
    {
        public MapperModel()
        {
            Fields = new List<FieldItem>();
        }

        public MapperModel(string path)
        {
            Fields = new List<FieldItem>();
            InitializeFields(path);
        }

        public List<FieldItem> Fields { get; }

        /// <summary>
        /// Creates FieldItem from information provided by XElement.
        /// </summary>
        /// <exception cref="NullReferenceException">When item.Attribute(attrName) does not exist</exception>
        private static FieldItem PopulateField(XElement item)
        {
            string cellName = item.Attribute("CellName").Value;
            var propertyType = Type.GetType(item.Attribute("Type").Value);
            string propertyName = item.Attribute("PropertyName").Value;
            int columnIndex = int.Parse(item.Attribute("ColumnId").Value);
            int rowIndex = int.Parse(item.Attribute("RowId").Value);
            string format = (item.Attribute("Format") != null) ? item.Attribute("Format").Value : string.Empty;
            bool global = (item.Attribute("GlobalField") != null) && bool.Parse(item.Attribute("GlobalField").Value);
            bool directCellData = (item.Attribute("DirectCellData") != null) && bool.Parse(item.Attribute("DirectCellData").Value);

            return new FieldItem(cellName, propertyType, propertyName, columnIndex, rowIndex, format, global, directCellData);
        }

        private void InitializeFields(string path)
        {
            var root = XElement.Load(new XmlTextReader(path));

            var items = root.Descendants("Item").ToList();

            if (items.Count > 0)
            {
                Fields.AddRange(items.Select(PopulateField));
            }
        }
    }
}