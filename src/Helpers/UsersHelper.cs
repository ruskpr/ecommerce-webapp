using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Data;
using SleekClothing.Models;
using System.Security.Claims;

namespace SleekClothing.Helpers
{
    public class UsersHelper
    {
        public static IdentityUser GetUser(ApplicationDbContext context, ClaimsPrincipal user)
        {
            var userid = user.Identities.First().Claims.First().Value;
             
            return context.Users.FirstOrDefault(x => x.Id == userid);
        }

        

        public static bool UserExists()
        {
            bool ret = false;

            return ret;
        }
    }
}
