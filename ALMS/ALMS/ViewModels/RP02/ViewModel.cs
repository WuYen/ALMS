using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALMS.ViewModels.RP02
{
    public class ViewModel
    {
    }
    public class SearchViewModel
    {
        public DateTime DateBeg { get; set; }

        public string DateBegStr { get { return this.DateBeg.ToString("yyyyMMdd"); } }

        public DateTime DateEnd { get; set; }
        public string DateEndStr { get { return this.DateEnd.ToString("yyyyMMdd"); } }
    }
}