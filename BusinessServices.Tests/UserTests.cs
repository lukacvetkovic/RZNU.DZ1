using System;
using BusinessServices.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessServices.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void RegisterGet()
        {
            IUserServices us = ServicesFactory.GetUserServices();

            var user = us.GetUserByEmail("test@test.hr");
            if (user == null)
            {
                us.Register("test@test.hr", "123456", "Test User");
                user = us.GetUserByEmail("test@test.hr");
            }

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void Authenticate()
        {
            IUserServices us = ServicesFactory.GetUserServices();

            var user = us.GetUserByEmail("test@test.hr");
            if (user == null)
            {
                us.Register("test@test.hr", "123456", "Test User");
                user = us.GetUserByEmail("test@test.hr");
            }

            Assert.IsNotNull(user);

            int userId = us.Authenticate(user.Email, user.Password);

            Assert.AreNotEqual(userId, 0);
        }
    }
}
