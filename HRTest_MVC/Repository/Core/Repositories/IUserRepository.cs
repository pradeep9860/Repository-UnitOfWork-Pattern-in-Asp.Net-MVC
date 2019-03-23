using HRTest_MVC.Models;
using HRTest_MVC.ViewModel;

namespace HRTest_MVC.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        UserWithRoleViewModel Login(LoginViewModel model);
    }
}