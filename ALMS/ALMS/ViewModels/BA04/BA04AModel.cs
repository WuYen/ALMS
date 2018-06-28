using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALMS.Models;

namespace ALMS.ViewModels.BA04
{
    [MetadataType(typeof(BA04A_MD))]
    public class BA04AModel : BA04A
    {
        public ModelStateDictionary ModelState { get; set; }
    }

    public class BA04A_MD
    {
        [Required(ErrorMessage = "必填")]
        public string SET_NO { get; set; }

        [Required(ErrorMessage = "必填")]
        public string SET_NM { get; set; }
    }
}