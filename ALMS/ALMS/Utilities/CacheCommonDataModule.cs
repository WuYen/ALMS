using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALMS.Models;

namespace ALMS.Utilities
{
    public class CacheCommonDataModule
    {
        public static List<DA01A> GetDA01A()
        {
            var list = new List<DA01A>();

            var cacheData = CacheHelper.Get("DA01A");

            if (cacheData == null)
            {
                var entity = new ALMSEntities();
                var DA01A = entity.DA01A.ToList();
                CacheHelper.Set("DA01A", DA01A);
            }

            list = CacheHelper.Get("DA01A") as List<DA01A>;

            return list;
        }

        public static List<DA02A> GetDA02A()
        {
            var list = new List<DA02A>();

            var cacheData = CacheHelper.Get("DA02A");

            if (cacheData == null)
            {
                var entity = new ALMSEntities();
                var DA02A = entity.DA02A.ToList();
                CacheHelper.Set("DA02A", DA02A);
            }

            list = CacheHelper.Get("DA02A") as List<DA02A>;

            return list;
        }
    }
}