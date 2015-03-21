using UserManagementService.Interfaces;

namespace UserManagementService.Implementations
{
    /// <summary>
    /// Wraps up settings for the user service.
    /// </summary>
    public class UserSettingsService : IUserSettingsService
    {
        public UserSettingsService()
        {
            MinPasswordLength = 6;
            PasswordMustHaveNoneAlphaNumeric = false;
            PasswordMustHaveNumbers = true;
            MustHaveUpperAndLower = false;
        }

        public int MinPasswordLength { get; set; }
        public bool MustHaveUpperAndLower { get; set; }
        public bool PasswordMustHaveNoneAlphaNumeric { get; set; }
        public bool PasswordMustHaveNumbers { get; set; }
        public bool UsernameOnlyAlphaNumerics { get; set; }
    }
}
