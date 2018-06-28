using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALMS.Models;

namespace ALMS.ViewModels.BA01
{
    [MetadataType(typeof(BA03A_MD))]
    public class BA01AModel : BA01A
    {
        public ModelStateDictionary ModelState { get; set; }
    }

    public class BA03A_MD
    {
        [Required(ErrorMessage = "必填")]
        public string ACC_NO { get; set; }

        [Required(ErrorMessage = "必填")]
        public string ACC_NM { get; set; }

        [Required(ErrorMessage = "必填")]
        public string SMO_YN { get; set; }
    }
}