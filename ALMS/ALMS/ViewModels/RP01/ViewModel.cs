using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALMS.ViewModels.RP01
{
    public class ViewModel
    {
    }

    public class SearchViewModel
    {
        public DateTime? DT_BEG { get; set; }
        public string DT_BEG_STR
        {
            get
            {
                return DT_BEG.HasValue ? DT_BEG.Value.ToString("yyyyMMdd") : "";
            }
        }

        public DateTime? DT_END { get; set; }
        public string DT_END_STR
        {
            get
            {
                return DT_END.HasValue ? DT_END.Value.ToString("yyyyMMdd") : "";
            }
        }

        public int? Type { get; set; }
    }
}