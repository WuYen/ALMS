using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALMS.Utilities
{
    public class ResultHelper<T>
    {
        public T Data { get; set; }

        public string Message { get; set; }
    }

    public class ResultHelperBatch<T, K>
    {
        public List<T> Insert { get; set; }
        public List<T> Update { get; set; }
        public List<K> Delete { get; set; }
        public string Message { get; set; }
    }
}