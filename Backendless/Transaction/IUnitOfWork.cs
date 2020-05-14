using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  interface IUnitOfWork : UnitOfWorkCreate, UnitOfWorkUpdate, UnitOfWorkDelete, UnitOfWorkFind, UnitOfWorkAddRelation, UnitOfWorkSetRelation, UnitOfWorkDeleteRelation, UnitOfWorkExecutor
  {   
  }
}
