using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALMS.ViewModels
{
    public abstract class ServiceBase : IDisposable
    {
        public Models.ALMSEntities _entity;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (_entity == null) return;
            _entity.Dispose();
        }
    }
}