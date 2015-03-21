using NUnit.Framework;
using UserManagement.Shared;
using UserManagementService.Implementations;

namespace UserManagementServiceTests
{
    public class TestUser : UserBase<int>
    {

    }

    [TestFixture]
    public class UserValidiatorTests
    {

        [Test]
        public void TestUserNames()
        {
            var validator = new UserValidator<TestUser, int>();
            validator.AllowOnlyAlphanumericUserNames = true;

            var r1 = validator.Validate(new TestUser {UserName = "Jon", Email = "jon@jon.com"});
            Assert.IsNotNull(r1);
            Assert.IsTrue(r1.Valid);
            Assert.AreEqual(0, r1.Errors.Length);

            var r2 = validator.Validate(new TestUser { UserName = "Jon1", Email = "jon@jon.com" });
            Assert.IsNotNull(r2);
            Assert.IsTrue(r2.Valid);
            Assert.AreEqual(0, r2.Errors.Length);

            var r3 = validator.Validate(new TestUser { UserName = "Jon1*", Email = "jon@jon.com" });
            Assert.IsNotNull(r3);
            Assert.IsFalse(r3.Valid);
            Assert.AreEqual(1, r3.Errors.Length);

            validator.AllowOnlyAlphanumericUserNames = false;

            var r4 = validator.Validate(new TestUser { UserName = "Jon1*", Email = "jon@jon.com" });
            Assert.IsNotNull(r4);
            Assert.IsTrue(r4.Valid);
            Assert.AreEqual(0, r4.Errors.Length);
        }

        [Test]
        public void TestUserEmails()
        {
            var validator = new UserValidator<TestUser, int>();

            var r1 = validator.Validate(new TestUser { UserName = "Jon", Email = "jon@jon.com" });
            Assert.IsNotNull(r1);
            Assert.IsTrue(r1.Valid);
            Assert.AreEqual(0, r1.Errors.Length);

            var r2 = validator.Validate(new TestUser { UserName = "Jon", Email = "jon.com" });
            Assert.IsNotNull(r2);
            Assert.IsFalse(r2.Valid);
            Assert.AreEqual(1, r2.Errors.Length);
        }
    }
}
