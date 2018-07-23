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

        public static List<DA03A> GetDA03A()
        {
            var list = new List<DA03A>();

            var cacheData = CacheHelper.Get("DA03A");

            if (cacheData == null)
            {
                var entity = new ALMSEntities();
                var DA03A = entity.DA03A.ToList();
                CacheHelper.Set("DA03A", DA03A);
            }

            list = CacheHelper.Get("DA03A") as List<DA03A>;

            return list;
        }

        public static List<BA01A> GetBA01A()
        {
            var list = new List<BA01A>();

            var cacheData = CacheHelper.Get("BA01A");

            if (cacheData == null)
            {
                var entity = new ALMSEntities();
                var BA01A = entity.BA01A.ToList();
                CacheHelper.Set("BA01A", BA01A);
            }

            list = CacheHelper.Get("BA01A") as List<BA01A>;

            return list;
        }
        public static BA01A GetBA01A(int? BA01A_ID)
        {
            if (BA01A_ID.HasValue)
            {
                return GetBA01A().Where(x => x.BA01A_ID == BA01A_ID).First();
            }
            else
            {
                return new BA01A();
            }
        }
        public static List<BA02A> GetBA02A()
        {
            var list = new List<BA02A>();

            var cacheData = CacheHelper.Get("BA02A");

            if (cacheData == null)
            {
                var entity = new ALMSEntities();
                var BA02A = entity.BA02A.ToList();
                CacheHelper.Set("BA02A", BA02A);
            }

            list = CacheHelper.Get("BA02A") as List<BA02A>;

            return list;
        }
        public static List<BA02B> GetBA02B()
        {
            var list = new List<BA02B>();

            var cacheData = CacheHelper.Get("BA02B");

            if (cacheData == null)
            {
                var entity = new ALMSEntities();
                var BA02B = entity.BA02B.ToList();
                CacheHelper.Set("BA02B", BA02B);
            }

            list = CacheHelper.Get("BA02B") as List<BA02B>;

            return list;
        }
        public static List<BA02B> GetBA02B(int? BA02A_ID)
        {
            if (BA02A_ID.HasValue)
            {
                return GetBA02B().Where(x => x.BA02A_ID == BA02A_ID).ToList();
            }
            return new List<BA02B>();
        }

        public static List<BA03A> GetBA03A()
        {
            var list = new List<BA03A>();

            var cacheData = CacheHelper.Get("BA03A");

            if (cacheData == null)
            {
                var entity = new ALMSEntities();
                var BA03A = entity.BA03A.ToList();
                CacheHelper.Set("BA03A", BA03A);
            }

            list = CacheHelper.Get("BA03A") as List<BA03A>;

            return list;
        }

        private static Dictionary<string, string> TypeDictionary = new Dictionary<string, string> { { "稅", "A" }, { "財", "B" } };

        public static Dictionary<string, string> GetTypeDictionary()
        {
            return TypeDictionary;
        }
    }
}