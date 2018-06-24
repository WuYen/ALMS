using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ALMS.Utilities
{
    public class GridHelper
    {
        /// <summary> 取得查詢Grid </summary>
        /// <param name="settings"></param>
        /// <param name="name"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="verticalScrollableHeight"></param>
        public static void GetSearchBasicSetting(GridViewSettings settings, string name, string controller, string action, int verticalScrollableHeight = 300)
        {
            settings.Name = name;
            settings.CallbackRouteValues = new { Controller = controller, Action = action };
            settings.ClientSideEvents.BeginCallback = "SearchGridBegCall";
            settings.ClientSideEvents.EndCallback = "SearchGridEndCall";
            settings.Width = Unit.Percentage(100);

            settings.SettingsBehavior.AllowSelectByRowClick = true;
            settings.SettingsBehavior.AllowSelectSingleRowOnly = true;
            settings.ClientSideEvents.RowDblClick = "SearchGridSelect";
            settings.Styles.Header.BackColor = System.Drawing.ColorTranslator.FromHtml("#e8e8e8");
            settings.Styles.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0072c6");
            settings.CommandColumn.Caption = " ";
            settings.CommandColumn.Visible = true;
            settings.CommandColumn.ShowSelectCheckbox = true;

            settings.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
            settings.Settings.VerticalScrollableHeight = verticalScrollableHeight;
            settings.Settings.ShowFooter = false;
            settings.SettingsPager.AlwaysShowPager = true;
        }

        /// <summary> 取得單檔Grid </summary>
        /// <param name="settings"></param>
        /// <param name="name"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="verticalScrollableHeight"></param>
        public static void GetSingleBasicSetting(GridViewSettings settings, string name, string controller, string action)
        {
            settings.Name = name;
            settings.Width = Unit.Percentage(100);
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.CallbackRouteValues = new { Controller = controller, Action = action };

            settings.SettingsEditing.Mode = GridViewEditingMode.EditForm;

            settings.ClientSideEvents.BeginCallback = "GridViewBegCallback";
            settings.ClientSideEvents.EndCallback = "GridViewEndCallback";

            settings.Styles.Header.BackColor = System.Drawing.ColorTranslator.FromHtml("#e8e8e8");
            settings.Styles.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0072c6");

            settings.SettingsEditing.AddNewRowRouteValues = new { Controller = controller, Action = "Insert" };
            settings.SettingsEditing.UpdateRowRouteValues = new { Controller = controller, Action = "Update" };
            settings.SettingsEditing.DeleteRowRouteValues = new { Controller = controller, Action = "Delete" };

            settings.SettingsPager.AlwaysShowPager = true;
            settings.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
            settings.Settings.VerticalScrollableHeight = 300;
        }

        /// <summary> 取得報表Grid </summary>
        /// <param name="settings"></param>
        /// <param name="name"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="verticalScrollableHeight"></param>
        public static void GetReportBasicSetting(GridViewSettings settings, string name, string controller, string action, int verticalScrollableHeight = 300)
        {
            settings.Name = name;
            settings.CallbackRouteValues = new { Controller = controller, Action = action };
            settings.ClientSideEvents.BeginCallback = "GridViewBegCallback";
            settings.Width = Unit.Percentage(100);

            settings.Styles.Header.BackColor = System.Drawing.ColorTranslator.FromHtml("#e8e8e8");
            settings.Styles.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0072c6");

            settings.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
            settings.Settings.VerticalScrollableHeight = verticalScrollableHeight;
            settings.Settings.ShowFooter = false;
            settings.SettingsPager.AlwaysShowPager = true;
        }


        /// <summary>設定Grid-Textbox</summary>
        /// <param name="i"></param>
        /// <param name="displayFormatString"></param>        
        public static void SetTextBox(MVCxGridViewColumn column, string displayFormatString)
        {
            column.EditorProperties().TextBox(
                p =>
                {
                    if (!string.IsNullOrWhiteSpace(displayFormatString))
                    {
                        p.DisplayFormatString = displayFormatString;
                    }
                });
        }

        /// <summary>設定Grid-ComboBox  </summary>
        /// <param name="column"></param>
        /// <param name="valueField"></param>
        /// <param name="textField"></param>
        /// <param name="dataSource"></param>
        public static void SetComboBox(MVCxGridViewColumn column, string valueField, string textField, object dataSource)
        {
            column.EditorProperties().ComboBox(
                p =>
                {
                    p.TextField = textField;
                    p.ValueField = valueField;
                    p.DataSource = dataSource;
                });
        }

        /// <summary>設定Grid-字數過長處理</summary>
        /// <param name="settings"></param>
        /// <param name="fieldName"></param>
        /// <param name="length"></param>
        public static void SetColumnDisplayText(GridViewSettings settings, string fieldName, int length = 5)
        {
            settings.CustomColumnDisplayText += (s, e) =>
            {
                if (e.Column.FieldName == fieldName)// || e.Column.FieldName == "ITM_NM"
                {
                    if (e.Value != null)
                    {
                        string cellValue = e.Value.ToString();
                        if (cellValue.Length > length)
                            e.DisplayText = cellValue.Substring(0, length) + "...";
                    }
                }
            };

            settings.HtmlDataCellPrepared += (s, e) =>
            {
                if (e.DataColumn.FieldName == fieldName)// || e.DataColumn.FieldName == "ITM_NM"
                    if (e.CellValue != null)
                        e.Cell.ToolTip = e.CellValue.ToString();
            };
        }

    }
}