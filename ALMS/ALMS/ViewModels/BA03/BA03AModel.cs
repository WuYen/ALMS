using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALMS.Models;

namespace ALMS.ViewModels.BA03
{
    [MetadataType(typeof(BA03A_MD))]
    public class BA03AModel : BA03A
    {
        public ModelStateDictionary ModelState { get; set; }
    }

    public class BA03A_MD
    {
        [Required(ErrorMessage = "必填")]
        public string DEP_NO { get; set; }

        [Required(ErrorMessage = "必填")]
        public string DEP_NM { get; set; }
    }
}