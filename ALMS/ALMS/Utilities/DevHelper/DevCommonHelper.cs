using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace ALMS.Utilities
{
    public enum BtnType { Search, AddNew, Reload, Save, Cancel, Delete, Filter, Confirm, Edit, Import };
    public class DevCommonHelper
    {
        private enum BtnSettingType { Text, Image }

        /// <summary> 設定Button </summary>
        /// <param name="settings"></param>
        /// <param name="type"></param>
        public static void SetButtonSetting(ButtonSettings settings, BtnType type)
        {
            settings.Text = GetButtonText(type);
            var image = GetButtonIcon(type);
            if (type == BtnType.Search)
            {
                settings.Images.Image.Url = image;
            }
            else
            {
                settings.Images.Image.IconID = image;
            }

        }

        /// <summary> 設定Menu-Button </summary>
        /// <param name="i"></param>
        /// <param name="type"></param>
        public static void SetMenuButtonSetting(MVCxMenuItem i, BtnType type)
        {
            i.Text = GetButtonText(type);
            var image = GetButtonIcon(type);
            if (type == BtnType.Search)
            {
                i.Image.Url = image;
            }
            else
            {
                i.Image.IconID = image;
            }
        }

        private static string GetButtonText(BtnType type)
        {
            var value = string.Empty;
            switch (type)
            {
                case BtnType.Search:
                    value = "查詢";
                    break;
                case BtnType.AddNew:
                    value = "新增";
                    break;
                case BtnType.Reload:
                    //value = Resources.Resource.BtnReload;
                    break;
                case BtnType.Save:
                    value = "儲存";
                    break;
                case BtnType.Cancel:
                    value = "取消";
                    break;
                case BtnType.Delete:
                    value = "刪除";
                    break;
                case BtnType.Filter:
                    value = "篩選";
                    break;
                case BtnType.Confirm:
                    value = "確認";
                    break;
                case BtnType.Edit:
                    value = "編輯";
                    break;
                case BtnType.Import:
                    value = "匯入";
                    break;
            }
            return value;
        }

        private static string GetButtonIcon(BtnType type)
        {
            var value = string.Empty;
            switch (type)
            {
                case BtnType.Search:
                    value = "~/Content/Icon/Search.png";
                    break;
                case BtnType.AddNew:
                    value = "actions_additem_16x16";
                    break;
                case BtnType.Reload:
                    value = "actions_refresh2_16x16";
                    break;
                case BtnType.Save:
                    value = "save_save_16x16office2013";
                    break;
                case BtnType.Cancel:
                    value = "actions_cancel_16x16";
                    break;
                case BtnType.Delete:
                    value = "actions_deletelist_16x16";
                    break;
                case BtnType.Filter:
                    value = "filter_filter_16x16office2013";
                    break;
                case BtnType.Confirm:
                    value = "actions_apply_16x16office2013";
                    break;
                case BtnType.Edit:
                    value = "edit_edit_16x16office2013";
                    break;
                case BtnType.Import:
                    value = "actions_download_16x16office2013";
                    break;
            }
            return value;
        }

        /// <summary> 設定PopupControl-Search </summary>
        /// <param name="settings"></param>
        /// <param name="helper"></param>
        public static void SetPopupControlSetting(PopupControlSettings settings)
        {
            //settings.PopupElementID = "BtnSearch";
            settings.PopupVerticalAlign = PopupVerticalAlign.BottomSides;
            settings.PopupVerticalAlign = PopupVerticalAlign.Below;
            settings.PopupHorizontalAlign = PopupHorizontalAlign.LeftSides;

            settings.AllowDragging = true;
            settings.ShowOnPageLoad = false;
            settings.CloseAction = CloseAction.CloseButton;
            settings.ShowHeader = true;
            settings.HeaderText = "查詢";
            settings.Styles.Content.Paddings.PaddingTop = 0;
            settings.Styles.Content.Paddings.PaddingBottom = 0;
        }

        /// <summary>設定Menu-ToolBar</summary>
        /// <param name="settings"></param>
        public static void SetMenuSetting(MenuSettings settings)
        {
            settings.Width = Unit.Percentage(100);
            settings.ItemAutoWidth = false;
            settings.EnableAnimation = true;
            settings.Styles.Style.BackColor = System.Drawing.Color.White;
            settings.ControlStyle.Border.BorderStyle = BorderStyle.Solid;
            settings.ControlStyle.Border.BorderColor = System.Drawing.Color.Gray;
            settings.ControlStyle.Border.BorderWidth = 1;
            settings.Styles.Item.BackColor = System.Drawing.Color.White;
            settings.Styles.Item.HoverStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0072c6");
            settings.Styles.Item.HoverStyle.ForeColor = System.Drawing.Color.White;
            settings.ControlStyle.Paddings.PaddingTop = 5;
            settings.ControlStyle.Paddings.PaddingBottom = 5;
        }
    }
}