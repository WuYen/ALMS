using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALMS.ViewModels.RP03;
using ALMS.ViewModels.RP03.Service;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;

namespace ALMS.Controllers
{
    public class RP03Controller : Controller
    {
        private RP03Service _Service = new RP03Service();
        // GET: RP03
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Grid(SearchViewModel search)
        {
            return PartialView("_Grid", GetData(search, false));
        }

        public ActionResult GridCustomCall(SearchViewModel search)
        {
            return PartialView("_Grid", GetData(search, true));
        }

        public ActionResult ExportToExcel(SearchViewModel search)
        {
            var dt = GetData(search, false);

           
            var newDataTable = dt.Copy();
            var row = newDataTable.NewRow();
            var summaryRow = GetSummaryRow();
            row["ACC_NO"] = "";
            row["ACC_NM"] = summaryRow["ACC_NM"];
            row["CUR_MY"] = summaryRow["CUR_MY"];
            row["TOT_MY"] = summaryRow["TOT_MY"];         
            newDataTable.Rows.Add(row);

            return GridViewExtension.ExportToXlsx(GetExortSetting(), newDataTable, new XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG });
        }

        private DataTable GetData(SearchViewModel search, bool reload)
        {
            search.Type = "";
            search.DateBeg = new DateTime(2018, 07, 01);
            search.DateEnd = new DateTime(2018, 07, 31);
            var data = Session["RP03Data"] as DataTable;
            if (reload || data == null)
            {
                var temp = _Service.GetData(search.Type,search.DateBegStr, search.DateEndStr);
                data = temp.Tables[0];          
                ViewBag.Summary = temp.Tables[1].Rows[0];
                Session["RP03Data2"] = temp.Tables[1].Rows[0];
                Session["RP03Data"] = data;
            }
            return data;
        }

        private DataRow GetSummaryRow()
        {
            return Session["RP03Data2"] as DataRow;
        }

        private GridViewSettings GetExortSetting()
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "GridView";
            settings.Styles.Header.BackColor = System.Drawing.ColorTranslator.FromHtml("#e8e8e8");
            settings.Styles.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0072c6");
            settings.Columns.Add(column =>
            {
                column.FieldName = "ACC_NO";
                column.Caption = "科目編號";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "ACC_NM";
                column.Caption = "科目名稱";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "CUR_MY";
                column.Caption = "本期金額";
                column.EditorProperties().SpinEdit(
                p =>
                {
                    p.MinValue = 0;
                    p.MaxValue = 999999999;
                    p.DisplayFormatString = "#,#";
                });
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "TOT_MY";
                column.Caption = "累計金額";
                column.EditorProperties().SpinEdit(
                p =>
                {
                    p.MinValue = 0;
                    p.MaxValue = 999999999;
                    p.DisplayFormatString = "#,#";
                });
            });

            return settings;
        }
    }
}