using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Resturant.Core.Utilities
{

    public class GridData
    {
        private readonly List<GridColumn> _columns = new List<GridColumn>();
        //PermissionEntities pentity = new PermissionEntities();
        public string JsonData;
        public string JsonColumn;
        public string Key { get; set; }
        // public List<FormControls> JsonFrmCntrls;
        //  public Actions JsonActionsCntrls;
        public bool RtlDirection;
        //public GridData()
        //{
        //    JsonFrmCntrls = new List<FormControls>();
        //    JsonActionsCntrls = new Actions();

        //}
        public GridData GetGriData(string data, params GridColumn[] gridColumns)
        {

            foreach (var gridColumn in gridColumns)
            {
                _columns.Add(new GridColumn { dataField = gridColumn.dataField, validationRules = gridColumn.validationRules, setCellValueS = gridColumn.setCellValueS, lookup = gridColumn.lookup, editorOptions = gridColumn.editorOptions, caption = gridColumn.caption, dataType = gridColumn.dataType, width = gridColumn.width, format = gridColumn.format, visible = gridColumn.visible, groupIndex = gridColumn.groupIndex, calculateCellValues = gridColumn.calculateCellValues, columns = gridColumn.columns });
            }
            var columnsJson = JsonConvert.SerializeObject(_columns);
            var gridData = new GridData() { JsonData = data, JsonColumn = columnsJson, RtlDirection = true };

            return gridData;
        }
    }

    public class GridColumn
    {
        public string dataField;
        public string caption;
        public string dataType { get; set; }
        public List<validationRule> validationRules { get; set; }
        public editorOptionsformat editorOptions { get; set; }
        public string width { get; set; }
        public typeformat format { get; set; }
        public Nullable<bool> visible { get; set; } = true;
        public Nullable<int> groupIndex { get; set; }
        public bool? allowEditing { get; set; }
        public lookup lookup { get; set; }
        public string setCellValueS { get; set; }
        public string formatString { get; set; }
        public string cellTemplate { get; set; }
        public string selectedFilterOperation { get; set; }
        public string calculateCellValues { get; set; }
        public string alignment { get; set; } = "center";
        public int? visibleIndex { get; set; }
        public string name { get; set; }
        public int FormID { get; set; }
        public List<GridColumn> columns { get; set; }
        public GridColumn()
        {
            allowEditing = true;
        }

        //public string width;
    }
    public class validationRule
    {
        public string type { get; set; }
        public string message { get; set; }
        public string pattern { get; set; }
        public string validationCallback { get; set; }
        public int? max { get; set; }
        public int? min { get; set; }
    }
    public class lookup
    {
        public string dataSource { get; set; }
        public string displayExpr { get; set; }
        public string valueExpr { get; set; }
        public string dataSourceFunction { get; set; }
        public string columnName { get; set; }

    }

    public class typeformat
    {
        public string type { get; set; }
        public int precision { get; set; }
    }
    public class lookupDatasource
    {
        public string display { get; set; }
        public object value { get; set; }
    }
    public class editorOptionsformat
    {
        public string type { get; set; }
        public DateTime min { get; set; }
        public string disabled { get; set; } = "true";
    }

    public class ChaildColumns
    {
        public string dataField;
        public string caption;
    }

}
