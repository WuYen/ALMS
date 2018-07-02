using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALMS.Models;

namespace ALMS.Repositories
{
    public class TR01ARepository : RepositoryBase<TR01A, int>
    {
        public TR01ARepository(ALMSEntities entity) : base(entity)
        {
        }
    }
}