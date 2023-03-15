using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;

namespace DataAccess.Repositories.UserOperationClaimRepository
{
    public class EfUserOperationClaimClaimDal : EfEntityRepositoryBase<UserOperationClaim, SimpleContextDb>, IUserOperationClaimDal
    {
    }
}
