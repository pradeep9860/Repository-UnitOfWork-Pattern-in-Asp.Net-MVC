using HRTest_MVC.Core.Repositories;
using System;

namespace HRTest_MVC.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRolesRepository Roles { get; }
        int Complete();
    }
}