using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALMS.Models;

namespace ALMS.ViewModels.RP01
{
    public class RP01AModel : TR01A
    {
        public DateTime DtTRN_DT { get; set; }
        public string ACC_NO { get; set; }
        public string ACC_NM { get; set; }
    }
}