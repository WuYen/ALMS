using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALMS.Models;

namespace ALMS.Repositories
{
    public class BA02BRepository : RepositoryBase<BA02B, int>
    {
        public BA02BRepository(ALMSEntities entity) : base(entity)
        {
        }
    }
}