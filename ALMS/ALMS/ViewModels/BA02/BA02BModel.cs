using ALMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALMS.ViewModels.BA02
{
    [MetadataType(typeof(BA02B_MD))]
    public class BA02BModel: BA02B
    {
    }
    public class BA02B_MD
    {
        [Required(ErrorMessage = "必填")]
        public string CUS_NO { get; set; }

        [Required(ErrorMessage = "必填")]
        public string CUS_NM { get; set; }
    }
}