using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsSystem.Tests
{
    [TestClass()]
    public class UsersServiceTests
    {
        [TestMethod()]
        public void CheckAuthTest()
        {
            Models.Users user = new Models.Users();

            bool actual = UsersService.CheckAuth("admin", "admin",ref user);
            Assert.AreEqual(true, actual);
        }
    }
}