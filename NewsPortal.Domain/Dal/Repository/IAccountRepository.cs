using System;
using System.Linq;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Dal.Repository
{
    public interface IAccountRepository : IRepository<Account, Guid>
    {}
}