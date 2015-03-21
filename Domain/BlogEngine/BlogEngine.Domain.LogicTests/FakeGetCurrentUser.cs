using BlogEngine.Domain.Interfaces;

namespace BlogEngine.Domain.LogicTests
{
    public class FakeGetCurrentUser : IGetCurrentUserName
    {
        public string GetUserName()
        {
            return "MYUSERNAME";
        }
    }
}
