using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALMS.Models;

namespace ALMS.Repositories
{
    public class BA01ARepository : RepositoryBase<BA01A, int>
    {
        public BA01ARepository(ALMSEntities entity) : base(entity)
        {
        }
    }
}