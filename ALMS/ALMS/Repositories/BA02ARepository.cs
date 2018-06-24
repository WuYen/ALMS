using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALMS.Models;

namespace ALMS.Repositories
{
    public class BA02ARepository : RepositoryBase<BA02A, int>
    {
        public BA02ARepository(ALMSEntities entity) : base(entity)
        {
        }
    }
}