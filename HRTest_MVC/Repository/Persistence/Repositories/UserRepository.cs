using HRTest_MVC.Core.Repositories;
using HRTest_MVC.Models; 
using System.Data.Entity;
using System.Linq;
using HRTest_MVC.ViewModel;
using System;
using ApplicationCore.Helper;

namespace HRTest_MVC.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PlutoContext context) : base(context)
        {

        }

        public UserWithRoleViewModel Login(LoginViewModel model)
        {
            UserWithRoleViewModel userModel = new UserWithRoleViewModel();
            var pass = PasswordEncryptor.MD5Hash(model.Password);
            var user = PlutoContext.Users.FirstOrDefault(a => a.Email == model.Email && a.Password == pass);
            if (user != null)
            {
                userModel.User = user;
                var roles = PlutoContext.Roles.FirstOrDefault(a => a.Id == user.RoleId);
                if (roles != null)
                {
                    userModel.Roles = roles;
                }
            }
            
            return userModel;
        }

        public PlutoContext PlutoContext
        {
            get { return Context as PlutoContext; }
        }
    }
}