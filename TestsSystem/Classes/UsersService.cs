using System;
using System.Linq;
using TestsSystem.Models;

namespace TestsSystem
{
    public static class UsersService
    {
        public static Users currentUser { get; set; }

        public static bool CheckAuth(String Login, String Password, ref Users user)
        {
            user = MainClass.db.Users.Where(x => x.Login == Login && x.Password == Password).FirstOrDefault();

            if (user == null)
                return false;

            return true;
        }

        public static bool SameLogin(String Login)
        {
            var user = MainClass.db.Users.Where(x => x.Login == Login).FirstOrDefault();

            if (user == null)
                return false;

            return true;
        }
    }
}
