using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ALMS.Utilities
{
    public static class BatchGridHelper
    {
        /// <summary>取得BatchGrid基本設定</summary>
        /// <param name="settings"></param>
        /// <param name="name"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        public static void GetBasicSetting(GridViewSettings settings, string name, string controller, string action, int pageSize = 50, int hideDataCellsAtWindowInnerWidth = 800)
        {
            settings.Name = name;
            settings.CallbackRouteValues = new { Controller = controller, Action = action };
            settings.Width = Unit.Percentage(100);
            settings.SettingsAdaptivity.AdaptivityMode = GridViewAdaptivityMode.HideDataCellsWindowLimit;
            settings.SettingsAdaptivity.HideDataCellsAtWindowInnerWidth = hideDataCellsAtWindowInnerWidth;
            settings.ControlStyle.Paddings.Padding = Unit.Pixel(0);
            settings.ControlStyle.Border.BorderWidth = Unit.Pixel(1);

            settings.Styles.Header.BackColor = System.Drawing.ColorTranslator.FromHtml("#e8e8e8");
            settings.Styles.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0072c6");
            settings.CommandColumn.HeaderStyle.BackColor = System.Drawing.Color.White;

            settings.SettingsPager.Visible = true;
            settings.SettingsPager.PageSize = pageSize;

            settings.Settings.ShowGroupPanel = false;
            settings.Settings.ShowFilterRow = false;
            settings.SettingsBehavior.AllowSelectByRowClick = true;

            settings.CellEditorInitialize = (s, e) =>
            {
                ASPxEdit editor = (ASPxEdit)e.Editor;
                editor.ValidationSettings.Display = Display.None;
            };

            settings.SettingsBehavior.AllowSort = false;
            settings.SettingsBehavior.AllowDragDrop = false;
        }

        /// <summary>設定Editing</summary>
        /// <param name="settings"></param>
        /// <param name="controller"></param>
        public static void SetBatchEditing(GridViewSettings settings, string controller)
        {
            settings.CustomActionRouteValues = new { Controller = controller, Action = "DetailGridCustomUpdate" };
            settings.SettingsEditing.BatchUpdateRouteValues = new { Controller = controller, Action = "DetailGridBatchUpdate" };

            settings.SettingsEditing.Mode = GridViewEditingMode.Batch;
            settings.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Cell;
            settings.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
            //settings.SettingsEditing.BatchEditSettings.HighlightDeletedRows = true;
            settings.SettingsEditing.BatchEditSettings.HighlightDeletedRows = false;
            settings.SettingsEditing.BatchEditSettings.ShowConfirmOnLosingChanges = true;
        }

        /// <summary>CommandColumn基本設定</summary>
        /// <param name="settings"></param>
        /// <param name="visible"></param>
        public static void SetCommandColumn(GridViewSettings settings, bool visible)
        {
            settings.CommandColumn.Visible = visible;
            settings.CommandColumn.Width = Unit.Pixel(40);
            settings.CommandColumn.ShowDeleteButton = true;
            settings.CommandColumn.CellStyle.Paddings.Padding = 0;
            settings.CommandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        }

        /// <summary> BtnDetailAddNew、DeleteButton 
        /// With ClientSideEvent BtnDetailAddNewClick()
        /// </summary>
        /// <param name="settings">GridViewSettings</param>
        /// <param name="helper">this.Html</param>        
        public static void SetCommandButton(GridViewSettings settings, HtmlHelper helper, bool isCalculate = false)
        {
            settings.CommandColumn.SetHeaderTemplateContent(c =>
            {
                helper.DevExpress().Button(btn =>
                {
                    //EditorSetting.GetButtonDefault(settings, "BtnDetailAddNew", Resources.Resource1.Add);
                    btn.Name = "BtnDetailAddNew";
                    btn.ClientSideEvents.Click = "BtnDetailAddNewClick";
                    btn.Text = "Add";
                    btn.Images.Image.Url = "~/Content/Icon/add_circle_outline_grey_18x18.png";
                    btn.Styles.Style.Paddings.PaddingTop = 0;
                    btn.Styles.Style.Paddings.PaddingBottom = 0;
                    btn.Styles.Style.Paddings.PaddingLeft = 0;
                    btn.Styles.Style.Paddings.PaddingRight = 0;
                }).GetHtml();
            });

            settings.SettingsCommandButton.DeleteButton.Styles.Style.Paddings.Padding = 0;
            settings.SettingsCommandButton.DeleteButton.Image.Url = "~/Content/Icon/delete_grey_18x18.png";
            settings.SettingsCommandButton.DeleteButton.Text = "Delete";

            settings.CommandColumn.ShowRecoverButton = false;
            /*
            if (isCalculate)
            {
                settings.Styles.CommandColumnItem.CssClass = "commandCell";
                //settings.CommandColumn.ButtonRenderMode = GridCommandButtonRenderMode.Button;

                settings.CommandColumn.ShowRecoverButton = false;
                var btnRecover = new GridViewCommandColumnCustomButton() { ID = "customRecover", Text = Resources.Resource.BtnRecover };
                btnRecover.Styles.Style.CssClass = "commandButtonRecoverClass";//use a custom CSS class
                settings.CommandColumn.CustomButtons.Add(btnRecover);

                //settings.CommandColumn.ButtonRenderMode = GridCommandButtonRenderMode.Button;
            }
            */

            settings.SettingsCommandButton.RenderMode = GridCommandButtonRenderMode.Button;
        }

        /// <summary>Column基本設定</summary>
        /// <param name="column"></param>
        /// <param name="fieldName"></param>
        /// <param name="caption"></param>
        /// <param name="showEditorInBatchEditMode"></param>
        public static void SetColumns(MVCxGridViewColumn column, string fieldName, string caption, bool showEditorInBatchEditMode = true)
        {
            column.FieldName = fieldName;
            column.Caption = caption;
            column.Settings.ShowEditorInBatchEditMode = showEditorInBatchEditMode;
            if (!showEditorInBatchEditMode)
            {
                column.CellStyle.BackColor = System.Drawing.Color.Beige;
            }
        }

        /// <summary>設定Column-SpinEdit</summary>
        /// <param name="p"></param>
        /// <param name="places"></param>
        public static void SetColumnSpinEdit(MVCxColumnSpinEditProperties p, int decimalPlaces = 2)
        {
            p.DisplayFormatString = "G29";
            p.DecimalPlaces = decimalPlaces;
            /*
            p.DisplayFormatString = "N" + places.ToString();
            p.DecimalPlaces = places;
            */
        }

        /// <summary>設定Column-ComboBox</summary>
        /// <param name="p"></param>
        /// <param name="valueField"></param>
        /// <param name="textField"></param>
        /// <param name="dataSource"></param>
        public static void SetColumnComboBox(MVCxColumnComboBoxProperties p, string valueField, string textField, object dataSource)
        {
            p.ValueField = valueField;
            p.TextField = textField;
            p.DataSource = dataSource;
        }

        /// <summary>設定Column-Memo</summary>
        /// <param name="p"></param>
        public static void SetColumnMemo(MVCxColumnMemoProperties p)
        {

        }

        /// <summary>設定Column加總</summary>
        /// <param name="settings"></param>
        /// <param name="helper"></param>
        /// <param name="column"></param>
        public static void SetColumnSummary(GridViewSettings settings, HtmlHelper helper, MVCxGridViewColumn column)
        {
            column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            ASPxSummaryItem summaryItem = new ASPxSummaryItem(column.FieldName, DevExpress.Data.SummaryItemType.Sum);
            summaryItem.Tag = column.FieldName + "_Sum";
            summaryItem.DisplayFormat = "<strong>{0}</strong>";
            settings.TotalSummary.Add(summaryItem);

            column.SetFooterTemplateContent(c =>
            {
                helper.DevExpress().Label(lbSettings =>
                {
                    lbSettings.Name = "label" + column.FieldName;
                    lbSettings.Properties.EnableClientSideAPI = true;
                    ASPxSummaryItem summaryItem1 = c.Grid.TotalSummary.First(i => i.Tag == (column.FieldName + "_Sum"));
                    if (c.Grid.GetTotalSummaryValue(summaryItem1) != null)
                    {
                        lbSettings.Text = c.Grid.GetTotalSummaryValue(summaryItem1).ToString();
                    }
                    else
                    {
                        lbSettings.Text = "";
                    }
                }).Render();
            });

            settings.Settings.ShowFooter = true;
        }

        public static void SetAllErrors()
        {

        }
    }
}