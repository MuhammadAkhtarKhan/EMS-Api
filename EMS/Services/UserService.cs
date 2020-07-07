using EMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Services
{
    public class UserService
    {
        public UserViewModel ValidateUser(string userName, string password)
        {
            // Here you can write the code to validate
            // User from database and return accordingly
            // To test we use dummy list here
            var userList = GetUserList();
            var user = userList.Find(x => x.UserName == userName && x.Password == password);
            return user;
        }

        public List<UserViewModel> GetUserList()
        {
            List<UserViewModel> users = null;
            using (var ctx = new EMSEntities())
            {
                users = ctx.USERS.Select(x => new UserViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Password = x.Password,
                    //Email = x.Email
                }).ToList();
            }
            return users;
        }
    }
}