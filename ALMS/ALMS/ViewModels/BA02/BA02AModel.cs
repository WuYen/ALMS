using ALMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALMS.ViewModels.BA02
{
    [MetadataType(typeof(BA02A_MD))]
    public class BA02AModel: BA02A
    {
    }
    public class BA02A_MD
    {
        [Required(ErrorMessage = "必填")]
        public string CPN_NO { get; set; }

        [Required(ErrorMessage = "必填")]
        public string CPN_NM { get; set; }
    }
}