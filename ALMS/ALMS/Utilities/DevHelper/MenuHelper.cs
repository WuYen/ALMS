using DevExpress.Web.Mvc;
using System.Web.UI.WebControls;

namespace ALMS.Utilities
{
    public class MenuHelper
    {
        public static void GetToolBarSetting(MenuSettings settings, string clickEventName = "ToolBarClick")
        {
            settings.Name = "ToolBar";
            settings.ClientSideEvents.ItemClick = clickEventName;
            settings.Width = Unit.Percentage(100);
            settings.ItemAutoWidth = false;
            settings.EnableAnimation = true;
            settings.Styles.Style.BackColor = System.Drawing.Color.White;
            settings.ControlStyle.Border.BorderStyle = BorderStyle.Solid;
            settings.ControlStyle.Border.BorderColor = System.Drawing.ColorTranslator.FromHtml("#c0c0c0");
            settings.ControlStyle.Border.BorderWidth = 1;
            settings.Styles.Item.BackColor = System.Drawing.Color.White;
            settings.Styles.Item.HoverStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0072c6");
            settings.Styles.Item.HoverStyle.ForeColor = System.Drawing.Color.White;
            settings.ControlStyle.Paddings.PaddingTop = 5;
            settings.ControlStyle.Paddings.PaddingBottom = 5;
        }
    }
}