using LibManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibManagementAPI.Helper
{
    public class RoleHelper
    {

        public static async Task<bool> IsAdminOfLibrary(LibraryContext context, string userId)
        {
            var _userID = int.Parse(userId);
            var user = await context.TblUsers.Where(x => x.UserId == _userID).FirstAsync();

            if (user == null)
            {
                return false;
            }

            if (int.Parse(user.UserRole) == 3)
            {
                return true;
            }

            var libs = await context.TblLibraryBranches.Select(x => x).ToListAsync();

            if (libs.Count > 1)
            {
                return false;
            }

            return libs[0].LibraryBranchBranchId == int.Parse(user.UserRole);
            //return true;
        }

        public static async Task<TblUser> GetCurrentUser(LibraryContext context, string userId)
        {
            var _userID = int.Parse(userId);
            return await context.TblUsers.Where(x => x.UserId == _userID).FirstAsync();
        }
    }
}
