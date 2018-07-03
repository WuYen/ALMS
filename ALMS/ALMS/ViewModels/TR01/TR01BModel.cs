using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALMS.Models;

namespace ALMS.ViewModels.TR01
{
    [MetadataType(typeof(TR01B_MD))]
    public class TR01BModel
    {
        public int TR01A_ID { get; set; }
        public Nullable<int> BA01A_ID { get; set; }
        public string ACC_NO { get; set; }
        public string ACC_NM { get; set; }
        public string SUM_RM { get; set; }
        public Nullable<decimal> DEB_MY { get; set; }
        public Nullable<decimal> CRE_MY { get; set; }
    }

    public class TR01B_MD
    {
        [Required(ErrorMessage = "必填")]
        public int? BA01A_ID { get; set; }
    }
}