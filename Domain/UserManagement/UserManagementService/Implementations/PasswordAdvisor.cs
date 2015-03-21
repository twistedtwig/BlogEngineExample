
namespace UserManagementService.Implementations
{
    public enum PasswordScore
    {
        Blank = 0,
        VeryWeak = 1,
        Weak = 2,
        Medium = 3,
        Strong = 4,
        VeryStrong = 5
    }

    public class PasswordAdvisor
    {
        /// <summary>
        /// asserts the strength of a password from 0 to 5, (0 being blank, 5 very strong).
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static PasswordScore CheckStrength(string password)
        {
            var validtor = new PasswordValidator();

            int score = 1;

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;

            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (validtor.AssertHasNumeric(password))
                score++;
            if (validtor.AssertUpperAndLowerCharacers(password))
                score++;
            if (validtor.AssertNonAlphaNumeric(password))
                score++;

            return (PasswordScore)score;
        }
    }
}
