using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortsApi.Models;

namespace PortsApi
{
    public class UsersLogic : BaseLogic
    {
        public User GetUser(string userName)
        {
            User user = new User();
                
            if (userName == null)
            {
                return user;
            }

            user = DB.CommonMngUsers.Where(u => u.UserName == userName).Select(u => new User
            {
                UserID = u.UserId,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
            }).SingleOrDefault();
                return user;
        }
    }
}
