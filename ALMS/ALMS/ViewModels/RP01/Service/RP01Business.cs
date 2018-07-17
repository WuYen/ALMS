using ALMS.Models;
using ALMS.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ALMS.ViewModels.RP01.Service
{
    public class RP01Business
    {
        public static List<RP01AModel> FromEntity(List<TR01A> entityList)
        {
            var detailList = new List<RP01AModel>();
            foreach (var item in entityList)
            {
                var BA01A = CacheCommonDataModule.GetBA01A(item.BA01A_ID);
                detailList.Add(new RP01AModel()
                {
                    TR01A_ID = item.TR01A_ID,
                    BA01A_ID = item.BA01A_ID,
                    CRE_MY = item.CRE_MY,
                    DEB_MY = item.DEB_MY,
                    SUM_RM = item.SUM_RM,
                    ACC_NO = BA01A.ACC_NO,
                    ACC_NM = BA01A.ACC_NM,
                    TRN_DT = item.TRN_DT,
                    DtTRN_DT = DateTime.ParseExact(item.TRN_DT, "yyyyMMdd", CultureInfo.InvariantCulture),
                    VOU_NO = item.VOU_NO,
                    BA02A_ID = item.BA02A_ID,
                    BA02B_ID = item.BA02B_ID,
                    BA03A_ID = item.BA03A_ID,
                    DA03A_ID = item.DA03A_ID,
                });
            }
            return detailList;
        }
    }
}