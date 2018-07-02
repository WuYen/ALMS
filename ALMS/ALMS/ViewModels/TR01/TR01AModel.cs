using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALMS.Models;

namespace ALMS.ViewModels.TR01
{
    [MetadataType(typeof(TR01A_MD))]
    public class TR01AModel
    {
        public DateTime DtTRN_DT { get; set; }
        public string TRN_DT { get; set; }
        public Nullable<int> DA03A_ID { get; set; }
        public string VOU_NO { get; set; }
        public Nullable<int> BA02A_ID { get; set; }
        public Nullable<int> BA02B_ID { get; set; }
        public Nullable<int> BA03A_ID { get; set; }
    }

    public class TR01A_MD
    {
        //[Required(ErrorMessage = "必填")]
        //public string ACC_NO { get; set; }}
    }
}