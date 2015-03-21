using NUnit.Framework;
using UserManagementService.Implementations;

namespace UserManagementServiceTests
{
    [TestFixture]
    public class PasswordValidatorTests
    {

        [Test]
        public void TestMinLength()
        {
            var validator = new PasswordValidator();
            validator.MinLength = 6;
            validator.MustHaveNonAlphaNumeric = false;
            validator.MustHaveNumeric = false;
            validator.MustHaveUpperAndLowerChars = false;

            var r1 = validator.Validate("short");
            Assert.IsNotNull(r1);
            Assert.IsFalse(r1.Valid);
            Assert.Greater(r1.Errors.Length, 0);

            var r2 = validator.Validate("shorts");
            Assert.IsNotNull(r2);
            Assert.IsTrue(r2.Valid);
            Assert.AreEqual(r2.Errors.Length, 0);
        }

        [Test]
        public void TestMustHaveNonAlphaNumeric()
        {
            var validator = new PasswordValidator();
            validator.MinLength = 6;
            validator.MustHaveNonAlphaNumeric = true;
            validator.MustHaveNumeric = false;
            validator.MustHaveUpperAndLowerChars = false;

            var r1 = validator.Validate("shorts");
            Assert.IsNotNull(r1);
            Assert.IsFalse(r1.Valid);
            Assert.Greater(r1.Errors.Length, 0);

            var r2 = validator.Validate("short}");
            Assert.IsNotNull(r2);
            Assert.IsTrue(r2.Valid);
            Assert.AreEqual(r2.Errors.Length, 0);

            var r3 = validator.Validate("}short");
            Assert.IsNotNull(r3);
            Assert.IsTrue(r3.Valid);
            Assert.AreEqual(r3.Errors.Length, 0);

            var r4 = validator.Validate("shor}t");
            Assert.IsNotNull(r4);
            Assert.IsTrue(r4.Valid);
            Assert.AreEqual(r4.Errors.Length, 0);
        }

        [Test]
        public void TestMustHaveNumeric()
        {
            var validator = new PasswordValidator();
            validator.MinLength = 6;
            validator.MustHaveNonAlphaNumeric = false;
            validator.MustHaveNumeric = true;
            validator.MustHaveUpperAndLowerChars = false;

            var r1 = validator.Validate("shorts");
            Assert.IsNotNull(r1);
            Assert.IsFalse(r1.Valid);
            Assert.Greater(r1.Errors.Length, 0);

            var r2 = validator.Validate("sho1ts");
            Assert.IsNotNull(r2);
            Assert.IsTrue(r2.Valid);
            Assert.AreEqual(r2.Errors.Length, 0);

            var r3 = validator.Validate("1shots");
            Assert.IsNotNull(r3);
            Assert.IsTrue(r3.Valid);
            Assert.AreEqual(r3.Errors.Length, 0);

            var r4 = validator.Validate("short1");
            Assert.IsNotNull(r4);
            Assert.IsTrue(r4.Valid);
            Assert.AreEqual(r4.Errors.Length, 0);
        }

        [Test]
        public void TestMustUpperAndLower()
        {
            var validator = new PasswordValidator();
            validator.MinLength = 6;
            validator.MustHaveNonAlphaNumeric = false;
            validator.MustHaveNumeric = false;
            validator.MustHaveUpperAndLowerChars = true;

            var r1 = validator.Validate("shorts");
            Assert.IsNotNull(r1);
            Assert.IsFalse(r1.Valid);
            Assert.Greater(r1.Errors.Length, 0);

            var r2 = validator.Validate("Shorts");
            Assert.IsNotNull(r2);
            Assert.IsTrue(r2.Valid);
            Assert.AreEqual(r2.Errors.Length, 0);

            var r3 = validator.Validate("shOrts");
            Assert.IsNotNull(r3);
            Assert.IsTrue(r3.Valid);
            Assert.AreEqual(r3.Errors.Length, 0);

            var r4 = validator.Validate("shortS");
            Assert.IsNotNull(r4);
            Assert.IsTrue(r4.Valid);
            Assert.AreEqual(r4.Errors.Length, 0);
        }
    }
}
