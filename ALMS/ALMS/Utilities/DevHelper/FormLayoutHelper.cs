using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace ALMS.Utilities
{
    public static class FormLayoutHelper
    {
        /// <summary>設定FormLayout基本設定</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settings"></param>
        /// <param name="name"></param>
        /// <param name="colCount"></param>
        public static void GetBasicSetting<T>(FormLayoutSettings<T> settings, string name, int colCount, int width = 600, bool isSearch = false)
        {
            settings.Name = name;

            settings.SettingsAdaptivity.AdaptivityMode = FormLayoutAdaptivityMode.SingleColumnWindowLimit;
            settings.SettingsAdaptivity.SwitchToSingleColumnAtWindowInnerWidth = width;
            settings.ColCount = colCount;

            if (!isSearch)
            {
                settings.Styles.LayoutGroup.Cell.Paddings.PaddingRight = Unit.Pixel(0);
                settings.UseDefaultPaddings = false;
            }
        }

        /// <summary> 設定查詢FormLayout </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settings"></param>
        /// <param name="name"></param>
        /// <param name="colCount"></param>
        /// <param name="width"></param>
        public static void GetSearchSetting<T>(FormLayoutSettings<T> settings, string name, int colCount, int width = 600)
        {
            GetBasicSetting(settings, name, colCount, width, true);
            settings.Styles.Style.Paddings.Padding = 0;
            settings.ControlStyle.Border.BorderWidth = 1;
            settings.ControlStyle.Border.BorderColor = System.Drawing.ColorTranslator.FromHtml("#c0c0c0");
            settings.ControlStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f9f9f9");
            settings.SettingsItemCaptions.VerticalAlign = FormLayoutVerticalAlign.Middle;
        }

        /// <summary>設定FormLayoutItem</summary>
        /// <param name="i"></param>
        /// <param name="name"></param>
        /// <param name="caption"></param>
        public static void SetItemBasicSetting(MVCxFormLayoutItem i, string name, string caption)
        {
            i.Name = name;
            i.FieldName = name;
            i.Caption = caption;
        }

        /// <summary>設定FormLayout-Item-Textbox</summary>
        /// <param name="i"></param>
        /// <param name="isReadOnly"></param>        
        public static void SetTextBox(MVCxFormLayoutItem i, bool isReadOnly = false)
        {
            i.NestedExtension().TextBox(s =>
            {
                if (isReadOnly)
                {
                    s.ReadOnly = true;
                    s.ControlStyle.CssClass = "DisableEditor";
                    s.ControlStyle.BackColor = System.Drawing.Color.Beige;
                }
                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                s.ShowModelErrors = true;

                //s.Width = Unit.Percentage(100);                              
            });
        }

        /// <summary>設定FormLayout-Item-RadioButtonList</summary>
        /// <param name="i"></param>
        /// <param name="isReadOnly"></param>        
        public static void SetRadioButtonList(MVCxFormLayoutItem i, List<ListEditItem> items, bool isReadOnly = false)
        {
            i.NestedExtension().RadioButtonList(s =>
            {
                if (isReadOnly)
                {
                    s.ReadOnly = true;
                    s.ControlStyle.CssClass = "DisableEditor";
                    s.ControlStyle.BackColor = System.Drawing.Color.Beige;
                }
                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                s.ShowModelErrors = true;
                s.Properties.RepeatDirection = RepeatDirection.Horizontal;
                //s.Width = Unit.Percentage(100);          
                s.Properties.Items.AddRange(items);
            });
        }

        /// <summary>設定FormLayout-Item-DateEdit</summary>
        /// <param name="i"></param>
        /// <param name="isReadOnly"></param>        
        public static void SetDateEdit(MVCxFormLayoutItem i, bool isReadOnly = false)
        {
            i.NestedExtension().DateEdit(s =>
            {
                if (isReadOnly)
                {
                    s.ReadOnly = true;
                    s.ControlStyle.CssClass = "DisableEditor";
                    s.ControlStyle.BackColor = System.Drawing.Color.Beige;
                    s.Properties.ButtonStyle.BackColor = System.Drawing.Color.Beige;
                    s.Properties.DropDownButton.Enabled = false;
                    s.Properties.ClientSideEvents.GotFocus = "function(s, e) { s.HideDropDown(); }";
                }
                else
                {
                    s.Properties.ClientSideEvents.GotFocus = "function(s, e) { s.ShowDropDown(); }";
                }
                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                s.ShowModelErrors = true;
                //s.Width = Unit.Percentage(100);
                s.Properties.EditFormat = EditFormat.Custom;
                s.Properties.EditFormatString = "yyyy/MM/dd";
                s.Properties.DisplayFormatString = "yyyy/MM/dd";
            });
        }

        /// <summary>設定FormLayout-Item-ComboBox</summary>
        /// <param name="i"></param>
        /// <param name="valueField"></param>
        /// <param name="textField"></param>
        /// <param name="dataSource"></param>
        /// <param name="isReadOnly"></param>
        public static void SetComboBox(MVCxFormLayoutItem i, string valueField, string textField, object dataSource, bool isReadOnly = false)
        {
            i.NestedExtension().ComboBox(s =>
            {
                if (isReadOnly)
                {
                    s.ReadOnly = true;
                    s.ControlStyle.BackColor = System.Drawing.Color.Beige;
                    s.Properties.ButtonStyle.BackColor = System.Drawing.Color.Beige;
                    s.ControlStyle.CssClass = "DisableEditor";
                }
                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                s.ShowModelErrors = true;
                //s.Width = Unit.Percentage(100);
                s.Properties.ValueField = valueField;
                s.Properties.TextField = textField;
                s.Properties.DataSource = dataSource;
            });
        }

        /// <summary> 設定Search-FormLayout-Item-ComboBox </summary>
        /// <param name="i"></param>
        /// <param name="valueField"></param>
        /// <param name="textField"></param>
        /// <param name="dataSource"></param>
        public static void SetSearchComboBox(MVCxFormLayoutItem i, string valueField, string textField, object dataSource)
        {
            SetComboBox(i, valueField, textField, dataSource);

            i.NestedExtension().ComboBox(s =>
            {
                s.Properties.ClearButton.DisplayMode = ClearButtonDisplayMode.OnHover;
            });
        }

        /// <summary>設定FormLayout-Item-Memo</summary>
        /// <param name="i"></param>
        /// <param name="rows"></param>
        /// <param name="isReadOnly"></param>        
        public static void SetMemo(MVCxFormLayoutItem i, int rows = 1, bool isReadOnly = false)
        {
            i.NestedExtension().Memo(s =>
            {
                if (isReadOnly)
                {
                    s.ReadOnly = true;
                    s.ControlStyle.CssClass = "DisableEditor";
                    s.ControlStyle.BackColor = System.Drawing.Color.Beige;
                }
                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                s.ShowModelErrors = true;
                s.Properties.Rows = rows;
            });
        }

        /// <summary> >設定FormLayout-Item-SpinEdit </summary>
        /// <param name="i"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="isReadOnly"></param>
        public static void SetSpinEdit(MVCxFormLayoutItem i, int decimalPlaces = 2, bool isReadOnly = false)
        {
            i.NestedExtension().SpinEdit(s =>
            {
                if (isReadOnly)
                {
                    s.ReadOnly = true;
                    s.ControlStyle.CssClass = "DisableEditor";
                    s.ControlStyle.BackColor = System.Drawing.Color.Beige;
                    s.Properties.ButtonStyle.BackColor = System.Drawing.Color.Beige;
                }
                s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                s.ShowModelErrors = true;

                s.Properties.DisplayFormatString = "G29";
                s.Properties.DecimalPlaces = decimalPlaces;
                s.Properties.DisplayFormatInEditMode = true;
            });
        }

        /// <summary>設定FormLayout-Group</summary>
        /// <param name="g"></param>
        /// <param name="caption"></param>
        /// <param name="colCount"></param>
        public static void SetGroupItem(MVCxFormLayoutGroup g, string caption, int colCount)
        {
            g.Caption = caption;
            g.GroupBoxDecoration = GroupBoxDecoration.HeadingLine;
            g.GroupBoxStyle.Caption.ForeColor = System.Drawing.Color.Gray;
            g.ColCount = colCount;
        }
    }
}