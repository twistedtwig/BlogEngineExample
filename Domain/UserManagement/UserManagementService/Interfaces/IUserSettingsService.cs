namespace UserManagementService.Interfaces
{
    public interface IUserSettingsService
    {
        int MinPasswordLength { get; set; }
        bool MustHaveUpperAndLower { get; set; }
        bool PasswordMustHaveNoneAlphaNumeric { get; set; }
        bool PasswordMustHaveNumbers { get; set; }

        bool UsernameOnlyAlphaNumerics { get; set; }
    }
}
