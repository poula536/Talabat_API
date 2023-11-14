using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                { 
                    DisplayName = "Poula Mansour",
                    Email = "poulamansour@gmail.com",
                    UserName = "poula",
                    PhoneNumber = "01286495675"
                };
                await userManager.CreateAsync(user,"Pa$$w0rd");
            }
        }
    }
}
