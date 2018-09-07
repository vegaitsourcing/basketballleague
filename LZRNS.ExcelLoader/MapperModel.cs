using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LZRNS.ExcelLoader
{
    class MapperModel
    {
        #region Private fields
        private List<FieldItem> fields;
        private string configPath;
        #endregion Private fields

        public MapperModel()
        {
            this.fields = new List<FieldItem>();
        }
        
        public MapperModel(String path)
        {
            this.fields = new List<FieldItem>();
            this.configPath = path;
        }

        #region Private Methods
        private void InitializeFields(string path)
        {
            XElement root;

            root = XElement.Load(new XmlTextReader(path));

            var items = root.Descendants("Item").ToList();

            //if any
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

        // this can be also dynamic !!!
        private FieldItem PopulateField(XElement item)
        {
            FieldItem fieldItem = new FieldItem();

            fieldItem.CellName = item.Attribute("CellName").Value;
            fieldItem.PropertyType = Type.GetType(item.Attribute("Type").Value);
            fieldItem.ColumnIndex =  int.Parse(item.Attribute("ColumnId").Value);
            fieldItem.RowIndex = int.Parse(item.Attribute("RowId").Value);
            fieldItem.Format = item.Attribute("Format").Value;

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
